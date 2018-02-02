using System;

namespace ChinaPay.B3B.DataTransferObject.Policy {

    /// <summary>
    /// 普通默认政策
    /// </summary>
    public class NormalDefaultPolicyInfo
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 成人默认出票方
        /// </summary>
        public Guid AdultProviderId { get; set; }
        /// <summary>
        /// 成人默认出票方名称
        /// </summary>
        public string AdultProviderName { get; set; }
        /// <summary>
        /// 成人默认出票方简称
        /// </summary>
        public string AdultProviderAbbreviateName { get; set; }

        /// <summary>
        /// 成人默认佣金
        /// </summary>
        public decimal AdultCommission { get; set; }
        /// <summary>
        /// 儿童默认出票方
        /// </summary>
        public Guid ChildProviderId { get; set; }
        /// <summary>
        /// 儿童默认出票方名称
        /// </summary>
        public string ChildProviderName { get; set; }
        /// <summary>
        /// 儿童默认出票方简称
        /// </summary>
        public string ChildProviderAbbreviateName { get; set; }
        /// <summary>
        /// 儿童默认佣金
        /// </summary>
        public decimal ChildCommission { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
    }
}
