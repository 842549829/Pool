using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    public abstract class RefundDetailBill : DetailBill {
        internal RefundDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }
    }
}
