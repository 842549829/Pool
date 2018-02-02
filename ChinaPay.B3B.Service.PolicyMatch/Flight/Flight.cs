namespace ChinaPay.B3B.Service.PolicyMatch {
    using System.Collections.Generic;

    /// <summary>
    /// 政策匹配后的航班信息
    /// </summary>
    public class MatchedFlight {
        /// <summary>
        /// 原航班信息
        /// </summary>
        public FlightQuery.Domain.Flight OriginalFlight {
            get;
            internal set;
        }

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal LowestPrice {
            get;
            internal set;
        }
        /// <summary>
        /// 最低价对应的政策类型
        /// </summary>
        public Common.Enums.PolicyType PolicyType {
            get;
            internal set;
        }
    }

    public class InstructionalFlight
    {
        /// <summary>
        /// 原航班信息
        /// </summary>
        public FlightQuery.Domain.Flight OriginalFlight
        {
            get;
            internal set;
        }

        /// <summary>
        /// 原始政策
        /// </summary>
        public DataTransferObject.Policy.SpecialPolicyInfo OriginalPolicy { get; set; }

        /// <summary>
        /// 信誉评级
        /// </summary>
        public decimal CompannyGrade
        {
            get;
            set;
        }
        /// <summary>
        /// 结算价
        /// </summary>
        public decimal  SettleAmount { get; set; }

        /// <summary>
        /// 资源数
        /// </summary>
        public int ResourceAmount { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public ChinaPay.B3B.Service.Statistic.SpecialProductStatisticOnVoyage Statistic { get; set; }

        public bool IsRepeated
        {
            get;
            set;
        }
    }    
}