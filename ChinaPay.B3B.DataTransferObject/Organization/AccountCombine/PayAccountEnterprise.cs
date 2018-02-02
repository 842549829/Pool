using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class PayAccountEnterprise :AccountInfo
    {
       /// <summary>
       /// 支付密码
       /// </summary>
       public string PayPassword { get; set; }
       /// <summary>
       /// 确认支付密码
       /// </summary>
       public string ConfirmPayPassword { get; set; }
       /// <summary>
       /// 公司名称
       /// </summary>
       public string CompanyName { get; set; }
       /// <summary>
       /// 机构代码
       /// </summary>
       public string OrginationCode { get; set; }
       /// <summary>
       /// 公司电话
       /// </summary>
       public string CompanyPhone { get; set; }
       /// <summary>
       /// 法人姓名
       /// </summary>
       public string LegalPerson { get; set; }
       /// <summary>
       /// 法人身份证号
       /// </summary>
       public string CertNo { get; set; }
       /// <summary>
       /// 手机号
       /// </summary>
       public string CellPhone { get; set; }
       /// <summary>
       /// 邮箱
       /// </summary>
       public string Email { get; set; }
    }
}
