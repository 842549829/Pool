using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
   public class RoyaltyProfitCondition
    {
       /// <summary>
       /// 出票开始时间
       /// </summary>
       public DateTime? ETDZStartDate { get; set; }
       /// <summary>
       /// 出票结束时间
       /// </summary>
       public DateTime? ETDZEndDate { get; set; }
       /// <summary>
       /// PNR
       /// </summary>
       public string PNR { get; set; }
       /// <summary>
       /// 订单号
       /// </summary>
       public decimal? OrderId { get; set; }
       /// <summary>
       /// 票号
       /// </summary>
       public string TicketNo { get; set; }
       /// <summary>
       /// 类型
       /// </summary>
       public RoyaltyReportType? PaymentType { get; set; }
       /// <summary>
       /// 支付状态
       /// </summary>
       public bool? IsSuccess { get; set; }
       /// <summary>
       /// 是否是国付通支付
       /// </summary>
       public bool? IsPoolPay { get; set; }
       /// <summary>
       /// 所属OEM
       /// </summary>
       public Guid? Royalty { get; set; }
       /// <summary>
       /// 所属公司Id
       /// </summary>
       public Guid? IncomeGroupId { get; set; }
       /// <summary>
       /// 采购账号
       /// </summary>
       public Guid? PurchaseId { get; set; }
    }
}