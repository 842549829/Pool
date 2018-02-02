using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Foundation.Domain
{
   public class RefundAndReschedulingDetail
    {
       public RefundAndReschedulingDetail()
       {
           this.Id = Guid.NewGuid();
       }
       public RefundAndReschedulingDetail(Guid id)
       {
           this.Id = id;
       }
       public Guid Id { get; private set; }
       /// <summary>
       /// 舱位
       /// </summary>
       public string Bunks { get; set; }
       /// <summary>
       /// 离站前退票规定
       /// </summary>
       public string ScrapBefore { get; set; }
       /// <summary>
       /// 离站后退票规定
       /// </summary>
       public string ScrapAfter { get; set; }
       /// <summary>
       /// 离站前改期规定
       /// </summary>
       public string ChangeBefore { get; set; }
       /// <summary>
       /// 离站后改期规定
       /// </summary>
       public string ChangeAfter { get; set; }
       /// <summary>
       /// 签转规定
       /// </summary>
       public string Endorse { get; set; }
       /// <summary>
       /// 航空公司
       /// </summary>
       public string Airline { get; set; }
    }
}
