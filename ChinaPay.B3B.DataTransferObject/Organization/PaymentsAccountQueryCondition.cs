using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    using B3B.Common.Enums;

    public class PaymentsAccountQueryCondition
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 支付账号
        /// </summary>
        public string PaymentAccount { get; set; }
        /// <summary>
        /// 公司账号
        /// </summary>
        public string UserName { get; set; }
        ///// <summary>
        ///// 状态
        ///// </summary>
        //public CompanyStatus Status { get; set; }
        public bool? Enabled { get; set; }
        public bool Audited { get; set; }
    }
}
