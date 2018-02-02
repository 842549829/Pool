using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Foundation
{
   public class RefundAndReschedulingDetailView
    {
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
       /// 适用条件
       /// </summary>
       public string Condition { get; set; }
       /// <summary>
       /// 废票规定
       /// </summary>
       public string Scrap { get; set; }
       /// <summary>
       /// 升舱规定
       /// </summary>
       public string Upgrade { get; set; }
       /// <summary>
       /// 备注
       /// </summary>
       public string Remark { get; set; }
       /// <summary>
       /// 航空公司
       /// </summary>
       public string Airline { get; set; }
       /// <summary>
       /// 舱位
       /// </summary>
       public string Bunks { get; set; }
    }
}
