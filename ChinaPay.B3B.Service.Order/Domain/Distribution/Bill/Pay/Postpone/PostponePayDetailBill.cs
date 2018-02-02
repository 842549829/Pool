using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone {
    public class PostponePayDetailBill : PayDetailBill {
        internal PostponePayDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }

        /// <summary>
        /// 改期费
        /// </summary>
        public decimal PostponeFee {
            get;
            internal set;
        }
    }
}
