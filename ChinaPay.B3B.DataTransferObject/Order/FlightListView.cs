using System;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class FlightListView {
        /// <summary>
        /// 出发城市
        /// </summary>
        public string DepartureCity { get; set; }
        /// <summary>
        /// 出发机场
        /// </summary>
        public string DepartureAirport { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string ArrivalCity { get; set; }
        /// <summary>
        /// 到达机场
        /// </summary>
        public string ArrivalAirport { get; set; }
        /// <summary>
        /// 乘运人代码
        /// </summary>
        public string Carrier {get;set;}
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Bunk { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public DateTime TakeoffTime { get; set; }
        /// <summary>
        /// 发布票面价
        /// </summary>
        public decimal ReleasedFare { get; set; }
        /// <summary>
        /// 真实票面价
        /// </summary>
        public decimal Fare { get; set; }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee { get; set; }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public decimal BAF { get; set; }
        /// <summary>
        /// 出发航站楼
        /// </summary>
        public string DepartureTeminal { get; set; }
        /// <summary>
        /// 到达航站楼
        /// </summary>
        public string ArrivalTeminal { get; set; }
    }
}
