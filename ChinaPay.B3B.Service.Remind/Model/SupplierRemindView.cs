namespace ChinaPay.B3B.Service.Remind.Model {
    /// <summary>
    /// 资源方订单提醒信息
    /// </summary>
    public class SupplierRemindView {
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
