using System;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay;
using ChinaPay.B3B.Service.Distribution.Domain.Tradement;
using ChinaPay.Data;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    public class PostponeRefundBill : BillBase {
        LazyLoader<PostponePayBill> _payBillLoader;

        internal PostponeRefundBill(decimal applyformId)
            : base(applyformId) {
            _payBillLoader = new LazyLoader<PostponePayBill>(() => DistributionQueryService.QueryPostponePayBill(ApplyformId));
        }

        /// <summary>
        /// 改期申请单号
        /// </summary>
        public decimal ApplyformId {
            get { return Id; }
        }
        /// <summary>
        /// 申请方
        /// </summary>
        public Refund.Postpone.PostponeRefundRoleBill Applier {
            get;
            internal set;
        }
        /// <summary>
        /// 受理方
        /// </summary>
        public PlatformBill<Refund.Postpone.PostponeRefundRoleBill, Refund.Postpone.PostponeRefundRoleBillSource, Refund.Postpone.PostponeRefundDetailBill> Accepter {
            get;
            internal set;
        }
        /// <summary>
        /// 退款信息
        /// </summary>
        public Refundment Tradement {
            get;
            internal set;
        }
        /// <summary>
        /// 交易状态
        /// </summary>
        public bool Succeed {
            get { return Applier != null && Applier.Success; }
        }
        /// <summary>
        /// 原支付账单
        /// </summary>
        public PostponePayBill PayBill {
            get {
                return _payBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("缺少原支付账单信息");
                _payBillLoader.SetData(value);
            }
        }

        internal void RefundSuccess(DateTime refundTime) {
            Applier.TradeSuccess(refundTime);
            Accepter.TradeSuccess(refundTime);
        }

        public override IEnumerable<RoleBill> RoleBills {
            get {
                var roleBills = new List<RoleBill>();
                if(Applier != null) roleBills.Add(Applier);
                if(Accepter != null && Accepter.Deduction != null) roleBills.Add(Accepter.Deduction);
                return roleBills;
            }
        }
        public override Tradement.Tradement TradementBase {
            get { return Tradement; }
        }
        public override PlatformBasicProfit PlatformBasicProfit {
            get {
                if(Accepter != null) {
                    return new PlatformBasicProfit {
                        TradeFee = Accepter.TradeFee,
                        Premium = Accepter.Premium,
                        Account = Accepter.Account,
                        Success = Accepter.Success
                    };
                }
                return null;
            }
        }
        public override DateTime? TradeTime {
            get { return Applier.Time; }
        }
    }
}