using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using Data.DataMapping;

    public class CompanyGroupLimitationInfo : CompanyGroupLimitation {
        public Guid Company { get; set; }
        public Guid Group { get; set; }
    }
}
