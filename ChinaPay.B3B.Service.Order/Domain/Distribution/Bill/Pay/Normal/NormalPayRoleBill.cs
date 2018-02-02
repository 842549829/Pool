using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Distribution.Domain.Role;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal {
    public class NormalPayRoleBill : PayRoleBill<NormalPayRoleBillSource, NormalPayDetailBill> {
        internal NormalPayRoleBill(TradeRole owner)
            : base(owner) {
        }
        internal NormalPayRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
        }

        internal Refund.Normal.NormalRefundRoleBill MakeRefundBill(Refund.RefundInfo refundInfo, IEnumerable<Refund.Normal.NormalRefundRoleBill> refundedBills) {
            return Owner.MakeRefundBill(this, refundInfo, refundedBills);
        }
        internal Refund.Normal.NormalRefundRoleBill MakeErrorRefundBill(Refund.ErrorRefundInfo refundInfo, IEnumerable<Refund.Normal.NormalRefundRoleBill> refundedBills) {
            return Owner.MakeErrorRefundBill(this, refundInfo, refundedBills);
        }
        internal void RevisePrice(TradeInfo trade) {
            Owner.RefreshPrice(trade, this);
        }
        internal void RefreshReleaseFare(TradeInfo trade) {
            Owner.RefreshReleaseFare(trade, this);
            Source.RefreshReleaseFare();
            RefreshAmount();
        }
        internal void RefreshFare(TradeInfo trade) {
            Owner.RefreshFare(trade, this);
            Source.RefreshFare();
        }
    }
}