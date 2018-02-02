using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    public abstract class TradeRoyalty : TradeRole {
        internal TradeRoyalty(Guid id)
            : base(id) {
        }

        internal TradeRoyalty(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        protected override string GetAccount() {
            var account = Organization.AccountService.Query(this.Id, Common.Enums.AccountType.Receiving);
            if(account != null && account.Valid) {
                return account.No;
            }
            return string.Empty;
        }
        protected override decimal GetServiceCharge(bool isThirdRelation, Flight flight) {
            return 0;
        }
        protected override decimal GetPostponeFee(Order.Domain.Applyform.PostponeFlight flight) {
            return 0;
        }
        protected override decimal GetAnticipation(Flight flight, decimal serviceCharge, decimal commission, decimal increasing) {
            return commission + increasing;
        }
        protected override decimal GetRefundServiceCharge(bool isThirdRelation, Bill.Refund.RefundFlight refundFlight) {
            return 0;
        }
        protected override decimal GetRefundFee(Flight flight, Guid passenger, Bill.Refund.RefundFlight refundFlight) {
            return 0;
        }
        protected override decimal GetRefundAnticipation(Bill.Pay.Normal.NormalPayDetailBill payBill, decimal refundCommission, decimal refundIncreasing, decimal refundServiceCharge, decimal refundRate, decimal refundFee) {
            return refundCommission + refundIncreasing;
        }
    }
}