using System;

namespace ChinaPay.B3B.Service.Statistic {
    internal class GeneralProductSpeedStatisticInfo {
        public Guid Company { get; set; }
        public string Carrier { get; set; }
        public Common.Enums.TicketType TicketType { get; set; }
        public byte Type { get; set; }
        public int Speed { get; set; }
    }
}
