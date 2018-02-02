using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class AccountBasicEnterprise
    {
       /// <summary>
       /// 企业名称
       /// </summary>
       public string AccountName { get; set; }

       /// <summary>
       /// 缩写名称
       /// </summary>
       public string AbbreviateName { get; set; }

       /// <summary>
       /// 组织机构代码
       /// </summary>
       public string OrginationCode { get; set; }

       /// <summary>
       /// 公司电话
       /// </summary>
       public string CompanyPhone { get; set; }

       /// <summary>
       /// 联系人
       /// </summary>
       public string ContactName { get; set; }

       /// <summary>
       /// 联系人电话
       /// </summary>
       public string ContactPhone { get; set; }

       /// <summary>
       /// 公司类型
       /// </summary>
       public CompanyType CompanyType { get; set; }

       /// <summary>
       /// 是否希望申请POS机
       /// </summary>
       public bool IsNeedApply { get; set; }

       /// <summary>
       /// 身份证号码
       /// </summary>
       public string IDCard { get; set; }

       /// <summary>
       /// 所属哪个OEM下
       /// </summary>
       public Guid? OemOwner { get; set; }
    }
}
