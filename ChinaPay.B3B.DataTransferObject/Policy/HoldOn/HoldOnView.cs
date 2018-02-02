using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Policy.HoldOn {
    public class HoldOnView {
        /// <summary>
        /// 平台挂起项集合
        /// </summary>
        public IEnumerable<HoldOnItem> Platform { get; set; }
        /// <summary>
        /// 发布方挂起项集合
        /// </summary>
        public IEnumerable<HoldOnItem> Publisher { get; set; }
    }
}
