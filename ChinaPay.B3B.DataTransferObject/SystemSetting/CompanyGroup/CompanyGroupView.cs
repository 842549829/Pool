using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup {
    /// <summary>
    /// 公司组
    /// </summary>
    public class CompanyGroupView {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否只能采购自己的政策
        /// </summary>
        public bool PurchaseMyPolicyOnly { get; set; }
        /// <summary>
        /// 限制信息集合
        /// </summary>
        public IEnumerable<Limitation> Limitations { get; set; }
    }
}
