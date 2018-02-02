using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class CredentialsUpdateInfoQueryCondition {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 乘机人姓名
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// 提交日期
        /// </summary>
        public Range<DateTime> CommitDateRange { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool? Success { get; set; }
    }
}
