using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    public class PlatformRoyalty : TradeRoyalty {
        internal PlatformRoyalty(Guid id)
            : base(id) {
        }
        internal PlatformRoyalty(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        public override TradeRoleType RoleType {
            get { return TradeRoleType.Platform; }
        }
        protected override decimal GetTradeRate(TradeInfo trade) {
            return 0;
        }
        protected override string GetAccount() {
            return SystemManagement.SystemParamService.PlatformIncomeAccount;
        }
        protected override decimal GetPostponeFee(Order.Domain.Applyform.PostponeFlight flight) {
            return flight.PostponeFee;
        }
        protected override decimal GetRefundFee(Flight flight, Guid passenger, Bill.Refund.RefundFlight refundFlight) {
            return refundFlight.FeeForPurchaser;
        }
        protected override decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight) {
            return 0;
        }
    }
}