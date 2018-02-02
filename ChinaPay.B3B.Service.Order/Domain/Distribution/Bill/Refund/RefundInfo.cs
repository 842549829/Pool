using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    /// <summary>
    /// 退款信息
    /// </summary>
    public class RefundInfo {
        List<RefundFlight> _flights;
        List<Guid> _passengers;

        public RefundInfo(decimal orderId, decimal applyformId) {
            OrderId = orderId;
            ApplyformId = applyformId;
            _flights = new List<RefundFlight>();
            _passengers = new List<Guid>();
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
        /// <summary>
        /// 是否包含资源方
        /// </summary>
        public bool HasSupplier {
            get;
            set;
        }

        public IEnumerable<RefundFlight> Flights {
            get {
                return _flights;
            }
        }

        public IEnumerable<Guid> Passengers {
            get {
                return _passengers;
            }
        }

        public void AddFlight(RefundFlight flight) {
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

        internal decimal GetRate(Guid flight, Guid passenger) {
            var refundFlight = GetFlight(flight);
            return refundFlight == null ? 0 : refundFlight.Rate;
        }
        internal RefundFlight GetFlight(Guid flight) {
            return _flights.Find(item => item.Id == flight);
        }

        /// <summary>
        /// 是否包含某乘机人
        /// </summary>
        public bool ContainsPassenger(Guid passenger) {
            return _passengers.Contains(passenger);
        }
        public bool ContainsFlight(Guid flight) {
            return _flights.Exists(f => f.Id == flight);
        }
        internal bool Contains(Guid flight, Guid passenger) {
            return ContainsFlight(flight) && ContainsPassenger(passenger);
        }
    }

    public class RefundFlight {
        private decimal _rate;

        public RefundFlight(Guid id) {
            Id = id;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 手续费率
        /// </summary>
        public decimal Rate {
            get { return _rate; }
            set {
                if(value > 1 || value < 0)
                    throw new Exception("费率范围 (0 - 1)");
                _rate = value;
            }
        }

        public decimal FeeForProvider { get; set; }

        public decimal FeeForSupplier { get; set; }

        public decimal FeeForPurchaser { get; set; }

        /// <summary>
        /// 退还服务费金额
        /// 若为空，则不需要退还服务费，否则为具体金额
        /// </summary>
        public decimal? RefundServiceCharge { get; set; }
    }
}