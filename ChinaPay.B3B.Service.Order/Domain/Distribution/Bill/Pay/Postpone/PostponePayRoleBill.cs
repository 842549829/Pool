using System;
using ChinaPay.B3B.Service.Distribution.Domain.Role;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone {
    public class PostponePayRoleBill : PayRoleBill<PostponePayRoleBillSource, PostponePayDetailBill> {
        internal PostponePayRoleBill(TradeRole owner)
            : base(owner) {
        }
        internal PostponePayRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
        }

        internal Refund.Postpone.PostponeRefundRoleBill MakeRefundBill() {
            return this.Owner.MakeRefundBill(this);
        }
    }
}
