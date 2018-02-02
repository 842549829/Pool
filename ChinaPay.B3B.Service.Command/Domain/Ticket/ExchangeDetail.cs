using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    /// <summary>
    /// 电子客票兑换信息。
    /// </summary>
    public class ExchangeDetail
    {
        /// <summary>
        /// 空港
        /// </summary>
        public string Airport { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public FlightNumber FlightNumber { get; set; }
        /// <summary>
        /// 舱位等级
        /// </summary>
        public string ClassOfService { get; set; }
        /// <summary>
        /// 起飞时间；
        /// </summary>
        public DateTime DepartureTime { get; set; }
        /// <summary>
        /// 客票状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public PNRPair PnrPair { get; set; }
    }
}
