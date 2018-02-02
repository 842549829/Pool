using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;

    public class PolicySettingQueryParameter {
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        // public VoyageType? VoyageType { get; set; }
        /// <summary>
        /// true表示 扣点 false表示贴点 null表示所有
        /// </summary>
        public bool? Rebate { get; set; }
        public DateTime? EffectiveTimeStart { get; set; }
        public DateTime? EffectiveTimeEnd { get; set; }
        public int IsTieDian { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
