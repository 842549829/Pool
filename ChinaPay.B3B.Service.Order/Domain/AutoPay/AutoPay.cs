using System;
using ChinaPay.B3B.Common.Enums;
namespace ChinaPay.B3B.Service.Order.Domain.AutoPay
{
    /// <summary>
    /// 代扣
    /// </summary>
    public class AutoPay
    {
        /// <summary>
        /// 代扣订单
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 代扣账户
        /// </summary>
        public string PayAccountNo { get; set; }
        /// <summary>
        /// 代扣方式
        /// </summary>
        public WithholdingAccountType PayType { get; set; }
        /// <summary>
        /// 订单类型（0订单，1申请单）
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 生成代扣时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 是否代扣成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 是否处理过
        /// </summary>
        public bool ProcessState { get; set; }

    }
}
