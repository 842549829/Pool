using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
   public class VerfiCode
    {
       public VerfiCode()
       {
           this.Id = Guid.NewGuid();
           this.SendTime = DateTime.Now;
       }

       public VerfiCode(Guid id)
       {
           this.Id = id;
       }

       public Guid Id { get; set; }
       /// <summary>
       /// 账号
       /// </summary>
       public string AccountNo { get; set; }
       /// <summary>
       /// IP地址
       /// </summary>
       public string IP { get; set; }
       /// <summary>
       /// 联系电话
       /// </summary>
       public string CellPhone { get; set; }
       /// <summary>
       /// 验证码类型
       /// </summary>
       public VerfiCodeType Type { get; set; }
       /// <summary>
       /// 验证码
       /// </summary>
       public string Code { get; set; }
       /// <summary>
       /// 发送时间
       /// </summary>
       public DateTime SendTime { get; set; }
    }
}
