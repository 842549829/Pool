using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class CompanyGroupMemberListInfo
    {
       public Guid CompanyId { get; set; }
       /// <summary>
       /// 公司名称
       /// </summary>
       public string CompanyName { get; set; }
       /// <summary>
       /// 公司账号
       /// </summary>
       public string UserNo { get; set; }
       /// <summary>
       /// 所在城市
       /// </summary>
       public string City { get; set; }
       /// <summary>
       /// 联系人
       /// </summary>
       public string Contact { get; set; }
       /// <summary>
       /// 联系电话
       /// </summary>
       public string ContactPhone { get; set;}
       /// <summary>
       /// 公司组
       /// </summary>
       public string Group { get; set; }
       /// <summary>
       /// 注册时间
       /// </summary>
       public DateTime RegisterTime { get; set; }
    }
}
