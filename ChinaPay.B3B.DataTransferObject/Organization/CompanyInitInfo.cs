using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class CompanyInitInfo
    {
       /// <summary>
       /// 公司Id
       /// </summary>
       public Guid CompanyId { get; set; }
       /// <summary>
       /// 公司类型
       /// </summary>
       public CompanyType CompanyType { get; set; }
       /// <summary>
       /// 公司账号
       /// </summary>
       public string UserNo { get; set; }
       /// <summary>
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }
    }
}
