using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 航班班期
    /// </summary>
    public class Schedual
    {
        public Schedual()
        {
        }

        public Schedual(FlightNumber flightNumber, string departureAirport, string arrivalAirport,
                        TimeSpan departureTime, TimeSpan arrivalTime, short addDays)
        {
            AddDays = addDays;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
            ArrivalAirport = arrivalAirport;
            DepartureAirport = departureAirport;
            FlightNumber = flightNumber;
        }

        /// <summary>
        /// 航班号
        /// </summary>
        public FlightNumber FlightNumber { get; protected set; }
        
        /// <summary>
        /// 出发机场三字码
        /// </summary>
        public string DepartureAirport { get; protected set; }

        /// <summary>
        /// 到达机场三字码
        /// </summary>
        public string ArrivalAirport { get; private set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public TimeSpan DepartureTime { get; private set; }

        /// <summary>
        /// 到达机场
        /// </summary>
        public TimeSpan ArrivalTime { get; private set; }

        /// <summary>
        /// 跨天飞行时的天数；
        /// </summary>
        public short AddDays { get; private set; }
    }
}