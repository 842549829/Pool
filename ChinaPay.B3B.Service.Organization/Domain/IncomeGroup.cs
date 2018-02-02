using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 分销OEM收益组
    /// </summary>
   public class IncomeGroup
    {
       public Guid Id
       {
           get;
           set;
       }
       public Guid Company { get; set; }

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
       /// 操作者帐号
       /// </summary>
       public string Creator
       {
           get;
           set;
       }

       public DateTime CreateTime
       {
           get;
           set;
       }
       /// <summary>
       /// 扣点设置
       /// </summary>
       public List<IncomeGroupDeductGlobal> Setting
       {
           get;
           set;
       }
    }
}
