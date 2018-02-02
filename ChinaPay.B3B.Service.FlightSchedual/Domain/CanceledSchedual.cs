using System;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 航班取消类
    /// </summary>
    public class CanceledSchedual : SchedualDetail
    {
        public CanceledSchedual(Schedual schedual, DateTime flightDate)
            : base(schedual, flightDate)
        {
        }
    }
}
