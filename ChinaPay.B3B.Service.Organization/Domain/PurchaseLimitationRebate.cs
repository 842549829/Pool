using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
   public class PurchaseLimitationRebate
    {
       public Guid LimitationId { get; set; }
       /// <summary>
       /// 采买限制类型
       /// </summary>
       public PurchaseLimitationRateType Type{get;set;}
       /// <summary>
       /// 返点
       /// </summary>
       public decimal? Rebate { get; set; }
       /// <summary>
       /// 是否仅允许购买自己的政策
       /// </summary>
       public bool AllowOnlySelf { get; set; }
    }
}
