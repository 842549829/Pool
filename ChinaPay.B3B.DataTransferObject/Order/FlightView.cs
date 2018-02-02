using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class FlightView {
        /// <summary>
        /// 序号
        /// </summary>
        public int SerialNo { get; set; }
        /// <summary>
        /// 出发机场三字码
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达机场三字码
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public DateTime TakeoffTime { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime LandingTime { get; set; }
        /// <summary>
        /// 航空公司二字码
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }
        /// <summary>
        /// Y舱标准价
        /// </summary>
        public decimal YBPrice { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string AirCraft { get; set; }
        /// <summary>
        /// 舱位代码
        /// </summary>
        public string Bunk { get; set; }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal Fare { get; set; }
        /// <summary>
        /// 舱位类型
        /// </summary>
        public B3B.Common.Enums.BunkType Type { get; set; }

        /// <summary>
        /// 是否是共享航班
        /// </summary>
        public bool IsShare { get; set; }

        /// <summary>
        /// 到达航站楼
        /// </summary>
        public string ArrivalTerminal { get; set; }

        /// <summary>
        /// 出发航站楼
        /// </summary>
        public string DepartureTerminal { get; set; }
    }
}