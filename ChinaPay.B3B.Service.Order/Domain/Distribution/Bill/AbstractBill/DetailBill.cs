using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill {
    /// <summary>
    /// 账单明细
    /// </summary>
    public abstract class DetailBill {
        private decimal? _amount = null;

        protected DetailBill(Guid passenger, Flight flight) {
            this.Passenger = passenger;
            this.Flight = flight;
        }
        /// <summary>
        /// 乘机人编号
        /// </summary>
        public Guid Passenger {
            get;
            private set;
        }
        /// <summary>
        /// 航段信息
        /// </summary>
        public Flight Flight {
            get;
            private set;
        }

        /// <summary>
        /// 预期金额
        /// </summary>
        public decimal Anticipation {
            get;
            internal set;
        }
        /// <summary>
        /// 交易手续费
        /// </summary>
        public decimal TradeFee {
            get;
            internal set;
        }
        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal Amount {
            get {
                if(_amount == null) {
                    _amount = this.Anticipation + this.TradeFee;
                }
                return _amount.Value;
            }
            internal set {
                _amount = value;
            }
        }
        internal void RefreshAmount() {
            _amount = null;
        }
    }
}