using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
  public class SpreadTicketView
    {
      /// <summary>
      /// 开始时间
      /// </summary>
      public DateTime? BeginFinishTime { get; set; }
      /// <summary>
      /// 结束时间
      /// </summary>
      public DateTime? EndFinishTime { get; set; }
      /// <summary>
      ///  机票状态
      /// </summary>
      public ChinaPay.B3B.Common.Enums.TicketState? TicketState { get; set; }
      /// <summary>
      /// 交易方
      /// </summary>
      public Guid? Bargainer { get; set; }
      /// <summary>
      /// 交易方类型
      /// </summary>
      public CompanyType? BargainType { get; set; }
      /// <summary>
      /// 推广方
      /// </summary>
      public Guid? Spreader { get; set; }
    }
}
