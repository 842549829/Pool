using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 资源方
    /// </summary>
    public class Supplier : TradeRoyalty {
        internal Supplier(Guid id)
            : base(id) {
        }
        internal Supplier(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        public override TradeRoleType RoleType {
            get { return TradeRoleType.Supplier; }
        }
        protected override decimal GetTradeRate(TradeInfo trade) {
            var supplierParams = Organization.CompanyService.GetCompanyParameter(this.Id);
            return supplierParams == null ? SystemManagement.SystemParamService.DefaultTradeRateForSpecial : GetTradeRate(trade.SpecialProductType, supplierParams);
        }
        protected override decimal GetServiceCharge(bool isThirdRelation, Flight flight) {
            return flight.ReleasedFare - flight.Fare;
        }
        protected override decimal GetAnticipation(Flight flight, decimal serviceCharge, decimal commission, decimal increasing) {
            return commission + serviceCharge + increasing;
        }
        protected override decimal GetRefundServiceCharge(bool isThirdRelation, Bill.Refund.RefundFlight refundFlight) {
            return (refundFlight.RefundServiceCharge ?? 0) * -1;
        }
        protected override decimal GetRefundFee(Flight flight, Guid passenger, Bill.Refund.RefundFlight refundFlight) {
            return refundFlight.FeeForSupplier;
        }
        protected override decimal GetRefundAnticipation(Bill.Pay.Normal.NormalPayDetailBill payBill, decimal refundCommission, decimal refundIncreasing, decimal refundServiceCharge, decimal refundRate, decimal refundFee) {
            return refundServiceCharge + refundCommission + refundIncreasing + refundFee;
        }
        protected override decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight) {
            return 0;
        }
    }
}