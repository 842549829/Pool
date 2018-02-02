using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class ExternalInterfaceQueryCondition
    {
       /// <summary>
       /// 用户名
       /// </summary>
       public string UserNo { get; set; }
       /// <summary>
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }
       /// <summary>
       /// 是否已开通外接口
       /// </summary>
       public bool? IsOpenExternalInterface { get; set; }
       /// <summary>
       /// 开通接口开始时间
       /// </summary>
       public DateTime? OpenTimeStart { get; set; }
       /// <summary>
       /// 开通接口结束时间
       /// </summary>
       public DateTime? OpenTimeEnd { get; set; }
    }
}
