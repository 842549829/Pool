using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 政策信息
    /// </summary>
    public class CommonProductInfo : ProductInfo {
        internal CommonProductInfo()
            : base() {
        }
        internal CommonProductInfo(decimal orderId)
            : base(orderId) {
        }

        /// <summary>
        /// 出票方式
        /// </summary>
        public ETDZMode ETDZMode {
            get;
            internal set;
        }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool RequireChangePNR {
            get;
            internal set;
        }

        protected override RefundAndReschedulingProvision QueryRefundAndReschedulingProvision(decimal orderId) {
            return OrderQueryService.QueryProviderRefundAndReschedulingProvision(orderId);
        }
    }
} 