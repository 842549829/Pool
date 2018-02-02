namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using ChinaPay.B3B.DataTransferObject.Permission;
    using ChinaPay.B3B.Common.Enums;

    /// <summary>
    /// 推广公司列表查询条件
    /// </summary>
    public class GenerilazationQueryCondition {
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName {
            get;
            set;
        }
        /// <summary>
        /// 公司账号
        /// </summary>
        public string UserName {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public CompanyStatus? Status {
            get;
            set;
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public ChinaPay.Core.Range<DateTime>? RegisterDate {
            get;
            set;
        }
        /// <summary>
        /// 平台角色
        /// </summary>
        public CompanyType UserRoleValue { get; set; }
    }
}
