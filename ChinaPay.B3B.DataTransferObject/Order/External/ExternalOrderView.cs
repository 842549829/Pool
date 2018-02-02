namespace ChinaPay.B3B.DataTransferObject.Order.External {
    public class ExternalOrderView {
        /// <summary>
        /// 订单号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}