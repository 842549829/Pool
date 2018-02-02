using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    /// <summary>
    /// 用户角色
    /// </summary>
    [System.Flags]
    public enum UserRole {
        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台|平台工作人员")]
        Platform = 1,
        /// <summary>
        /// 出票方
        /// </summary>
        [Description("出票方|提供政策产品者")]
        Provider = 2,
        /// <summary>
        /// 产品方
        /// </summary>
        [Description("产品方|提供特殊政策者")]
        Supplier = 4,
        /// <summary>
        /// 采购方
        /// </summary>
        [Description("采购方|为平台代理利润者")]
        Purchaser = 8,
        /// <summary>
        /// 分销OEM
        /// </summary>
        [Description("分销OEM|分销OEM")]
        DistributionOEM = 16
    }
    public enum Website {
        /// <summary>
        /// 运维平台
        /// </summary>
        [Description("运维平台")]
        Maintenance,
        /// <summary>
        /// 交易平台
        /// </summary>
        [Description("交易平台")]
        Transaction
    }
}