using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain
{
    /// <summary>
    /// 采买限制在政策匹配中的传输对象；
    /// </summary>
    public class PurchaseLimitationInfo
    {
        /// <summary>
        /// 提供者编号
        /// </summary>
        //public Guid ProviderId { get; set; }

        /// <summary>
        /// 采买限制编号
        /// </summary>
        public Guid LimitationId { get; set; }

        /// <summary>
        /// 返点
        /// </summary>
        public decimal Debate { get; set; }
    }
}
