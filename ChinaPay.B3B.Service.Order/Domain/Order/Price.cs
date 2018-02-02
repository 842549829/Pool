using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Order.Domain {
    public class Price {
        public Price(decimal fare, decimal airportFee, decimal baf) {
            this.Fare = fare;
            this.AirportFee = airportFee;
            this.BAF = baf;
        }
        /// <summary>
        /// 空值
        /// </summary>
        public static Price Zero {
            get {
                return new Price(0, 0, 0);
            }
        }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal Fare {
            get;
            private set;
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            private set;
        }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public decimal BAF {
            get;
            private set;
        }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total {
            get {
                return this.Fare + this.AirportFee + this.BAF;
            }
        }

        public static Price operator +(Price left, Price right) {
            decimal fare = 0, airportFee = 0, baf = 0;
            if(left != null) {
                fare = left.Fare;
                airportFee = left.AirportFee;
                baf = left.BAF;
            }
            if(right != null) {
                fare += right.Fare;
                airportFee += right.AirportFee;
                baf += right.BAF;
            }
            return new Price(fare, airportFee, baf);
        }
    }
}
