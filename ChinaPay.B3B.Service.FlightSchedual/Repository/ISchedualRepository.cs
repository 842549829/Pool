using System;
using ChinaPay.B3B.Service.FlightSchedual.Domain;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository
{
    /// <summary>
    /// 航班时刻表，不提供修改和删除方法；当有航班变动时，会在新表中加入数据；
    /// </summary>
    interface ISchedualRepository
    {
        int Save(DelayedSchedual delayedSchedual);
        int Save(TransferedSchedual transferedSchedual);
        int Save(CanceledSchedual canceledSchedual);
        int Save(NormalSchedual normalSchedual);
        int Save(Schedual schedual);
        /// <summary>
        /// 根据航班号、起飞机场、到达机场和航班日期，得到航班信息。
        /// </summary>
        /// <param name="flightNumber">航班号</param> 
        /// <param name="departureAirport">起飞机场</param>
        /// <param name="arrivalAirport">到达机场</param>
        /// <param name="datetime">航班日期</param>
        /// <returns>
        /// 航班时刻信息
        /// </returns>
        Schedual Query(FlightNumber flightNumber, string departureAirport, string arrivalAirport, DateTime datetime);
    }
}
