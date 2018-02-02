using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Integral.Domain
{
    /// <summary>
    /// 用户总积分
    /// </summary>
    public class IntegralCount
    {
        public Guid CompnayId { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int IntegralCounts { get; set; }
        /// <summary>
        /// 可用积分
        /// </summary>
        public int IntegralAvailable { get; set; }
        /// <summary>
        /// 可扣积分
        /// </summary>
        public int IntegralSurplus { get; set; }
        /// <summary>
        /// 不可扣积分
        /// </summary>
        public int IntegralNotDeduct { get; set; }
        /// <summary>
        /// 消费或获得积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 是否是不可扣积分
        /// </summary>
        public bool IsNotDeduct { get; set; }
    }
}
