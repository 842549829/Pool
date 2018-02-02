using System.ComponentModel;
namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 订单状态
    /// </summary>
    [System.Flags]
    public enum OrderStatus {
        /// <summary>
        /// 待确认资源
        /// </summary>
        [Description ("待确认资源")]
        Applied = 1,
        /// <summary>
        /// 确认失败
        /// </summary>
        [Description("确认失败")]
        ConfirmFailed = 2,
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        Ordered = 4,
        /// <summary>
        /// 已支付，待提供资源
        /// </summary>
        [Description("已支付，待提供资源")]
        PaidForSupply = 8,
        /// <summary>
        /// 拒绝提供资源
        /// </summary>
        [Description("拒绝提供资源")]
        DeniedWithSupply = 16,
        /// <summary>
        /// 已支付，待出票
        /// </summary>
        [Description("已支付，待出票")]
        PaidForETDZ = 32,
        /// <summary>
        /// 拒绝出票
        /// </summary>
        [Description("拒绝出票")]
        DeniedWithETDZ = 64,
        /// <summary>
        /// 取消出票
        /// </summary>
        [Description("取消出票")]
        Canceled = 128,
        /// <summary>
        /// 出票成功
        /// </summary>
        [Description("出票成功")]
        Finished = 256,
    }
}
