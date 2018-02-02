using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR {
    public class FlightView {
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate { get; set; }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public Time TakeoffTime { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public Time LandingTime { get; set; }
        /// <summary>
        /// 仓位
        /// </summary>
        public string Bunk { get; set; }
    }
}
