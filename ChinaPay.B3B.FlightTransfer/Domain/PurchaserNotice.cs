using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class PurchaserNotice
    {
        public Guid NoticeId { get; set; }
        public Guid PurchaserId { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public int FlightCount { get; set; }
        public int OrderCount { get; set; }
        /// <summary>
        /// 是否已通知
        /// </summary>
        public bool Informed { get; set; }
        /// <summary>
        /// 通知是否成功
        /// </summary>
        public bool InformSuccess { get; set; }

        public string InformMethod { get; set; }

        public string Remark { get; set; }

        public bool SenedMsg { get; set; }

        /// <summary>
        /// 通知时间
        /// </summary>
        public DateTime? InformTime { get; set; }

        public TransferType TransferType { get; set; }
        public string Departure { get; set; }
        public string Arraival { get; set; }
        public string FlightNO { get; set; }


        public string OperatorAccount { get; set; }
        public string OperatorName { get; set; }
    }
}
