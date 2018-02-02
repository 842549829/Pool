using System;
using ChinaPay.B3B.Service.Distribution.Domain.Role;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay {
    /// <summary>
    /// 角色支付账单
    /// </summary>
    public class PayRoleBill<TPayRoleBillSource, TPayDetailBill> : RoleBill<TPayRoleBillSource, TPayDetailBill>
        where TPayRoleBillSource : PayRoleBillSource<TPayDetailBill>
        where TPayDetailBill : PayDetailBill {

        internal PayRoleBill(TradeRole owner)
            : base(owner) {
        }
        internal PayRoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
        }
    }
}