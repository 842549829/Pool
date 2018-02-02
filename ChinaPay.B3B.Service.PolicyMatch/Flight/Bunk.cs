namespace ChinaPay.B3B.Service.PolicyMatch {
    using System.Collections.Generic;
    using DataTransferObject.Policy;
    using OriginalBunk = FlightQuery.Domain.Bunk;

    /// <summary>
    /// 政策匹配后的舱位信息
    /// </summary>
    public class MatchedBunk {
        /// <summary>
        /// 原始舱位
        /// </summary>
        public OriginalBunk OriginalBunk { get; internal set; }
        /// <summary>
        /// 匹配到的政策
        /// </summary>
        public IEnumerable<MatchedPolicy> Policies { get; internal set; }
    }
}