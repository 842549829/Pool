using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Report
{
   public class PurchaseStatisticView
    {
       /// <summary>
       /// 报表开始时间
       /// </summary>
       public DateTime? ReportStartDate { get; set; }
       /// <summary>
       /// 报表结束时间
       /// </summary>
       public DateTime? ReportEndDate { get; set; }
       /// <summary>
       /// 采购商Id
       /// </summary>
       public Guid? Purchase { get; set; }
       /// <summary>
       /// 是否有交易
       /// </summary>
       public bool? IsHasTrade { get; set; }
       /// <summary>
       /// 承运人
       /// </summary>
       public string Carrier { get; set; }
       /// <summary>
       /// 出港地
       /// </summary>
       public string Departure { get; set; }
    }
}
