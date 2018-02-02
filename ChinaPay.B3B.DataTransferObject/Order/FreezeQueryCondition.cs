using System;
using System.ComponentModel;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 冻结/解冻信息查询条件
    /// </summary>
    public class FreezeQueryCondition {
        public decimal? OrderId { get; set; }
        public decimal? ApplyformId { get; set; }
        public bool? Success { get; set; }
        public FreezeType? Type { get; set; }
        public Range<DateTime> RequestDate { get; set; }
    }
    public enum FreezeType {
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Freeze,
        /// <summary>
        /// 解冻
        /// </summary>
        [Description("解冻")]
        Unfreeze
    }
}