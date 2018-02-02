using System; 

namespace ChinaPay.B3B.DataTransferObject.Integral
{
    /// <summary>
    /// 积分参数设置类
    /// </summary>
    public class IntegralParameterView
    {
        /// <summary>
        /// 是否启用签到送积分
        /// </summary>
        public bool IsSignIn { get; set; }
        /// <summary>
        /// 登录送的积分数
        /// </summary>
        public int SignIntegral { get; set; }
        /// <summary>
        /// 是否启用未登录降分
        /// </summary>
        public bool IsDrop { get; set; }
        /// <summary>
        /// 每消费100元可得的积分
        /// </summary>
        public int ConsumptionIntegral { get; set; }
        /// <summary>
        /// 最多扣去积分
        /// </summary>
        public int MostBuckle { get; set; }
        /// <summary>
        /// 可用积分比例
        /// </summary>
        public decimal AvailabilityRatio { get; set; }
        /// <summary>
        /// 适用国付通账号得到积分的倍数
        /// </summary>
        public decimal Multiple { get; set; }
        /// <summary>
        /// 积分有效期范围
        /// </summary>
        public ChinaPay.B3B.Common.Enums.IntegralRangeTime RangeReset { get; set; }
        /// <summary>
        /// 指定日期
        /// </summary>
        public DateTime? SpecifiedDate { get; set; }
    }
}
