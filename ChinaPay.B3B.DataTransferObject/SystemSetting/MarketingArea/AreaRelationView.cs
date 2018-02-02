using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea
{
  public class AreaRelationView
    {
      /// <summary>
      /// 省份代码
      /// </summary>
      public string Province
      {
          get;
          set;
      }
      /// <summary>
      /// 省份名称
      /// </summary>
      public string ProcinceName
      {
          get;
          set;
      }
      /// <summary>
      /// 区域名称
      /// </summary>
      public string AreaName
      {
          get;
          set;
      }
    }
}
