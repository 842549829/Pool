using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 分润失败记录查询条件
    /// </summary>
    public class RoyaltyFailedRecordQueryCondition {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 出票日期范围
        /// </summary>
        public Range<DateTime> ETDZDateRange { get; set; }
    }
}
