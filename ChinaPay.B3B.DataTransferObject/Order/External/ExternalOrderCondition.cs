using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Order.External
{
   public class ExternalOrderCondition
    {
       /// <summary>
       /// 创建开始时间
       /// </summary>
       public DateTime? StartTime { get; set; }
       /// <summary>
       /// 创建结束时间
       /// </summary>
       public DateTime? EndTime { get; set; }
       /// <summary>
       /// 来源平台
       /// </summary>
       public PlatformType? PlatformType { get; set; }
       /// <summary>
       /// Pnr
       /// </summary>
       public string Pnr { get; set; }
       /// <summary>
       /// 出发城市
       /// </summary>
       public string Departure { get; set; }
       /// <summary>
       /// 到达城市
       /// </summary>
       public string Arrival { get; set; }
       /// <summary>
       /// 支付状态
       /// </summary>
       public PayStatus? PayStatus { get; set; }
       /// <summary>
       /// 是否已出票
       /// </summary>
       public bool? IsEtdzed { get; set; }
       /// <summary>
       /// 外部订单号
       /// </summary>
       public string ExternalOrderId { get; set; }
       /// <summary>
       /// 内部订单号
       /// </summary>
       public decimal? InternalOrderId { get; set; }
       /// <summary>
       /// 乘机人
       /// </summary>
       public string Passenger { get; set; }
       /// <summary>
       /// 出票方
       /// </summary>
       public Guid? ProviderId { get; set; }
    }
}
