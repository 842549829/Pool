using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Tradement;
using ChinaPay.Data;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay {
    /// <summary>
    /// 改期支付账单信息
    /// </summary>
    public class PostponePayBill : BillBase {
        Payment _tradement = null;
        LazyLoader<Refund.PostponeRefundBill> _refundBillLoader;

        internal PostponePayBill(decimal orderId, decimal applyformId)
            : base(applyformId) {
            OrderId = orderId;
            _refundBillLoader = new LazyLoader<PostponeRefundBill>(() => DistributionQueryService.QueryPostponeRefundBill(ApplyformId));
        }

        /// <summary>
        /// 原订单号
        /// </summary>
        public decimal OrderId {
            get;
            private set;
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
        public Pay.Postpone.PostponePayRoleBill Applier {
            get;
            internal set;
        }
        /// <summary>
        /// 受理方
        /// </summary>
        public PlatformBill<Pay.Postpone.PostponePayRoleBill, Pay.Postpone.PostponePayRoleBillSource, Pay.Postpone.PostponePayDetailBill> Accepter {
            get;
            internal set;
        }
        /// <summary>
        /// 支付信息
        /// </summary>
        public Tradement.Payment Tradement {
            get {
                if(_tradement == null && Applier != null) {
                    _tradement = new Payment {
                        Amount = Math.Abs(Applier.Amount),
                        PayAccount = Applier.Owner.Account,
                        PayeeAccount = Accepter == null ? string.Empty : Accepter.Deduction.Owner.Account
                    };
                }
                return _tradement;
            }
            internal set {
                if(value == null) throw new InvalidOperationException("支付信息不能为空");
                _tradement = value;
            }
        }
        /// <summary>
        /// 交易状态
        /// </summary>
        public bool Succeed {
            get { return Applier != null && Applier.Success; }
        }
        /// <summary>
        /// 退款账单信息
        /// </summary>
        public PostponeRefundBill RefundBill {
            get {
                return _refundBillLoader.QueryData();
            }
        }

        internal void PaySuccess(string payAccount, string payTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime, string channelTradeNo) {
            Tradement.PaySuccess(payAccount, payTradeNo, payInterface, payAccountType, channelTradeNo);
            Applier.TradeSuccess(payTime);
            Applier.Owner.Account = payAccount;
            Accepter.TradeSuccess(payTime);
        }

        internal PostponeRefundBill MakeRefundBill(string remark) {
            if(!Succeed) throw new InvalidOperationException("改期费未成功支付，不能退款");
            if(RefundBill != null) throw new Core.CustomException("改期费是一次性退完，不能重复退");

            var refundBill = new PostponeRefundBill(ApplyformId) {
                PayBill = this,
                Remark = remark,
                Applier = Applier.MakeRefundBill()
            };
            var accepter = new Role.Platform(Accepter.Account);
            refundBill.Accepter = accepter.MakeRefundBill(Accepter.Deduction, new[] { refundBill.Applier });
            refundBill.Tradement = Tradement.MakeRefundment(Math.Abs(Applier.Amount), 0, ApplyformId.ToString());
            return refundBill;
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