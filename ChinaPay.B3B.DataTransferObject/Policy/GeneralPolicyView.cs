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
    public class GeneralPolicyView
    {

        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline
        {
            get;
            set;
        }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo
        {
            get;
            set;
        }
        /// <summary>
        /// 适用行程
        /// </summary>
        public VoyageType TripType
        {
            get;
            set;
        }
        /// <summary>
        /// 出发机场
        /// </summary>
        public IEnumerable<string> Departures
        {
            get;
            set;
        }
        /// <summary>
        /// 到达机场
        /// </summary>
        public IEnumerable<string> Arrivals
        {
            get;
            set;
        }

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
        public IEnumerable<string> WithoutVoyages
        {
            get;
            internal set;
        }
        /// <summary>
        /// 去程航班限制
        /// </summary>
        public LimitType OutwardFlightLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 去程航班过滤 
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班限制
        /// </summary>
        public LimitType ReturnFlightLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 旅游天数
        /// 去程与回程的时间间隔
        /// </summary>
        public int Interval
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 舱位集合
        /// </summary>
        public IEnumerable<string> Bunks
        {
            get;
            set;
        }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType
        {
            get;
            set;
        }
        /// <summary>
        /// 去程日期
        /// </summary>
        public Range<DateTime> OutwardFlightDateRange
        {
            get;
            set;
        }
        /// <summary>
        /// 回程日期
        /// </summary>
        public Range<DateTime> ReturnFlightDateRange
        {
            get;
            set;
        }
        /// <summary>
        /// 出票开始日期
        /// </summary>
        public DateTime ETDZBeginDate
        {
            get;
            set;
        }
        /// <summary>
        /// 返点信息
        /// </summary>
        public GeneralPolicyDeduction Deduction
        {
            get;
            set;
        }
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public bool RequireAudit
        {
            get;
            set;
        }
        /// <summary>
        /// 需换编码出票
        /// </summary>
        public bool RequireChangePNR
        {
            get;
            set;
        }
        /// <summary>
        /// 自动出票
        /// </summary>
        public bool AutoETDZ
        {
            get;
            set;
        }
        /// <summary>
        /// 政策状态
        /// </summary>
        public AuditStatus? AuditStatus { get; set; }
        /// <summary>
        /// 适用往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }
    }
}