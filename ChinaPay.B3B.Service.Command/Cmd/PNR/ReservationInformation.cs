using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.FlightQuery;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 旅客预订信息
    /// </summary>
    public class ReservationInformation
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// 旅客类型
        /// </summary>
        public PassengerType PassengerType { get; set; }

        /// <summary>
        /// 旅客
        /// </summary>
        public List<Passenger> Passengers { get; set; }

        /// <summary>
        /// 航段信息
        /// </summary>
        public List<Segment> Segments { get; set; }

        /// <summary>
        /// 旅客联系电话
        /// </summary>
        public string PassengerPhone { get; set; }
    }
}
