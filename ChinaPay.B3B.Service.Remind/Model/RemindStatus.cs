using System.ComponentModel;

namespace ChinaPay.B3B.Service.Remind.Model {
    public enum RemindStatus {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        OrderedForPay,
        /// <summary>
        /// 同意改期,待支付
        /// </summary>
        [Description("待支付改期费")]
        AgreedForPostponeFee,
        /// <summary>
        /// 已申请,待确认座位
        /// </summary>
        [Description("待确认座位")]
        AppliedForConfirm,
        /// <summary>
        /// 已支付,待提供座位
        /// </summary>
        [Description("待提供座位")]
        PaidForSupply,
        /// <summary>
        /// 已支付,待出票
        /// </summary>
        [Description("待出票")]
        PaidForETDZ,
        /// <summary>
        /// 待退票
        /// </summary>
        [Description("待退票")]
        AppliedForRefund,
        /// <summary>
        /// 待废票
        /// </summary>
        [Description("待废票")]
        AppliedForScrap,
        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        AgreedForReturnMoney,
    }
}