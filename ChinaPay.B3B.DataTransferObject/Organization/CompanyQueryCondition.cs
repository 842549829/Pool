namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using B3B.Common.Enums;
    using ChinaPay.Core;

    /// <summary>
    /// 平台查询单位列表条件
    /// </summary>
    public class CompanyQueryCondition {
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
        public string Account {
            get;
            set;
        }
        /// <summary>
        /// 省份代码
        /// </summary>
        public string Province {
            get;
            set;
        }
        /// <summary>
        /// 城市代码
        /// </summary>
        public string City {
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
        /// 类型
        /// </summary>
        public CompanyType? Type {
            get;
            set;
        }
        /// <summary>
        /// 注册日期
        /// </summary>
        public Range<DateTime?> RegisterDate {
            get;
            set;
        }
        /// <summary>
        /// 审核日期
        /// </summary>
        public Range<DateTime?> AuditDate {
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
    }
}