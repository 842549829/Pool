using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 采买限制
    /// </summary>
    public class PurchaseLimitation
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departures { get; set; }
        /// <summary>
        /// 所属采购限制组
        /// </summary>
        public Guid LimitationGroupId { get; set; }
        /// <summary>
        /// 采购
        /// </summary>
        public IList<PurchaseLimitationRebate> Rebate { get; set; }
    }
}
