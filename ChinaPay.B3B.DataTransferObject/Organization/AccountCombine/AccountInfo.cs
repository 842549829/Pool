using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class AccountInfo
    {
       /// <summary>
       /// 账号
       /// </summary>
       public string AccountNo { get; set; }

       /// <summary>
       /// 密码
       /// </summary>
       public string Password { get; set; }

       /// <summary>
       /// 确认密码
       /// </summary>
       public string ComfirmPassword { get; set; }
       /// <summary>
       /// 自定义国付通账号
       /// </summary>
       public string PoolPayUserName { get; set; }
       /// <summary>
       /// 对私国付通账号
       /// </summary>
       public bool IsPersonAccountNo { get; set; }
    }
}
