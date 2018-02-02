using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    using B3B.Common.Enums;

    /// <summary>
    /// 特价政策
    /// </summary>
    public class PromotionPolicyRegisterView
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 去程班期
        /// </summary>
        public Schedule OutwardScheduleDay { get; set; }
        
        /// <summary>
        /// 去程航班
        /// </summary>
        public string DepartureFlightLimit { get; set; }
        /// <summary>
        /// 航班限制
        /// </summary>
        public LimitType FlightLimit { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public int AheadDays { get; set; }
        /// <summary>
        /// 退改签规定
        /// </summary>
        public RefundAndReschedulingProvision Provision { get; set; }
        /// <summary>
        /// 政策备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 政策组集合
        /// </summary>
        public IEnumerable<Item> Items { get; set; }
        /// <summary>
        /// 特价政策组
        /// </summary>
        public class Item
        {
            /// <summary>
            /// 航班日期
            /// </summary>
            public Range<DateTime> FlightDateRange { get; set; }
            /// <summary>
            /// 出票开始日期
            /// </summary>
            public DateTime ETDZBeginDate { get; set; }
            /// <summary>
            /// 舱位
            /// </summary>
            public string Bunk { get; set; }
            /// <summary>
            /// 发布方式
            /// </summary>
            public PriceType Mode { get; set; }
            /// <summary>
            /// 价格或折扣
            /// </summary>
            public decimal PriceOrDiscount { get; set; }
            /// <summary>
            /// 客票类型
            /// </summary>
            public TicketType TicketType { get; set; }
            /// <summary>
            /// 返佣
            /// </summary>
            public Deduction Deduction { get; set; }
            /// <summary>
            /// 是否需要审核
            /// </summary>
            public bool RequireAudit { get; set; }
            /// <summary>
            /// 是否需要换编码
            /// </summary>
            public bool RequireChangePNR { get; set; }
        }
    }
}