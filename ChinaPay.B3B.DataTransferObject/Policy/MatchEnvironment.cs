using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    public class MatchEnvironment {
        public IEnumerable<PolicySettingInfo> PolicySettings { get; set; }
        public IEnumerable<PolicyHarmonyInfo> PolicyHarmonies { get; set; }
        public IEnumerable<Data.DataMapping.WorkingHours> WorkingHours { get; set; }
    }
}