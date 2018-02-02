using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR {
    using B3B.Common.Enums;

    /// <summary>
    /// 旅客订座记录（Passenger Name Record）
    /// </summary>
    public class PNRContentView {
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
        public IEnumerable<PassengerView> Passengers { get; set; }

        /// <summary>
        /// 航段信息
        /// </summary>
        public IEnumerable<FlightView> Flights { get; set; }

        /// <summary>
        /// 旅客联系电话
        /// </summary>
        public string PassengerPhone { get; set; }
    }
}