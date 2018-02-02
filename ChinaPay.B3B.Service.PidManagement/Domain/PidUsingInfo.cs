using System;

namespace ChinaPay.B3B.Service.PidManagement.Domain
{
    public class PidUsingInformation
    {
        public Guid UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public Int64 Total { get; set; }
    }
}
