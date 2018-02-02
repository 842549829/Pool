using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 退/废票信息
    /// </summary>
    public class RefundProcessView {
        /// <summary>
        /// 新编码信息
        /// </summary>
        public PNRPair NewPNR { get; set; }
        /// <summary>
        /// 退/废票手续费信息
        /// </summary>
        public IEnumerable<RefundFeeView> Items { get; set; }
    }
}