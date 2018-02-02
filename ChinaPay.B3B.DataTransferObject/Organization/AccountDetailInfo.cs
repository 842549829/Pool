using System;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class AccountDetailInfo{
        public CompanyType CompanyType { get; set; }
        public Guid CompanyId { get; set; }
        public string AbbreviateName { get; set; }
        public string Administrator { get; set; }
        public string PaymentInterface { get; set; }
        public AccountType AccountType { get; set; }
        public string Account { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
