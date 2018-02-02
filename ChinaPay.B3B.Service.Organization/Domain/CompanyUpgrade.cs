using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
  public class CompanyUpgrade
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 公司机构代码
        /// </summary>
        public string OrginationCode { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string OfficePhones { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 负责人电话
        /// </summary>
        public string ManagerPhone { get; set; }
        /// <summary>
        /// 紧急联系人姓名
        /// </summary>
        public string EmergencyName { get; set; }
        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string EmergencyPhone { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public CompanyType Type { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType AccountType { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }
    }
}
