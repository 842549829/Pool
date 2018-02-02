using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    public class DelayedSchedual : SchedualDetail
    {
        public DelayedSchedual(Schedual schedual, DateTime flightDate, DateTime newFlightDate, TimeSpan newDepartureTime, TimeSpan newArrivalTime, short newAddDays) :
            base(schedual, flightDate)
        {
            NewFlightDate = newFlightDate;
            NewDepartureTime = newDepartureTime;
            NewArrivalTime = newArrivalTime;
            NewAddDays = newAddDays;
        }

        /// <summary>
        /// 新日期
        /// </summary>
        public DateTime NewFlightDate { get; set; }
        /// <summary>
        /// 新起飞时间
        /// </summary>
        public TimeSpan NewDepartureTime { get; set; }
        /// <summary>
        /// 新到达时间
        /// </summary>
        public TimeSpan NewArrivalTime { get; set; }
        /// <summary>
        /// 新跨越天数
        /// </summary>
        public short NewAddDays { get; set; }
    }
}