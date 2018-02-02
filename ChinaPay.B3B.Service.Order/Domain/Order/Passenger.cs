using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.Core.Exception;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 乘机人信息
    /// </summary>
    public class Passenger {
        List<Ticket> _tickets = new List<Ticket>();

        internal Passenger()
            : this(Guid.NewGuid()) {
        }
        internal Passenger(Guid id) {
            this.Id = id;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name {
            get;
            internal set;
        }
        /// <summary>
        /// 乘机人类型
        /// </summary>
        public PassengerType PassengerType {
            get;
            internal set;
        }
        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType CredentialsType {
            get;
            internal set;
        }
        /// <summary>
        /// 证件号
        /// </summary>
        public string Credentials {
            get;
            internal set;
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone {
            get;
            internal set;
        }
        /// <summary>
        /// 价格信息
        /// </summary>
        public Price Price {
            get {
                return _tickets.Aggregate(Price.Zero, (current, item) => current + item.Price);
            }
        }
        /// <summary>
        /// 票号信息
        /// </summary>
        public IEnumerable<Ticket> Tickets {
            get {
                return _tickets.AsReadOnly();
            }
        }
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        internal void AddTicket(Ticket ticket) {
            if(ticket == null) throw new ArgumentNullException("ticket");
            if(!string.IsNullOrWhiteSpace(ticket.No) && _tickets.Exists(item => item.No == ticket.No)) throw new RepeatedItemException("不能重复添加同一票号");
            _tickets.Add(ticket);
        }
        internal void FillFlights(IEnumerable<Flight> flights) {
            _tickets = Ticket.GetTickets(flights).ToList();
        }
        internal void FillTicketNos(string settleCode, IEnumerable<string> ticketNos, ETDZMode mode) {
            if (_tickets.Count != ticketNos.Count()) throw new CustomException("输入票号数与实际票号数量不一致");
            var index = 0;
            foreach(var item in ticketNos) {
                _tickets[index].ETDZ(settleCode, item, mode);
                index++;
            }
        }
        internal bool ContainsTicket(string ticketNo) {
            return GetTicket(ticketNo) != null;
        }
        internal void UpdateCredentials(string credentials) {
            if(string.IsNullOrWhiteSpace(credentials)) throw new ArgumentNullException("credentials", "证件号不能为空");
            this.Credentials = credentials;
        }
        internal void UpdateTicketNo(string originalTicketNo, string settleCode, string ticketNo) {
            var ticket = GetTicket(originalTicketNo);
            ticket.UpdateTicketNo(settleCode, ticketNo);
        }
        internal Ticket GetTicket(string ticketNo) {
            return this._tickets.FirstOrDefault(item => item.No == ticketNo);
        }
        public Passenger Copy() {
            return new Passenger(this.Id) {
                Name = this.Name,
                PassengerType = this.PassengerType,
                CredentialsType = this.CredentialsType,
                Credentials = this.Credentials,
                Phone = this.Phone,
                _tickets = this._tickets,
                Birthday = this.Birthday
            };
        }
        internal void RefreshPrice() {
            _tickets.ForEach(ticket => ticket.RefreshPrice());
        }
        internal static Passenger GetPassenger(DataTransferObject.Order.PassengerView view) {
            return new Passenger {
                PassengerType = view.PassengerType,
                Name = view.Name.ToUpper(),
                CredentialsType = view.CredentialsType,
                Credentials = view.Credentials.ToUpper(),
                Phone = view.Phone,
                Birthday =  view.BirthDay
            };
        }


    }
}