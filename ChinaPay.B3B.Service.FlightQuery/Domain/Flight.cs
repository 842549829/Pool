using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    /// <summary>
    /// 航班信息
    /// </summary>
    public class Flight {
        internal Flight(string airline, string flightNo) {
            this.Airline = airline;
            this.FlightNo = flightNo;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public string Airline {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司名称
        /// </summary>
        public string AirlineName {
            get;
            internal set;
        }
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo {
            get;
            private set;
        }
        /// <summary>
        /// 机型
        /// </summary>
        public string AirCraft {
            get;
            internal set;
        }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate {
            get;
            internal set;
        }
        /// <summary>
        /// 起飞时间
        /// </summary>
        public Time TakeoffTime {
            get;
            internal set;
        }
        /// <summary>
        /// 降落时间
        /// </summary>
        public Time LandingTime {
            get;
            internal set;
        }
        /// <summary>
        /// 出发机场信息
        /// </summary>
        public Airport Departure {
            get;
            internal set;
        }
        /// <summary>
        /// 到达机场信息
        /// </summary>
        public Airport Arrival {
            get;
            internal set;
        }
        /// <summary>
        /// 是否有餐食
        /// </summary>
        public bool HasFood {
            get;
            internal set;
        }
        /// <summary>
        /// 是否经停
        /// </summary>
        public bool IsStop {
            get;
            internal set;
        }
        /// <summary>
        /// 标准票面价
        /// </summary>
        public decimal StandardPrice {
            get;
            internal set;
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            internal set;
        }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public BAFValueView BAF {
            get;
            internal set;
        }
        /// <summary>
        /// 舱位列表
        /// </summary>
        public IEnumerable<Bunk> Bunks {
            get;
            internal set;
        }
        /// <summary>
        /// 过滤掉的舱位列表
        /// </summary>
        public IEnumerable<Bunk> FilteredBunks {
            get;
            internal set;
        }
        /// <summary>
        /// 飞行天数
        /// </summary>
        public int DaysInterval { get; internal set; }
    }
}