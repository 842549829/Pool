using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Core.Extension;
using Izual;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
    class PolicyProcessor : RequestProcessorBase, Processor.IPolicyProcessor {
        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, ExternalPolicyFilter filter) {
            RequestResult<IEnumerable<ExternalPolicyView>> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout
                };
                var signValue = new Dictionary<string, string>
                                    {
                                        {"userName", Platform.UserName},
                                        {"pnr", pnrPair.PNR}
                                    };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                var matchResponse = platform.PnrMatchAirp(Platform.UserName, pnrPair.PNR, signText);
                response = matchResponse;
                result = parseMatchResponse(matchResponse, filter);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "通过编码匹配易行政策");
                result = new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "匹配政策");
            return result;
        }

        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, string pnrContent, string patContent, ExternalPolicyFilter filter) {
            if(filter.VoyageType != ItineraryType.OneWay) {
                return new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "不支持行程:" + filter.VoyageType.GetDescription()
                };
            }
            RequestResult<IEnumerable<ExternalPolicyView>> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var platform = new IBEService {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout
                };
                var pnrCode = filter.UseBPNR?pnrPair.BPNR:pnrPair.PNR;
                var pnrText = GetPnrParameter(pnrContent, patContent);
                var signValue = new Dictionary<string, string>
                                {
                                    {"userName", Platform.UserName},
                                    {"pnr", pnrCode},
                                    {"pnrText", pnrText}
                                };
                var signText = Sign(signValue);
                request = GetRequestValue(signValue, signText);
                response = platform.ParsePnrMatchAirp(Platform.UserName, pnrCode, pnrText, signText);
                result = parseMatchResponse(response, filter);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "通过编码内容匹配易行政策");
                result = new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "匹配政策");
            return result;
        }

        private RequestResult<IEnumerable<ExternalPolicyView>> parseMatchResponse(string matchResponse, ExternalPolicyFilter filter) {
            var result = new RequestResult<IEnumerable<ExternalPolicyView>>();
            var doc = new XmlDocument();
            doc.LoadXml(matchResponse);
            string errMessage;
            if(ResponseSuccess(doc, out errMessage)) {
                result.Success = true;
                result.Result = parsePolicies(doc, filter);
            } else {
                result.Success = false;
                result.ErrMessage = errMessage;
            }
            return result;
        }
        private IEnumerable<ExternalPolicyView> parsePolicies(XmlDocument doc, ExternalPolicyFilter filter) {
            var result = new List<ExternalPolicyView>();
            var policiesNode = doc.SelectSingleNode("/result/priceinfos");
            if(policiesNode != null) {
                var lowestRebate = filter.B3BMaxRebate + Platform.Setting.RebateBalance;
                var platformDeduct = Platform.Setting.Deduct;
                var provider = Platform.Setting.Provider;
                var autoPayTypes = Platform.Setting.PayInterface;
                foreach(XmlNode policyNode in policiesNode.ChildNodes) {
                    // 过滤掉特殊高返
                    // 是否特殊高返  0:不是  1:是
                    if(GetAttributeValue(policyNode, "isSphigh") == "1") continue;
                    // 是否支持我们现在的支付方式
                    var supportPayTypes = GetAttributeValue(policyNode, "payType").Select(p => Platform.GetPayInterface(p.ToString())).ToList();
                    if(supportPayTypes.Any() && !supportPayTypes.Any(autoPayTypes.Contains)) continue;

                    try {
                        var policy = new YeexingPolicyView {
                            Platform = Platform.PlatformInfo,
                            Provider = provider,
                            OriginalContent = policyNode.OuterXml,
                            OfficeNo = string.Empty,
                            RequireAuth = false,
                            PolicyType = null
                        };
                        // 客票类型 1:B2B  2:BSP
                        var ticketTypeValue = GetAttributeValue(policyNode, "tickType");
                        policy.TicketType = ticketTypeValue == "1"
                                                ? Common.Enums.TicketType.B2B
                                                : Common.Enums.TicketType.BSP;
                        // 客票类型过滤
                        if(filter.TicketType.HasValue && policy.TicketType != filter.TicketType.Value) {
                            continue;
                        }
                        // 返点
                        policy.Disc = GetAttributeValue(policyNode, "disc");
                        var rebate = decimal.Parse(policy.Disc) / 100;
                        if(rebate < lowestRebate) continue;
                        policy.OriginalRebate = rebate;
                        // 返现  由于没有返现了，暂时不处理该字段
                        policy.extReward = GetAttributeValue(policyNode, "extReward");
                        // 处理平台留点
                        if(rebate > platformDeduct) {
                            rebate -= platformDeduct;
                        } else {
                            rebate = 0;
                        }
                        policy.Rebate = rebate;

                        // 政策编号
                        policy.Id = GetAttributeValue(policyNode, "plcid");
                        // ibe价格
                        policy.IBEPrice = GetAttributeValue(policyNode, "ibePrice");
                        policy.ParValue = decimal.Parse(policy.IBEPrice);
                        // 是否需要换编码 0:不需要  1:需要
                        var requireChangePnrValue = GetAttributeValue(policyNode, "changePnr");
                        policy.RequireChangePNR = requireChangePnrValue == "1";
                        // 政策备注
                        policy.Condition = policy.Remark = GetAttributeValue(policyNode, "memo");
                        // 出票速度  HH分钟mm秒
                        var etdzSpeedValue = GetAttributeValue(policyNode, "outTime");
                        var etdzSpeed = 180;
                        var etdzSpeedMatch = Regex.Match(etdzSpeedValue, "((?<mm>\\d*)分钟)?(?<ss>\\d*)秒");
                        if(etdzSpeedMatch.Success) {
                            var mmValue = etdzSpeedMatch.Groups["mm"] == null
                                              ? string.Empty
                                              : etdzSpeedMatch.Groups["mm"].Value;
                            var mm = 0;
                            int.TryParse(mmValue, out mm);
                            var ss = 0;
                            int.TryParse(etdzSpeedMatch.Groups["ss"].Value, out ss);
                            if(mm * 60 + ss > 0) {
                                etdzSpeed = mm * 60 + ss;
                            }
                        }
                        policy.ETDZSpeed = etdzSpeed;
                        // 工作时间
                        var workTimeValue = DateTime.Today.IsWeekend()
                                                ? GetAttributeValue(policyNode, "restWorkTime")
                                                : GetAttributeValue(policyNode, "workTime");
                        Time workStartTime;
                        Time workEndTime;
                        parseWorkTime(workTimeValue,out workStartTime,out workEndTime);
                        policy.ScrapStart = policy.WorkStart = workStartTime;
                        policy.ScrapEnd = policy.WorkEnd = workEndTime;

                        Time workTimeStart;
                        Time workTimeEnd;
                        parseWorkTime(GetAttributeValue(policyNode, "workTime"), out workTimeStart,out workTimeEnd);
                        policy.WorkRefundTimeStart = policy.WorkTimeStart = workTimeStart;
                        policy.WorkRefundTimeEnd = policy.WorkTimeEnd = workTimeEnd;
                        Time restWorkTimeStart;
                        Time restWorkTimeEnd;
                        parseWorkTime(GetAttributeValue(policyNode, "restWorkTime"), out restWorkTimeStart, out restWorkTimeEnd);
                        policy.RestRefundTimeStart = policy.RestWorkTimeStart = restWorkTimeStart;
                        policy.RestRefundTimeEnd = policy.RestWorkTimeEnd = restWorkTimeEnd;


                        //Time workRefundTimeStart;
                        //Time workRefundTimeEnd;
                        //parseWorkTime(GetAttributeValue(policyNode, "workReturnTime"), out workRefundTimeStart, out workRefundTimeEnd);
                        //policy.WorkRefundTimeStart = workRefundTimeStart;
                        //policy.WorkRefundTimeEnd = workRefundTimeEnd;


                        //Time restRefundTimeStart;
                        //Time restRefundTimeEnd;
                        //parseWorkTime(GetAttributeValue(policyNode,"restReturnTime"),out restRefundTimeStart,out restRefundTimeEnd);
                        //policy.RestRefundTimeStart = restRefundTimeStart;
                        //policy.RestRefundTimeEnd = restRefundTimeEnd;

                        // 支持的支付方式
                        policy.PayInterfaces = (from char c in GetAttributeValue(policyNode, "payType")
                                                from pi in Platform.PayInterfaces
                                                where pi.Value == c.ToString()
                                                select pi.Key).ToList();
                        result.Add(policy);
                    } catch(Exception ex) {
                        LogService.SaveExceptionLog(ex, "解析易行政策数据");
                    }
                }
            }
            return result;
        }

        private static void parseWorkTime(string workTimeValue,out Time timeFrom,out Time timeTo)
        {
            YeexingPolicyView policy;
            var workTimeMatch = Regex.Match(workTimeValue, "(?<sHH>\\d{1,2}?):(?<smm>\\d{1,2}?)-(?<eHH>\\d{1,2}?):(?<emm>\\d{1,2}?)");
            if (workTimeMatch.Success)
            {
                var startHour = int.Parse(workTimeMatch.Groups["sHH"].Value);
                var startMin = int.Parse(workTimeMatch.Groups["smm"].Value);
                var endHour = int.Parse(workTimeMatch.Groups["eHH"].Value);
                var endMin = int.Parse(workTimeMatch.Groups["emm"].Value);
                if (endHour >= 24)
                {
                    endHour = 23;
                    endMin = 59;
                }
                timeFrom = new Izual.Time(startHour, startMin, 0);
                timeTo = new Izual.Time(endHour, endMin, 0);
            }
            else
            {
                timeFrom = new Izual.Time(8, 0, 0);
                timeTo = new Izual.Time(18, 59, 0);
            }
        }
    }
}