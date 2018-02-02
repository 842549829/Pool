using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.FlightTransfer
{
  public class FlightTransferCondition
    {
      /// <summary>
      /// 航空公司
      /// </summary>
      public string Carrier { get; set; }
      /// <summary>
      /// 原始航班号
      /// </summary>
      public string OriginalFlightNo { get; set; }
      /// <summary>
      /// 变更类型
      /// </summary>
      public TransferType? TransferType { get; set; }
      /// <summary>
      /// 原始起飞时间
      /// </summary>
      public DateTime? OriginalTakeOffLowerTime { get; set; }
      /// <summary>
      /// 原始起飞时间
      /// </summary>
      public DateTime? OriginalTakeOffUpperTime { get; set; }
    }
}
