using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
  public class AccountBasicIndividual
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
      /// 公司类型
      /// </summary>
      public CompanyType CompanyType { get; set; }

      /// <summary>
      /// 是否需要申请POS
      /// </summary>
      public bool IsNeedApply { get; set; }

      /// <summary>
      /// 所属哪个OEM下
      /// </summary>
      public Guid? OemOwner { get; set; }

    }
}
