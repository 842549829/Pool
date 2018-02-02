using System;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    /// <summary>
    /// 旅客期望预订的航班的信息
    /// </summary>
    public class ReservationSegmentInfo
    {
        /// <summary>
        /// 承运人
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// 内部编号
        /// </summary>
        public string InternalNumber { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public string Number
        {
            get { return Carrier + InternalNumber; }
        }

        /// <summary>
        /// 舱位等级
        /// </summary>
        public string ClassOfService { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 离港机场代码
        /// </summary>
        public string DepartureAirportCode { get; set; }

        /// <summary>
        /// 到港机场代码
        /// </summary>
        public string ArrivalAirportCode { get; set; }

    }
}
