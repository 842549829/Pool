using System;

namespace ChinaPay.B3B.DataTransferObject.FlightQuery {
    public class AirportView {
        /// <summary>
        /// 机场代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 机场名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 航站楼
        /// </summary>
        public string Terminal { get; set; }
        /// <summary>
        /// 起飞/降落时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}