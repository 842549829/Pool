using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    public class TransitPoint : IComparable<TransitPoint>
    {
        public const string FormatString =
            @"(?<CityCode>[A-Z]{3})\s{3}(?<ArrivalHour>\d{2})(?<ArrivalMinute>\d{2})(?:\s{2}|\+(?<ArrivalAddDays>\d))\s{2}(?<DepartureHour>\d{2})(?<DepartureMinute>\d{2})(?:\s{2}|\+(?<DepartureAddDays>\d))";

        /// <summary>
        /// 机场代码
        /// </summary>
        public string AirportCode { get; private set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        public Time ArrivalTime { get; private set; }

        /// <summary>
        /// 到达跨天
        /// </summary>
        public int ArrivalAddDays { get; private set; }

        /// <summary>
        /// 起飞时间
        /// </summary>
        public Time DepartureTime { get; private set; }

        /// <summary>
        /// 起飞跨天
        /// </summary>
        public int DepartureAddDays { get; private set; }

        public TransitPoint(string airportCode, Time arrivalTime, int arrivalAddDays, Time departureTime, 
                            int departureAddDays)
        {
            AirportCode = airportCode;
            ArrivalTime = arrivalTime;
            ArrivalAddDays = arrivalAddDays;
            DepartureTime = departureTime;
            DepartureAddDays = departureAddDays;
        }

        public int CompareTo(TransitPoint other)
        {
            // 按照到达时间比较
            return ArrivalTime.CompareTo(other.ArrivalTime);
        }
    }
}
