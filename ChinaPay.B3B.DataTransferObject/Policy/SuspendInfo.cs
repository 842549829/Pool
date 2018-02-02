
namespace ChinaPay.B3B.DataTransferObject.Policy {
    using System;
    using System.Collections.Generic;
    using B3B.Common.Enums;

    /// <summary>
    /// 政策挂起信息
    /// </summary>
    public class SuspendInfo {
        /// <summary>
        /// 公司类型
        /// </summary>
        public CompanyType CompanyType { get; set; }
        /// <summary>
        /// 被挂起政策的公司 Id
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 公司挂起的航空公司
        /// </summary>
        public IEnumerable<string> SuspendByCompany { get; set; }
        /// <summary>
        /// 平台挂起的航空公司
        /// </summary>
        public IEnumerable<string> SuspendByPlatform { get; set; }
    }
}
