using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class CompanyAuditInfo
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public CompanyType CompanyType { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType AccountType { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
        /// <summary>
        /// 审核类型
        /// </summary>
        public string AuditType { get; set; }
        /// <summary>
        /// 来源渠道
        /// </summary>
        public string SourceType { get; set; }
        /// <summary>
        /// 推广方Id
        /// </summary>
        public Guid SpreadId { get; set; }
    }
}
