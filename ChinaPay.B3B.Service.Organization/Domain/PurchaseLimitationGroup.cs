using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 采买限制组
    /// </summary>
   public class PurchaseLimitationGroup
    {
       public PurchaseLimitationGroup()
       {
           this.Id = Guid.NewGuid();
       }
       public PurchaseLimitationGroup(Guid limitationGroupId)
       {
           this.Id = limitationGroupId;
       }
       public Guid Id { get; set; }
       /// <summary>
       /// 是否是全局或者是分组
       /// </summary>
       public bool IsGlobal { get; set; }
       /// <summary>
       /// 所属公司Id
       /// </summary>
       public Guid? CompanyId { get; set; }
       /// <summary>
       /// 所属公司组Id
       /// </summary>
       public Guid? IncomeGroupId { get; set; }
       /// <summary>
       /// 采买限制
       /// </summary>
       public IList<PurchaseLimitation> Limitation { get; set; }
    }
}
