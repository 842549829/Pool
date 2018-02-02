using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order.External
{
   public class ExternalOrderListView:OrderListView
    {
       /// <summary>
       /// 来源平台
       /// </summary>
       public PlatformType PlatformType { get; set; }
       /// <summary>
       /// 外部订单号
       /// </summary>
       public string ExternalOrderId { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus PayStatus { get; set; }
        /// <summary>
        /// 是否自动支付
        /// </summary>
        public bool IsAutoPay { get; set; }
    }
}
