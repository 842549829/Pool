using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Integral.Domain
{
    /// <summary>
    /// 积分
    /// </summary>
    public class IntegralInfo
    { 
        public IntegralInfo()
            : this(Guid.NewGuid()) {
        }
        public IntegralInfo(Guid id)
        {
            this.ID = id;
        } 
        public Guid ID { get; set; }
        /// <summary>
        /// 消费者编号
        /// </summary>
        public Guid CompnayId { get; set; }
        /// <summary>
        /// 获得积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 获得积分途径
        /// </summary>
        public ChinaPay.B3B.Common.Enums.IntegralWay IntegralWay { get; set; }
        /// <summary>
        /// 获取积分时间
        /// </summary>
        public DateTime AccessTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
