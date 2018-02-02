using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
   public class PlatformExternalOrderView
    {
       /// <summary>
       /// 支付时间（开始）
       /// </summary>
       public DateTime? BeginPayTime { get; set; }
       /// <summary>
       /// 支付时间（结束）
       /// </summary>
       public DateTime? EndPayTime { get; set; }
       /// <summary>
       /// 内部订单号
       /// </summary>
       public decimal? OrderId { get; set; }
       /// <summary>
       /// 外部订单号
       /// </summary>
       public string ExternalOrderId { get; set; }
       /// <summary>
       /// 航空公司
       /// </summary>
       public string Airline { get; set; }
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
       public bool? Payed { get; set; }
       /// <summary>
       /// PNR
       /// </summary>
       public string PNR { get; set; }
       /// <summary>
       /// 订单来源
       /// </summary>
       public PlatformType? OrderSource { get; set; }
       /// <summary>
       /// 出票状态
       /// </summary>
       public short? ETDZStatus { get; set; }
    }
}
