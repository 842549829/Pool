using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class CompanyGroupMemberParameter
    {
       /// <summary>
       /// 父公司Id
       /// </summary>
       public Guid Superior { get; set; }
       /// <summary>
       /// 公司组Id
       /// </summary>
       public Guid GroupId { get; set; }
       /// <summary>
       /// 公司名称
       /// </summary>
       public string CompanyName { get; set; }
       /// <summary>
       /// 公司账号
       /// </summary>
       public string UserNo { get; set; }
       /// <summary>
       /// 联系人
       /// </summary>
       public string Contact { get; set; }
    }
}
