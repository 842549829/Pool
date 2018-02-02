using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class InformRecord
    {
        public Guid TransferId { get; set; }
        public Guid PurchaserId { get; set; }
        public string PurchaserAccount { get; set; }
        public string FlightNO { get; set; }
        public string Carrier { get; set; }
        public string CarrierName {
            get
            {
                if (string.IsNullOrEmpty(Carrier)) return string.Empty;
                var carrier = FoundationService.Airlines.FirstOrDefault(a => a.Code.Value == Carrier.ToUpper());
                if (carrier == null)
                {
                    return string.Empty;
                }
                return carrier.ShortName;
            }
        }
        public string DepartureName { get; set; }
        public string Departure { get; set; }
        public string DepartureCityName { get; set; }
        public string Arrival { get; set; }
        public string ArrivalName { get; set; }
        public string ArrivalCityName { get; set; }
        public TransferType TransferType { get; set; }
        public DateTime? InformTime { get; set; }
        public InformType? InformType { get; set; }
        public InformResult? InformResult { get; set; }
        public string InformAccount { get; set; }
        public string InfromerName { get; set; }
    }

    public class FlightTransferStatInfo
    {
        public int CarrierCount { get; set; }
        public int FlightCount { get; set; }
        public int PurchaserCount { get; set; }
        public int OrderCount { get; set; }
        public DateTime LastQSTime { get; set; }
        public int ToBeInformCount { get; set; }
    }

    public class PurchaseFlightTransferInfo
    {
        public Guid PurchaseId { get; set; }
        public int FlightCount { get; set; }
        public int OrderCount { get; set; }
        public DateTime LastQSTime { get; set; }
    }

    public class PurchaseTransferInformation
    {
        #region  OldFlightInfo
        public string OriginalCarrier { get; set; }
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
            get
            {
                if (string.IsNullOrEmpty(Carrier)) return string.Empty;
                var carrier = FoundationService.Airlines.FirstOrDefault(a => a.Code.Value == Carrier.ToUpper());
                if (carrier == null)
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
        public int AddDays { get; set; }
        #endregion

        /// <summary>
        /// 航班变更类型
        /// </summary>
        public TransferType TransferType { get; set; }
    }
}