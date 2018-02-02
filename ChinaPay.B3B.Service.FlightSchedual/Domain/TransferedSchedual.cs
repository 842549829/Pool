using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 航班更换类
    /// </summary>
    public class TransferedSchedual : SchedualDetail
    {
        public TransferedSchedual(Schedual schedual, DateTime flightDate, FlightNumber newFlightNumber,
                               DateTime newFlightDate, TimeSpan newDepartureTime, TimeSpan newArrivalTime,
                               short newAddDays) :
                                   base(schedual, flightDate)
        {
            NewFlightNumber = newFlightNumber;
            NewFlightDate = newFlightDate;
            NewDepartureTime = newDepartureTime;
            NewArrivalTime = newArrivalTime;
            NewAddDays = newAddDays;
        }

        public FlightNumber NewFlightNumber { get; set; }
        public DateTime NewFlightDate { get; set; }
        public TimeSpan NewDepartureTime { get; set; }
        public TimeSpan NewArrivalTime { get; set; }
        public short NewAddDays { get; set; }
    }
}