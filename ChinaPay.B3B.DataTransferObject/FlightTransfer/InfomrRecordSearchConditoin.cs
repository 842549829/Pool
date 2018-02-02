using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.FlightTransfer
{
    /// <summary>
    /// 清Q通知结果查询条件
    /// </summary>
    public class InfomrRecordSearchConditoin
    {
        public string Carrier { get; set; }
        public string FlightNo { get; set; }
        public TransferType? TransferType { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public Guid? PurchaserId { get; set; }
        public InformType? InformType { get; set; }
        public InformResult? InformResult { get; set; }
        public DateTime? InformTimeFrom { get; set; }
        public DateTime? InformTimeTo { get; set; }
    }
}