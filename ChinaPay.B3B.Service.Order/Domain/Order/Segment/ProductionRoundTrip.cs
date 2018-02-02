using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Order.Domain.Segment {
    /// <summary>
    /// 往返产品舱往返
    /// </summary>
    public class ProductionRoundTrip : RoundTrip {
        decimal _totalParValue = 0;
        Price _totalPrice = null;

        /// <param name="carrier">乘运人</param>
        /// <param name="outwardFlight">去程航班信息</param>
        /// <param name="returnFlight">回程航班信息</param>
        /// <param name="totalParValue">总票面价</param>
        internal ProductionRoundTrip(Carrier carrier, Flight outwardFlight, Flight returnFlight, decimal totalParValue)
            : base(carrier, outwardFlight, returnFlight) {
            _totalParValue = totalParValue;
        }
        /// <summary>
        /// 价格信息
        /// </summary>
        public override Price Price {
            get {
                if(null == _totalPrice) {
                    var totalAirportFee = this.OutwardFlight.Price.AirportFee + this.ReturnFlight.Price.AirportFee;
                    var totalBAF = this.OutwardFlight.Price.BAF + this.ReturnFlight.Price.BAF;
                    _totalPrice = new Price(_totalParValue, totalAirportFee, totalBAF);
                }
                return _totalPrice;
            }
        }
    }
}