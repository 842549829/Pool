using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup {
    public class Limitation {
        /// <summary>
        /// 航空公司集合
        /// </summary>
        public IEnumerable<string> Airlines { get; set; }
        /// <summary>
        /// 出发机场集合
        /// </summary>
        public IEnumerable<string> Departures { get; set; }
        /// <summary>
        /// 是否只能采购自己的政策
        /// 如果是，则需设置默认返点
        /// 用于未发布政策时
        /// </summary>
        public bool PurchaseMyPolicyOnlyForNonePolicy { get; set; }
        /// <summary>
        /// 默认返点
        /// 用于未发布政策时
        /// </summary>
        public decimal? DefaultRebateForNonePolicy { get; set; }
    }
}
