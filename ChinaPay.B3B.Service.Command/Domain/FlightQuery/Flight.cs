using System;
using System.Collections.Generic;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    public class Flight
    {
        public FlightNumber Number { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline
        {
            get { return Number.Carrier; }
        }

        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo
        {
            get { return Number.InternalNumber; }
        }

        /// <summary>
        /// 出发机场三字码
        /// </summary>
        public string Departure
        {
            get;
            set;
        }
        /// <summary>
        /// 出发机场航站楼
        /// </summary>
        public string TerminalOfDeparture
        {
            get;
            set;
        }
        /// <summary>
        /// 到达机场三字码
        /// </summary>
        public string Arrival
        {
            get;
            set;
        }
        /// <summary>
        /// 到达机场航站楼
        /// </summary>
        public string TerminalOfArrival
        {
            get;
            set;
        }

        public Time DepartureTime { get; set; }

        public Time ArrivalTime { get; set; }

        public int TransitPoint { get; set; }

        /// <summary>
        /// 航班日期（想去掉，但有三处使用，后面再改）
        /// </summary>
        public DateTime FlightDate
        {
            get;
            set;
        }

        /// <summary>
        /// 起飞时间（改名）
        /// </summary>
        public Time TakeoffTime
        {
            get { return DepartureTime; }
        }
        /// <summary>
        /// 降落时间
        /// </summary>
        public Time LandingTime
        {
            get { return ArrivalTime; }
        }

        /// <summary>
        /// 机型
        /// </summary>
        public string AirCraft
        {
            get;
            set;
        }

        /// <summary>
        /// 是否共享航班
        /// </summary>  
        public bool IsShareFlight
        {
            get { return !string.IsNullOrWhiteSpace(ShareFlightNo); }
        }

        /// <summary>
        /// 共享航班编号
        /// </summary>
        public string ShareFlightNo
        {
            get;
            internal set;
        }

        /// <summary>
        /// 有无餐食
        /// </summary>
        public bool HasFood
        {
            get;
            internal set;
        }

        /// <summary>
        /// 是否经停
        /// </summary>
        public bool IsStop
        {
            get { return TransitPoint > 0; }
        }

        /// <summary>
        /// 舱位
        /// </summary>
        public IEnumerable<Bunk> Bunks
        {
            get;
            set;
        }

        /// <summary>
        /// 跨天飞行时的天数；
        /// </summary>
        public int AddDays { get; set; }
    }
}
