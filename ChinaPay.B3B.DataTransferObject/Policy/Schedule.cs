using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 班期
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// 工作日或日期
        /// </summary>
        public string WeekOrDay { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public DateMode Choice { get; set; }


    }
}
