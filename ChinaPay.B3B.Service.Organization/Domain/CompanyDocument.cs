using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
  public class CompanyDocument
    {
       /// <summary>
       /// 公司Id
       /// </summary>
       public Guid Company { get; set; }
       /// <summary>
       /// 营业执照
       /// </summary>
       public byte[] BussinessLicense { get; set; }
       /// <summary>
       /// 航协认可证书
       /// </summary>
       public byte[] IATALicense { get; set; }
       /// <summary>
       /// 身份证
       /// </summary>
       public byte[] CertLicense { get; set; }
       /// <summary>
       /// 从业时间
       /// </summary>
       public int? BussinessTime { get; set; }
    }
}
