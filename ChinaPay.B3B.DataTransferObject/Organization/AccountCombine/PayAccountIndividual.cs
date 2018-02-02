using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
  public class PayAccountIndividual :AccountInfo
    {
        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayPassword { get; set; }

        /// <summary>
        /// 确认支付密码
        /// </summary>
        public string ComfirmPayPassword { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string CertNo { get; set; }
        
        /// <summary>
        /// 手机号码
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}
