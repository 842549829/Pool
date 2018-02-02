using System;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class RefundFeeView {
        public RefundFeeView(AirportPair airportPair, decimal rate, decimal fee) {
            this.AirportPair = airportPair;
            this.Rate = rate;
            this.Fee = fee;
        }
        /// <summary>
        /// 航段信息
        /// </summary>
        public AirportPair AirportPair { get; private set; }
        /// <summary>
        /// 手续费率
        /// </summary>
        public decimal Rate { get; private set; }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal Fee { get; private set; }
    }

    /// <summary>
    ///差额退款费
    /// </summary>
    public class BalanceRefundFeeView
    {
        public Guid Voyage { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Fee { get; set; }
    }
}