using System;

namespace ChinaPay.B3B.Service.Remind.Model {
    public class RemindInfo {
        public decimal Id { get; set; }
        public RemindStatus Status { get; set; }
        public string Carrier { get; set; }
        public Guid Acceptor { get; set; }

        public string CustomNO { get; set; }
    }
}