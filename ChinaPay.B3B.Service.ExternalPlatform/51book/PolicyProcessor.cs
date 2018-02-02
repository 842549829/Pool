using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.ExternalPlatform._51book {
    class PolicyProcessor : RequestProcessorBase, Processor.IPolicyProcessor {
        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, ExternalPolicyFilter filter) {
            throw new NotImplementedException();
        }

        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, string pnrContent, string patContent, ExternalPolicyFilter filter) {
            RequestResult<IEnumerable<ExternalPolicyView>> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                pnrContent = pnrContent.Trim().TrimEnd('>');
                patContent = patContent.Trim().TrimEnd('>');
                var requestModel = new _51bookMatchPolicy.getPolicyByPnrTxtRequest {
                    agencyCode = Platform.UserName,
                    pnrTxt = pnrContent,
                    pataTxt = patContent,
                    needSpeRulePolicy = 0,
                    needSpePricePolicySpecified = true,
                    needSpePricePolicy = 1,
                    needSpeRulePolicySpecified = true,
                    onlyOnWorking = 0,
                    onlyOnWorkingSpecified = true,
                    allowSwitchPnr = 1,
                    allowSwitchPnrSpecified = true
                };
                var signValue = new Dictionary<string, string>
                                {
                                    {"agencyCode", Platform.UserName},
                                    {"allowSwitchPnr", requestModel.allowSwitchPnr.ToString()},
                                    {"needSpePricePolicy", requestModel.needSpePricePolicy.ToString()},
                                    {"needSpeRulePolicy", requestModel.needSpeRulePolicy.ToString()},
                                    {"onlyOnWorking", requestModel.onlyOnWorking.ToString()}
                                };
                requestModel.sign = Sign(signValue);
                var platform = new _51bookMatchPolicy.GetPolicyByPnrTxtServiceImpl_1_0Service {
                    Url = Platform.Address_GetPolicyByPnrText,
                    Timeout = Platform.Timeout
                };
                request = GetModelString(requestModel);
                var responseModel = platform.getPolicyByPnrTxt(requestModel);
                response = GetModelString(responseModel);
                result = parseResponse(responseModel, filter);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "通过编码匹配51book政策");
                result = new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(request, response, "匹配政策");
            return result;
        }

        private RequestResult<IEnumerable<ExternalPolicyView>> parseResponse(_51bookMatchPolicy.getPolicyByPnrTxtReply response, ExternalPolicyFilter filter) {
            var result = new RequestResult<IEnumerable<ExternalPolicyView>>();
            if(response.returnCode == "S") {
                result.Success = true;
                result.Result = parsePolicies(response.policyList, filter);
            } else {
                result.Success = false;
                result.ErrMessage = response.returnMessage;
            }
            return result;
        }
        private IEnumerable<ExternalPolicyView> parsePolicies(IEnumerable<_51bookMatchPolicy.wsPolicyData> policyDatas, ExternalPolicyFilter filter) {
            var result = new List<ExternalPolicyView>();
            if(policyDatas != null) {
                var lowestRebate = filter.B3BMaxRebate + Platform.Setting.RebateBalance;
                var platformDeduct = Platform.Setting.Deduct;
                var provider = Platform.Setting.Provider;
                foreach(var item in policyDatas) {
                    // 返点过滤
                    var rebate = (decimal)item.commisionPoint;
                    if(rebate < lowestRebate) continue;
                    TicketType? ticketType = null;
                    if(item.policyType == "B2B") {
                        ticketType = TicketType.B2B;
                    } else if(item.policyType == "BSP") {
                        ticketType = TicketType.BSP;
                    }
                    // 过滤客票类型
                    if(filter.TicketType.HasValue) {
                        if(ticketType.HasValue) {
                            if(ticketType.Value != filter.TicketType.Value) {
                                continue;
                            }
                        } else {
                            ticketType = filter.TicketType;
                        }
                    }

                    try {
                        var policy = new ExternalPolicyView {
                            Platform = Platform.PlatformInfo,
                            Provider = provider,
                            OriginalContent = GetModelString(item),
                            Id = item.policyId.ToString(),
                            OriginalRebate = rebate
                        };
                        // 政策类型
                        if(item.productType == 1) {
                            policy.PolicyType = PolicyType.Normal;
                        } else if(item.productType == 2) {
                            policy.PolicyType = PolicyType.Bargain;
                        } else {
                            continue;
                        }
                        // 客票类型
                        policy.TicketType = ticketType.HasValue ? ticketType.Value : TicketType.BSP;
                        // 处理平台留点
                        policy.Rebate = rebate > platformDeduct ? rebate - platformDeduct : 0;
                        // 是否需要换编码
                        policy.RequireChangePNR = item.needSwitchPNR == 1;
                        // 工作时间
                        Izual.Time workStartTime;
                        Izual.Time workEndTime;
                        parseTimeZone(item.workTime, out workStartTime, out workEndTime);
                        policy.WorkStart = workStartTime;
                        policy.WorkEnd = workEndTime;   //TODO   没有看到文档或交互数据实例， 退废票时间暂未加载
                        // 废票时间
                        Izual.Time scrapStartTime;
                        Izual.Time scrapEndTime;
                        parseTimeZone(item.vtWorkTime, out scrapStartTime, out scrapEndTime);
                        policy.ScrapStart = scrapStartTime;
                        policy.ScrapEnd = scrapEndTime;
                        // 出票速度
                        var etdzSpeed = 180;
                        var etdzSpeedMatch = Regex.Match(item.ticketSpeed, "((?<mm>\\d*)分钟)?(?<ss>\\d*)秒?");
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
                        // 出票方Office号
                        policy.OfficeNo = item.supplyOfficeNo;
                        // 是否需要授权
                        policy.RequireAuth = !string.IsNullOrWhiteSpace(policy.OfficeNo);
                        // 政策备注
                        policy.Condition = policy.Remark = item.comment;
                        result.Add(policy);
                    } catch(Exception ex) {
                        LogService.SaveExceptionLog(ex, "解析51book政策数据");
                    }
                }
            }
            return result;
        }
        private bool parseTimeZone(string timeZoneText, out Izual.Time startTime, out Izual.Time endTime) {
            var scrapTimeMatch = Regex.Match(timeZoneText, "(?<sHH>\\d{1,2}?):(?<smm>\\d{1,2}?)-(?<eHH>\\d{1,2}?):(?<emm>\\d{1,2}?)");
            if(scrapTimeMatch.Success) {
                var startHour = int.Parse(scrapTimeMatch.Groups["sHH"].Value);
                var startMin = int.Parse(scrapTimeMatch.Groups["smm"].Value);
                var endHour = int.Parse(scrapTimeMatch.Groups["eHH"].Value);
                var endMin = int.Parse(scrapTimeMatch.Groups["emm"].Value);
                startTime = new Izual.Time(startHour, startMin, 0);
                endTime = new Izual.Time(endHour, endMin, 0);
                return true;
            } else {
                startTime = new Izual.Time(8, 0, 0);
                endTime = new Izual.Time(18, 59, 0);
                return false;
            }
        }
    }
}