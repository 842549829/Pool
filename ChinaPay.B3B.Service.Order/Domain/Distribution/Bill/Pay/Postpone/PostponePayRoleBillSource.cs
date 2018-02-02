using System.Linq;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone {
    public class PostponePayRoleBillSource : PayRoleBillSource<PostponePayDetailBill> {
        decimal? _postponeFee = null;

        /// <summary>
        /// 改期费
        /// </summary>
        public decimal PostponeFee {
            get {
                if(!_postponeFee.HasValue) {
                    _postponeFee = Details.Sum(item => item.PostponeFee);
                }
                return _postponeFee.Value;
            }
            internal set {
                _postponeFee = value;
            }
        }

        protected override System.Collections.Generic.IEnumerable<PostponePayDetailBill> GetDetailBills() {
            return DistributionQueryService.QueryPostponePayDetailBills(this.BillId);
        }
    }
}