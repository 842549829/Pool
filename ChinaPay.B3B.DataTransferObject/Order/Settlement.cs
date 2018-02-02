namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 结算信息
    /// </summary>
    public class Settlement {
        /// <summary>
        /// 结算价
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 返点
        /// </summary>
        public decimal Rebate { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
    }
}
