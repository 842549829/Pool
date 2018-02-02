using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    public class AccountQueryParameter {
        public string AbbreviateName { get; set; }
        public string PaymentAccount { get; set; }
        public string Administrator { get; set; }
        public bool? Enabled { get; set; }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
