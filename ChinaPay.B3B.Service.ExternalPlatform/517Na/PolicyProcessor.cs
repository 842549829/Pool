using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Core.Extension;
using Izual;

namespace ChinaPay.B3B.Service.ExternalPlatform._517Na {
    class PolicyProcessor : ProcessorBase, Processor.IPolicyProcessor {
        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, ExternalPolicyFilter filter) {
            throw new NotImplementedException();
        }

        public RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, string pnrContent, string patContent, ExternalPolicyFilter filter) {
            if(filter.VoyageType != ItineraryType.OneWay && filter.VoyageType != ItineraryType.Roundtrip) {
                return new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "不支持行程:" + filter.VoyageType.GetDescription()
                };
            }
            RequestResult<IEnumerable<ExternalPolicyView>> result;
            var request = string.Empty;
            var response = string.Empty;
            try {
                var signValue = new Dictionary<string, string>
                                {
                                    {"pnrcontent", GetPnrParameter(pnrContent)},
                                    {"patcontent", GetPnrParameter(patContent)}
                                };
                request = GetRequest("get_benefit_pnrcontent", signValue);
                var platform = new _517Na.BenefitInterface {
                    Url = Platform.Address,
                    Timeout = Platform.Timeout,
                    RequestEncoding = Platform.Encoding
                };
                response = platform.InterfaceFacade(request);
                result = parseMatchResponse(response, filter);
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "通过编码内容匹配517Na政策");
                result = new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "系统错误，请联系平台"
                };
                response = ex.Message;
            }
            SaveRequestLog(response, request, "匹配政策");
            return result;
        }

        private RequestResult<IEnumerable<ExternalPolicyView>> parseMatchResponse(string response, ExternalPolicyFilter filter) {
            var result = new RequestResult<IEnumerable<ExternalPolicyView>>();
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string errorMessage;
            if(ResponseSuccess(doc, out errorMessage)) {
                result.Success = true;
                result.Result = parsePolicies(doc.SelectNodes("/benefit/item"), filter);
            } else {
                result.Success = false;
                result.ErrMessage = errorMessage;
            }
            return result;
        }
        private IEnumerable<ExternalPolicyView> parsePolicies(XmlNodeList policyNodes, ExternalPolicyFilter filter) {
            var result = new List<ExternalPolicyView>();
            if(policyNodes != null) {
                var lowestRebate = filter.B3BMaxRebate + Platform.Setting.RebateBalance;
                var platformDeduct = Platform.Setting.Deduct;
                var provider = Platform.Setting.Provider;
                foreach(XmlNode node in policyNodes) {
                    var dataArray = node.InnerText.Split('^');
                    if(dataArray.Length != 28) continue;

                    var isHighRebatePolicy = dataArray[16];
                    // 过滤特殊高返政策
                    if(isHighRebatePolicy == "1") continue;
                    var rebateText = dataArray[9];
                    var originalRebate = decimal.Parse(rebateText) / 100;
                    // 根据政策差，过滤没有优势的政策
                    if(originalRebate < lowestRebate) continue;
                    var ticketTypeText = dataArray[8];
                    var ticketType = ticketTypeText == "2" ? TicketType.B2B : TicketType.BSP;
                    // 过滤客票类型
                    if(filter.TicketType.HasValue && filter.TicketType.Value != ticketType) continue;

                    var policy = new _517NaPolicyView {
                        Platform = Platform.PlatformInfo,
                        Provider = provider,
                        OriginalContent = node.OuterXml,
                        Id = dataArray[0],
                        TicketType = ticketType,
                        OriginalRebate = originalRebate,
                        Rebate = originalRebate > platformDeduct ? originalRebate - platformDeduct : 0,
                        PolicyType = null,
                        Condition = dataArray[11],
                        SubId = dataArray[18]
                    };
                    policy.Remark = policy.Condition;
                    policy.RequireChangePNR = dataArray[17] == "True";
                    policy.OfficeNo = dataArray[27];
                    policy.RequireAuth = !string.IsNullOrWhiteSpace(policy.OfficeNo);
                    // 票面价
                    var fareText = dataArray[20];
                    policy.ParValue = decimal.Parse(fareText);
                    // 出票速度
                    var etdzSpeedText = dataArray[25];
                    int etdzSpeed;
                    if(!int.TryParse(etdzSpeedText, out etdzSpeed)) {
                        etdzSpeed = 180;
                    }
                    policy.ETDZSpeed = etdzSpeed;
                    // 工作时间
                    var workTimeText = DateTime.Today.IsWeekend() ? dataArray[15] : dataArray[14];
                    Izual.Time workStartTime;
                    Izual.Time workEndTime;
                    parseTimeZone(workTimeText, out workStartTime, out workEndTime);
                    policy.WorkStart = workStartTime;
                    policy.WorkEnd = workEndTime;
                    Time workTimeStart;
                    Time workTimeEnd;
                    parseTimeZone(dataArray[14], out workTimeStart, out workTimeEnd);
                    policy.WorkTimeStart = workTimeStart;
                    policy.WorkTimeEnd = workTimeEnd;
                    Time restWorkTimeStart;
                    Time restWorkTimeEnd;
                    parseTimeZone(dataArray[15], out restWorkTimeStart, out restWorkTimeEnd);
                    policy.RestWorkTimeStart = restWorkTimeStart;
                    policy.RestWorkTimeEnd = restWorkTimeEnd;


                    // 废票时间
                    var scrapTimeText = DateTime.Today.IsWeekend() ? dataArray[24] : dataArray[23];
                    Izual.Time scrapStartTime;
                    Izual.Time scrapEndTime;
                    parseTimeZone(scrapTimeText, out scrapStartTime, out scrapEndTime);
                    policy.ScrapStart = scrapStartTime;
                    policy.ScrapEnd = scrapEndTime;

                    Time workRefundTimeStart;
                    Time workRefundTimeEnd;
                    parseTimeZone(dataArray[23], out workRefundTimeStart, out workRefundTimeEnd);
                    policy.WorkRefundTimeStart = workRefundTimeStart;
                    policy.WorkRefundTimeEnd = workRefundTimeEnd;

                    Time restRefundTimeStart;
                    Time restRefundTimeEnd;
                    parseTimeZone(dataArray[24], out restRefundTimeStart, out restRefundTimeEnd);
                    policy.RestRefundTimeStart = restRefundTimeStart;
                    policy.RestRefundTimeEnd = restRefundTimeEnd;

                    result.Add(policy);
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