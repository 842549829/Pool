using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    public class PostponeFlight {
        /// <summary>
        /// 原航班信息
        /// </summary>
        public Flight OriginalFlight {
            get;
            internal set;
        }
        /// <summary>
        /// 新航班信息
        /// </summary>
        public Flight NewFlight {
            get;
            internal set;
        }
        /// <summary>
        /// 改期手续费
        /// </summary>
        public decimal PostponeFee {
            get;
            internal set;
        }
    }
}