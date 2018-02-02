namespace ChinaPay.B3B.DataTransferObject.Policy {
    using System;
    using B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Common;
    using ChinaPay.Core;

    /// <summary>
    /// 政策比较条件
    /// </summary>
    public class PolicyCompareCondition {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 航班日期范围
        /// </summary>
        public Range<DateTime?> FlightDateRange { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType? VoyageType { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType? TicketType { get; set; }
    }
}