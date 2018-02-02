using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Statistic;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Interface.Cache;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    internal class PNRImport2 : Processor
    {
        private string _pnrContext;
        private IssuedPNR _pnr;
        public PNRImport2(string userName, string sign)
            : base(userName, sign)
        {

        }
        public PNRImport2(string pnrContext, string userName, string sign)
            : base(userName, sign)
        {
//            pnrContext = @"
//>pn
// 1.路晓丝 2.闵书蝶 3.任夜南 4.孙元冬 JNYK42
// 5.  MU5818 Y   WE30JAN  SHAKMG HK4   1610 1940          E T2-
// 6.KMG/T KMG-T/0871-8886666/KMG RUYA TICKETS CO.LTD/YU ZHEN ABCDEFG
// 7.15110166432
// 8.BY OPT 820 2012/11/23 1711A
// 9.TL/1540/24NOV/KMG666
//10.SSR FOID MU HK1 NI152525199012025783/P4
//11.SSR FOID MU HK1 NI152525199012028140/P3
//12.SSR FOID MU HK1 NI152525199012027228/P2
//13.SSR FOID MU HK1 NI15252519901202318X/P1
//14.SSR ADTK 1E BY KMG25NOV12/1711 OR CXL MU5818 Y30JAN
//15.OSI MU CTCT 13800138000/P1
//16.OSI MU CTCT 13800138000/P2
//17.OSI MU CTCT 13800138000/P3
//18.OSI MU CTCT 13800138000/P4
//19.RMK TJ AUTH KMG215
//20.RMK CA/NE2PQB                                                               +
//21.RMK TLWBINSD                                                                -
//22.KMG666
//>
//
//                                      服务器耗时：0毫秒/0毫秒 网络传输总耗时：30毫秒
//>pat:a
//>PAT:A
//01 Y FARE:CNY1900.00 TAX:CNY50.00 YQ:CNY140.00  TOTAL:2090.00
//>SFC:01
//>";
            _pnrContext = ReserveViewConstuctor.GetPnrContent(pnrContext);
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_pnrContext)) throw new InterfaceInvokeException("8", "编码内容");
            _pnr = Parser.GetPNRDetail(_pnrContext);
            if (_pnr == null) throw new InterfaceInvokeException("1", "编码内容");
            if (DataTransferObject.Common.PNRPair.IsNullOrEmpty(_pnr.Code)) throw new InterfaceInvokeException("1", "内容中缺少编码");
            if (string.IsNullOrWhiteSpace(_pnr.Code.PNR) && string.IsNullOrWhiteSpace(_pnr.Code.BPNR)) throw new InterfaceInvokeException("1", "编码信息不全");
        }

        protected override string ExecuteCore()
        {
            var flights = ReserveViewConstuctor.GetQueryFlightView(_pnr.Segments, _pnr.ItineraryType, _pnr.Passenges.First().Value.Type, _pnr.IsTeam);
            var policyFilterCondition = GetPolicyFilter(flights);
            PolicyType policyType = queryPolicyType(flights);
            //匹配政策
            List<MatchedPolicy> matchedPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunk(policyFilterCondition, _pnr.Passenges.First().Value.Type, 10).ToList();
            List<MatchedPolicy> matchedSpeciafPolicies = null;
            if (policyType != PolicyType.Special)
            {
                policyFilterCondition = GetPolicyFilter(flights, PolicyType.Special);
                matchedSpeciafPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunk(policyFilterCondition, _pnr.Passenges.First().Value.Type, 10).ToList();
            }
            StringBuilder str = new StringBuilder();

            if (matchedPolicies == null)
                throw new InterfaceInvokeException("9", "没有找到相关政策");

            if (policyType != PolicyType.Special)
            {
                var list = from item in matchedPolicies
                           let generalPolicy = item.OriginalPolicy as IGeneralPolicy
                           let regulation = item.OriginalPolicy as IHasRegulation
                           select new
                           {
                               Id = item.Id,
                               Type = (int)item.PolicyType,
                               Fare = item.ParValue.TrimInvaidZero(),
                               Rebate = (item.Commission * 100).TrimInvaidZero(),
                               Commission = (item.ParValue - item.SettleAmount).TrimInvaidZero(),
                               Amount = item.SettleAmount,
                               Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                               EI = ReplaceEnter((regulation == null ? getEI(flights) : getProvision(regulation))),
                               OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                               Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                               Confirm = item.ConfirmResource,
                               generalPolicy.ChangePNR,
                               EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                               RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                               ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                               EtdzSpeed = item.Speed.ETDZ / 60,
                               RefundSpeed = item.Speed.Refund / 60
                           };
                str.Append("<policies>");
                foreach (var item in list)
                {
                    str.Append("<policy>");
                    str.AppendFormat("<id>{0}</id>", item.Id);
                    str.AppendFormat("<type>{0}</type>", item.Type);
                    str.AppendFormat("<fare>{0}</fare>", item.Fare);
                    str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                    str.AppendFormat("<commission>{0}</commission>", item.Commission);
                    str.AppendFormat("<amount>{0}</amount>", item.Amount);
                    str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                    str.AppendFormat("<ei>{0}</ei>", item.EI);
                    //需要授权的office号才有，否则为空字符
                    str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                    str.AppendFormat("<condition>{0}</condition>", item.Condition);
                    str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                    str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
                    str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                    str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                    str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                    str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                    str.Append("</policy>");
                }

                var queryList = from item in matchedSpeciafPolicies
                                where item != null && item.OriginalPolicy != null
                                let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                                let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                                select new
                                {
                                    Id = item.Id,
                                    Type = (int)item.PolicyType,
                                    Fare = item.ParValue.TrimInvaidZero(),
                                    Rebate = (item.Commission * 100).TrimInvaidZero(),
                                    Commission = (item.ParValue - item.SettleAmount).TrimInvaidZero(),
                                    Amount = item.SettleAmount,
                                    Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                                    EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                                    OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                                    Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                                    Confirm = item.ConfirmResource,
                                    ChangePNR = "",
                                    EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                                    EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
                                    RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60
                                };
                foreach (var item in queryList)
                {
                    str.Append("<policy>");
                    str.AppendFormat("<id>{0}</id>", item.Id);
                    str.AppendFormat("<type>{0}</type>", item.Type);
                    str.AppendFormat("<fare>{0}</fare>", item.Fare);
                    str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                    str.AppendFormat("<commission>{0}</commission>", item.Commission);
                    str.AppendFormat("<amount>{0}</amount>", item.Amount);
                    str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                    str.AppendFormat("<ei>{0}</ei>", item.EI);
                    //需要授权的office号才有，否则为空字符
                    str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                    str.AppendFormat("<condition>{0}</condition>", item.Condition);
                    str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                    str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
                    str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                    str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                    str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                    str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                    str.Append("</policy>");
                }
                foreach (var item in matchedSpeciafPolicies)
                {
                    if (!matchedPolicies.Contains(item))
                    {
                        matchedPolicies.Add(item);
                    }
                }
            }
            else
            {
                var queryList = from item in matchedPolicies
                                where item != null && item.OriginalPolicy != null
                                let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                                let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                                select new
                                {
                                    Id = item.Id,
                                    Type = (int)item.PolicyType,
                                    Fare = item.ParValue.TrimInvaidZero(),
                                    Rebate = (item.Commission * 100).TrimInvaidZero(),
                                    Commission = 0,
                                    Amount = item.SettleAmount,
                                    Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                                    EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                                    OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                                    Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                                    Confirm = item.ConfirmResource,
                                    ChangePNR = "",
                                    EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                                    EtdzSpeed = item.Speed.ETDZ / 60,
                                    RefundSpeed = item.Speed.Refund / 60
                                };
                foreach (var item in queryList)
                {
                    str.Append("<policy>");
                    str.AppendFormat("<id>{0}</id>", item.Id);
                    str.AppendFormat("<type>{0}</type>", item.Type);
                    str.AppendFormat("<fare>{0}</fare>", item.Fare);
                    str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                    str.AppendFormat("<commission>{0}</commission>", item.Commission);
                    str.AppendFormat("<amount>{0}</amount>", item.Amount);
                    str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                    str.AppendFormat("<ei>{0}</ei>", item.EI);
                    //需要授权的office号才有，否则为空字符
                    str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                    str.AppendFormat("<condition>{0}</condition>", item.Condition);
                    str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                    str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
                    str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                    str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                    str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                    str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                    str.Append("</policy>");
                }
            }
            str.Append("</policies>");
            //将匹配出来的政策存入缓存中 
            CustomContext context = CustomContext.NewContext();
            context[_pnr.Code.BPNR + _pnr.Code.PNR] = matchedPolicies;
            ContextCenter.Instance.Save(context);
            str.AppendFormat("<key>{0}</key>", context.Id.ToString());
            return str.ToString();
        }

        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            PolicyType policyType = queryPolicyType(flights);
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = policyType,
                Purchaser = Company.CompanyId
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.ItineraryType == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.ItineraryType == ItineraryType.Notch ? VoyageType.Notch : (_pnr.ItineraryType == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.SuitReduce = _pnr.Price == null ? false : (_pnr.Price != null && flights.Count() == 2 ? _pnr.Price.Fare < flights.Sum(item => item.Fare) : false);
            policyFilterCondition.PatPrice = _pnr.Price == null ? (decimal?)null : _pnr.Price.Fare;
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = true;
            return policyFilterCondition;
        }
        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights, PolicyType type)
        {
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = type,
                Purchaser = Company.CompanyId
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.ItineraryType == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.ItineraryType == ItineraryType.Notch ? VoyageType.Notch : (_pnr.ItineraryType == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.SuitReduce = _pnr.Price == null ? false : (_pnr.Price != null && flights.Count() == 2 ? _pnr.Price.Fare < flights.Sum(item => item.Fare) : false);
            policyFilterCondition.PatPrice = _pnr.Price == null ? (decimal?)null : _pnr.Price.Fare;
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = true;
            return policyFilterCondition;
        }

        private PolicyType queryPolicyType(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            PolicyType policyType = PolicyType.Bargain;
            if (_pnr.IsTeam)
            {
                policyType = PolicyType.Team;
            }
            // 根据舱位的类型来决定政策类型
            if (flights.Any(f => f.BunkType == BunkType.Promotion || f.BunkType == BunkType.Production || f.BunkType == BunkType.Transfer))
            {
                policyType = PolicyType.Bargain;
            }
            else
            {
                switch (flights.First().BunkType)
                {
                    case BunkType.Economic:
                    case BunkType.FirstOrBusiness:
                        policyType = PolicyType.Normal | PolicyType.Bargain;
                        break;
                    case BunkType.Promotion:
                    case BunkType.Production:
                    case BunkType.Transfer:
                        policyType = PolicyType.Bargain;
                        break;
                    default:
                        policyType = PolicyType.Special;
                        break;
                }
            }
            return policyType;
        }
        private IEnumerable<DataTransferObject.FlightQuery.FlightView> processFlights(string source, PassengerType passengerType, IEnumerable<DataTransferObject.FlightQuery.FlightView> originalFlights)
        {
            IEnumerable<DataTransferObject.FlightQuery.FlightView> flights = null;
            if (passengerType == PassengerType.Child)
            {
                var airline = Service.FoundationService.QueryAirline(originalFlights.First().AirlineCode);
                foreach (var item in originalFlights)
                {
                    var childrenBunks = airline.GetChildOrderableBunks(item.Departure.Code, item.Arrival.Code, item.Departure.Time.Date);
                    var bunk = childrenBunks.First(b => b.Code.Value == item.BunkCode.ToUpper());
                    item.Discount = bunk.Discount;
                    item.Fare = Utility.Calculator.Round(item.YBPrice * item.Discount.Value, 1);
                    item.AirportFee = 0M;
                    item.BAF = item.ChildBAF;
                }
                flights = originalFlights;
            }
            return flights;
        }
        private IEnumerable<Service.PolicyMatch.Domain.VoyageFilterInfo> getVoyageFilterInfos(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            return (from item in flights
                    select new Service.PolicyMatch.Domain.VoyageFilterInfo
                    {
                        Flight = new Service.PolicyMatch.Domain.FlightFilterInfo
                        {
                            Airline = item.AirlineCode,
                            Departure = item.Departure.Code,
                            Arrival = item.Arrival.Code,
                            FlightDate = item.Departure.Time.Date,
                            FlightNumber = item.FlightNo,
                            StandardPrice = item.YBPrice,
                            IsShare = item.IsShare
                        },
                        Bunk = item.BunkType.HasValue ? new Service.PolicyMatch.Domain.BunkFilterInfo
                        {
                            Code = item.BunkCode,
                            Discount = item.Discount.HasValue ? item.Discount.Value : 0,
                            Type = item.BunkType.Value
                        } : null
                    }).ToList();
        }

        //public Dictionary<string, List<MatchedPolicy>> MatchedPolicyCache
        //{
        //    get
        //    {
        //        if (_context.Session["InterfaceMatchedPolicy"] != null)
        //        {
        //            return _context.Session["InterfaceMatchedPolicy"] as Dictionary<string, List<MatchedPolicy>>;
        //        }
        //        return new Dictionary<string, List<MatchedPolicy>>();
        //    }
        //    set
        //    {
        //        _context.Session["InterfaceMatchedPolicy"] = value;
        //    }
        //}

        private string getTimeRange(Izual.Time start, Izual.Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }

        private static string ReplaceEnter(string input)
        {
            string result = input.Replace("\n", "<br />").Replace("\"", string.Empty).Replace("'", string.Empty);
            return result.Replace("\r", "");
        }
        private object getProvisionList(IHasRegulation regulation)
        {
            if (regulation == null) return new[] { new { key = string.Empty, value = string.Empty } };
            return new[]
            {
                new {key="更改规定" ,value= regulation.ChangeRegulation},
                new {key="作废规定",value= regulation.InvalidRegulation},
                new {key="退票规定" ,value= regulation.RefundRegulation},
                new {key="签转规定" ,value= regulation.EndorseRegulation}
            };
        }

        private string getEI(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            Dictionary<string, string> RenderedEI = new Dictionary<string, string>();
            var htmlReg = new Regex("<.+?>");
            var voyages = flights;
            var flightView = voyages.ElementAt(0);
            string ei = "(" + flightView.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
            RenderedEI.Add(flightView.BunkCode, flightView.EI);
            foreach (var item in voyages)
            {
                if (!RenderedEI.Any(b => b.Key == item.BunkCode && b.Value == item.EI))
                {
                    ei += "|" + "(" + item.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
                    RenderedEI.Add(item.BunkCode, item.EI);
                }
            }
            return ei;
        }
        private string getProvision(IHasRegulation regulation)
        {
            if (regulation == null) return string.Empty;
            return "更改规定：" + regulation.ChangeRegulation + "|作废规定：" + regulation.InvalidRegulation +
                   "|退票规定：" + regulation.RefundRegulation + "|签转规定：" + regulation.EndorseRegulation;
        }
    }
}