using System;
using ChinaPay.B3B.Service.Distribution.Domain.Role;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone {
    public class PostponeRefundRoleBill : RefundRoleBill<PostponeRefundRoleBillSource, PostponeRefundDetailBill> {
        LazyLoader<PostponePayRoleBill> _payRoleBillLoader = null;

        internal PostponeRefundRoleBill(TradeRole owner)
            : base(owner) {
            _payRoleBillLoader = new LazyLoader<PostponePayRoleBill>();
        }
        internal PostponeRefundRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
            _payRoleBillLoader = new LazyLoader<PostponePayRoleBill>(() => DistributionQueryService.QueryPostponePayRoleBill(this.Id));
        }

        public PostponePayRoleBill PayRoleBill {
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
