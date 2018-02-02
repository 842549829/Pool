using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.Core;
using System.Linq;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 航班信息
    /// </summary>
    public class Flight {
        BaseBunk _bunk = null;
        decimal? _ybPrice = null;

        internal Flight(string carrierCode, string departure, string arrival, DateTime takeoffTime)
            : this(Guid.NewGuid(), new Carrier(carrierCode), new Airport(departure), new Airport(arrival), takeoffTime) {
            this.ReservateFlight = this.Id;
        }
        internal Flight(Guid id, Carrier carrier, Airport departure, Airport arrival, DateTime takeoffTime) {
            this.Id = id;
            this.Carrier = carrier;
            this.Departure = departure;
            this.Arrival = arrival;
            this.TakeoffTime = takeoffTime;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 所在客票
        /// </summary>
        public Ticket Ticket {
            get;
            internal set;
        }
        /// <summary>
        /// 航段序号
        /// </summary>
        public int Serial {
            get;
            internal set;
        }
        /// <summary>
        /// 乘运人
        /// </summary>
        public Carrier Carrier {
            get;
            private set;
        }
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo {
            get;
            internal set;
        }
        /// <summary>
        /// 是否共享航班
        /// </summary>
        public bool IsShare { get; internal set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string AirCraft {
            get;
            internal set;
        }
        /// <summary>
        /// 出发地
        /// </summary>
        public Airport Departure {
            get;
            private set;
        }
        /// <summary>
        /// 到达地
        /// </summary>
        public Airport Arrival {
            get;
            private set;
        }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public DateTime TakeoffTime {
            get;
            internal set;
        }
        /// <summary>
        /// 降落时间
        /// </summary>
        public DateTime LandingTime {
            get;
            internal set;
        }
        /// <summary>
        /// 舱位信息
        /// </summary>
        public BaseBunk Bunk {
            get { return _bunk; }
            internal set {
                _bunk = value;
                _bunk.SetYBPrice(this.YBPrice);
            }
        }
        /// <summary>
        /// 价格信息
        /// </summary>
        public Price Price {
            get {
                return new Price(this.Bunk.Fare, this.AirportFee, this.BAF);
            }
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            internal set;
        }
        /// <summary>
        /// 燃油附加费
        /// </summary>
        public decimal BAF {
            get;
            internal set;
        }
        /// <summary>
        /// 标准价
        /// </summary>
        public decimal YBPrice {
            get {
                if(!_ybPrice.HasValue) {
                    _ybPrice = FoundationService.QueryBasicPriceValue(this.Carrier.Code, this.Departure.Code, this.Arrival.Code, this.TakeoffTime);
                    if(_ybPrice <= 0) throw new CustomException("未获取到运价");
                }
                return _ybPrice.Value;
            }
            internal set {
                _ybPrice = value;
                if(_bunk != null) {
                    _bunk.SetYBPrice(value);
                }
            }
        }
        /// <summary>
        /// 关联航段
        /// </summary>
        internal Guid? AssociateFlight {
            get;
            set;
        }
        /// <summary>
        /// 订座时的原始航段
        /// </summary>
        public Guid ReservateFlight { get; internal set; }

        public Flight Copy() {
            return new Flight(Guid.NewGuid(), this.Carrier, this.Departure, this.Arrival, this.TakeoffTime) {
                Serial = this.Serial,
                FlightNo = this.FlightNo,
                AirCraft = this.AirCraft,
                LandingTime = this.LandingTime,
                Bunk = this.Bunk,
                AirportFee = this.AirportFee,
                BAF = this.BAF,
                YBPrice = this.YBPrice,
                AssociateFlight = this.Id,
                ReservateFlight = this.ReservateFlight,
                Ticket = this.Ticket,
                IsShare = this.IsShare,
                ReleaseFare = this.ReleaseFare,
                Increasing = this.Increasing
            };
        }
        internal bool IsSameVoyage(AirportPair airportPair) {
            if(airportPair == null) return false;
            return this.Departure.Code == airportPair.Departure && this.Arrival.Code == airportPair.Arrival;
        }
        internal bool IsSameVoyage(string departure, string arrival) {
            return this.Departure.Code == departure && this.Arrival.Code == arrival;
        }
        internal void ReviseFare(decimal fare) {
            this.Bunk.ReviseFare(fare);
        }
        internal decimal ReviseReleasedFare(decimal releasedFare) {
            if(this.Bunk is Domain.Bunk.SpecialBunk) {
                return (this.Bunk as Domain.Bunk.SpecialBunk).ReviseReleasedFare(releasedFare);
            } else {
                throw new NotSupportedException("仅支持修改特殊舱位的价格");
            }
        }
        internal void ModifyBunk(string code) {
            var bunks = FoundationService.QueryBunk(Carrier.Code, Departure.Code, Arrival.Code, TakeoffTime.Date, code);
            var ei = string.Empty;
            if(bunks.Any()) {
                var generalBunk = bunks.FirstOrDefault(item => item is Foundation.Domain.GeneralBunk);
                if(generalBunk == null) {
                    ei = bunks.First().EI;
                } else {
                    ei = generalBunk.EI;
                }
            }
            this.Bunk.ModifyCode(code, ei);
        }
        internal static bool IsSameFlight(Flight first, Flight second) {
            if(first != null && second != null) {
                return Carrier.Equals(first.Carrier, second.Carrier) && first.FlightNo == second.FlightNo && first.TakeoffTime.Date == second.TakeoffTime.Date;
            }
            return false;
        }
        internal static Flight GetFlight(DataTransferObject.Order.FlightView flightView, PassengerType passengerType, PolicyMatch.MatchedPolicy policy, decimal? fare) {
            var result = new Flight(flightView.Airline, flightView.Departure, flightView.Arrival, flightView.TakeoffTime) {
                TakeoffTime = flightView.TakeoffTime,
                LandingTime = flightView.LandingTime,
                FlightNo = flightView.FlightNo,
                AirCraft = flightView.AirCraft,
                IsShare = flightView.IsShare,
                ArrivalTerminal = flightView.ArrivalTerminal,
                DepartureTerminal = flightView.DepartureTerminal
            };
            result.Bunk = BaseBunk.CreateBunk(flightView.Airline, flightView.Bunk, flightView.Type, flightView.Departure,
                                              flightView.Arrival, flightView.TakeoffTime, passengerType, policy, result.YBPrice, fare);

            if(result.Bunk is SpecialBunk) {
                if((result.Bunk as SpecialBunk).ReleasedFare <= 0) {
                    throw new CustomException("价格异常，不能生成订单");
                }
            } else if(result.Bunk.Fare <= 0) {
                throw new CustomException("价格异常，不能生成订单");
            }

            decimal airportFee = 0M, baf = 0M;
            var bafView = FoundationService.QueryBAF(flightView.Airline, flightView.Departure, flightView.Arrival, flightView.TakeoffTime);
            if(passengerType == PassengerType.Adult) {
                airportFee = FoundationService.QueryAirportFee(flightView.AirCraft);
                baf = bafView == null ? 0 : bafView.Adult;
            } else {
                baf = bafView == null ? 0 : bafView.Child;
            }
            result.AirportFee = airportFee;
            result.BAF = baf;
            return result;
        }

        /// <summary>
        /// 出发航站楼
        /// </summary>
        public string DepartureTerminal { get; set; }

        /// <summary>
        /// 到达航站楼
        /// </summary>
        public string ArrivalTerminal { get; set; }

        /// <summary>
        /// 航段Y舱标准价
        /// </summary>
        public decimal ReleaseFare { get; set; }

        /// <summary>
        /// OEM加价值
        /// </summary>
        public decimal Increasing { get; set; }
    }
}