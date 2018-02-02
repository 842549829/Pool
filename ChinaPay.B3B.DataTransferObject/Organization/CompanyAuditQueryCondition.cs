using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
  public class CompanyAuditQueryCondition
    {
      /// <summary>
      /// 账号
      /// </summary>
      public string UserNo { get; set; }
      /// <summary>
      /// 公司名称
      /// </summary>
      public string CompanyName { get; set; }
      /// <summary>
      /// 公司类型
      /// </summary>
      public CompanyType? CompanyType { get; set; }
      /// <summary>
      /// 账户类型
      /// </summary>
      public AccountBaseType? AccountType { get; set; }
      /// <summary>
      /// 申请时间开始
      /// </summary>
      public DateTime? ApplyTimeStart { get; set; }
      /// <summary>
      /// 申请时间结束
      /// </summary>
      public DateTime? ApplyTimeEnd { get; set; }
      /// <summary>
      /// 审核类型
      /// </summary>
      public string AuditType { get; set; }
      /// <summary>
      /// 来源渠道
      /// </summary>
      public string SourceType { get; set; }
    }
}
