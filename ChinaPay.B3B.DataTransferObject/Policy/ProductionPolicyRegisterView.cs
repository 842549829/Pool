namespace ChinaPay.B3B.DataTransferObject.Policy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Common;
    using ChinaPay.Core;

    /// <summary>
    /// 往返产品政策
    /// </summary>
    public class ProductionPolicyRegisterView
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
        /// 到达
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
        /// 回程班期
        /// </summary>
        public Schedule ReturnScheduleDay { get; set; }
        /// <summary>
        /// 去程航班限制
        /// </summary>
        public LimitType OutwardFlightLimit { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班限制
        /// </summary>
        public LimitType ReturnTripFlightLimit { get; set; }
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public int AheadDays { get; set; }
        /// <summary>
        /// 旅游天数
        /// 去程与回程的时间间隔
        /// </summary>
        public int Interval { get; set; }
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
        /// 往返产品政策组
        /// </summary>
        public class Item
        {
            /// <summary>
            /// 去程航班日期
            /// </summary>
            public Range<DateTime> OutwardFlightDateRange { get; set; }
            /// <summary>
            /// 回程航班日期
            /// </summary>
            public Range<DateTime> ReturnTripFlightDateRange { get; set; }
            /// <summary>
            /// 出票开始日期
            /// </summary>
            public DateTime ETDZBeginDate { get; set; }
            /// <summary>
            /// 舱位
            /// </summary>
            public string Bunk { get; set; }
            /// <summary>
            /// 价格
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// 客票类型
            /// </summary>
            public TicketType TicketType { get; set; }
            /// <summary>
            /// 返佣信息
            /// </summary>
            public Deduction Deduction { get; set; }
            /// <summary>
            /// 是否需要换编码
            /// </summary>
            public bool RequireChangePNR { get; set; }
            /// <summary>
            /// 是否需要审核
            /// </summary>
            public bool RequireAudit { get; set; }
        }
    }
}
