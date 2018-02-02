namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using B3B.Common.Enums;

    public class SupplierCreatureInfo {
        public Guid ProviderId { get; set; }

        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 使用期限开始时间
        /// </summary>
        public DateTime? PeriodStartOfUse { get; set; }
        /// <summary>
        /// 使用期限结束时间
        /// </summary>
        public DateTime? PeriodEndOfUse { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }

        public string Contact { get; set; }
        public string ContactPhone { get; set; }
        public string Faxes { get; set; }
        public string Email { get; set; }
        public string MSN { get; set; }
        public string QQ { get; set; }
        public string ZipCode { get; set; }
        public string Area { get; set; }
        public CompanyType CompanyType { get; set; }

    }
}
