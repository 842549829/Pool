using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.Distribution.Domain.Bill;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Postpone;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Postpone;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 平台
    /// </summary>
    public class Platform : BaseRole {
        PlatformRoyalty _deduction;

        internal Platform()
            : base(Organization.Domain.Platform.Instance.Id) {
        }
        internal Platform(string account)
            : base(Organization.Domain.Platform.Instance.Id, account) {
        }
        public PlatformRoyalty Deduction {
            get {
                if(_deduction == null) {
                    _deduction = new PlatformRoyalty(this.Id);
                }
                return _deduction;
            }
        }

        protected override string GetAccount() {
            return SystemManagement.SystemParamService.PlatformIncomeAccount;
        }
        protected string GetAccount(decimal amount) {
            return amount < 0 ? SystemManagement.SystemParamService.PlatformPayoutAccount : SystemManagement.SystemParamService.PlatformIncomeAccount;
        }

        /// <summary>
        /// 生成支付账单
        /// </summary>
        internal PlatformBill<NormalPayRoleBill, NormalPayRoleBillSource, NormalPayDetailBill> MakePayBill(TradeInfo trade, IEnumerable<NormalPayRoleBill> tradeRolePayBills, Deduction deduction) {
            var payBill = new PlatformBill<NormalPayRoleBill, NormalPayRoleBillSource, NormalPayDetailBill>();
            if(deduction != null && (deduction.Rebate != 0 || deduction.Increasing != 0)) {
                payBill.Deduction = Deduction.MakePayBill(trade, deduction);
                if(payBill.Deduction.Owner != null) {
                    payBill.Deduction.Owner.Account = GetAccount(payBill.Deduction.Amount);
                }
            }
            payBill.Premium = getPremiumProfit(tradeRolePayBills, payBill.Deduction);
            payBill.TradeFee = makeTradeFeeProfit(tradeRolePayBills);
            return payBill;
        }
        /// <summary>
        /// 生成支付账单
        /// </summary>
        internal PlatformBill<PostponePayRoleBill, PostponePayRoleBillSource, PostponePayDetailBill> MakePayBill(IEnumerable<Order.Domain.Applyform.PostponeFlight> flights, IEnumerable<Order.Domain.Passenger> passengers, IEnumerable<PostponePayRoleBill> tradeRolePayBills) {
            var result = new PlatformBill<PostponePayRoleBill, PostponePayRoleBillSource, PostponePayDetailBill> {
                Deduction = this.Deduction.MakePayBill(flights, passengers),
            };
            result.Deduction.Owner.Account = SystemManagement.SystemParamService.PlatformIncodeAccountForPostpone;
            result.Premium = getPremiumProfit(tradeRolePayBills, result.Deduction);
            result.TradeFee = makeTradeFeeProfit(tradeRolePayBills);
            result.Account = Deduction.Account;
            return result;
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        internal PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> MakeRefundBill(NormalPayRoleBill payBill, RefundInfo refundInfo, IEnumerable<NormalRefundRoleBill> refundedBills, IEnumerable<NormalRefundRoleBill> tradeRoleRefundBills) {
            var refundBill = new PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> {
                Deduction = makeDeductionProfit(payBill, refundInfo, refundedBills)
            };
            refundBill.Premium = getPremiumProfit(tradeRoleRefundBills, refundBill.Deduction);
            refundBill.TradeFee = makeTradeFeeProfit(tradeRoleRefundBills);
            refundBill.Account = Account;
            return refundBill;
        }
        /// <summary>
        /// 生成差错退款账单
        /// </summary>
        internal PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> MakeErrorRefundBill(NormalPayRoleBill payBill, ErrorRefundInfo refundInfo, IEnumerable<NormalRefundRoleBill> refundedBills, IEnumerable<NormalRefundRoleBill> tradeRoleRefundBills) {
            var refundBill = new PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill>();
            refundBill.Premium = getPremiumProfit(tradeRoleRefundBills, refundBill.Deduction);
            refundBill.TradeFee = makeTradeFeeProfit(tradeRoleRefundBills);
            refundBill.Account = Account;
            return refundBill;
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        internal PlatformBill<PostponeRefundRoleBill, PostponeRefundRoleBillSource, PostponeRefundDetailBill> MakeRefundBill(PostponePayRoleBill payBill, IEnumerable<PostponeRefundRoleBill> tradeRoleRefundBills) {
            var refundBill = new PlatformBill<PostponeRefundRoleBill, PostponeRefundRoleBillSource, PostponeRefundDetailBill> {
                Deduction = makeDeductionProfit(payBill)
            };
            refundBill.Premium = getPremiumProfit(tradeRoleRefundBills, refundBill.Deduction);
            refundBill.TradeFee = makeTradeFeeProfit(tradeRoleRefundBills);
            refundBill.Account = Account;
            return refundBill;
        }

        private NormalRefundRoleBill makeDeductionProfit(NormalPayRoleBill payBill, RefundInfo refundInfo, IEnumerable<NormalRefundRoleBill> refundedBills) {
            return payBill == null ? null : payBill.MakeRefundBill(refundInfo, refundedBills);
        }
        private PostponeRefundRoleBill makeDeductionProfit(PostponePayRoleBill payBill) {
            return payBill == null ? null : payBill.MakeRefundBill();
        }
        private decimal makeTradeFeeProfit(IEnumerable<NormalPayRoleBill> tradeRoleBills) {
            return tradeRoleBills == null ? 0 : tradeRoleBills.Sum(item => item.Source.TradeFee) * -1;
        }
        private decimal makeTradeFeeProfit(IEnumerable<PostponePayRoleBill> tradeRoleBills) {
            return tradeRoleBills == null ? 0 : tradeRoleBills.Sum(item => item.Source.TradeFee) * -1;
        }
        private decimal makeTradeFeeProfit(IEnumerable<NormalRefundRoleBill> tradeRoleBills) {
            return tradeRoleBills == null ? 0 : tradeRoleBills.Sum(item => item.Source.TradeFee) * -1;
        }
        private decimal makeTradeFeeProfit(IEnumerable<PostponeRefundRoleBill> tradeRoleBills) {
            return tradeRoleBills == null ? 0 : tradeRoleBills.Sum(item => item.Source.TradeFee) * -1;
        }
        private decimal getPremiumProfit(IEnumerable<NormalPayRoleBill> tradeRoleBills, NormalPayRoleBill deductionBill) {
            decimal tradeRoleBalance = 0;
            if(tradeRoleBills != null) {
                var userRoleBalance = tradeRoleBills.Sum(item => item.Source.Anticipation);
                var platformDeduction = deductionBill == null ? 0 : deductionBill.Source.Anticipation;
                tradeRoleBalance = userRoleBalance + platformDeduction;
            }
            return tradeRoleBalance * -1;
        }
        private decimal getPremiumProfit(IEnumerable<PostponePayRoleBill> tradeRoleBills, PostponePayRoleBill deductionBill) {
            decimal tradeRoleBalance = 0;
            if(tradeRoleBills != null) {
                var userRoleBalance = tradeRoleBills.Sum(item => item.Source.Anticipation);
                var platformDeduction = deductionBill == null ? 0 : deductionBill.Source.Anticipation;
                tradeRoleBalance = userRoleBalance + platformDeduction;
            }
            return tradeRoleBalance * -1;
        }
        private decimal getPremiumProfit(IEnumerable<NormalRefundRoleBill> tradeRoleBills, NormalRefundRoleBill deductionBill) {
            decimal tradeRoleBalance = 0;
            if(tradeRoleBills != null) {
                var userRoleBalance = tradeRoleBills.Sum(item => item.Source.Anticipation);
                var platformDeduction = deductionBill == null ? 0 : deductionBill.Source.Anticipation;
                tradeRoleBalance = userRoleBalance + platformDeduction;
            }
            return tradeRoleBalance * -1;
        }
        private decimal getPremiumProfit(IEnumerable<PostponeRefundRoleBill> tradeRoleBills, PostponeRefundRoleBill deductionBill) {
            decimal tradeRoleBalance = 0;
            if(tradeRoleBills != null) {
                var userRoleBalance = tradeRoleBills.Sum(item => item.Source.Anticipation);
                var platformDeduction = deductionBill == null ? 0 : deductionBill.Source.Anticipation;
                tradeRoleBalance = userRoleBalance + platformDeduction;
            }
            return tradeRoleBalance * -1;
        }
    }
}