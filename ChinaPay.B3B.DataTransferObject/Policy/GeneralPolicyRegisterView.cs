using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    using B3B.Common.Enums;

    /// <summary>
    /// 普通政策
    /// </summary>
    public class GeneralPolicyRegisterView
    {
        /// <summary>
        /// 编号
        /// </summary>
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
        /// 适用行程
        /// </summary>
        public VoyageType TripType { get; set; }
        /// <summary>
        /// 出发机场
        /// </summary>
        public string Departures { get; set; }
        /// <summary>
        /// 到达机场
        /// </summary>
        public string Arrivals { get; set; } 

        /// <summary>
        /// 去程班期
        /// </summary>
        public Schedule OutwardScheduleDay { get; set; }
        /// <summary>
        /// 回程班期
        /// </summary>
        public Schedule ReturnScheduleDay { get; set; }

        /// <summary>
        /// 排除航线
        /// </summary>
        public string WithoutVoyages { get; set; }
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
        public LimitType ReturnFlightLimit { get; set; }
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 旅游天数
        /// 去程与回程的时间间隔
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 政策备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 政策组信息
        /// </summary>
        public IEnumerable<Item> Items { get; set; }
        /// <summary>
        /// 普通政策组
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
            /// 舱位集合
            /// </summary>
            public string Bunks { get; set; }
            /// <summary>
            /// 客票类型
            /// </summary>
            public TicketType TicketType { get; set; }
            /// <summary>
            /// 返佣信息
            /// </summary>
            public GeneralPolicyDeduction Deduction { get; set; }
            /// <summary>
            /// 是否自动出票
            /// </summary>
            public bool IsAutoETDZ { get; set; }
            /// <summary>
            /// 是否需要换编码
            /// </summary>
            public bool RequireChangePNR { get; set; }
            /// <summary>
            /// 是否需要审核
            /// </summary>
            public bool RequireAudit { get; set; }
            /// <summary>
            /// 是否用于往返降舱
            /// </summary>
            public bool UsedForConcessionalRoundTrip { get; set; }
        }
    }
}