using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    /// <summary>
    /// 票价
    /// </summary>
    public class AirportPairFares
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// 港口对
        /// </summary>
        public AirportPair AirportPair { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 里程数
        /// </summary>
        public int Mileage { get; set; }

        /// <summary>
        /// 票价列表
        /// </summary>
        public List<GraduatedFare> GraduatedFareList { get; set; }
    }
}
