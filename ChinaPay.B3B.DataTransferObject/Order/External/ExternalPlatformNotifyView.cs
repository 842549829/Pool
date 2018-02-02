namespace ChinaPay.B3B.DataTransferObject.Order.External {
    /// <summary>
    /// 外平台通知信息
    /// </summary>
    public abstract class ExternalPlatformNotifyView {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get; set; }
        /// <summary>
        /// 外平台业务编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 处理成功的响应
        /// </summary>
        public string Response { get; set; }
    }
    /// <summary>
    /// 支付成功通知信息
    /// </summary>
    public class PaySuccessNotifyView : ExternalPlatformNotifyView {
        /// <summary>
        /// 支付信息
        /// </summary>
        public Payment Payment { get; set; }
    }
    /// <summary>
    /// 出票成功通知信息
    /// </summary>
    public class ETDZSuccessNotifyView : ExternalPlatformNotifyView {
        /// <summary>
        /// 票号信息
        /// </summary>
        public TicketInfo Ticket { get; set; }
    }
    /// <summary>
    /// 暂不能出票通知信息
    /// </summary>
    public class DenyETDZNotifyView : ExternalPlatformNotifyView {
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
    /// <summary>
    /// 出票失败通知信息
    /// </summary>
    public class ETDZFailedNotifyView : ExternalPlatformNotifyView {
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
    /// <summary>
    /// 取消订单通知信息
    /// </summary>
    public class CancelOrderNotifyView : ExternalPlatformNotifyView {
    }
}