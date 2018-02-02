using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal {
    public class NormalRefundDetailBill : RefundDetailBill {
        internal NormalRefundDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }

        /// <summary>
        /// 退款手续费
        /// </summary>
        public decimal RefundFee {
            get;
            internal set;
        }
        /// <summary>
        /// 退款手续费率
        /// </summary>
        public decimal RefundRate {
            get;
            internal set;
        }

        /// <summary>
        /// 退还佣金
        /// </summary>
        public decimal Commission {
            get;
            internal set;
        }

        /// <summary>
        /// 退还加价金额
        /// </summary>
        public decimal Increasing {
            get;
            internal set;
        }

        /// <summary>
        /// 退还服务费
        /// </summary>
        public decimal ServiceCharge {
            get;
            internal set;
        }
    }
}
