using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Role;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal {
    public class NormalRefundRoleBill : RefundRoleBill<NormalRefundRoleBillSource, NormalRefundDetailBill> {
        LazyLoader<NormalPayRoleBill> _payRoleBillLoader = null;

        internal NormalRefundRoleBill(TradeRole owner)
            : base(owner) {
            _payRoleBillLoader = new LazyLoader<NormalPayRoleBill>();
        }
        internal NormalRefundRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
            _payRoleBillLoader = new LazyLoader<NormalPayRoleBill>(() => DistributionQueryService.QueryNormalPayRoleBill(this.Id));
        }

        public NormalPayRoleBill PayRoleBill {
            get {
                return _payRoleBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("原支付信息不能为空");
                _payRoleBillLoader.SetData(value);
            }
        }
    }
}
