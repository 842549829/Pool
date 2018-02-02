using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal {
    public class NormalPayDetailBill : PayDetailBill {
        internal NormalPayDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission {
            get;
            internal set;
        }

        /// <summary>
        /// 加价金额
        /// </summary>
        public decimal Increasing {
            get;
            internal set;
        }

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceCharge {
            get;
            internal set;
        }
    }
}