using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class PlatformProcessRefundView {
        /// <summary>
        /// 航段
        /// </summary>
        public AirportPair AirportPair { get; set; }
        /// <summary>
        /// 退还的服务费
        /// </summary>
        public decimal ServiceCharge { get; set; }
    }
}