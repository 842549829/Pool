using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
    /// <summary>
    /// 个人账户的所有信息
    /// </summary>
  public class AccountIndividual
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CertNo { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Faxes { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 县城
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        public CompanyType CompanyType { get; set; }
        /// <summary>
        /// 操作员账号
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 某些属性不需要验证
        /// </summary>
        public bool IsNotNeedCheck { get; set; }
        /// <summary>
        /// 所属OEM
        /// </summary>
        public Guid? OemOwner { get; set; }
    }
}
