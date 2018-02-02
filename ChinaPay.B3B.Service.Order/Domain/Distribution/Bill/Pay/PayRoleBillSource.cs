namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay {
    public abstract class PayRoleBillSource<TPayDetailBill> : RoleBillSource<TPayDetailBill>
        where TPayDetailBill : PayDetailBill {
    }
}