using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Integral.Domain
{
    /// <summary>
    /// 用户消费积分记录
    /// </summary>
    public class IntegralConsumption
    {
        public IntegralConsumption()
            : this(Guid.NewGuid()) {
        }
        public IntegralConsumption(Guid id)
        {
            this.Id = id;
        } 
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public Guid CompnayId { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string CompanyShortName { get; set; } 
        /// <summary>
        /// 消费者账号
        /// </summary>
        public string AccountNo { get; set; }
        /// <summary>
        /// 消费者名称
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 消费者电话
        /// </summary>
        public string AccountPhone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string DeliveryAddress { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary> 
        public string ExpressCompany { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary> 
        public string ExpressDelivery { get; set; }
        /// <summary>
        /// 消费积分
        /// </summary>
        public int ConsumptionIntegral { get; set; }
        /// <summary>
        /// 兑换商品编号
        /// </summary>
        public Guid CommodityId { get; set; }
        /// <summary>
        /// 兑换商品名称
        /// </summary>
        public string CommodityName { get; set; }
        /// <summary>
        /// 兑换时间
        /// </summary>
        public DateTime ExchangeTiem { get; set; }
        /// <summary>
        /// 兑换状态
        /// </summary>
        public ChinaPay.B3B.Common.Enums.ExchangeState Exchange { get; set; }
        /// <summary>
        /// 积分减少原因
        /// </summary>
        public ChinaPay.B3B.Common.Enums.IntegralWay Way { get; set; }
        /// <summary>
        /// 兑换商品数量
        /// </summary>
        public int CommodityCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 填写商品单时候的备注
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// oem提交状态
        /// </summary>
        public OEMCommodityState OEMCommodityState { get; set; }
        /// <summary>
        /// 申请来源
        /// </summary>
        public string OEMName { get; set; }
        /// <summary>
        /// oem编号
        /// </summary>
        public Guid? OEMID { get; set; }
    }
}
