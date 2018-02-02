using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone {
    public class PostponeRefundDetailBill : RefundDetailBill {
        internal PostponeRefundDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }
    }
}