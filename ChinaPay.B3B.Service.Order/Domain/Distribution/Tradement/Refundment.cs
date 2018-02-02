using System;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Tradement {
    /// <summary>
    /// 退款信息
    /// </summary>
    public class Refundment : Distribution.Domain.Tradement.Tradement {
        LazyLoader<Payment> _paymentLoader;

        internal Refundment()
            : this(Guid.NewGuid()) {
        }
        internal Refundment(Guid id) : base(id) {
            _paymentLoader = new LazyLoader<Payment>(() => DistributionQueryService.QueryPayment(TradeNo));
        }

        /// <summary>
        /// 支付信息
        /// </summary>
        public Payment Payment {
            get {
                return _paymentLoader.QueryData();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("原支付信息不能为空");
                _paymentLoader.SetData(value);
            }
        }

        public override string ToString() {
            return "退款";
        }
    }
}
