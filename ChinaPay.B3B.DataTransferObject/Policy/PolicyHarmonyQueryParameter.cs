using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;

    public class PolicyHarmonyQueryParameter {
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public DateTime? EffectTimeStart { get; set; }
        public DateTime? EffectTimeEnd { get; set; }
        public PolicyType? PolicyType { get; set; }
    }
}
