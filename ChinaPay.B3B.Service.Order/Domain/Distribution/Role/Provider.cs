using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 出票方
    /// </summary>
    public class Provider : TradeRoyalty {
        internal Provider(Guid id)
            : base(id) {
        }
        internal Provider(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        public override TradeRoleType RoleType {
            get { return TradeRoleType.Provider; }
        }
        protected override decimal GetTradeRate(TradeInfo trade) {
            var providerParams = Organization.CompanyService.GetCompanyParameter(this.Id);
            // 内部机构的手续费为0
            if(trade.ProviderRelationWithPurchaser == Common.Enums.RelationType.Interior) return 0;

            if(trade.IsSpecialProduct && !trade.IsThirdRelation) {
                return providerParams == null ? SystemManagement.SystemParamService.DefaultTradeRateForSpecial : GetTradeRate(trade.SpecialProductType, providerParams);
            } else {
                if(trade.ProviderRelationWithPurchaser == Common.Enums.RelationType.Brother) {
                    return providerParams == null ? SystemManagement.SystemParamService.DefaultTradeRateForBrother : providerParams.ProfessionRate;
                } else if(trade.ProviderRelationWithPurchaser == Common.Enums.RelationType.Junion) {
                    return providerParams == null ? SystemManagement.SystemParamService.DefaultTradeRateForJunior : providerParams.SubordinateRate;
                }
            }
            return 0;
        }
        protected override decimal CalcCommission(Flight flight, decimal rebate) {
            return base.CalcCommission(flight, rebate) * -1;
        }
        protected override decimal GetServiceCharge(bool isThirdRelation, Flight flight) {
            return isThirdRelation ? 0 : (flight.ReleasedFare - flight.Fare);
        }
        protected override decimal GetAnticipation(Flight flight, decimal serviceCharge, decimal commission, decimal increasing) {
            return flight.Fare + flight.AirportFee + flight.BAF + commission + serviceCharge + increasing;
        }
        protected override decimal GetRefundServiceCharge(bool isThirdRelation, Bill.Refund.RefundFlight refundFlight) {
            return isThirdRelation ? 0 : (refundFlight.RefundServiceCharge ?? 0) * -1;
        }
        protected override decimal GetRefundFee(Flight flight, Guid passenger, Bill.Refund.RefundFlight refundFlight) {
            return refundFlight.FeeForProvider;
        }
        protected override decimal GetRefundAnticipation(Bill.Pay.Normal.NormalPayDetailBill payBill, decimal refundCommission, decimal refundIncreasing, decimal refundServiceCharge, decimal refundRate, decimal refundFee) {
            // 退还票面价
            var requireRefundFare = payBill.Flight.Fare - refundFee;
            if(payBill.Flight.Fare > 0) {
                // 防止过度的收取手续费
                if(requireRefundFare < 0) requireRefundFare = 0;
            }
            // 退还总金额
            // 所有退票，都要收回佣金
            var requireRefundAmount = (requireRefundFare + payBill.Commission + payBill.Flight.AirportFee + payBill.Flight.BAF) * -1;
            // 退还服务费
            requireRefundAmount += refundServiceCharge;
            requireRefundAmount += refundIncreasing;
            return requireRefundAmount > 0 ? 0 : requireRefundAmount;
        }
        protected override decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight) {
            return refundFlight.Amount * -1;
        }
    }
}