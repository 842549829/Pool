using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class PostponeFeeView {
        /// <summary>
        /// 航段
        /// </summary>
        public AirportPair AirportPair { get; set; }
        /// <summary>
        /// 改期费
        /// </summary>
        public decimal Fee { get; set; }
    }
}
