using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class CompanyQueryConditionInfo
    {
        /// <summary>
        /// 公司全称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 公司账号
        /// </summary>
        public string Administrator { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool? Audited { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public CompanyType? Type { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType? AccountType { get; set; }
        /// <summary>
        /// 申请时间（起始）
        /// </summary>
        public DateTime? TimeStart { get; set; }
        /// <summary>
        ///申请时间（结束）
        /// </summary>
        public DateTime? TimeEnd { get; set; }
        /// <summary>
        /// 每页记录数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
       /// <summary>
       /// 审核类型
       /// </summary>
        public AuditType? AuditType { get; set; }
    }
}
