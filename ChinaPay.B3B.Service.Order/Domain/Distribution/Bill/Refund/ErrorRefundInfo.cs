using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    public class ErrorRefundInfo {
        List<Guid> _passengers;
        List<ErrorRefundFlight> _flights;

        public ErrorRefundInfo(decimal orderId, decimal applyformId) {
            OrderId = orderId;
            ApplyformId = applyformId;
            _passengers = new List<Guid>();
            _flights = new List<ErrorRefundFlight>();
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId {
            get;
            private set;
        }
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal ApplyformId {
            get;
            private set;
        }

        public IEnumerable<ErrorRefundFlight> Flights {
            get {
                return _flights;
            }
        }

        public IEnumerable<Guid> Passengers {
            get {
                return _passengers;
            }
        }

        public void AddFlight(ErrorRefundFlight flight) {
            if(flight != null) {
                if(!ContainsFlight(flight.Id)) {
                    _flights.Add(flight);
                }
            }
        }

        /// <summary>
        /// 添加该航段下待退的乘机人
        /// </summary>
        public void AddPassenger(Guid passenger) {
            if(!ContainsPassenger(passenger)) {
                _passengers.Add(passenger);
            }
        }
        /// <summary>
        /// 是否包含某航段
        /// </summary>
        public bool ContainsFlight(Guid flight) {
            return _flights.Exists(f => f.Id == flight);
        }
        /// <summary>
        /// 是否包含某乘机人
        /// </summary>
        public bool ContainsPassenger(Guid passenger) {
            return _passengers.Contains(passenger);
        }
        internal bool Contains(Guid flight, Guid passenger) {
            return ContainsFlight(flight) && ContainsPassenger(passenger);
        }
        internal ErrorRefundFlight GetFlight(Guid flight) {
            return _flights.FirstOrDefault(f => f.Id == flight);
        }
    }
    public class ErrorRefundFlight {
        public ErrorRefundFlight(Guid id) {
            Id = id;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 补退金额
        /// </summary>
        public decimal Amount {
            get;
            set;
        }
    }
}