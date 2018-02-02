using System.ComponentModel;

namespace ChinaPay.B3B.Service.Locker {
    public enum LockRole {
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        System,
        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台")]
        Platform,
        /// <summary>
        /// 出票方
        /// </summary>
        [Description("出票方")]
        Provider,
        /// <summary>
        /// 资源方
        /// </summary>
        [Description("资源方")]
        Supplier,
        /// <summary>
        /// 采购方
        /// </summary>
        [Description("采购方")]
        Purchaser
    }
}
