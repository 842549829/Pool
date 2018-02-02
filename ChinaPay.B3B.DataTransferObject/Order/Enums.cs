using System;
using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public enum OrderSource {
        /// <summary>
        /// 平台预订
        /// </summary>
        [Description("平台预订")]
        PlatformOrder,
        /// <summary>
        /// 编码导入
        /// </summary>
        [Description("编码导入")]
        CodeImport,
        /// <summary>
        /// 内容导入
        /// </summary>
        [Description("内容导入")]
        ContentImport,
        /// <summary>
        /// 接口导入
        /// </summary>
        [Description("接口导入")]
        InterfaceOrder,
        /// <summary>
        /// 接口预订
        /// </summary>
        [Description("接口预订")]
        InterfaceReservaOrder,
    }
    /// <summary>
    /// 退票类型
    /// </summary>
    public enum RefundType {
        /// <summary>
        /// 升舱全退
        /// </summary>
        [Description("升舱全退")]
        Upgrade,
        /// <summary>
        /// 自愿按客规退票
        /// </summary>
        [Description("自愿按客规退票")]
        Voluntary,
        /// <summary>
        /// 非自愿退票
        /// </summary>
        [Description("非自愿退票")]
        Involuntary,
        /// <summary>
        /// 特殊原因退票
        /// </summary>
        [Description("特殊原因退票")]
        SpecialReason
    }
    /// <summary>
    /// 申请类型
    /// </summary>
    [Description("申请类型"), Flags]
    public enum ApplyformType {
        /// <summary>
        /// 退票
        /// </summary>
        [Description("退票")]
        Refund = 1,
        /// <summary>
        /// 废票
        /// </summary>
        [Description("废票")]
        Scrap = 2,
        /// <summary>
        /// 改期
        /// </summary>
        [Description("改期")]
        Postpone = 4,
        /// <summary>
        /// 差错退款
        /// </summary>
        [Description("差错退款")]
        BlanceRefund = 8,
    }
    /// <summary>
    /// 退款业务类型
    /// </summary>
    public enum RefundBusinessType {
        /// <summary>
        /// 取消出票
        /// </summary>
        [Description("取消出票")]
        Cancel,
        /// <summary>
        /// 拒绝提供资源
        /// </summary>
        [Description("拒绝提供资源")]
        DenySupply,
        /// <summary>
        /// 退票
        /// </summary>
        [Description("退票")]
        Refund,
        /// <summary>
        /// 废票
        /// </summary>
        [Description("废票")]
        Scrap,
        /// <summary>
        /// 拒绝改期
        /// </summary>
        [Description("拒绝改期")]
        DenyPostpone,
        /// <summary>
        /// 超出支付时限后才支付
        /// </summary>
        [Description("支付超时")]
        PayTimeout,
        /// <summary>
        /// 差额退款
        /// </summary>
        [Description("差额退款")]
        BalanceRefund
    }
    /// <summary>
    /// 产品类型
    /// </summary>
    public enum ProductType {
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        General,
        /// <summary>
        /// 特价
        /// </summary>
        [Description("特价")]
        Promotion,
        /// <summary>
        /// 特殊
        /// </summary>
        [Description("特殊")]
        Special,
        /// <summary>
        /// 团队
        /// </summary>
        [Description("团队")]
        Team,
        /// <summary>
        /// 缺口
        /// </summary>
        [Description("缺口")]
        Notch
    }
    [Flags]
    public enum OrderRole {
        [Description("平台")]
        Platform = 1,
        [Description("采购")]
        Purchaser = 2,
        [Description("出票")]
        Provider = 4,
        [Description("产品")]
        Supplier = 8,
        [Description("OEM")]
        OEMOwner = 16,

    }
}
