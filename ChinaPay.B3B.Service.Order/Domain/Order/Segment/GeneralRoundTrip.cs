namespace ChinaPay.B3B.Service.Order.Domain.Segment {
    /// <summary>
    /// 普通往返
    /// </summary>
    public class GeneralRoundTrip : RoundTrip {
        /// <param name="carrier">乘运人</param>
        /// <param name="outwardFlight">去程航班信息</param>
        /// <param name="returnFlight">回程航班信息</param>
        internal GeneralRoundTrip(Carrier carrier, Flight outwardFlight, Flight returnFlight)
            : base(carrier, outwardFlight, returnFlight) {
        }
        /// <summary>
        /// 价格信息
        /// </summary>
        public override Price Price {
            get {
                return this.OutwardFlight.Price + this.ReturnFlight.Price;
            }
        }
    }
}
