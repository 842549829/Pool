using System.ComponentModel;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    public enum TradeRoleType {
        [Description("采购方")]
        Purchaser,
        [Description("资源方")]
        Supplier,
        [Description("出票方")]
        Provider,
        [Description("平台分润")]
        Platform,
        [Description("分润方")]
        Royalty
    }
}