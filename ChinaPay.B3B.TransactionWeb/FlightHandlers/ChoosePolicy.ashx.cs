using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.DataTransferObject.SystemManagement;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.Service.PolicyMatch.Domain;
using ChinaPay.B3B.Service.Remind;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.TransactionWeb.FlightReserveModule;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Flight = ChinaPay.B3B.Service.FlightQuery.Domain.Flight;
using FlightView = ChinaPay.B3B.DataTransferObject.FlightQuery.FlightView;
using PriceView = ChinaPay.B3B.DataTransferObject.Command.PNR.PriceView;
using Time = Izual.Time;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers
{
    /// <summary>
    /// 政策选择处理类
    /// </summary>
    public class ChoosePolicy : BaseHandler
    {
        private readonly List<string> hadPrice = new List<string>();

        /// <summary>
        /// 用户缓存匹配到的政策
        /// </summary>
        protected List<MatchedPolicy> MatchedPolicyCache
        {
            get
            {
                if (Session["MatchedPolicy"] != null)
                {
                    return Session["MatchedPolicy"] as List<MatchedPolicy>;
                }
                return new List<MatchedPolicy>();
            }
            set
            {
                if (Session["MatchedPolicy"] == null)
                {
                    Session["MatchedPolicy"] = value;
                }
                else
                {
                    if (value.Count == 0) return;
                    var cache = Session["MatchedPolicy"] as List<MatchedPolicy>;
                    foreach (MatchedPolicy policy in value)
                    {
                        if (cache.Any(p => p.Id == policy.Id))
                        {
                            cache.RemoveAll(p => p.Id == policy.Id);
                        }
                        cache.Add(policy);
                    }
                    Session["MatchedPolicy"] = cache;
                }
            }
        }


        public static AllowTicketType FilterByTime(DateTime takeOffTime)
        {
            var minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2 * 60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
        }


        /// <summary>
        /// 查询政策
        /// </summary>
        public object QueryPolicies(PolicyType policyType, int policyCount, string source, string policyOwner, bool needSubsidize, bool IsUsePatPrice)
        {
            return QueryAllPolicies(policyType, policyCount, source, policyOwner, needSubsidize, IsUsePatPrice, false);
        }

        /// <summary>
        /// 查询外平台政策
        /// </summary>
        /// <param name="policyType"></param>
        /// <param name="policyCount"></param>
        /// <param name="source"></param>
        /// <param name="policyOwner"></param>
        /// <param name="needSubsidize"></param>
        /// <param name="IsUsePatPrice"></param>
        /// <param name="maxdRebate"> </param>
        /// <returns></returns>
        public object QueryExternalPolicy(PolicyType policyType, int policyCount, string source, string policyOwner, bool needSubsidize, bool IsUsePatPrice, decimal maxdRebate)
        {
            return QueryAllPolicies(policyType, policyCount, source, policyOwner, needSubsidize, IsUsePatPrice, true, maxdRebate);
        }

        private object QueryAllPolicies(PolicyType policyType, int policyCount, string source, string policyOwner, bool needSubsidize, bool IsUsePatPrice, bool isExternalPolicy, decimal maxdRebate = 0)
        {
            PassengerType passengerType = getPassengerType(source);
            var policyFilterCondition = new PolicyFilterConditions
                {
                    PolicyType = policyType,
                    Purchaser = CurrentCompany.CompanyId
                };
            var orderView = Session["OrderView"] as OrderView;
            // 升舱时，指向原订单出票方
            if (FlightReserveModule.ChoosePolicy.UpgradeByPNRCodeSource == source || FlightReserveModule.ChoosePolicy.UpgradeByQueryFlightSource == source)
            {
                policyFilterCondition.Provider = Guid.Parse(policyOwner);
            }
            else if (FlightReserveModule.ChoosePolicy.ChangeProviderSource == source)
            {
                // 换出票方时，排除原订单出票方、产品方 和 采购方
                Order order = FlightReserveModule.ChoosePolicy.GetOriginalOrder(source);
                policyFilterCondition.ExcludeProviders.Add(order.Purchaser.CompanyId);
                if (order.Supplier != null)
                {
                    policyFilterCondition.ExcludeProviders.Add(order.Supplier.CompanyId);
                }
                if (order.Provider != null)
                {
                    policyFilterCondition.ExcludeProviders.Add(order.Provider.CompanyId);
                }
            }
            if (FlightReserveModule.ChoosePolicy.ImportSource == source)
            {
                policyFilterCondition.PatContent = orderView.PatContent;
                policyFilterCondition.PnrContent = orderView.PnrContent;
                policyFilterCondition.PnrPair = orderView.PNR;
            }
            IEnumerable<VoyageFilterInfo> voyages = getVoyageFilterInfos(source);
            // 特殊票时，只取航班查询处选择的价格
            if (policyType == PolicyType.Special)
            {
                PolicyView policyView = FlightReserveModule.ChoosePolicy.GetPolicyView(source);
                var firstFlight = voyages.FirstOrDefault();
                if (firstFlight != null && firstFlight.Bunk==null)
                {
                    policyFilterCondition.PatPrice = getPatPrice(source);
                }
                else if (policyView != null && firstFlight !=null && firstFlight.Bunk == null)
                {
                    policyFilterCondition.PublishFare = policyView.PublishFare;
                }
            }
            else
            {
                policyFilterCondition.PatPrice = getPatPrice(source);
            }
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = FlightReserveModule.ChoosePolicy.GetVoyageType(source);
            policyFilterCondition.SuitReduce = hasReduce(source);
            policyFilterCondition.NeedSubsidize = needSubsidize;
            policyFilterCondition.IsUsePatPrice = IsUsePatPrice || orderView.FdSuccess;
            policyFilterCondition.AllowTicketType = FilterByTime(voyages.Min(f => f.Flight.TakeOffTime));
            policyFilterCondition.MaxdRebate = maxdRebate;
            IEnumerable<MatchedPolicy> matchedPolicies = null;

            if (FlightReserveModule.ChoosePolicy.ChangeProviderSource == source && FlightReserveModule.ChoosePolicy.GetOriginalOrder(source).IsSpecial)
            {
                matchedPolicies = PolicyMatchServcie.MatchBunkForSpecial(policyFilterCondition, isExternalPolicy, policyCount).ToList();
            }
            else
            {
                matchedPolicies = PolicyMatchServcie.MatchBunk(policyFilterCondition, isExternalPolicy, passengerType, policyCount).ToList();
            }
            MatchedPolicyCache = matchedPolicies.ToList();
            if (policyType == PolicyType.Special)
            {
                return from item in matchedPolicies
                       where item != null && item.OriginalPolicy != null
                       let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                       let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                       select new
                           {
                               PolicyId = item.Id,
                               PolicyDesc = ReplaceEnter(specialPolicyInfo.Description),
                               spType = ReplaceEnter(specialPolicyInfo.Name),
                               specialPolicy = ReplaceEnter(specialPolicy.Type.ToString()),
                               PolicyOwner = item.Provider,
                               PolicyType = (int)PolicyType.Special,
                               Fare = item.ParValue.TrimInvaidZero(),
                               item.SettleAmount,
                               EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                               EIList = getProvisionList(item.OriginalPolicy as IHasRegulation),
                               Condition = ReplaceEnter(item.OriginalPolicy.Condition ?? "无"),
                               SuccessOrderCount = item.Statistics.Total.SuccessTicketCount,
                               WorkingTime = getTimeRange(item.WorkStart, item.WorkEnd),
                               VoyageSuccessOrderCount = item.Statistics.Voyage.SuccessTicketCount,
                               OrderSuccessRate = (item.Statistics.Total.OrderSuccessRate * 100).TrimInvaidZero() + "%",
                               item.NeedAUTH,
                               gradeFirst = Math.Floor(item.CompannyGrade),
                               gradeSecond = item.CompannyGrade / 0.1m % 10,
                               needApplication = specialPolicy.ConfirmResource,
                               WarnInfo =
                           (specialPolicy.Type == SpecialProductType.CostFree && !specialPolicy.IsSeat)
                               ? "需要候补<br /><a class='tips_btn standby_ticket'>什么是候补票？</a>"
                               : specialPolicy.ConfirmResource ? "需要申请<br /><a class='tips_btn'>什么是申请？</a>" : String.Empty,
                               RenderTicketPrice = specialPolicy.Type == SpecialProductType.CostFree || item.ParValue != 0,
                               PolicyTypes = item.PolicyType.GetDescription(),
                               IsFreeTicket = specialPolicy.Type == SpecialProductType.CostFree,
                               IsNOSeat = specialPolicy.Type == SpecialProductType.CostFree && !specialPolicy.IsSeat,
                               RelationType = (int)item.RelationType
                           };
            }
            return from item in matchedPolicies
                   let generalPolicy = item.OriginalPolicy as IGeneralPolicy
                   let regulation = item.OriginalPolicy as IHasRegulation
                   select new
                       {
                           Fare = item.ParValue.TrimInvaidZero(),
                           Rebate = (item.Commission * 100).TrimInvaidZero() + "%",
                           dRebate = item.Commission,
                           Commission = (item.ParValue - item.SettleAmount).TrimInvaidZero(),
                           SettleAmount = item.SettleAmount.TrimInvaidZero(),
                           WorkingTime = getTimeRange(item.WorkStart, item.WorkEnd),
                           ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                           ETDZEfficiency = (item.Speed.ETDZ / 60) + "分钟",
                           RefundEfficiency = (item.Speed.Refund / 60) + "分钟",
                           TicketType = (item.OriginalPolicy == null ? (item.OriginalExternalPolicy == null ? TicketType.BSP : item.OriginalExternalPolicy.TicketType) : item.OriginalPolicy.TicketType).ToString(),
                           PolicyId = item.Id,
                           PolicyOwner = item.Provider,
                           PolicyType = (int)item.PolicyType,
                           OfficeNo = item.OriginalPolicy == null ? item.OfficeNumber : item.OriginalPolicy.OfficeCode,
                           EI = ReplaceEnter((regulation == null ? getEI(source) : getProvision(regulation))),
                           EIList = getProvisionList(item.OriginalPolicy as IHasRegulation),
                           Condition =
                       (item.OriginalPolicy == null && item.OriginalExternalPolicy == null
                            ? "无"
                            : ReplaceEnter(item.OriginalPolicy != null ? item.OriginalPolicy.Condition : item.OriginalExternalPolicy.Condition) ?? "无")
                       + ((generalPolicy != null && generalPolicy.ChangePNR)||(item.IsExternal&&item.OriginalExternalPolicy.RequireChangePNR) ? "。需要换编码出票" : String.Empty),
                           NeedAUTH = item.OriginalPolicy == null ? item.NeedAUTH : item.OriginalPolicy.NeedAUTH,
                           PolicyTypes = item.PolicyType.GetDescription(),
                           IsBusy = OrderRemindService.QueryProviderRemindInfo(item.Provider).ETDZ > 5,
                           item.HasSubsidized,
                           RelationType = (int)item.RelationType,
                           setChangePNREnable = !item.IsExternal && (generalPolicy==null||!generalPolicy.ChangePNR)    //采购是否能设置是否允许换编码
                       };
        }

        public static string ReplaceEnter(string input)
        {
            string result = input.Replace("\n", "<br/>").Replace("\"", String.Empty).Replace("'", String.Empty);
            ;
            return result.Replace("\r", "");
        }

        private string getTimeRange(Time start, Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        public string ProduceOrder(Guid policyId, PolicyType policyType, Guid publisher, string officeNo, string source, int choise, bool needAUTH, bool HasSubsidized,
            bool IsUsePatPrice,bool forbidChnagePNR)
        {
            var orderView = Session["OrderView"] as OrderView;
            var flights = Session["ReservedFlights"] as IEnumerable<FlightView>;
            MatchedPolicy matchedPolicy = MatchedPolicyCache.FirstOrDefault(p => p.Id == policyId);
            if (matchedPolicy == null) throw new CustomException("政策选择超时");
            if (flights.First().BunkType != null && orderView.Source == OrderSource.PlatformOrder&&
                (flights.First().BunkType == BunkType.Free || matchedPolicy.OriginalPolicy is SpecialPolicyInfo && ((SpecialPolicyInfo)matchedPolicy.OriginalPolicy).Type==SpecialProductType.LowToHigh))
            {
                SpecialPolicy policy = PolicyManageService.GetSpecialPolicy(policyId);
                //低打高返和集团票性质一样 不需要去订坐 2013-4-3 wangsl
                //if (policy != null && (policy.SynBlackScreen||policy.Type==SpecialProductType.LowToHigh))
                    if (policy != null && policy.SynBlackScreen)
                {
                    PNRPair pnr = PNRHelper.ReserveSeat(flights, orderView.Passengers);
                    orderView.PNR = pnr;
                }
            }
            Order order = OrderProcessService.ProduceOrder(orderView, matchedPolicy, CurrentUser,BasePage.OwnerOEMId, forbidChnagePNR, (AuthenticationChoise)choise);
            FlightQuery.ClearFlightQuerySessions();
            if (order.Source == OrderSource.PlatformOrder && !PNRPair.IsNullOrEmpty(order.ReservationPNR) && !String.IsNullOrWhiteSpace(order.Product.OfficeNo))
                if (needAUTH) authorize(order.ReservationPNR, officeNo, source, BasePage.OwnerOEMId);
            return order.Id.ToString();
        }

        /// <summary>
        /// 生成申请单
        /// </summary>
        public string ProduceApplyform(Guid policyId, PolicyType policyType, string officeNo, decimal orderId, string source, int choise, bool needAUTH, bool IsUsePatPrice)
        {
            var applyformView = Session["ApplyformView"] as UpgradeApplyformView;
            MatchedPolicy matchedPolicy = MatchedPolicyCache.FirstOrDefault(p => p.Id == policyId);
            if (matchedPolicy == null) throw new CustomException("政策选择超时");
            decimal newOrderId = OrderProcessService.Apply(orderId, applyformView, matchedPolicy, CurrentUser, BasePage.OwnerOEMId);
            FlightQuery.ClearFlightQuerySessions();
            if (needAUTH) authorize(applyformView.NewPNR, officeNo, source, BasePage.OwnerOEMId);
            BasePage.ReleaseLock(orderId);
            return newOrderId.ToString();
        }

        public object QueryRecommand(string departure, string arrival, DateTime flightDate, decimal currentPrice)
        {
            DateTime prev = flightDate.AddDays(-1).Date;
            DateTime current = flightDate.Date;
            DateTime next = flightDate.AddDays(1).Date;
            var dateItems = new List<DateTime>();
            if (prev > DateTime.Today) dateItems.Add(prev);
            dateItems.Add(current);
            dateItems.Add(next);

            Dictionary<DateTime, IEnumerable<Flight>> historyFlights = FlightQueryService.QueryFlightFromHistory(new UpperString(departure), new UpperString(arrival), dateItems);
            if (currentPrice <= 0)
            {
                FlightView flight = FlightReserveModule.ChoosePolicy.GetFlights(FlightReserveModule.ChoosePolicy.ImportSource).First();
                if (flight.BunkType == BunkType.Economic || flight.BunkType == BunkType.FirstOrBusiness)
                {
                    currentPrice = flight.Fare;
                }
                else
                {
                    PriceView patPrice = FlightReserveModule.ChoosePolicy.GetPATPrice(FlightReserveModule.ChoosePolicy.ImportSource);
                    if (patPrice != null) currentPrice = patPrice.Fare;
                }
                if (currentPrice <= 0)
                {
                    return new
                        {
                            Today = new object[0],
                            Yestoday = new object[0],
                            Tomorrow = new object[0]
                        };
                }
            }
            Dictionary<DateTime, IEnumerable<InstructionalFlight>> matchedFlight = PolicyMatchServcie.MatchInstructionalFlights(historyFlights, currentPrice,
                CurrentCompany.CompanyId, policys => MatchedPolicyCache = policys.ToList());
            for (int i = 0; i < dateItems.Count; i++)
            {
                FliterRepeater(matchedFlight.ElementAt(i));
            }
            return new
                {
                    Today = matchedFlight[current].Select(RecommandSelector),
                    Yestoday = matchedFlight.ContainsKey(prev) ? matchedFlight[prev].Select(RecommandSelector) : null,
                    Tomorrow = matchedFlight[next].Select(RecommandSelector)
                };
        }

        public string SaveNewFlight(string departure, string arrival, string takeoffTime, string landTime,
            DateTime flightDate, string carrierCode, string carrier
            , string flightNo, string airCarft, decimal YBPrice, decimal airportFee, decimal BAF, decimal discount,
            string Bank, decimal settleAmount, int seatCount, decimal AdultBAF, decimal ChildBAF, string DepartureName,
            string DepartureCity, string DepartureTerminal, string ArrivalName, string ArrivalCity, string ArrivalTerminal,
            Guid policyId, PolicyType policyType, Guid publisher, string officeNo, string source, int choise, bool needAUTH
            )
        {
            var flight = new List<FlightView>();
            var timeReg = new Regex("(\\d{1,2}):(\\d{1,2})", RegexOptions.Compiled);
            Match match1 = timeReg.Match(takeoffTime);
            Match match2 = timeReg.Match(landTime);
            flight.Add(new FlightView
                {
                    Serial = 1,
                    AirlineCode = carrierCode,
                    AirlineName = carrier,
                    Departure = new AirportView
                        {
                            Code = departure,
                            Name = DepartureName,
                            City = DepartureCity,
                            Terminal = DepartureTerminal,
                            Time = flightDate.AddHours(Int32.Parse(match1.Groups[1].Value)).AddMinutes(Int32.Parse(match1.Groups[2].Value))
                        },
                    Arrival = new AirportView
                        {
                            Code = arrival,
                            Name = ArrivalName,
                            City = ArrivalCity,
                            Terminal = ArrivalTerminal,
                            Time = flightDate.AddHours(Int32.Parse(match2.Groups[1].Value)).AddMinutes(Int32.Parse(match2.Groups[2].Value))
                        },
                    FlightNo = flightNo,
                    Aircraft = airCarft,
                    YBPrice = YBPrice,
                    AirportFee = airportFee,
                    BAF = BAF,
                    AdultBAF = AdultBAF,
                    ChildBAF = ChildBAF,
                    BunkCode = Bank,
                    SeatCount = seatCount,
                    Fare = YBPrice,
                    SettleAmount = settleAmount
                });

            Session["ReservedFlights"] = flight;
            var orderView = Session["OrderView"] as OrderView;
            if (orderView != null)
            {
                orderView.PNR = new PNRPair(String.Empty, String.Empty);
                orderView.Flights = flight.Select(view => new DataTransferObject.Order.FlightView
                    {
                        SerialNo = view.Serial,
                        Departure = view.Departure.Code,
                        Arrival = view.Arrival.Code,
                        TakeoffTime = view.Departure.Time,
                        LandingTime = view.Arrival.Time,
                        Airline = view.AirlineCode,
                        FlightNo = view.FlightNo,
                        YBPrice = view.YBPrice,
                        AirCraft = view.Aircraft,
                        Bunk = view.BunkCode,
                        Fare = view.Fare,
                        IsShare = view.IsShare,
                        ArrivalTerminal = view.Arrival.Terminal,
                        DepartureTerminal = view.Departure.Terminal
                    });
                Session["OrderView"] = orderView;
            }
            return ProduceOrder(policyId, policyType, publisher, officeNo, source, choise, needAUTH, false, false,false);
        }

        public object ExistsPNR()
        {
            var orderView = Session["OrderView"] as OrderView;
            if(orderView==null) return new {HasPNR=false,OrderId=0};
            var orderId = OrderQueryService.ExistsPNR(orderView.PNR, DateTime.Now.AddDays(-1), DateTime.Now,CurrentCompany.CompanyId);
            if (orderId==0)
            {
                return new
                {
                    HasPNR = false,
                    OrderId = 0
                };
            }
            else
            {
                return new
                {
                    HasPNR = true,
                    OrderId = orderId
                };

            }

        }

        private void FliterRepeater(KeyValuePair<DateTime, IEnumerable<InstructionalFlight>> flight)
        {
            foreach (InstructionalFlight item in flight.Value)
            {
                string indexer = flight.Key.ToShortDateString() + item.SettleAmount;
                if (!hadPrice.Contains(indexer))
                {
                    hadPrice.Add(indexer);
                }
                else
                {
                    item.IsRepeated = true;
                }
            }
        }

        private object RecommandSelector(InstructionalFlight flight)
        {
            Flight f = flight.OriginalFlight;
            SpecialProductView specialPolicyInfo = SpecialProductService.Query(flight.OriginalPolicy.Type);
            return new
                {
                    AirlineCode = f.Airline,
                    f.AirlineName,
                    f.FlightNo,
                    Aircraft = f.AirCraft,
                    f.Departure,
                    f.Arrival,
                    YBPrice = f.StandardPrice,
                    f.AirportFee,
                    BAF = f.BAF.Adult,
                    AdultBAF = f.BAF.Adult,
                    ChildBAF = f.BAF.Child,
                    f.IsStop,
                    LowerPrice = flight.SettleAmount,
                    f.DaysInterval,
                    TakeoffTime = String.Format("{0:00}:{1:00}", f.TakeoffTime.Hour, f.TakeoffTime.Minute),
                    f.FlightDate,
                    LandingTime = String.Format("{0:00}:{1:00}", f.LandingTime.Hour, f.LandingTime.Minute),
                    SeatCount = flight.ResourceAmount,
                    SuccessOrderCount = flight.Statistic.Total.SuccessTicketCount,
                    OrderSuccessRate = (flight.Statistic.Total.OrderSuccessRate * 100).TrimInvaidZero() + "%",
                    gradeFirst = Math.Floor(flight.CompannyGrade),
                    gradeSecond = flight.CompannyGrade / 0.1m % 10,
                    flight.IsRepeated,
                    policyId = flight.OriginalPolicy.Id,
                    policyType = (int)flight.OriginalPolicy.PolicyType,
                    publisher = flight.OriginalPolicy.Owner,
                    officeNo = flight.OriginalPolicy.OfficeCode,
                    needAUTH = flight.OriginalPolicy.NeedAUTH,
                    EIList = getProvisionList(flight.OriginalPolicy),
                    Condition = ReplaceEnter(flight.OriginalPolicy.Condition),
                    PolicyDesc = ReplaceEnter(specialPolicyInfo.Description),
                    spType = ReplaceEnter(specialPolicyInfo.Name),
                    specialPolicy = ReplaceEnter(flight.OriginalPolicy.Type.ToString()),
                    needApplication = flight.OriginalPolicy.ConfirmResource,
                    WarnInfo =
                        (flight.OriginalPolicy.Type == SpecialProductType.CostFree && !flight.OriginalPolicy.IsSeat)
                            ? "这是候补票<a class='tips_btn standby_ticket'>什么是候补票？</a>"
                            : flight.OriginalPolicy.ConfirmResource ? "<a class='tips_btn'>什么是申请？</a>" : String.Empty,
                };
        }

        private bool authorize(PNRPair pnr, string officeNo, string source, Guid oemId)
        {
            try
            {
                if (FlightReserveModule.ChoosePolicy.ReservateSource == source
                    || FlightReserveModule.ChoosePolicy.UpgradeByQueryFlightSource == source
                    || FlightReserveModule.ChoosePolicy.ChangeProviderSource == source)
                {
                    CommandService.AuthorizeByOfficeNo(pnr, officeNo,oemId);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                return false;
            }
        }

        private PassengerType getPassengerType(string source)
        {
            if (source == FlightReserveModule.ChoosePolicy.ChangeProviderSource)
            {
                Order originalOrder = FlightReserveModule.ChoosePolicy.GetOriginalOrder(source);
                return originalOrder.PNRInfos.First().Passengers.First().PassengerType;
            }
            else
            {
                return FlightReserveModule.ChoosePolicy.GetPassengers(source).First().PassengerType;
            }
        }

        private IEnumerable<VoyageFilterInfo> getVoyageFilterInfos(string source)
        {
            return (from item in FlightReserveModule.ChoosePolicy.GetFlights(source)
                    select new VoyageFilterInfo
                        {
                            Flight = new FlightFilterInfo
                                {
                                    Airline = item.AirlineCode,
                                    Departure = item.Departure.Code,
                                    Arrival = item.Arrival.Code,
                                    FlightDate = item.Departure.Time.Date,
                                    FlightNumber = item.FlightNo,
                                    StandardPrice = item.YBPrice,
                                    IsShare = item.IsShare,
                                    TakeOffTime = item.Departure.Time
                                },
                            Bunk = item.BunkType.HasValue
                                       ? new BunkFilterInfo
                                           {
                                               Code = item.BunkCode,
                                               Discount = item.Discount.HasValue ? item.Discount.Value : 0,
                                               Type = item.BunkType.Value
                                           }
                                       : null
                        }).ToList();
        }

        private bool hasReduce(string source)
        {
            if (source == FlightReserveModule.ChoosePolicy.ChangeProviderSource)
            {
                return FlightReserveModule.ChoosePolicy.GetOriginalOrder(source).IsReduce;
            }
            else
            {
                IEnumerable<FlightView> voyages = FlightReserveModule.ChoosePolicy.GetFlights(source);
                if (voyages.Count() == 2)
                {
                    PriceView patPrice = FlightReserveModule.ChoosePolicy.GetPATPrice(source);
                    if (patPrice != null)
                    {
                        return patPrice.Fare < voyages.Sum(item => item.Fare);
                    }
                }
                return false;
            }
        }

        private decimal? getPatPrice(string source)
        {
            if (source != FlightReserveModule.ChoosePolicy.ChangeProviderSource)
            {
                PriceView patInfo = FlightReserveModule.ChoosePolicy.GetPATPrice(source);
                if (patInfo != null) return patInfo.Fare;
            }
            return null;
        }

        private string getProvision(IHasRegulation regulation)
        {
            if (regulation == null) return String.Empty;
            return "   <span class='b'>更改规定：</span>"
                   + regulation.ChangeRegulation + "</span> "
                   + " <span><span class='b'>作废规定：</span>"
                   + regulation.InvalidRegulation + "</span> <span> <span><span class='b'>退票规定：</span>" + regulation.RefundRegulation
                   + "</span>   <span class='b'>签转规定：</span>"
                   + regulation.EndorseRegulation + "</span>";
        }

        private object getProvisionList(IHasRegulation regulation)
        {
            if (regulation == null) return new[] { new { key = String.Empty, value = String.Empty } };
            return new[]
                {
                    new {key = "更改规定", value = regulation.ChangeRegulation},
                    new {key = "作废规定", value = regulation.InvalidRegulation},
                    new {key = "退票规定", value = regulation.RefundRegulation},
                    new {key = "签转规定", value = regulation.EndorseRegulation}
                };
        }

        private string getEI(string source)
        {
            var RenderedEI = new Dictionary<string, string>();
            var htmlReg = new Regex("<.+?>");
            IEnumerable<FlightView> voyages = FlightReserveModule.ChoosePolicy.GetFlights(source);
            FlightView flightView = voyages.ElementAt(0);
            string ei = "(" + flightView.BunkCode + ")" + htmlReg.Replace(flightView.EI, String.Empty);
            RenderedEI.Add(flightView.BunkCode + flightView.EI, flightView.EI);
            foreach (FlightView item in voyages)
            {
                if (!RenderedEI.Any(b => b.Key == item.BunkCode + item.EI))
                {
                    ei += "<br />" + "(" + item.BunkCode + ")" + htmlReg.Replace(item.EI, String.Empty);
                    RenderedEI.Add(item.BunkCode + item.EI, item.EI);
                }
            }
            return ei;
        }
    }
}