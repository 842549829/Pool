using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Commodity.Domain
{
    /// <summary>
    /// 兑换商品
    /// </summary>
    public class Commodity
    {
        public Commodity()
            : this(Guid.NewGuid()) {
        }
        public Commodity(Guid id)
        {
            this.ID = id;
        } 
        /// <summary>
        /// 商品编号
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string CommodityName { get; set; }
        /// <summary>
        /// 展示图片路径
        /// </summary>
        public string CoverImgUrl { get; set; }
        /// <summary>
        /// 需要积分
        /// </summary>
        public int NeedIntegral { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        public int StockNumber { get; set; }
        /// <summary>
        /// 已经兑换数
        /// </summary>
        public int ExchangeNumber { get; set; } 
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; } 
        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ValidityTime { get; set; }
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public CommodityType Type { get; set; }

    }
}
