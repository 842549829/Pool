using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class CompanyListInfo
    {
       /// <summary>
       /// 公司Id
       /// </summary>
       public Guid CompanyId { get; set; }
       /// <summary>
       /// 用户名
       /// </summary>
       public string UserNo { get; set; }
       /// <summary>
       /// 公司类型
       /// </summary>
       public CompanyType CompanyType { get; set; }
       /// <summary>
       /// 账户类型
       /// </summary>
       public AccountBaseType AccountType { get; set; }
       /// <summary>
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }
       /// <summary>
       /// 联系人
       /// </summary>
       public string Contact { get; set; }
       /// <summary>
       /// 状态
       /// </summary>
       public bool Enabled { get; set; }
       /// <summary>
       /// 是否审核
       /// </summary>
       public bool Audited { get; set; }
       /// <summary>
       /// 审核时间
       /// </summary>
       public DateTime? AuditTime { get; set; }

       public DateTime? LastLoginTime { get; set; }

       public DateTime RegisterTime { get; set; }
       /// <summary>
       /// 公司是否是OEM
       /// </summary>
       public bool IsOem { get; set; }
    }
}
