using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Report
{
   public class ErrorRefundQueryCondition
    {
       /// <summary>
       /// 申请开始时间
       /// </summary>
       public DateTime? ApplyStartTime { get; set; }
       /// <summary>
       /// 申请结束时间
       /// </summary>
       public DateTime? ApplyEndTime { get; set; }
       /// <summary>
       /// 订单号
       /// </summary>
       public decimal? OrderId { get; set; }
       /// <summary>
       /// 出发城市
       /// </summary>
       public string Departure { get; set; }
       /// <summary>
       /// 到达城市
       /// </summary>
       public string Arrival { get; set; }
       /// <summary>
       /// 结算码
       /// </summary>
       public string SettleCode { get; set; }
       /// <summary>
       /// 票号
       /// </summary>
       public string TicketNo { get; set; }
       /// <summary>
       /// 申请单号
       /// </summary>
       public decimal? ApplyformId { get; set; }
       /// <summary>
       /// 乘机人
       /// </summary>
       public string Passenger { get; set; }
       /// <summary>
       /// 申请人
       /// </summary>
       public string ApplierAccount { get; set; }
       /// <summary>
       /// 处理人
       /// </summary>
       public string ProcessAccount { get; set; }
       /// <summary>
       /// 采购方
       /// </summary>
       public Guid? Purchase { get; set; }
       /// <summary>
       /// 出票方
       /// </summary>
       public Guid? Provider { get; set; }
    }
}
