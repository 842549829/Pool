using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class ExternalInterfaceInfo
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
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }
       /// <summary>
       /// 公司类型
       /// </summary>
       public CompanyType CompanyType { get; set; }
       /// <summary>
       /// 账号类型
       /// </summary>
       public AccountBaseType AccountType { get; set; }
       /// <summary>
       /// 是否开通外接口
       /// </summary>
       public bool IsOpenExternalInterface { get; set; }
       /// <summary>
       /// 开通时间
       /// </summary>
       public DateTime? OpenTime { get; set; }

    }
}
