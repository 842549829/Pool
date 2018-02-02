using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Foundation
{
    /// <summary>
    /// 非固定航行
    /// </summary>
    public class FixedNavigationView
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达地
        /// </summary>
        public string Arrival { get; set; }
    }

}
