using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 具体到某天的时刻表；
    /// </summary>
    public class SchedualDetail
    {
        public SchedualDetail(Schedual schedual, DateTime flightDate)
        {
            Schedual = schedual;
            FlightDate = flightDate;
        }

        /// <summary>
        /// 时刻信息
        /// </summary>
        public Schedual Schedual { get; set; }

        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate { get; set; }
    }
}
