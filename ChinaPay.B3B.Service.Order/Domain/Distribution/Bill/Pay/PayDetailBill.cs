using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay {
    public abstract class PayDetailBill : DetailBill {
        internal PayDetailBill(Guid passenger, Flight flight)
            : base(passenger, flight) {
        }
    }
}
