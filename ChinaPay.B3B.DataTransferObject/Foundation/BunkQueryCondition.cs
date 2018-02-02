using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Foundation
{
   public class BunkQueryCondition
    {
       /// <summary>
       /// 航班开始日期
       /// </summary>
       public DateTime? FlightBeginDate { get; set; }
       /// <summary>
       /// 航班截止日期
       /// </summary>
       public DateTime? FlightEndDate { get; set; }
       /// <summary>
       /// 航班状态
       /// </summary>
       public bool? Status { get; set; }
       /// <summary>
       /// 航空公司
       /// </summary>
       public string Airline { get; set; }
       /// <summary>
       /// 出发机场
       /// </summary>
       public string Departure { get; set; }
       /// <summary>
       /// 到达机场
       /// </summary>
       public string Arrival { get; set; }
       /// <summary>
       /// 舱位
       /// </summary>
       public string BunkCode { get; set; }
       /// <summary>
       /// 舱位类型
       /// </summary>
       public BunkType? BunkType { get; set; }
       /// <summary>
       /// 适用行程
       /// </summary>
       public VoyageTypeValue? VoyageType { get; set; }
    }
}
