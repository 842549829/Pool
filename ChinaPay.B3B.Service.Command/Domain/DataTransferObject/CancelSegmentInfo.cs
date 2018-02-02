using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 待取消航段信息
    /// </summary>
    public class CancelSegmentInfo
    {
        public FlightNumber FlightNumber { get; set; }
        public DateTime FlightDate { get; set; }}
}
