using System;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的航段信息
    /// </summary>
    public class ReservedSegmentInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public FlightNumber FlightNumber { get; set; }

        /// <summary>
        /// 是否共享航班
        /// </summary>
        public bool IsCodeShareFlight { get; set; }

        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate { get; set; }

        /// <summary>
        /// 出发机场
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// 到达机场
        /// </summary>
        public string ArrivalAirport { get; internal set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime DepartureDate { get; internal set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime ArrivalDate { get; internal set; }

        /// <summary>
        /// 出发机场航站楼
        /// </summary>
        public string TerminalOfDeparture { get; internal set; }

        /// <summary>
        /// 到达机场航站楼
        /// </summary>
        public string TerminalOfArrival { get; internal set; }

        /// <summary>
        /// 是否电子客票
        /// </summary>
        public bool IsETicket { get; internal set; }

        /// <summary>
        /// 跨天飞行时的天数
        /// </summary>
        public int AddDays { get; internal set; }

        /// <summary>
        /// 舱位
        /// </summary>
        public string ClassOfService { get; set; }

        /// <summary>
        /// 行动代码，即订票状态；
        /// </summary>
        public string SeatStatus { get; set; }

        /// <summary>
        /// 订座个数
        /// </summary>
        public int Seatings { get; set; }

    }
}
