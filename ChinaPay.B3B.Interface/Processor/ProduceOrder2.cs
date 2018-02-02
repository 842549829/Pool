using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Common;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.PolicyMatch.Domain;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Command;

namespace ChinaPay.B3B.Interface.Processor
{
    class ProduceOrder2 : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var flights = Context.GetParameterValue("flights");
            var passengers = Context.GetParameterValue("passengers");
            var contact = Context.GetParameterValue("contact");
            var policyType = Context.GetParameterValue("policyType");
            Vaild(flights, passengers, contact, policyType, InterfaceSetting);

            DataTransferObject.Order.OrderView orderView = new DataTransferObject.Order.OrderView();
            bindOrderView(flights, passengers, contact, orderView);
            var pnrh = new PNRHelper();
            try
            {
                if ((PolicyType)byte.Parse(policyType) != PolicyType.Special)
                {
                    PNRPair pnr = pnrh.ReserveSeat(loadFlightView(flights, passengers), orderView.Passengers, Employee, Company);
                    orderView.PNR = pnr;
                }
                if (pnrh.RequirePat(loadFlightView(flights, passengers), (PolicyType)byte.Parse(policyType)))
                    orderView.PATPrice = pnrh.Pat(orderView.PNR, loadFlightView(flights, passengers), PassengerType.Adult);
                MatchedPolicy policy = QueryPolicies((PolicyType)byte.Parse(policyType), flights, passengers, orderView);
                if (policy != null)
                {
                    if (policy.PolicyType == PolicyType.Special)
                    {
                        var p = PolicyManageService.GetSpecialPolicy(policy.Id);
                        if (p != null && p.SynBlackScreen)
                        {
                            PNRPair pnr = pnrh.ReserveSeat(loadFlightView(flights, passengers), orderView.Passengers, Employee, Company);
                            orderView.PNR = pnr;
                        }
                    }
                    orderView.IsTeam = false;
                    orderView.Source = OrderSource.InterfaceReservaOrder;
                    Order order = OrderProcessService.ProduceOrder(orderView, policy, Employee, Guid.Empty, false);
                    if (order.Source == OrderSource.InterfaceReservaOrder && !PNRPair.IsNullOrEmpty(order.ReservationPNR) && !String.IsNullOrWhiteSpace(order.Product.OfficeNo))
                        if (policy.NeedAUTH && !string.IsNullOrEmpty(policy.OfficeNumber)) authorize(order.ReservationPNR, policy.OfficeNumber);
                    return "<id>" + order.Id + "</id><payable>" + (policy.ConfirmResource ? 0 : 1) + "</payable>" + ReturnStringUtility.GetOrder(order);
                }
            }
            catch (Exception ex)
            {
                InterfaceInvokeException.ThrowCustomMsgException(ex.Message);
            }
            InterfaceInvokeException.ThrowCustomMsgException("生成订单失败，没有对应直达航班！");
            return "";
        }
        private bool authorize(PNRPair pnr, string officeNo)
        {
            try
            {
                CommandService.AuthorizeByOfficeNo(pnr, officeNo, Guid.Empty);
                return true;
            }
            catch (Exception ex)
            {
                LogService.SaveExceptionLog(ex);
                return false;
            }
        }
        private void bindOrderView(string flights, string passengers, string contact, DataTransferObject.Order.OrderView orderView)
        {
            orderView.Flights = loadFlightView(flights, passengers).Select(view => new DataTransferObject.Order.FlightView
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
                Type = view.BunkType == null ? BunkType.Economic : view.BunkType.Value,
                IsShare = view.IsShare,
                ArrivalTerminal = view.Arrival.Terminal,
                DepartureTerminal = view.Departure.Terminal
            });
            orderView.Passengers = loadPassenger(passengers, orderView);
            if (contact != "")
            {
                var c = contact.Split('|');
                orderView.Contact = new DataTransferObject.Order.Contact()
                {
                    Name = c[0],
                    Mobile = c[1],
                    Email = c[2]
                };
            }
            else
            {
                orderView.Contact = new DataTransferObject.Order.Contact()
                {
                    Name = Employee.Name,
                    Mobile = Employee.Cellphone,
                    Email = Employee.Email
                };
            }
            orderView.TripType = DataTransferObject.Command.PNR.ItineraryType.OneWay;
        }
        private static void Vaild(string flights, string passengers, string contact, string policyType, Service.Organization.Domain.ExternalInterfaceSetting setting)
        {

            if (string.IsNullOrEmpty(flights)) InterfaceInvokeException.ThrowParameterMissException("flights");
            if (string.IsNullOrEmpty(passengers)) InterfaceInvokeException.ThrowParameterMissException("passengers");
            if (string.IsNullOrEmpty(policyType)) InterfaceInvokeException.ThrowParameterMissException("policyType");
            if (flights.Split('|').Count() != 16) InterfaceInvokeException.ThrowParameterErrorException("flights");
            foreach (var item in passengers.Split('^'))
            {
                if (item.Split('|').Count() != 4) InterfaceInvokeException.ThrowParameterErrorException("passengers");
            }
            if (contact != "" && contact.Split('|').Count() != 3) InterfaceInvokeException.ThrowParameterErrorException("contact");
            if ((setting.PolicyTypes & PolicyType.Bargain) != (PolicyType)byte.Parse(policyType) && (setting.PolicyTypes & PolicyType.Normal) != (PolicyType)byte.Parse(policyType) && (setting.PolicyTypes & PolicyType.Team) != (PolicyType)byte.Parse(policyType) && (setting.PolicyTypes & PolicyType.Special) != (PolicyType)byte.Parse(policyType)) InterfaceInvokeException.ThrowNoAccessException();
            if (policyType != "2" && policyType != "4" && policyType != "8" && policyType != "16") InterfaceInvokeException.ThrowParameterErrorException("policyType");

        }
        private List<DataTransferObject.FlightQuery.FlightView> loadFlightView(string filghts, string p)
        {
            var str = filghts.Split('|');

            var flight = new List<DataTransferObject.FlightQuery.FlightView>();
            var timeReg = new Regex("(\\d{1,2}):(\\d{1,2})");
            Match match1 = timeReg.Match(str[4]);
            Match match2 = timeReg.Match(str[7]);
            flight.Add(new DataTransferObject.FlightQuery.FlightView
            {
                Serial = 1,
                AirlineCode = str[0],
                FlightNo = str[1],
                Aircraft = str[3],
                AirlineName = "",
                Departure = new DataTransferObject.FlightQuery.AirportView
                {
                    Code = str[4],
                    Name = "",
                    City = "",
                    Time = DateTime.Parse(str[2] + " " + str[5]),
                    Terminal = str[6]
                },
                Arrival = new DataTransferObject.FlightQuery.AirportView
                {
                    Code = str[7],
                    Name = "",
                    City = "",
                    Time = DateTime.Parse(str[2] + " " + str[8]).AddDays(int.Parse(str[10])),
                    Terminal = str[9]
                },
                //str[9] 飞行天数
                YBPrice = decimal.Parse(str[11]),
                BunkCode = str[12],
                BunkType = str[13] == "" ? (B3B.Common.Enums.BunkType?)null : (B3B.Common.Enums.BunkType)byte.Parse(str[13]),
                Fare = decimal.Parse(str[14]),
                AirportFee = 0,
                BAF = 0,
                AdultBAF = 0,
                ChildBAF = 0,
                SeatCount = p.Split('^').Count(),
                SettleAmount = decimal.Parse(str[15]),
            });

            return flight;
        }
        private List<DataTransferObject.Order.PassengerView> loadPassenger(string passengers, DataTransferObject.Order.OrderView orderView)
        {
            var list = new List<DataTransferObject.Order.PassengerView>();
            foreach (var item in passengers.Split('^'))
            {
                var p = item.Split('|');
                list.Add(new DataTransferObject.Order.PassengerView()
                {
                    Name = p[0],
                    CredentialsType = (Common.Enums.CredentialsType)byte.Parse(p[1]),
                    Credentials = p[2],
                    Phone = p[3],
                    PassengerType = Common.Enums.PassengerType.Adult
                });
            }
            return list;
        }
        private MatchedPolicy QueryPolicies(PolicyType policyType, string flights, string passengers, DataTransferObject.Order.OrderView orderView)
        {
            PassengerType passengerType = PassengerType.Adult;
            var policyFilterCondition = new PolicyFilterConditions
            {
                PolicyType = policyType,
                Purchaser = Company.CompanyId
            };
            IEnumerable<VoyageFilterInfo> voyages = getVoyageFilterInfos(flights, passengers);
            // 特殊票时，只取航班查询处选择的价格
            if (policyType == PolicyType.Special)
            {
                PolicyView policyView = null;
                var flightFlight = voyages.FirstOrDefault();
                if (flightFlight != null && flightFlight.Bunk == null)
                {
                    policyFilterCondition.PatPrice = orderView.PATPrice != null ? orderView.PATPrice.Fare : (decimal?)null;
                }
                else if (policyView != null && flightFlight != null && flightFlight.Bunk == null)
                {
                    policyFilterCondition.PublishFare = policyView.PublishFare;
                }
            }
            else
            {
                policyFilterCondition.PatPrice = orderView.PATPrice != null ? orderView.PATPrice.Fare : (decimal?)null;
            }
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = VoyageType.OneWay;
            policyFilterCondition.SuitReduce = hasReduce(voyages, orderView.PATPrice != null ? orderView.PATPrice.Fare : (decimal?)null);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
            policyFilterCondition.AllowTicketType = FilterByTime(voyages.Min(f => f.Flight.TakeOffTime));
            policyFilterCondition.MaxdRebate = 0;

            IEnumerable<MatchedPolicy> matchedPolicies = PolicyMatchServcie.MatchBunk(policyFilterCondition, false, passengerType, 1).ToList();
            if (!matchedPolicies.Any() || matchedPolicies.FirstOrDefault() == null) InterfaceInvokeException.ThrowNotFindPolicyException();
            return matchedPolicies.FirstOrDefault();
        }
        public static AllowTicketType FilterByTime(DateTime takeOffTime)
        {
            var minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2 * 60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
        }
        public static bool hasReduce(IEnumerable<VoyageFilterInfo> voyages, decimal? fare)
        {
            if (voyages.Count() == 2)
            {
                if (fare.HasValue)
                {
                    return fare < voyages.Sum(item => item.Flight.StandardPrice);
                }
            }
            return false;
        }

        private IEnumerable<VoyageFilterInfo> getVoyageFilterInfos(string flights, string passengers)
        {
            return (from item in loadFlightView(flights, passengers)
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

        //private string ReplaceEnter(string input)
        //{
        //    string result = input.Replace("\n", "<br/>").Replace("\"", String.Empty).Replace("'", String.Empty);
        //    return result.Replace("\r", "");
        //}

    }
}