using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup {
    /// <summary>
    /// 公司组列表查询条件
    /// </summary>
    public class CompanyGroupQueryCondition {
        /// <summary>
        /// 组名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建账号
        /// </summary>
        public string RegisterAccount { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public Range<DateTime?> RegisterDateRange { get; set; }
    }
}