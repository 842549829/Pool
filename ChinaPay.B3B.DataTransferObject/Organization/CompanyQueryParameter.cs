using System;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class CompanyQueryParameter {
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string UserNo { get; set; }
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
        public CompanyAuditStatus? CompanyAuditStatus { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public CompanyType? Type { get; set; }
        /// <summary>
        /// 帐号类别 企业/个人
        /// </summary>
        public AccountBaseType? AccountType { get; set; }
    }

}
