using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 分润方
    /// </summary>
    public class Royalty : TradeRoyalty {
        internal Royalty(Guid id)
            : base(id) {
        }
        internal Royalty(Guid id, string account, decimal rate)
            : base(id, account, rate) {
        }

        public override TradeRoleType RoleType {
            get { return TradeRoleType.Royalty; }
        }
        protected override decimal GetTradeRate(TradeInfo trade) {
            return SystemManagement.SystemParamService.TradeRateForRoyalty;
        }
        protected override decimal ProcessTradeFee(decimal tradeFee) {
            return Utility.Calculator.Ceiling(tradeFee, -1);
        }
        protected override decimal ProcessCommission(decimal commission) {
            return Utility.Calculator.Floor(commission, -1);
        }
        protected override decimal GetErrorRefundAnticipation(ErrorRefundFlight refundFlight) {
            return 0;
        }
    }
}