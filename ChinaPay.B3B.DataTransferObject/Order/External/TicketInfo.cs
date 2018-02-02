using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Order.External {
    public class TicketInfo {
        /// <summary>
        /// 新编码
        /// </summary>
        public Common.PNRPair NewPNR { get; set; }
        /// <summary>
        /// 结算代码
        /// </summary>
        public string SettleCode { get; set; }
        /// <summary>
        /// 票号信息
        /// </summary>
        public IEnumerable<TicketNoView.Item> TicketNos { get; set; }
    }
}
