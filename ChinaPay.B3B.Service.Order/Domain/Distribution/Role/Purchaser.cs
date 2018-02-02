using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 采购方
    /// </summary>
    public class Purchaser : TradeRole {
        internal Purchaser(Guid id)
            : base(id) {
        }
        internal Purchaser(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        public override TradeRoleType RoleType {
            get { return TradeRoleType.Purchaser; }
        }
        protected override decimal GetTradeRate(TradeInfo trade) {
            return 0;
        }
        protected override string GetAccount() {
            var account = Organization.AccountService.Query(this.Id, Common.Enums.AccountType.Payment);
            if(account != null && account.Valid) {
                return account.No;
            }
            return string.Empty;
        }
        protected override decimal GetServiceCharge(bool isThirdRelation, Flight flight) {
            return flight.Fare - flight.ReleasedFare;
        }
        /// <summary>
        /// 获取改期时的交易手续费
        /// </summary>
        protected override decimal GetPostponeTradeFee() {
            return SystemManagement.SystemParamService.PostponeTradeFee;
        }
        protected override decimal GetPostponeFee(Order.Domain.Applyform.PostponeFlight flight) {
            return flight.PostponeFee * -1;
        }
        protected override decimal GetAnticipation(Flight flight, decimal serviceCharge, decimal commission, decimal increasing) {
            return (flight.Fare + flight.AirportFee + flight.BAF - serviceCharge - commission - increasing) * -1;
        }
        protected override decimal GetRefundIncreasing(Bill.Pay.Normal.NormalPayDetailBill payBill, bool providerRefundServiceCharge) {
            // 如果产品发布方要退服务费，则要将当时被加价的金额收回来
            return providerRefundServiceCharge ? payBill.Increasing * -1 : 0;
        }
        protected override decimal GetRefundServiceCharge(bool isThirdRelation, Bill.Refund.RefundFlight refundFlight) {
            return refundFlight.RefundServiceCharge ?? 0;
        }
        protected override decimal GetRefundFee(Flight flight, Guid passenger, Bill.Refund.RefundFlight refundFlight) {
            return refundFlight.FeeForPurchaser * -1;
        }
        protected override decimal GetRefundAnticipation(Bill.Pay.Normal.NormalPayDetailBill payBill, decimal refundCommission, decimal refundIncreasing, decimal refundServiceCharge, decimal refundRate, decimal refundFee) {
            // 票面价退还金额
            var requireRefundFare = payBill.Flight.Fare + refundFee;
            if(payBill.Flight.Fare > 0) {
                // 防止过度的收取手续费
                if(requireRefundFare < 0) requireRefundFare = 0;
            }
            // 如果有服务费，则加上需要退还的服务费
            requireRefundFare += refundServiceCharge;
            // 退还总金额
            var requireRefundAmount = requireRefundFare + payBill.Flight.AirportFee + payBill.Flight.BAF;
            // 所有退票，都要收回佣金
            requireRefundAmount += refundCommission;
            // 收回加价金额
            requireRefundAmount += refundIncreasing;
            return requireRefundAmount < 0 ? 0 : requireRefundAmount;
        }
        protected override decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight) {
            return refundFlight.Amount;
        }
    }
}