using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 退款失败记录查询条件
    /// </summary>
    public class RefundFailedRecordQueryCondition {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal? ApplyformId { get; set; }
        /// <summary>
        /// 退款日期范围
        /// </summary>
        public Range<DateTime> RefundDateRange { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public RefundBusinessType? BusinessType { get; set; }
    }
}
