using System;
using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    /// <summary>
    /// 申请单处理状态
    /// </summary>
    [Flags]
    public enum ApplyformProcessStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        [Description("待处理")] Applied = 1,

        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")] Processing = 2,

        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")] Finished = 4,
    }

    /// <summary>
    /// 差额退款申请单处理状态
    /// </summary>
    [Flags]
    public enum BalanceRefundProcessStatus
    {
        /// <summary>
        /// 待平台处理
        /// </summary>
        [Description("待平台处理")] AppliedForPlatform = 1,

        /// <summary>
        /// 待出票方处理
        /// </summary>
        [Description("待出票方处理")] AppliedForProvider = 2,

        /// <summary>
        /// 出票方
        /// 同意退/废票,待财务退款
        /// </summary>
        [Description("出票方 同意,待财务退款")] AgreedByProviderBusiness = 4,

        /// <summary>
        /// 出票方
        /// 拒绝退/废票,待平台处理
        /// </summary>
        [Description("出票方 拒绝,待平台处理")] DeniedByProviderBusiness = 8,

        /// <summary>
        /// 财务拒绝退款，待业务重新处理
        /// </summary>
        [Description("财务拒绝退款，待业务重新处理")] DeniedByProviderTreasurer = 16,

        /// <summary>
        /// 退款结束
        /// </summary>
        [Description("处理结束")] Finished = 32,

        /// <summary>
        /// 平台拒绝退款
        /// </summary>
        [Description("拒绝退款")] DenyRefund = 64,
    }

    /// <summary>
    /// 退/废票申请单状态
    /// </summary>
    [Flags]
    public enum RefundApplyformStatus
    {
        /// <summary>
        /// 待平台处理
        /// </summary>
        [Description("待平台处理")] AppliedForPlatform = 1,

        /// <summary>
        /// 待取消编码
        /// </summary>
        [Description("待取消编码")] AppliedForCancelReservation = 2,

        /// <summary>
        /// 待出票方处理
        /// </summary>
        [Description("待出票方处理")] AppliedForProvider = 4,

        /// <summary>
        /// 出票方
        /// 同意退/废票,待财务退款
        /// </summary>
        [Description("出票方 同意退/废票,待财务退款")] AgreedByProviderBusiness = 8,

        /// <summary>
        /// 出票方
        /// 拒绝退/废票,待平台处理
        /// </summary>
        [Description("出票方 拒绝退/废票,待平台处理")] DeniedByProviderBusiness = 16,

        /// <summary>
        /// 财务拒绝退款，待业务重新处理
        /// </summary>
        [Description("财务拒绝退款，待业务重新处理")] DeniedByProviderTreasurer = 32,

        /// <summary>
        /// 拒绝退/废票
        /// </summary>
        [Description("拒绝退/废票")] Denied = 64,

        /// <summary>
        /// 已退/废票
        /// </summary>
        [Description("已退/废票")] Refunded = 128,
    }

    /// <summary>
    /// 改期申请单状态
    /// </summary>
    [Flags]
    public enum PostponeApplyformStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Applied = 1,

        /// <summary>
        /// 同意改期，待支付改期费
        /// </summary>
        Agreed = 2,

        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// 已支付，待改期
        /// </summary>
        Paid = 8,

        /// <summary>
        /// 拒绝改期
        /// </summary>
        Denied = 16,

        /// <summary>
        /// 已改期
        /// </summary>
        Postponed = 32
    }

    ///// <summary>
    ///// 升舱申请单状态
    ///// </summary>
    //public enum UpgradApplyformStatus {
    //    /// <summary>
    //    /// 申请中
    //    /// </summary>
    //    Applied,
    //    /// <summary>
    //    /// 同意升舱
    //    /// </summary>
    //    Agreed,
    //    ///// <summary>
    //    ///// 拒绝升舱
    //    ///// </summary>
    //    //Denied,
    //}
}