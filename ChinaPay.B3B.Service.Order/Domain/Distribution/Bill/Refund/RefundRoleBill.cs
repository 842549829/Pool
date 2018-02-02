using System;
using ChinaPay.B3B.Service.Distribution.Domain.Role;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    /// <summary>
    /// 角色退款账单
    /// </summary>
    public abstract class RefundRoleBill<TRefundRoleBillSource, TRefundDetailBill> : RoleBill<TRefundRoleBillSource, TRefundDetailBill>
        where TRefundRoleBillSource : RefundRoleBillSource<TRefundDetailBill>
        where TRefundDetailBill : RefundDetailBill {

        internal RefundRoleBill(TradeRole owner)
            : base(owner) {
        }
        internal RefundRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
        }
    }
}