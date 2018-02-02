using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class RefundFailedRecord {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal ApplyformId { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public RefundBusinessType BusinessType { get; set; }
        /// <summary>
        /// 支付交易流水号
        /// </summary>
        public string PayTradeNo { get; set; }
        /// <summary>
        /// 请求退款时间
        /// </summary>
        public DateTime RefundTime { get; set; }
        /// <summary>
        /// 退款失败账号集合
        /// </summary>
        public string RefundFailedInfo { get; set; }
    }
}
