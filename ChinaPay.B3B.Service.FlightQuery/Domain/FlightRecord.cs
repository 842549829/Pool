using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    class FlightRecord {
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime FlightDate { get; set; }
        public string Content { get; set; }
    }
}