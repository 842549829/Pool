using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
  public class DistributionOEMCondition
    {
      /// <summary>
      /// 授权开始时间
      /// </summary>
      public DateTime? RegisterBeginTime
      {
          get;
          set;
      }
      /// <summary>
      /// 授权结束时间
      /// </summary>
      public DateTime? RegisterEndTime
      {
          get;
          set;
      }
      /// <summary>
      /// 用户名
      /// </summary>
      public string UserNo
      {
          get;
          set;
      }
      /// <summary>
      /// 授权状态
      /// </summary>
      public bool? AutorizationStatus
      {
          get;
          set;
      }
      /// <summary>
      /// 授权域名
      /// </summary>
      public string DomainName
      {
          get;
          set;
      }
      /// <summary>
      /// 公司简称
      /// </summary>
      public string AbbreviateName
      {
          get;
          set;
      }
      /// <summary>
      /// OEM名称
      /// </summary>
      public string SiteName
      {
          get;
          set;
      }
    }
}
