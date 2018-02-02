using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Common {
    public enum PayInterface {
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay,
        /// <summary>
        /// 汇付
        /// </summary>
        [Description("汇付")]
        ChinaPnr,
        /// <summary>
        /// 财付通
        /// </summary>
        [Description("财付通")]
        Tenpay,
        /// <summary>
        /// 快钱
        /// </summary>
        [Description("快钱")]
        _99Bill,
        /// <summary>
        /// 虚拟 即 国付通
        /// </summary>
        [Description("虚拟")]
        Virtual = 9
    }
    /// <summary>
    /// 支付账号类型
    /// </summary>
    public enum PayAccountType {
        /// <summary>
        /// 现金
        /// </summary>
        [Description("现金")]
        Cash,
        /// <summary>
        /// 信用
        /// </summary>
        [Description("信用")]
        Credit
    }
}