using System;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class BasicPriceView {
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public string Airline {
            get;
            set;
        }
        /// <summary>
        /// 出发机场代码
        /// </summary>
        public string Departure {
            get;
            set;
        }
        /// <summary>
        /// 到达机场代码
        /// </summary>
        public string Arrival {
            get;
            set;
        }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate {
            get;
            set;
        }
        /// <summary>
        /// 出票日期
        /// </summary>
        public DateTime ETDZDate {
            get;
            set;
        }
        /// <summary>
        /// 公布价格
        /// </summary>
        public decimal Price {
            get;
            set;
        }
        /// <summary>
        /// 里程数
        /// </summary>
        public decimal Mileage {
            get;
            set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Departure))
                throw new ArgumentNullException("Departure");
            if(string.IsNullOrWhiteSpace(this.Arrival))
                throw new ArgumentNullException("Arrival");
            if(!string.IsNullOrWhiteSpace(this.Airline) && !Regex.IsMatch(this.Airline.Trim(), AirlineView.AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("所属航空公司代码格式错误");
            if(!Regex.IsMatch(this.Departure.Trim(), AirportView.AirportCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("出发地代码格式错误");
            if(!Regex.IsMatch(this.Arrival.Trim(), AirportView.AirportCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("到达地代码格式错误");
            if(string.Compare(this.Departure.Trim(), this.Arrival.Trim(), true) == 0)
                throw new ChinaPay.Core.CustomException("出发地与到达地不能相同");
        }
    }
}
