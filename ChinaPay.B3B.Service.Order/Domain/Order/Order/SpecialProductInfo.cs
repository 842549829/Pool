using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 特殊产品信息
    /// </summary>
    public class SpeicalProductInfo : ProductInfo {
        private bool _isThirdRelation = false;
        internal SpeicalProductInfo()
            : base() {
        }
        internal SpeicalProductInfo(decimal orderId, bool isThirdRelation)
            : base(orderId) {
            _isThirdRelation = isThirdRelation;
        }

        /// <summary>
        /// 特殊产品类型
        /// </summary>
        public SpecialProductType SpeicalProductType {
            get;
            internal set;
        }
        /// <summary>
        /// 是否需要确认
        /// </summary>
        public bool RequireConfirm {
            get;
            internal set;
        }
        
        protected override RefundAndReschedulingProvision QueryRefundAndReschedulingProvision(decimal orderId) {
            if(_isThirdRelation)
                return OrderQueryService.QuerySupplierRefundAndReschedulingProvision(orderId);
            else
                return OrderQueryService.QueryProviderRefundAndReschedulingProvision(orderId);
        }
    }
}