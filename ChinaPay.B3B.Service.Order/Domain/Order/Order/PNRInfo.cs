using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Utility;
using FlightView = ChinaPay.B3B.DataTransferObject.Order.FlightView;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;
using PriceView = ChinaPay.B3B.DataTransferObject.Order.PriceView;

namespace ChinaPay.B3B.Service.Order.Domain
{
    /// <summary>
    /// 编码信息
    /// </summary>
    public class PNRInfo
    {
        private readonly List<Flight> _flights;
        private readonly List<Passenger> _passengers;

        internal PNRInfo()
            : this(Guid.NewGuid()) { }

        internal PNRInfo(Guid id)
        {
            Id = id;
            _passengers = new List<Passenger>();
            _flights = new List<Flight>();
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// 编码对信息
        /// </summary>
        public PNRPair Code { get; internal set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public PNRType Type
        {
            get { return _passengers.Exists(item => item.PassengerType == PassengerType.Child) ? PNRType.Child : PNRType.Adult; }
        }

        /// <summary>
        /// 行程类型
        /// </summary>
        public ItineraryType TripType
        {
            get { return Parser.GetItineraryType(_flights.Select(f => new AirportPair(f.Departure.Code, f.Arrival.Code)).ToList()); }
        }

        /// <summary>
        /// 乘机人信息
        /// </summary>
        public IEnumerable<Passenger> Passengers
        {
            get { return _passengers.AsReadOnly(); }
        }

        /// <summary>
        /// 航段信息
        /// </summary>
        public IEnumerable<Flight> Flights
        {
            get { return _flights.AsReadOnly(); }
        }

        /// <summary>
        /// 单人价格信息
        /// </summary>
        public Price SinglePrice
        {
            get { return _flights.Aggregate(Price.Zero, (current, item) => current + item.Price); }
        }

        public string PNRContent { get; set; }

        public string PatContent { get; set; }

        internal void AddPassenger(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException("passenger", "乘机人信息不能为空");
            if (_passengers.Count > 0)
            {
                if (passenger.PassengerType != _passengers[0].PassengerType)
                {
                    throw new ApplicationException("成人/婴儿不能与儿童订同一编码");
                }
                Passenger samePassenger = _passengers.Find(p => string.Compare(p.Credentials, passenger.Credentials, StringComparison.OrdinalIgnoreCase) == 0
                                                                || string.Compare(p.Name, passenger.Name, StringComparison.OrdinalIgnoreCase) == 0);
                if (samePassenger != null)
                {
                    throw new RepeatedItemException("同一编码中不能出现相同名字或证件号的乘机人");
                }
            }
            _passengers.Add(passenger);
        }

        internal void AddFlight(Flight flight)
        {
            if (flight == null) throw new ArgumentNullException("flight", "航段信息不能为空");
            if (_flights.Exists(item => item.Id == flight.Id ||
                                        (Carrier.Equals(item.Carrier, flight.Carrier) && item.FlightNo == flight.FlightNo)))
                throw new RepeatedItemException("不能重复添加相同航段");
            _flights.Add(flight);
        }

        internal bool ContainsPassenger(string passengerName, string credentials) { return GetPassenger(passengerName, credentials) != null; }
        internal bool ContainsCredentials(string credentials) { return _passengers.Any(p => p.Credentials == credentials); }
        internal bool ContainsTicket(string ticketNo) { return getPassenger(ticketNo) != null; }
        internal bool ContainsFlight(AirportPair airportPair) { return GetFlight(airportPair) != null; }

        internal Passenger UpdateCredentials(string passengerName, string originalCredentials, string newCredentials, bool execCommand, out bool success, Guid oemId)
        {
            success = true;
            Passenger passenger = GetPassenger(passengerName, originalCredentials);
            if (execCommand)
            {
                success = updateCredentials(passengerName, originalCredentials, newCredentials, passenger.CredentialsType,oemId);
            }
            passenger.UpdateCredentials(newCredentials);
            return passenger;
        }

        internal Passenger UpdateTicketNo(string originalTicketNo, string settleCode, string newTicketNo)
        {
            Passenger newTicketPassenger = getPassenger(newTicketNo);
            if (newTicketPassenger != null) throw new CustomException("新票号[" + newTicketNo + "]已存在");
            Passenger passenger = getPassenger(originalTicketNo);
            if (passenger == null) throw new CustomException("票号[" + originalTicketNo + "]不存在");
            passenger.UpdateTicketNo(originalTicketNo, settleCode, newTicketNo);
            return passenger;
        }

        internal PNRPair FillTicketNos(PNRPair etdzPNR, ETDZMode mode, string settleCode, IEnumerable<TicketNoView.Item> ticketNoItems, Guid oemId)
        {
            PNRPair result = validateTicketNos(etdzPNR, ticketNoItems,oemId);
            foreach (Passenger passenger in Passengers)
            {
                TicketNoView.Item ticketNos = ticketNoItems.FirstOrDefault(item => string.Compare(item.Name, passenger.Name, StringComparison.OrdinalIgnoreCase) == 0);
                if (ticketNos == null) throw new CustomException("缺少乘机人[" + passenger.Name + "]的票号信息。");
                passenger.FillTicketNos(settleCode, ticketNos.TicketNos, mode);
            }
            Code = result;
            return result;
        }

        internal Passenger GetPassenger(string name, string credentials)
        {
            return _passengers.FirstOrDefault(item => string.Compare(item.Name, name, StringComparison.OrdinalIgnoreCase) == 0
                                                      && string.Compare(item.Credentials, credentials, StringComparison.OrdinalIgnoreCase) == 0);
        }

        internal Flight GetFlight(AirportPair airportPair) { return _flights.FirstOrDefault(item => item.IsSameVoyage(airportPair)); }
        internal bool IsSamePNR(PNRPair pnr) { return PNRPair.Equals(Code, pnr); }

        internal bool CancelReservation(IEnumerable<Guid> passengers, IEnumerable<Guid> flights, Guid oemId)
        {
            if (isAllPassengers(passengers) && isAllFlights(flights))
            {
                return cancelPNR(oemId);
            }
            else if (isAllPassengers(passengers))
            {
                return cancelFlights(flights,oemId);
            }
            else if (isAllFlights(flights))
            {
                return cancelPassengers(passengers,oemId);
            }
            else
            {
                return false;
            }
        }

        internal PNRInfo UpdateContentForRefund(RefundOrScrapApplyform refundOrScrapApplyform)
        {
            PNRInfo result = null;
            IEnumerable<Guid> passengers = refundOrScrapApplyform.GetAppliedPassengers();
            IEnumerable<Guid> flights = refundOrScrapApplyform.GetAppliedFlights();
            if (isAllPassengers(passengers) && isAllFlights(flights))
            {
                removeAll();
            }
            else if (isAllPassengers(passengers))
            {
                removeFlights(flights);
            }
            else if (isAllFlights(flights))
            {
                removePassengers(passengers);
            }
            else
            {
                if (PNRPair.IsNullOrEmpty(refundOrScrapApplyform.NewPNR)) throw new CustomException("未提供分离后的新编码");
                result = separate(refundOrScrapApplyform.NewPNR, passengers);
                result.removeFlights(flights);
            }
            return result;
        }

        internal PNRInfo UpdateContentForPostpone(PostponeApplyform postponeApplyform)
        {
            PNRInfo result = null;
            IEnumerable<Guid> passengers = postponeApplyform.GetAppliedPassengers();
            if (isAllPassengers(passengers))
            {
                updateFlights(postponeApplyform.Flights);
            }
            else
            {
                if (PNRPair.IsNullOrEmpty(postponeApplyform.NewPNR)) throw new CustomException("未提供分离后的新编码");
                result = separate(postponeApplyform.NewPNR, passengers);
                result.updateFlights(postponeApplyform.Flights);
            }
            return result;
        }

        internal bool RequireSeparate(BaseApplyform applyform)
        {
            IEnumerable<Guid> passengers = applyform.GetAppliedPassengers();
            if (applyform is PostponeApplyform)
            {
                return !isAllPassengers(passengers);
            }
            else if (applyform is RefundOrScrapApplyform)
            {
                IEnumerable<Guid> flights = applyform.GetAppliedFlights();
                return !isAllPassengers(passengers) && !isAllFlights(flights);
            }
            return false;
        }

        /// <summary>
        /// 更新特殊票资源数
        /// </summary>
        /// <param name="pnrPair"></param>
        /// <param name="isFullFlight">是否所有航段都是实际航段【非弃程】</param>
        /// <param name="patPrice"></param>
        /// <param name="isStandby"></param>
        /// <param name="checkPat"></param>
        /// <param name="isthridRelation"></param>
        /// <returns></returns>
        internal bool UpdateContentForResource(PNRPair pnrPair, bool isFullFlight, decimal? patPrice, bool isStandby, Guid oemId, bool checkPat = false, bool isthridRelation = false)
        {
            bool result = false;
            if (!(_flights.First().Bunk is FreeBunk) && (checkPat || !isthridRelation))
            {
                if (!patPrice.HasValue) throw new CustomException("缺少编码价格信息");
                checkFare(patPrice.Value);
                apportionFare(patPrice.Value);
                _passengers.ForEach(passenger => passenger.RefreshPrice());
                result = true;
            }
            Code = pnrPair;
            if (!isStandby && (!checkPat || !isthridRelation))
            {
                //if (string.IsNullOrEmpty(pnrPair.PNR))
                //{
                //    var firstFlight = _flights.First();
                //    var transferPNRResult = CommandService.TransferPNRCode(pnrPair, new FlightNumber(firstFlight.Carrier.Code, firstFlight.FlightNo), firstFlight.TakeoffTime.Date);
                //    if (!transferPNRResult.Success) throw new CustomException("提取编码信息失败");
                //    pnrPair.PNR = transferPNRResult.Result.PnrPair.PNR;
                //}

                ExecuteResult<ReservedPnr> pnrDetailExecResult = CommandService.GetReservedPnr(pnrPair,oemId);
                if (!pnrDetailExecResult.Success) throw new CustomException("提取编码信息失败");
                if (pnrDetailExecResult.Result.HasCanceled) throw new CustomException("编码为取消状态");
                checkPassengers(pnrDetailExecResult.Result.Passengers);
                checkFlights(pnrDetailExecResult.Result.Voyage.Segments, isFullFlight);
                updateFlights(pnrDetailExecResult.Result.Voyage.Segments);
                PNRContent = pnrDetailExecResult.Result.PnrRawData;
                var firstPassenger = Passengers.First();
                PatContent = Command.Domain.Utility.ContentBulider.GetPatString(Flights.First().Bunk.Code,
                    firstPassenger.Price.Fare, firstPassenger.Price.AirportFee, firstPassenger.Price.BAF);
            }
            return result;
        }

        internal decimal ReviseReleasedFare(decimal releasedFare) { return _flights.Sum(flight => flight.ReviseReleasedFare(releasedFare)); }

        internal void ReviseFare(IEnumerable<PriceView> priceViews)
        {
            if (priceViews == null) throw new ArgumentNullException("priceViews");
            if (priceViews.Count() != _flights.Count) throw new CustomException("航段数据错误");
            if (priceViews.Sum(pv => pv.Fare) != SinglePrice.Fare) throw new CustomException("票面总价错误");
            if (priceViews.Sum(pv => pv.AirportFee) != SinglePrice.AirportFee) throw new CustomException("机建费总额错误");
            _flights.ForEach(flight =>
                                 {
                                     PriceView priceView = priceViews.FirstOrDefault(pv => flight.IsSameVoyage(pv.AirportPair));
                                     if (priceView == null) throw new CustomException(string.Format("缺少航段[{0}-{1}]的数据", flight.Departure.Code, flight.Arrival.Code));
                                     flight.ReviseFare(priceView.Fare);
                                     flight.AirportFee = priceView.AirportFee;
                                 });
            _passengers.ForEach(passenger => passenger.RefreshPrice());
        }

        private void checkPassengers(IEnumerable<Command.Domain.PNR.Passenger> passengers)
        {
            if (passengers.Count() != _passengers.Count) throw new CustomException("编码中乘机人与订单中的乘机人信息不符合");
            foreach (Passenger item in _passengers)
            {
                Command.Domain.PNR.Passenger passenger = passengers.FirstOrDefault(p => string.Compare(p.Name, item.Name, StringComparison.OrdinalIgnoreCase) == 0);
                if (passenger == null) throw new CustomException("编码中缺少乘机人[" + item.Name + "]的信息");
                if (!string.IsNullOrWhiteSpace(passenger.CertificateNumber) &&
                    String.Compare(passenger.CertificateNumber, item.Credentials, StringComparison.OrdinalIgnoreCase) != 0)
                    throw new CustomException("编码中乘机人[" + item.Name + "]与订单中的证件号不符合");
                if (passenger.Type != item.PassengerType) throw new CustomException("编码中乘机人[" + item.Name + "]与订单中的类型不符合");
            }
        }

        private void checkFlights(IEnumerable<Segment> segments, bool isFullFlight)
        {
            if (isFullFlight && segments.Count() != _flights.Count) throw new CustomException("编码中航段与订单中的航段信息不符合");
            foreach (Flight flight in _flights)
            {
                string voyageString = "[" + flight.Departure.Code + "-" + flight.Arrival.Code + "]";
                Segment segment = segments.FirstOrDefault(item => flight.IsSameVoyage(item.AirportPair));
                if (segment == null) throw new CustomException("编码中缺少" + voyageString + "的航段信息");
                if (string.Compare(flight.Carrier.Code, segment.AirlineCode, true) != 0) throw new CustomException("编码中航段" + voyageString + "与订单的乘运人不符合");
                if (flight.FlightNo != segment.InternalNo) throw new CustomException("编码中航段" + voyageString + "与订单的航班号不符合");
                if (flight.TakeoffTime.Date != segment.Date.Date) throw new CustomException("编码中航段" + voyageString + "与订单的航班日期不符合");
                if (!segment.Status.Contains("K") && !segment.Status.Contains("RR")) throw new CustomException("编码中航段" + voyageString + "状态错误");
            }
        }

        private void updateFlights(IEnumerable<Segment> segments)
        {
            foreach (Flight flight in _flights)
            {
                Segment segment = segments.FirstOrDefault(item => flight.IsSameVoyage(item.AirportPair));
                flight.ModifyBunk(segment.CabinSeat);
                flight.IsShare = segment.IsShared;
            }
        }

        private PNRPair validateTicketNos(PNRPair etdzPNR, IEnumerable<TicketNoView.Item> ticketNoItems, Guid oemId)
        {
            PNRPair validatePNR = etdzPNR;
            if (PNRPair.IsNullOrEmpty(etdzPNR))
            {
                validatePNR = Code;
            }
            if (ConfigurationManager.AppSettings["ValidateTicketNo"] == "1")
            {
                ExecuteResult<IEnumerable<Command.Domain.PNR.Passenger>> execResult = CommandService.GetTicketNumbersByPnr(validatePNR,oemId);
                if (execResult.Success)
                {
                    foreach (TicketNoView.Item item in ticketNoItems)
                    {
                        if (execResult.Result.FirstOrDefault(pt => pt.Name == item.Name && pt.TicketNumbers == item.TicketNos) == null)
                        {
                            throw new CustomException("乘机人[" + item.Name + "]的票号[" + item.TicketNos + "]与编码中的票号信息不匹配");
                        }
                    }
                }
            }
            return validatePNR;
        }

        private PNRInfo separate(PNRPair newPNRCode, IEnumerable<Guid> passengers)
        {
            var newPnrInfo = new PNRInfo
                {
                    Code = newPNRCode
                };
            foreach (Guid item in passengers)
            {
                Passenger passenger = getPassenger(item);
                if (passenger == null) throw new CustomException("不存在的乘机人");
                newPnrInfo.AddPassenger(passenger.Copy());
            }
            foreach (Flight flight in Flights)
            {
                newPnrInfo.AddFlight(flight.Copy());
            }
            removePassengers(passengers);
            return newPnrInfo;
        }

        private void removePassengers(IEnumerable<Guid> passengers)
        {
            foreach (Guid passenger in passengers)
            {
                _passengers.RemoveAll(item => item.Id == passenger);
            }
        }

        private void removeFlights(IEnumerable<Guid> flights)
        {
            foreach (Guid flight in flights)
            {
                removeFlight(flight);
            }
        }

        private void removeFlight(Guid flight) { _flights.RemoveAll(item => item.Id == flight || item.AssociateFlight == flight); }

        private void removeAll()
        {
            _passengers.Clear();
            _flights.Clear();
        }

        private void updateFlights(IEnumerable<PostponeFlight> postponeFlights)
        {
            foreach (PostponeFlight item in postponeFlights)
            {
                Flight originalFlight = getFlight(item.NewFlight.Departure.Code, item.NewFlight.Arrival.Code);
                if (originalFlight == null) throw new NotFoundException("原航段信息不存在。" + item.OriginalFlight.Id);
                removeFlight(originalFlight.Id);
                AddFlight(item.NewFlight);
            }
        }

        private Passenger getPassenger(string ticketNo) { return _passengers.FirstOrDefault(item => item.ContainsTicket(ticketNo)); }
        private Passenger getPassenger(Guid passenger) { return _passengers.FirstOrDefault(item => item.Id == passenger); }
        private Flight getFlight(string departure, string arrival) { return _flights.FirstOrDefault(item => item.IsSameVoyage(departure, arrival)); }
        private bool isAllFlights(IEnumerable<Guid> flights) { return _flights.All(flight => flights.Any(item => item == flight.Id)); }
        private bool isAllPassengers(IEnumerable<Guid> passengers) { return _passengers.All(passenger => passengers.Any(item => item == passenger.Id)); }

        private void checkFare(decimal actualTotalFare)
        {
            decimal releasedTotalFare = Flights.Where(item => item.Bunk is SpecialBunk).Sum(item => (item.Bunk as SpecialBunk).ReleasedFare);
            if (releasedTotalFare < actualTotalFare) throw new CustomException("真实票面价不能高于发布价格");
        }

        private void apportionFare(decimal actualTotalFare)
        {
            int flightCount = _flights.Count();
            decimal averageFare = Calculator.Round(actualTotalFare/flightCount, 1);
            int flightIndex = 0;
            foreach (Flight item in _flights)
            {
                flightIndex++;
                if (flightIndex == flightCount)
                {
                    decimal apportionedFare = averageFare*(flightIndex - 1);
                    item.ReviseFare(actualTotalFare - apportionedFare);
                }
                else
                {
                    item.ReviseFare(averageFare);
                }
            }
        }

        private bool cancelPNR(Guid oemId)
        {
            return CommandService.CancelPNR(Code,oemId);
        }

        private bool cancelPassengers(IEnumerable<Guid> passengers,Guid oemId)
        {
            IEnumerable<string> passengerNames = from p in _passengers
                                                 from pId in passengers
                                                 where p.Id == pId
                                                 select p.Name;
            return CommandService.CancelPassengersByPNR(passengerNames.ToArray(), Code,oemId);
        }

        private bool cancelFlights(IEnumerable<Guid> flights,Guid oemID)
        {
            var voyages = from f in _flights
                                               from fId in flights
                                               where f.Id == fId
                                               select new CancelSegmentInfo()
                                               {
                                                   FlightDate = f.TakeoffTime.Date,
                                                   FlightNumber =  new FlightNumber(f.Carrier.Code,f.FlightNo)
                                               };
            return CommandService.CancelVoyagesByPNR(voyages.ToArray(), Code,oemID);
        }

        private bool updateCredentials(string passengerName, string oldCrendentials, string newCrendentials, CredentialsType passengerType, Guid oemId)
        {
            var execResult = CommandService.ModifyCertificateNumber(Code, passengerName, oldCrendentials, newCrendentials, passengerType,oemId);
            return execResult.Success;
        }

        internal static PNRInfo GetPNRInfo(OrderView orderView, MatchedPolicy policy)
        {
            if (policy == null) throw new ArgumentNullException("policy");
            var result = new PNRInfo
                {
                    Code = orderView.PNR
                };
            PassengerType passengerType = orderView.Passengers.First().PassengerType;
            decimal? averageFare = null;
            if (orderView.PATPrice != null)
            {
                averageFare = Calculator.Round(orderView.PATPrice.Fare/orderView.Flights.Count(), 1);
            }
            Flight preFlight = null;
            var increasing = policy.OemInfo==null?0:policy.OemInfo.ProfitType == OemProfitType.PriceMarkup ? policy.OemInfo.TotalProfit / orderView.Flights.Count() : 0;
            foreach (FlightView item in orderView.Flights.OrderBy(f => f.TakeoffTime))
            {
                Flight flight = Flight.GetFlight(item, passengerType, policy, averageFare);
                flight.Increasing = increasing;
                if (preFlight == null)
                {
                    // 第一段
                    flight.Serial = 1;
                }
                else
                {
                    if (preFlight.Arrival.Code == flight.Departure.Code)
                    {
                        flight.Serial = preFlight.Serial + 1;
                    }
                    else
                    {
                        // 缺口程会跳一段(中间跳过的是搭进去的那一段)
                        flight.Serial = preFlight.Serial + 2;
                    }
                }
                result.AddFlight(flight);
                preFlight = flight;
            }
            // 调整航班价格信息
            if (orderView.PATPrice != null)
            {
                if (policy.PolicyType == PolicyType.Special)
                {
                    // 特殊政策 不调整票面价
                }
                else if (policy.PolicyType == PolicyType.Bargain)
                {
                    // 对于按价格或折扣方式发布的特价政策不调整票面价
                    var promotionPolicy = policy.OriginalPolicy as BargainPolicyInfo;
                    if (promotionPolicy == null || promotionPolicy.PriceType == PriceType.Commission || promotionPolicy.PriceType == PriceType.Subtracting ||
                        promotionPolicy.Price < 0)
                    {
                        result.reviseFare(orderView.PATPrice.Fare);
                    }
                }
                else if (orderView.Source != OrderSource.PlatformOrder && !orderView.IsTeam && result.Flights.Count() == 1 && result.Flights.First().Bunk is GeneralBunk)
                {
                    // 非预订时，普通舱位的单程不调整票面价
                }
                else
                {
                    result.reviseFare(orderView.PATPrice.Fare);
                }

                // 机建
                result.reviseAirportFee(orderView.PATPrice.AirportTax);
            }
            foreach (PassengerView item in orderView.Passengers)
            {
                Passenger passenger = Passenger.GetPassenger(item);
                passenger.FillFlights(result.Flights);
                result.AddPassenger(passenger);
            }
            return result;
        }

        private void reviseFare(decimal totalFare)
        {
            decimal fareTotalBalance = Flights.Sum(item => item.Bunk.Fare) - totalFare;
            if (fareTotalBalance == 0) return;
            int flightCount = Flights.Count();
            decimal averageBalance = Calculator.Round(fareTotalBalance/flightCount, 1);
            int flightIndex = 0;
            decimal apportioned = 0;
            foreach (Flight flight in Flights)
            {
                flightIndex++;
                if (flightIndex == flightCount)
                {
                    flight.ReviseFare(flight.Price.Fare - (fareTotalBalance - apportioned));
                }
                else
                {
                    flight.ReviseFare(flight.Bunk.Fare - averageBalance);
                }
                apportioned += averageBalance;
            }
        }

        private void reviseAirportFee(decimal totalAirportFee)
        {
            decimal airportFeeTotalBalance = Flights.Sum(item => item.AirportFee) - totalAirportFee;
            if (airportFeeTotalBalance == 0) return;
            int flightCount = Flights.Count();
            decimal average = Calculator.Round(totalAirportFee/flightCount, 0);
            int flightIndex = 0;
            decimal apportioned = 0;
            foreach (Flight flight in Flights)
            {
                flightIndex++;
                if (flightIndex == flightCount)
                {
                    flight.AirportFee = totalAirportFee - apportioned;
                }
                else
                {
                    flight.AirportFee = average;
                }
                apportioned += average;
            }
        }
    }
}