namespace ChinaPay.B3B.DataTransferObject.Order {
    public class PriceView {
        /// <summary>
        /// 航段
        /// </summary>
        public Common.AirportPair AirportPair { get; set; }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal Fare { get; set; }
        /// <summary>
        /// 机建费
        /// </summary>
        public decimal AirportFee { get; set; }
    }
}