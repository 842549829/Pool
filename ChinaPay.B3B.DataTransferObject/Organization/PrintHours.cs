namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using Izual;

    public class WorkingHoursView {
        public Guid Company { get; set; }
        public Time WorkStart { get; set; }
        public Time WorkEnd { get; set; }
        public Time RefundStart { get; set; }
        public Time RefundEnd { get; set; }
    }
}
