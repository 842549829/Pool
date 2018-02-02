using ChinaPay.B3B.Common.Enums;
using System;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class RelationInfo
    {
       /// <summary>
       /// 公司类型
       /// </summary>
       public CompanyType CompanyType { get; set; }

       /// <summary>
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }

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
       public string ContactPhone { get; set; }

       /// <summary>
       /// 用户名
       /// </summary>
       public string UserNo { get; set; }

       /// <summary>
       /// 注册时间
       /// </summary>
       public DateTime RegisterTime { get; set; }
    }
}
