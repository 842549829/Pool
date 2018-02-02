using System;

namespace ChinaPay.B3B.Service.Statistic {
    internal class SpecialProductSupplyInfo {
        public Guid Company { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int TicketCount { get; set; }
        public bool Success { get; set; }
        public DateTime SupplyDate { get; set; }
    }
}