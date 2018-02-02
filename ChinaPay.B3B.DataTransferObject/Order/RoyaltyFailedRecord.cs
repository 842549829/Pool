using System;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class RoyaltyFailedRecord {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime ETDZTime { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal TradeAmount { get; set; }
        /// <summary>
        /// 分润信息
        /// </summary>
        public string RoyaltyInfo { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string FailedReason { get; set; }
    }
}