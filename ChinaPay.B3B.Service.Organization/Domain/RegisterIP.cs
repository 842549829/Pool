using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
   public class RegisterIP
    {
       /// <summary>
       /// IP地址
       /// </summary>
       public string IP { get; set; }
       /// <summary>
       /// 次数，最多3次
       /// </summary>
       public int Number { get; set; }
       /// <summary>
       /// 注册日期
       /// </summary>
       public DateTime RegisterDate { get; set; }
    }
}
