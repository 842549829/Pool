using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Bunk = ChinaPay.B3B.Service.FlightQuery.Domain.Bunk;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers {
    /// <summary>
    /// FlightQuery 的摘要说明
    /// </summary>
    public class FlightQuery : BaseHandler {
        /// <summary>
        /// 查询单程航班
        /// </summary>
        public object QueryOWFlights(string airline, string departure, string arrival, DateTime flightDate) {
            ClearFlightQuerySessions();
            var originalFlights = Service.FlightQueryService.QueryOWFlights(departure, arrival, flightDate, airline,BasePage.OwnerOEMId);
            saveFlights(originalFlights, VoyageSerial.First);
            return originalFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 查询往返第一程航班
        /// </summary>
        public object QueryRTFirstTripFlights(string airline, string departure, string arrival, DateTime flightDate) {
            ClearFlightQuerySessions();
            var originalFlights = Service.FlightQueryService.QueryRTFirstTripFlights(departure, arrival, flightDate, airline, BasePage.OwnerOEMId);
            saveFlights(originalFlights, VoyageSerial.First);
            return originalFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 查询往返第二程航班
        /// </summary>
        public object QueryRTSecondTipFlights(string firstTripPolicyInfoArgs, string firstTripFlightArgs, DateTime flightDate) {
            var firstTripFlight = FlightView.Parse(firstTripFlightArgs);
            var firstTripPolicy = PolicyView.Parse(firstTripPolicyInfoArgs);
            if(firstTripFlight == null || firstTripPolicy == null) throw new CustomException("缺少去程信息");
            var originalFlights = Service.FlightQueryService.QueryRTSecondTipFlights(firstTripFlight.Arrival.Code,
                                                                                     firstTripFlight.Departure.Code,
                                                                                     flightDate,
                                                                                     firstTripFlight.AirlineCode,
                                                                                     firstTripFlight.Arrival.Time,
                                                                                     firstTripPolicy.Type,
                                                                                     getFirstSelectedBunk(firstTripFlightArgs), BasePage.OwnerOEMId);
            saveFlights(originalFlights, VoyageSerial.Second);
            return originalFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 匹配单程政策
        /// </summary>
        public object MatchOWPolicy(Guid? publisher) {
            var originalFlights = getFlights(VoyageSerial.First);
            var matchedFlights = Service.PolicyMatch.PolicyMatchServcie.MatchOneWayFlights(originalFlights, CurrentCompany.CompanyId, publisher).ToList();
            saveLowestFare(originalFlights.First().FlightDate, matchedFlights);
            return matchedFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 匹配往返第一程政策
        /// </summary>
        public object MatchRTFirstTripPolicy(DateTime backDate, Guid? publisher) {
            var originalFlights = getFlights(VoyageSerial.First);
            var matchedFlights = Service.PolicyMatch.PolicyMatchServcie.MatchRoundTripDepartureFlights(originalFlights, backDate, CurrentCompany.CompanyId, publisher).ToList();
            return matchedFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 匹配往返第二程政策
        /// </summary>
        public object MatchRTSecondTripPolicy(string firstTripFlightArgs, Guid? publisher) {
            var firstTripSelectedBunk = getFirstSelectedBunk(firstTripFlightArgs);
            var senondTripOriginalFlights = getFlights(VoyageSerial.Second);
            var senondTripMatchedFlights = Service.PolicyMatch.PolicyMatchServcie.MatchRoundTripReturnFlights(firstTripSelectedBunk, senondTripOriginalFlights, CurrentCompany.CompanyId, publisher).ToList();
            return senondTripMatchedFlights.Select(constructFlightQueryResultView).GetEnumerator();
        }
        /// <summary>
        /// 查看航班的所有舱位价格
        /// 单程
        /// </summary>
        public object QueryOWBunks(string airline, string flightNo, Guid? publisher) {
            var flights = getFlights(VoyageSerial.First);
            var matchFlight = flights.FirstOrDefault(item => item.Airline == airline && item.FlightNo == flightNo);
            var matchedBunks = Service.PolicyMatch.PolicyMatchServcie.MatchOneWayFlight(matchFlight, CurrentCompany.CompanyId, publisher).ToList();
            var basicPrice = Service.FoundationService.QueryBasicPrice(airline,
                                                           matchFlight.Departure.Code,
                                                           matchFlight.Arrival.Code,
                                                           matchFlight.FlightDate);
            return (from matchedBunk in matchedBunks
                    where matchedBunk.Policies.Any()
                    from bunkInfo in constructBunkView(matchedBunk, basicPrice)
                    orderby bunkInfo.Amount
                    select bunkInfo).GetEnumerator();
        }
        /// <summary>
        /// 查看航班的所有舱位价格
        /// 往返第一程
        /// </summary>
        public object QueryRTFirstTripBunks(string airline, string flightNo, DateTime backDate, Guid? publisher) {
            var flights = getFlights(VoyageSerial.First);
            var matchFlight = flights.FirstOrDefault(item => item.Airline == airline && item.FlightNo == flightNo);
            var matchedBunks = Service.PolicyMatch.PolicyMatchServcie.MatchRoundTripDepartureFlight(matchFlight, backDate, CurrentCompany.CompanyId, publisher);
            var basicPrice = Service.FoundationService.QueryBasicPrice(airline,
                                                                       matchFlight.Departure.Code,
                                                                       matchFlight.Arrival.Code,
                                                                       matchFlight.FlightDate);
            return (from matchedBunk in matchedBunks
                    where matchedBunk.Policies.Any()
                    from bunkInfo in constructBunkView(matchedBunk, basicPrice)
                    orderby bunkInfo.Amount
                    select bunkInfo).GetEnumerator();
        }
        /// <summary>
        /// 查看航班的所有舱位价格
        /// 往返第二程
        /// </summary>
        public object QueryRTSecondTripBunks(string firstTripFlightArgs, string airline, string flightNo, Guid? publisher) {
            var firstTripSelectedBunk = getFirstSelectedBunk(firstTripFlightArgs);
            var secondTripFlights = getFlights(VoyageSerial.Second);
            var secondTripMatchFlight = secondTripFlights.FirstOrDefault(item => item.Airline == airline && item.FlightNo == flightNo);
            var secondTripMatchedBunks = Service.PolicyMatch.PolicyMatchServcie.MatchRoundTripReturnFlight(firstTripSelectedBunk, secondTripMatchFlight, CurrentCompany.CompanyId, publisher);
            return (from matchedBunk in secondTripMatchedBunks
                    where matchedBunk.Policies.Any()
                    from bunkInfo in constructBunkView(matchedBunk, null)
                    orderby bunkInfo.Amount
                    select bunkInfo).GetEnumerator();
        }

        /// <summary>
        /// 获取航班的经停信息
        /// </summary>
        /// <param name="flightNo"></param>
        /// <returns></returns>
        public object GetFlightStopInfo(string flightNo, string flightDate)
        {
            var AVHInfo = CommandService.GetTransitPoints(flightNo, DateTime.Parse(flightDate), BasePage.OwnerOEMId);
            if (AVHInfo.Success && AVHInfo.Result.First() != null)
            {
                var transitPoint = AVHInfo.Result.First();
                var arriveTime = ParseTime(flightDate, transitPoint.ArrivalTime,transitPoint.ArrivalAddDays);
                var departureTime = ParseTime(flightDate, transitPoint.DepartureTime,transitPoint.DepartureAddDays);
                if (arriveTime == DateTime.MinValue || departureTime == DateTime.MinValue) return new { IsSuccess = false };
                return new
                {
                    IsSuccess = true,
                    Obj = string.Format("经停城市:{0}<br />到达时间:{1:yyyy-MM-dd HH:mm}<br />起飞时间:{2:yyyy-MM-dd HH:mm}",
                        Service.FoundationService.QueryCityNameByAirportCode(transitPoint.AirportCode),
                        arriveTime,
                        departureTime)
                };
            }
            return new { IsSuccess = false };
        }
        static readonly Regex reg = new Regex("(?<Hour>\\d{2})(?<Minute>\\d{2})");
        private DateTime ParseTime(string flightDate, Time time, int addDays) {
            //var matchInfo = reg.Match(time);
            //if(matchInfo.Success) {
            //    return DateTime.Parse(flightDate).Date
            //        .AddHours(int.Parse(matchInfo.Groups["Hour"].Value))
            //        .AddMinutes(int.Parse(matchInfo.Groups["Minute"].Value));
            //}
            //return DateTime.MinValue;
            return DateTime.Parse(flightDate).AddHours(time.Hour).AddMinutes(time.Minute).AddDays(addDays);
        }

        private Service.FlightQuery.Domain.Bunk getFirstSelectedBunk(string firstTripFlightArgs) {
            var firstTripFlight = DataTransferObject.FlightQuery.FlightView.Parse(firstTripFlightArgs);
            var firstTripFlights = getFlights(DataTransferObject.FlightQuery.VoyageSerial.First);
            var firstTripOriginalFlight = firstTripFlights.First(item => item.Airline == firstTripFlight.AirlineCode && item.FlightNo == firstTripFlight.FlightNo);
            var firstTripSelectedBunk = firstTripOriginalFlight.Bunks.First(item => item.Code == firstTripFlight.BunkCode);
            return firstTripSelectedBunk;
        }

        private static void saveLowestFare(DateTime flightDate, IEnumerable<Service.PolicyMatch.MatchedFlight> matchedFlights) {
            try {
                var matchedFlightsWithPolicy = matchedFlights.Where(item => item.LowestPrice > 0).ToList();
                if(matchedFlightsWithPolicy.Any()) {
                    var lowestFare = matchedFlightsWithPolicy.Min(item => item.LowestPrice);
                    if(lowestFare > 0) {
                        var lowestFlight = matchedFlightsWithPolicy.First(item => item.LowestPrice == lowestFare);
                        var product = DataTransferObject.Order.ProductType.General;
                        switch(lowestFlight.PolicyType) {
                            case PolicyType.Special:
                                product = DataTransferObject.Order.ProductType.Special;
                                break;
                            case PolicyType.Bargain:
                                product = DataTransferObject.Order.ProductType.Promotion;
                                break;
                        }
                        var discount = ChinaPay.Utility.Calculator.Round(lowestFlight.LowestPrice / lowestFlight.OriginalFlight.StandardPrice, -2);
                        Service.RecommendService.SaveFlightLowerFare(lowestFlight.OriginalFlight.Departure.Code,
                                                                     lowestFlight.OriginalFlight.Arrival.Code,
                                                                     flightDate,
                                                                     lowestFare, discount, product);
                    }
                }
            } catch { }
        }

        private IEnumerable<Service.FlightQuery.Domain.Flight> getFlights(VoyageSerial voyageSerial) {
            return Session[getFlightQuerySessionKey(voyageSerial)] as IEnumerable<Service.FlightQuery.Domain.Flight>;
        }
        private void saveFlights(IEnumerable<Service.FlightQuery.Domain.Flight> flights, VoyageSerial voyageSerial) {
            Session[getFlightQuerySessionKey(voyageSerial)] = flights;
        }

        private static string getFlightQuerySessionKey(VoyageSerial voyageSerial) {
            return "FlightQuerySessionKey" + voyageSerial.ToString();
        }
        public static void ClearFlightQuerySessions() {
            ClearFlightQuerySession();
            CurrentSession.Remove("ReservedFlights");
            CurrentSession.Remove("FlightPolicy");
            CurrentSession.Remove("OrderView");
            CurrentSession.Remove("ApplyformView");
            CurrentSession.Remove("PNRContent");
            CurrentSession.Remove("Passengers");
            CurrentSession.Remove("MatchedPolicy");
        }
        public static void ClearFlightQuerySession() {
            CurrentSession.Remove(getFlightQuerySessionKey(VoyageSerial.First));
            CurrentSession.Remove(getFlightQuerySessionKey(VoyageSerial.Second));
        }
        private static System.Web.SessionState.HttpSessionState CurrentSession {
            get { return System.Web.HttpContext.Current.Session; }
        }

        private FlightInfo constructFlightQueryResultView(Service.FlightQuery.Domain.Flight flight) {
            return new FlightInfo {
                AirlineCode = flight.Airline,
                AirlineName = flight.AirlineName,
                FlightNo = flight.FlightNo,
                Aircraft = flight.AirCraft,
                Departure = new FlightInfo.Airport {
                    Code = flight.Departure.Code,
                    Name = flight.Departure.AbbrivateName,
                    City = flight.Departure.City,
                    Terminal = flight.Departure.Terminal,
                    Time = flight.TakeoffTime.ToString()
                },
                Arrival = new FlightInfo.Airport {
                    Code = flight.Arrival.Code,
                    Name = flight.Arrival.AbbrivateName,
                    City = flight.Arrival.City,
                    Terminal = flight.Arrival.Terminal,
                    Time = flight.LandingTime.ToString()
                },
                YBPrice = flight.StandardPrice,
                AirportFee = flight.AirportFee,
                BAF = flight.BAF.Adult,
                AdultBAF = flight.BAF.Adult,
                ChildBAF = flight.BAF.Child,
                IsStop = flight.IsStop,
                DaysInterval = flight.DaysInterval,
                FlightDate = flight.FlightDate
            };
        }
        private FlightInfo constructFlightQueryResultView(Service.PolicyMatch.MatchedFlight flight) {
            var result = constructFlightQueryResultView(flight.OriginalFlight);
            result.LowerPrice = flight.LowestPrice;
            return result;
        }
        private IEnumerable<BunkInfo> constructBunkView(MatchedBunk matchedBunk, BasicPrice basicPrice) {
            return from policy in matchedBunk.Policies
                   where policy != null
                   select constructBunkView(matchedBunk.OriginalBunk, policy, basicPrice);
        }
        private BunkInfo constructBunkView(Bunk bunk, MatchedPolicy policy, BasicPrice price) {
            var result = new BunkInfo() {
                Policy = new PolicyView() {
                    Id = policy.OriginalPolicy == null ? Guid.Empty : policy.OriginalPolicy.Id,
                    Owner = policy.Provider,
                    Type = policy.PolicyType,
                    CustomerResource = false
                }
            };
            result.ShowPrice = policy.ParValue != 0;
            if(policy.PolicyType == PolicyType.Special) {
                // 特殊票是单独处理的
                var specialPolicy = policy.OriginalPolicy as DataTransferObject.Policy.SpecialPolicyInfo;
                result.Code = bunk == null ? string.Empty : bunk.Code;
                result.SeatCount = bunk == null ? specialPolicy.ResourceAmount : bunk.SeatCount; // 剩余位置数 从政策上获取
                result.Fare = policy.ParValue.TrimInvaidZero(); // 票面价从政策上取
                result.Rebate = string.Empty; // 无返点
                result.Amount = policy.SettleAmount;
                result.Description = "特殊票";
                result.BunkType = bunk == null ? new BunkType?() : bunk.Type;
                switch(specialPolicy.Type) {
                    case SpecialProductType.Singleness:
                    case SpecialProductType.Disperse:
                        result.Policy.CustomerResource = true;
                        break;
                    case SpecialProductType.CostFree:
                        result.Policy.CustomerResource = !specialPolicy.SynBlackScreen;
                        result.ShowPrice = true;
                        break;
                }
            } else {
                result.Code = bunk.Code;
                result.SeatCount = bunk.SeatCount;
                result.Fare = policy.ParValue.TrimInvaidZero();
                result.Rebate = policy.Commission.TrimInvaidZero();
                result.Amount = policy.SettleAmount;

                if(policy.PolicyType == PolicyType.Bargain && bunk is Service.FlightQuery.Domain.GeneralBunk) {
                    result.Description = "特价票";
                } else {
                    if(bunk is Service.FlightQuery.Domain.FirstOrBusinessBunk) {
                        result.Description = (bunk as Service.FlightQuery.Domain.FirstOrBusinessBunk).Description;
                    } else if(bunk is Service.FlightQuery.Domain.EconomicBunk) {
                        result.Description = "经济舱";
                    } else if(bunk is Service.FlightQuery.Domain.PromotionBunk) {
                        result.Description = (bunk as Service.FlightQuery.Domain.PromotionBunk).Description;
                    } else if(bunk is Service.FlightQuery.Domain.ProductionBunk) {
                        result.Description = "往返产品";
                    } else {
                        result.Description = string.Empty;
                    }
                }
                result.BunkType = bunk.Type;
            }
            if(bunk != null && bunk is Service.FlightQuery.Domain.GeneralBunk) {
                result.Discount = ((bunk as Service.FlightQuery.Domain.GeneralBunk).Discount).TrimInvaidZero();
                if (policy.PolicyType == PolicyType.Special)
                {
                    result.RenderDiscount = price != null&&policy.ParValue!=0 ? Math.Round(policy.ParValue / price.Price, 2).ToString() : string.Empty;
                }
                else
                {
                    result.RenderDiscount = ((bunk as Service.FlightQuery.Domain.GeneralBunk).Discount).TrimInvaidZero();
                }
            } else {
                result.RenderDiscount = result.Discount = string.Empty;
            }
            // 退改签规定
            // 普通政策时，获取基础数据中普通舱位的退改签信息
            // 其他情况，获取政策上的退改签信息
            if((policy.PolicyType == PolicyType.Normal || policy.PolicyType == PolicyType.NormalDefault || policy.PolicyType == PolicyType.OwnerDefault) && bunk is Service.FlightQuery.Domain.GeneralBunk) {
                result.EI = GetGeneralBunkRegulation(bunk);
            } else {
                if(policy.OriginalPolicy is DataTransferObject.Policy.IHasRegulation) {
                    var regulation = policy.OriginalPolicy as DataTransferObject.Policy.IHasRegulation;
                    result.EI = GetRegulation(regulation);
                } else {
                    result.EI = string.Empty;
                }
            }
            result.SuportChild = bunk != null && bunk.SuportChild;
            return result;
        }

        public static string GetGeneralBunkRegulation(Bunk bunk) {
            var pattern = new Regex("^[a-zA-Z\\d/]+$");
            var refundDetail = FoundationService.QueryDetailList(bunk.Owner.Airline,bunk.Code).Where(item=>pattern.IsMatch(item.Bunks));
            StringBuilder result = new StringBuilder();
            string refundRegulation = string.Empty;
            string changeRegulation = string.Empty;
            string endorseRegulation = string.Empty;
            foreach (var item in refundDetail)
            {
                refundRegulation += ("航班起飞前：" + item.ScrapBefore + "；航班起飞后：" + item.ScrapAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                changeRegulation += ("航班起飞前：" + item.ChangeBefore + "；航班起飞后：" + item.ChangeAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                endorseRegulation += item.Endorse.Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", ""); 
            }
            if (string.IsNullOrWhiteSpace(refundRegulation))
                refundRegulation = "以航司具体规定为准";
            if (string.IsNullOrWhiteSpace(changeRegulation))
                changeRegulation = "以航司具体规定为准";
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", refundRegulation);
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", changeRegulation);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", endorseRegulation);
            return result.ToString();
            //return (bunk as Service.FlightQuery.Domain.GeneralBunk).EI; 
        }

        public static string GetRegulation(IHasRegulation regulation)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("<p><span class=b>更改规定：</span>{0}</p>", regulation.ChangeRegulation);
            result.AppendFormat("<p><span class=b>作废规定：</span>{0}</p>", regulation.InvalidRegulation);
            result.AppendFormat("<p><span class=b>退票规定：</span>{0}</p>", regulation.RefundRegulation);
            result.AppendFormat("<p><span class=b>签转规定：</span>{0}</p>", regulation.EndorseRegulation);
            return result.ToString();



            //return ((string.IsNullOrWhiteSpace(regulation.RefundRegulation) ? string.Empty : (regulation.RefundRegulation.Trim() + "$")) +
            //        (string.IsNullOrWhiteSpace(regulation.InvalidRegulation) ? string.Empty : (regulation.InvalidRegulation.Trim() + "$")) +
            //        (string.IsNullOrWhiteSpace(regulation.ChangeRegulation) ? string.Empty : (regulation.ChangeRegulation.Trim() + "$")) +
            //        (string.IsNullOrWhiteSpace(regulation.EndorseRegulation) ? string.Empty : (regulation.EndorseRegulation.Trim() + "$"))).TrimEnd("$".ToCharArray()).Replace("$", "。");
        }

        class FlightInfo {
            public string AirlineCode { get; set; }
            public string AirlineName { get; set; }
            public string FlightNo { get; set; }
            public string Aircraft { get; set; }
            public Airport Departure { get; set; }
            public Airport Arrival { get; set; }
            public decimal YBPrice { get; set; }
            public decimal AirportFee { get; set; }
            public decimal BAF { get; set; }
            public decimal AdultBAF { get; set; }
            public decimal ChildBAF { get; set; }
            public bool IsStop { get; set; }
            public decimal? LowerPrice { get; set; }
            public int DaysInterval { get; set; }
            public DateTime FlightDate { get; set; }
            public class Airport {
                /// <summary>
                /// 机场代码
                /// </summary>
                public string Code { get; set; }
                /// <summary>
                /// 机场名称
                /// </summary>
                public string Name { get; set; }
                /// <summary>
                /// 城市名称
                /// </summary>
                public string City { get; set; }
                /// <summary>
                /// 航站楼
                /// </summary>
                public string Terminal { get; set; }
                /// <summary>
                /// 起飞/降落时间
                /// </summary>
                public string Time { get; set; }
            }
        }

        class BunkInfo {
            public PolicyView Policy { get; set; }
            public string Code { get; set; }
            public int SeatCount { get; set; }
            public Common.Enums.BunkType? BunkType { get; set; }
            public string EI { get; set; }
            public string Description { get; set; }
            public string Discount { get; set; }
            public string RenderDiscount { get; set; }
            public string Fare { get; set; }
            public string Rebate { get; set; }
            public decimal Amount { get; set; }
            public bool ShowPrice { get; set; }
            public bool SuportChild { get; set; }
        }
    }
}