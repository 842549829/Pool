using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using Data.DataMapping;

    public class CompanySettingsInfo {
        public WorkingSetting WorkingSetting { get; set; }
        public CompanyParameter Parameter { get; set; }
        public WorkingHours WorkingHours { get; set; }
    }
}
