using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 改期信息
    /// </summary>
    public class PostponeView {
        /// <summary>
        /// 新编码
        /// </summary>
        public PNRPair NewPNR { get; set; }
        /// <summary>
        /// 改期内容
        /// </summary>
        public IEnumerable<Item> Items { get; set; }
        public class Item {
            /// <summary>
            /// 航段信息
            /// </summary>
            public AirportPair AirportPair { get; set; }
            /// <summary>
            /// 航班号
            /// </summary>
            public string FlightNo { get; set; }
            /// <summary>
            /// 机型
            /// </summary>
            public string AirCraft { get; set; }
            /// <summary>
            /// 起飞时间
            /// </summary>
            public DateTime TakeoffTime { get; set; }
            /// <summary>
            /// 降落时间
            /// </summary>
            public DateTime LandingTime { get; set; }
        }
    }
}
