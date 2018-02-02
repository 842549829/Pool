using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 基础运价
    /// </summary>
    public class BasicPrice {
        internal BasicPrice()
            : this(Guid.NewGuid()) {
        }
        internal BasicPrice(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public UpperString AirlineCode {
            get;
            internal set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Airline Airline {
            get {
                return FoundationService.QueryAirline(AirlineCode);
            }
        }
        /// <summary>
        /// 出发机场代码
        /// </summary>
        public UpperString DepartureCode {
            get;
            internal set;
        }
        /// <summary>
        /// 出发机场
        /// </summary>
        public Airport Departure {
            get {
                return FoundationService.QueryAirport(DepartureCode);
            }
        }
        /// <summary>
        /// 到达机场代码
        /// </summary>
        public UpperString ArrivalCode {
            get;
            internal set;
        }
        /// <summary>
        /// 到达机场
        /// </summary>
        public Airport Arrival {
            get {
                return FoundationService.QueryAirport(ArrivalCode);
            }
        }
        /// <summary>
        /// 航班日期
        /// </summary>
        public DateTime FlightDate {
            get;
            internal set;
        }
        /// <summary>
        /// 出票日期
        /// </summary>
        public DateTime ETDZDate {
            get;
            internal set;
        }
        /// <summary>
        /// 公布价格
        /// </summary>
        public decimal Price {
            get;
            internal set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime { get; internal set; }

        /// <summary>
        /// 里程数
        /// </summary>
        public decimal Mileage {
            get;
            internal set;
        }
        public override string ToString() {
            return string.Format("航空公司:{0} 出发机场:{1} 到达机场:{2} 航班日期:{3} 出票日期:{4} 公布价格:{5} 里程数:{6}修改时间：{7:yyyy-MM-dd HH:mm}",
                AirlineCode.Value, DepartureCode.Value, ArrivalCode.Value, FlightDate.ToShortDateString(), ETDZDate.ToShortDateString(), Price, Mileage,ModifyTime);
        }

        internal static BasicPrice GetBasicPrice(BasicPriceView basicPriceView) {
            if(null == basicPriceView)
                throw new ArgumentNullException("basicPriceView");
            basicPriceView.Validate();
            return new BasicPrice() {
                AirlineCode = ChinaPay.Utility.StringUtility.Trim(basicPriceView.Airline),
                DepartureCode = ChinaPay.Utility.StringUtility.Trim(basicPriceView.Departure),
                ArrivalCode = ChinaPay.Utility.StringUtility.Trim(basicPriceView.Arrival),
                Price = basicPriceView.Price,
                Mileage = basicPriceView.Mileage,
                FlightDate = basicPriceView.FlightDate,
                ETDZDate = basicPriceView.ETDZDate,
                ModifyTime = basicPriceView.ModifyTime
            };
        }
        internal static BasicPrice GetBasicPrice(Guid id, BasicPriceView basicPriceView) {
            var basicPrice = GetBasicPrice(basicPriceView);
            basicPrice.Id = id;
            basicPrice.ModifyTime = basicPriceView.ModifyTime;
            return basicPrice;
        }
        internal static decimal CalcFare(decimal standardFare, decimal discount) {
            return Utility.Calculator.Round(standardFare * discount, 1);
        }
        internal static decimal CalcDiscount(decimal standardFare, decimal fare) {
            var discount = Utility.Calculator.Round(fare / standardFare, -2);
            if(CalcFare(standardFare, discount) != fare)
                discount = Utility.Calculator.Round(fare / standardFare, -3);
            return discount;
        }
    }
}