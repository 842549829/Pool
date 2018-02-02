using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    class NormalSchedual : SchedualDetail
    {
        public NormalSchedual(Schedual schedual, DateTime flightDate)
            : base(schedual, flightDate)
        {
        }
    }
}
