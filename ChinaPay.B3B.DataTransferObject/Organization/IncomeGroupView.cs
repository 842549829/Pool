using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class IncomeGroupView
    {
       public Guid IncomeGroupId { get; set; }
       /// <summary>
       /// 组别名称
       /// </summary>
       public string Name
       {
           get;
           set;
       }
       /// <summary>
       /// 组别描述
       /// </summary>
       public string Description
       {
           get;
           set;
       }
       /// <summary>
       /// 用户数量
       /// </summary>
       public int UserCount
       {
           get;
           set;
       }
    }
}
