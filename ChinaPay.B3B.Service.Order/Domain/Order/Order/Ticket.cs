using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.Core.Exception;

namespace ChinaPay.B3B.Service.Order.Domain {
    public class Ticket {
        const string TicketNoPattern = @"^\d{10}$";
        const string SettleCodePattern = @"^\d{3}$";
        private const int FlightCapacity = 2;
        List<Flight> _flights;
        Price _price = null;

        internal Ticket() {
            _flights = new List<Flight>(FlightCapacity);
            Serial = 1;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Serial {
            get;
            internal set;
        }
        /// <summary>
        /// 结算代码
        /// </summary>
        public string SettleCode {
            get;
            internal set;
        }
        /// <summary>
        /// 票号
        /// </summary>
        public string No {
            get;
            internal set;
        }
        public DateTime? ETDZTime {
            get;
            internal set;
        }
        public ETDZMode? ETDZMode {
            get;
            internal set;
        }
        /// <summary>
        /// 航段信息
        /// </summary>
        public IEnumerable<Flight> Flights {
            get {
                return _flights.AsReadOnly();
            }
        }
        /// <summary>
        /// 价格信息
        /// </summary>
        public Price Price {
            get {
                return _price ?? (_price = _flights.Aggregate(Price.Zero, (current, item) => current + item.Price));
            }
            internal set {
                if(value == null) throw new ArgumentNullException("price", "客票价格信息不能为空");
                _price = value;
            }
        }

        internal int FlightStartSerial {
            get { return (Serial - 1) * FlightCapacity + 1; }
        }
        internal int FlightEndSerial {
            get { return Serial * FlightCapacity; }
        }

        internal void AddFlight(Flight flight) {
            if(flight == null) throw new ArgumentNullException("flight");
            if(_flights.Exists(item => Flight.IsSameFlight(flight, item))) throw new RepeatedItemException("不能重复添加相同航段");
            if(_flights.Count >= FlightCapacity) throw new CustomException("一个票号内最多只能包含两个航段");
            _flights.Add(flight);
            flight.Ticket = this;
        }
        internal void RemoveFlight(Flight flight) {
            _flights.Remove(flight);
        }
        internal void UpdateTicketNo(string settleCode, string ticketNo) {
            validateTicketNo(ticketNo);
            if(!string.IsNullOrEmpty(settleCode)) {
                validateSettleCode(settleCode.Trim());
                SettleCode = settleCode.Trim();
            }
            this.No = ticketNo.Trim();
        }
        internal void ETDZ(string settleCode, string ticketNo, ETDZMode mode) {
            UpdateTicketNo(settleCode, ticketNo);
            this.ETDZTime = DateTime.Now;
            this.ETDZMode = mode;
        }
        private void validateTicketNo(string ticketNo) {
            if(string.IsNullOrWhiteSpace(ticketNo)) throw new ArgumentNullException("ticketNo", "票号不能为空");
            if(!Regex.IsMatch(ticketNo, TicketNoPattern)) throw new CustomException("票号必须为10位数字");
        }
        private void validateSettleCode(string settleCode) {
            if(settleCode == null) throw new ArgumentNullException("settleCode", "结算码不能为空");
            if(!Regex.IsMatch(settleCode, SettleCodePattern)) throw new CustomException("票号必须为10位数字");
        }


        internal void Load(IEnumerable<Flight> flights) {
            foreach(var flight in flights) {
                if(FlightStartSerial <= flight.Serial && flight.Serial <= FlightEndSerial) {
                    AddFlight(flight);
                    flight.Ticket = this;
                }
            }
        }
        public Ticket Copy() {
            return new Ticket {
                Serial = this.Serial,
                SettleCode = this.SettleCode,
                No = this.No,
                ETDZTime = this.ETDZTime,
                ETDZMode = this.ETDZMode,
                _flights = this._flights
            };
        }
        internal void RefreshPrice() {
            _price = null;
        }
        internal static IEnumerable<Ticket> GetTickets(IEnumerable<Flight> flights) {
            var result = new List<Ticket>();
            Ticket ticket = null;
            var ticketSerial = 1;
            foreach(var item in flights) {
                if(ticket == null || item.Serial % FlightCapacity == 1) {
                    ticket = new Ticket {
                        SettleCode = item.Carrier.SettleCode,
                        Serial = ticketSerial
                    };
                    result.Add(ticket);
                    ticketSerial++;
                }
                ticket.AddFlight(item);
                item.Ticket = ticket;

                if(item.Serial % FlightCapacity == 0) {
                    ticket = null;
                }
            }
            return result;
        }
    }
}