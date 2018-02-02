using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
  public class BargainDefaultPolicyQueryParameter
    {
      /// <summary>
      /// 航空公司
      /// </summary>
      public string Airline { get; set; }
      /// <summary>
      /// 省份代码
      /// </summary>
      public string ProvinceCode { get; set; }
      /// <summary>
      /// 出票方Id
      /// </summary>
      public Guid? AdultProviderId { get; set; }

      public int PageIndex { get; set; }
      public int PageSize { get; set; }
    }
}
