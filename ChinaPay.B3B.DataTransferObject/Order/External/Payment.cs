using System;

namespace ChinaPay.B3B.DataTransferObject.Order.External {
    public class Payment {
        /// <summary>
        /// 支付平台
        /// </summary>
        public Common.PayInterface PayInterface { get; set; }
        /// <summary>
        /// 支付方式  是否自动支付
        /// </summary>
        public bool IsAutoPay { get; set; }
        /// <summary>
        /// 交易号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
    }
}
