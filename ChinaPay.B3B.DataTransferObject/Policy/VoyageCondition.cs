using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 行程条件
    /// </summary>
    public class VoyageCondition
    {
        /// <summary>
        /// 行程编号
        /// </summary>
        public int VoyageNumber { get; set; }

        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// 到达
        /// </summary>
        public string  Arrial { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
