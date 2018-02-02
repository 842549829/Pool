using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain
{
    /// <summary>
    /// 紧急订单
    /// </summary>
    public class EmergentOrder
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Type { get; set; }
        /// <summary>
        /// 紧急内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 操作账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        ///申请类型
        /// </summary>
        public OrderIdType OrderIdTypeValue { get; set; }
    }
    public enum OrderIdType {
        Order,
        Apply
    }
}
