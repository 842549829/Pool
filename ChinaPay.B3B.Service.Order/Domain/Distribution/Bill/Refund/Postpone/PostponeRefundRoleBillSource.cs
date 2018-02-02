namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone {
    public class PostponeRefundRoleBillSource : RefundRoleBillSource<PostponeRefundDetailBill> {
        protected override System.Collections.Generic.IEnumerable<PostponeRefundDetailBill> GetDetailBills() {
            return DistributionQueryService.QueryPostponeRefundDetailBills(this.BillId);
        }
    }
}
