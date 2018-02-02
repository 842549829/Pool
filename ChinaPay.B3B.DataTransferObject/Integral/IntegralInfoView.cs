using System;

namespace ChinaPay.B3B.DataTransferObject.Integral
{
    /// <summary>
    /// 用户积分类
    /// </summary>
    public class IntegralInfoView
    {
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
