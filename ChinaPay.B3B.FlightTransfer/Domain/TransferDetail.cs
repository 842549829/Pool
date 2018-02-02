using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class TransferDetail
    {
        public Guid TransferId { get; set; }
        public PNRPair PNR { get; set; }
        public decimal OrderId { get; set; }

        #region  OldFlightInfo
        public string OriginalCarrier{get;set;}
        public string OriginalCarrierName
        {
            get
            {
                if (string.IsNullOrEmpty(OriginalCarrier)) return string.Empty;
                var carrier = FoundationService.Airlines.FirstOrDefault(a => a.Code.Value == OriginalCarrier.ToUpper());
                if (carrier == null)
                {
                    return string.Empty;
                }
                return carrier.ShortName;
            }
        }
        public string OriginalFlightNo { get; set; }
        public DateTime OriginalTakeoffTime { get; set; }
        public DateTime OriginalArrivalTime { get; set; }
        #endregion

        #region  NewFlightInfo

        public string Carrier { get; set; }
        public string CarrierName
        {
            get {
                if (string.IsNullOrEmpty(Carrier)) return string.Empty;
                var carrier = FoundationService.Airlines.FirstOrDefault(a => a.Code.Value == Carrier.ToUpper());
                if (carrier==null)
                {
                    return string.Empty;
                }
                return carrier.ShortName;
            }
        }
        public string FlightNo { get; set; }
        public DateTime? FlightDate
        {
            get;
            set;
        }
        public DateTime? TakeoffTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        #endregion

        /// <summary>
        /// 航班变更类型
        /// </summary>
        public TransferType TransferType { get; set; }

        public string PurchaserPhone { get; set; }
        public Guid PurchaserId { get; set; }
    }
}