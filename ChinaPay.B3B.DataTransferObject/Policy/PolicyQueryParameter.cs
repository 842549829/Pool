using System;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;

    public class PolicyQueryParameter {
        public PolicyType PolicyType { get; set; }
        public Guid? Owner { get; set; }
        public Guid? Operator { get; set; }
        public TicketType? TicketType { get; set; }
        public bool? Freezed { get; set; }
        public bool? Audited { get; set; }
        /// <summary>
        /// 挂起或未挂起
        /// </summary>
        public int? Suspended { get; set; }
        public string Airline { get; set; }
        public string OfficeCode { get; set; }
        public string Departure { get; set; }
        public string Transit { get; set; }
        public string Arrival { get; set; }
        public DateTime? DepartureDateStart { get; set; }
        public DateTime? DepartureDateEnd { get; set; }
        public string Creator { get; set; }

        public DateTime PubDateStart { get; set; }
        public DateTime PubDateEnd { get; set; }
        public string Bunks { get; set; }

        public decimal? InternalCommissionLower { get; set; }
        public decimal? InternalCommissionUpper { get; set; }
        public decimal? SubordinateCommissionLower { get; set; }
        public decimal? SubordinateCommissionUpper { get; set; }
        public decimal? ProfessionCommissionLower { get; set; }
        public decimal? ProfessionCommissionUpper { get; set; }
        public VoyageType? VoyageType { get; set; }
        public SpecialProductType? SpecialProductType { get; set; }
        public bool? PlatformAudited { get; set; }

        /// <summary>
        /// 排序方式。null、显示全部 1、仅显示有效，2、仅显示过期
        /// </summary>
        public int? Effective { get; set; }
        /// <summary>
        /// 排序方式。1、按时间倒序，2、按同行返点倒序
        /// </summary>
        public int OrderBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
