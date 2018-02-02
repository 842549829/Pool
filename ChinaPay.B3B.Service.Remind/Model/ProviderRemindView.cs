namespace ChinaPay.B3B.Service.Remind.Model {
    /// <summary>
    /// 出票方订单提醒信息
    /// </summary>
    public class ProviderRemindView {
        /// <summary>
        /// 出票待处理数
        /// </summary>
        public int ETDZ { get; set; }
        /// <summary>
        /// 退票待处理数
        /// </summary>
        public int Refund { get; set; }
        /// <summary>
        /// 废票待处理数
        /// </summary>
        public int Scrap { get; set; }
        /// <summary>
        /// 退款待处理数
        /// </summary>
        public int ReturnMoney { get; set; }
        /// <summary>
        /// 确认座位待处理数
        /// </summary>
        public int Confirm { get; set; }
        /// <summary>
        /// 提供座位待处理数
        /// </summary>
        public int Supply { get; set; }
        /// <summary>
        /// 待支付
        /// </summary>
        public int PayOrder { get; set; }
        /// <summary>
        /// 同意改期,待支付
        /// </summary>
        public int PayPostponeFee { get; set; }
    }
}
