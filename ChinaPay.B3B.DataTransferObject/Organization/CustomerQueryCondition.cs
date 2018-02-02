using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using ChinaPay.B3B.Common.Enums;
    /// <summary>
    /// 常旅客查询条件
    /// </summary>
    public class CustomerQueryCondition {
        public Guid Company { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex? Sex { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType? CredentialsType { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        public string Credentials { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
    }
}
