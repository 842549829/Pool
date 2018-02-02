using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Tradement;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay {
    /// <summary>
    /// 预订/导入支付信息
    /// </summary>
    public class NormalPayBill : BillBase {
        Payment _tradement = null;
        EnumerableLazyLoader<NormalRefundBill> _refundBillsLoader;

        internal NormalPayBill(decimal orderId)
            : base(orderId) {
            _refundBillsLoader = new EnumerableLazyLoader<NormalRefundBill>(() => DistributionQueryService.QueryNormalRefundBills(OrderId));
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId {
            get { return Id; }
        }
        /// <summary>
        /// 采购方
        /// </summary>
        public NormalPayRoleBill Purchaser {
            get;
            internal set;
        }
        /// <summary>
        /// 出票方
        /// </summary>
        public NormalPayRoleBill Provider {
            get;
            internal set;
        }
        /// <summary>
        /// 资源方
        /// </summary>
        public NormalPayRoleBill Supplier {
            get;
            internal set;
        }
        private List<NormalPayRoleBill> _royalties = null;
        /// <summary>
        /// 分润方
        /// </summary>
        public IEnumerable<NormalPayRoleBill> Royalties {
            get { return _royalties ?? (_royalties = new List<NormalPayRoleBill>()); }
            internal set {
                if(value == null) throw new InvalidOperationException("分润方不能为空");
                foreach(var royalty in value) {
                    AddRoyalty(royalty);
                }
            }
        }
        /// <summary>
        /// 支付信息
        /// </summary>
        public Payment Tradement {
            get {
                if(_tradement == null && Purchaser != null) {
                    _tradement = new Payment {
                        Amount = Math.Abs(Purchaser.Amount),
                        PayAccount = Purchaser.Owner.Account,
                        PayeeAccount = SystemManagement.SystemParamService.PlatformSettleAccount
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
        /// 平台
        /// </summary>
        public PlatformBill<NormalPayRoleBill, NormalPayRoleBillSource, NormalPayDetailBill> Platform {
            get;
            internal set;
        }
        /// <summary>
        /// 支付交易状态
        /// </summary>
        public bool PaySucceed {
            get {
                return Purchaser != null && Purchaser.Success;
            }
        }
        /// <summary>
        /// 分润状态
        /// </summary>
        public bool RoyaltySucceed {
            get {
                return Provider == null || Provider.Success;
            }
        }
        /// <summary>
        /// 是否需要子支付
        /// </summary>
        internal bool RequireSubPay {
            get {
                return Platform != null && Platform.TotalAmount < 0;
            }
        }
        /// <summary>
        /// 子支付是否成功
        /// </summary>
        internal bool SubPaySucceed {
            get { return !RequireSubPay || Platform.Success; }
        }
        /// <summary>
        /// 退款账单信息
        /// </summary>
        public IEnumerable<NormalRefundBill> RefundBills {
            get {
                return _refundBillsLoader.QueryDatas();
            }
        }

        internal void AddRoyalty(NormalPayRoleBill royalty) {
            if(royalty == null) throw new ArgumentNullException("royalty");
            if(_royalties == null) {
                _royalties = new List<NormalPayRoleBill>();
            }
            _royalties.Add(royalty);
        }
        internal void RemoveRoyalties() {
            _royalties = new List<NormalPayRoleBill>();
        }
        internal void PaySuccess(string payAccount, string payTradeNo, PayInterface payInterface, PayAccountType payAccountType, DateTime payTime, string channelTradeNo) {
            Tradement.PaySuccess(payAccount, payTradeNo, payInterface, payAccountType, channelTradeNo);
            Purchaser.TradeSuccess(payTime);
            Purchaser.Owner.Account = payAccount;
        }
        internal void SubPaySuccess(DateTime payTime) {
            if(!RequireSubPay) throw new CustomException("该单子不需要进行子支付");
            Platform.TradeSuccess(payTime);
        }
        internal void RoyaltySuccess(DateTime royaltyTime) {
            if(Provider != null) Provider.TradeSuccess(royaltyTime);
            if(Supplier != null) Supplier.TradeSuccess(royaltyTime);
            if(_royalties != null) {
                foreach(var royalty in _royalties) {
                    royalty.TradeSuccess(royaltyTime);
                }
            }
            if(!RequireSubPay && Platform != null) Platform.TradeSuccess(royaltyTime);
        }
        internal void RefreshPrice(TradeInfo trade) {
            checkRefresh(trade);
            Purchaser.RevisePrice(trade);
            if(Provider != null) Provider.RevisePrice(trade);
            if(Supplier != null) Supplier.RevisePrice(trade);
            if(_royalties != null) {
                foreach(var royalty in _royalties) {
                    royalty.RevisePrice(trade);
                }
            }
            if(Platform.Deduction != null) Platform.Deduction.RevisePrice(trade);
        }
        internal void RefreshReleaseFare(TradeInfo trade) {
            checkRefresh(trade);
            Purchaser.RefreshReleaseFare(trade);
            if(Provider != null) Provider.RefreshReleaseFare(trade);
            if(Supplier != null) Supplier.RefreshReleaseFare(trade);
            if(_royalties != null) {
                foreach(var royalty in _royalties) {
                    royalty.RefreshReleaseFare(trade);
                }
            }
            if(Platform != null && Platform.Deduction != null) Platform.Deduction.RefreshReleaseFare(trade);
            _tradement = null;
        }
        internal void RefreshFare(TradeInfo trade) {
            checkRefresh(trade);
            Purchaser.RefreshFare(trade);
            if(Provider != null) Provider.RefreshFare(trade);
            if(Supplier != null) Supplier.RefreshFare(trade);
            if(_royalties != null) {
                foreach(var royalty in _royalties) {
                    royalty.RefreshFare(trade);
                }
            }
            if(Platform != null && Platform.Deduction != null) Platform.Deduction.RefreshFare(trade);
        }
        /// <summary>
        /// 生成退款账单
        /// </summary>
        internal NormalRefundBill MakeRefundBill(decimal applyformId, Refund.RefundInfo refundInfo, string remark) {
            //if(!this.Success)
            //    throw new InvalidOperationException("未支付成功的订单，不能退款");
            var refundBill = new NormalRefundBill(OrderId, applyformId) {
                PayBill = this,
                Remark = remark
            };
            var tradeRoleRefundBills = new List<Refund.Normal.NormalRefundRoleBill>();
            refundBill.Purchaser = Purchaser.MakeRefundBill(refundInfo, getPurchaserRefundedBill(applyformId));
            tradeRoleRefundBills.Add(refundBill.Purchaser);
            if(Provider != null) {
                refundBill.Provider = Provider.MakeRefundBill(refundInfo, getProviderRefundedBill(applyformId));
                tradeRoleRefundBills.Add(refundBill.Provider);
            }
            if(Supplier != null) {
                refundBill.Supplier = Supplier.MakeRefundBill(refundInfo, getSupplierRefundedBill(applyformId));
                tradeRoleRefundBills.Add(refundBill.Supplier);
            }
            if(_royalties != null) {
                foreach(var royalty in _royalties) {
                    var royaltyRefundBill = royalty.MakeRefundBill(refundInfo, getRoyaltyRefundedBill(applyformId, royalty.Owner.Id));
                    refundBill.AddRoyalty(royaltyRefundBill);
                    tradeRoleRefundBills.Add(royaltyRefundBill);
                }
            }
            var platform = new Role.Platform(Platform.Account);
            refundBill.Platform = platform.MakeRefundBill(Platform.Deduction, refundInfo, getPlatformRefundedBill(applyformId), tradeRoleRefundBills);
            refundBill.Tradement = Tradement.MakeRefundment(Math.Abs(refundBill.Purchaser.Amount), getRefundedTradeFee(), applyformId.ToString());
            _refundBillsLoader.AppendData(refundBill);
            return refundBill;
        }
        /// <summary>
        /// 生成差错退款账单
        /// </summary>
        internal NormalRefundBill MakeErrorRefundBill(Refund.ErrorRefundInfo refundInfo, string remark) {
            var refundBill = new NormalRefundBill(OrderId, refundInfo.ApplyformId) {
                PayBill = this,
                Remark = remark
            };
            var tradeRoleRefundBills = new List<Refund.Normal.NormalRefundRoleBill>();
            refundBill.Purchaser = Purchaser.MakeErrorRefundBill(refundInfo, getPurchaserRefundedBill(refundInfo.ApplyformId));
            tradeRoleRefundBills.Add(refundBill.Purchaser);
            refundBill.Provider = Provider.MakeErrorRefundBill(refundInfo, getProviderRefundedBill(refundInfo.ApplyformId));
            tradeRoleRefundBills.Add(refundBill.Provider);
            var platform = new Role.Platform(Platform.Account);
            refundBill.Platform = platform.MakeErrorRefundBill(Platform.Deduction, refundInfo, getPlatformRefundedBill(refundInfo.ApplyformId), tradeRoleRefundBills);
            refundBill.Tradement = Tradement.MakeRefundment(Math.Abs(refundBill.Purchaser.Amount), getRefundedTradeFee(), refundInfo.ApplyformId.ToString());
            _refundBillsLoader.AppendData(refundBill);
            return refundBill;
        }

        private void checkRefresh(TradeInfo trade) {
            if(trade == null) throw new ArgumentNullException("trade");
            if(trade.Id != OrderId) throw new CustomException("订单不一致");
            if(RefundBills.Any()) throw new CustomException("已有退款账单，不能修改支付账单");
        }
        private IEnumerable<Refund.Normal.NormalRefundRoleBill> getPurchaserRefundedBill(decimal applyformId) {
            return RefundBills.Where(item => item.ApplyformId != applyformId && item.Purchaser != null).Select(item => item.Purchaser);
        }
        private IEnumerable<Refund.Normal.NormalRefundRoleBill> getProviderRefundedBill(decimal applyformId) {
            return RefundBills.Where(item => item.ApplyformId != applyformId && item.Provider != null).Select(item => item.Provider);
        }
        private IEnumerable<Refund.Normal.NormalRefundRoleBill> getSupplierRefundedBill(decimal applyformId) {
            return RefundBills.Where(item => item.ApplyformId != applyformId && item.Supplier != null).Select(item => item.Supplier);
        }
        private IEnumerable<Refund.Normal.NormalRefundRoleBill> getRoyaltyRefundedBill(decimal applyformId, Guid owner) {
            return from bill in RefundBills
                   where bill.ApplyformId != applyformId
                   from royalty in bill.Royalties
                   where royalty.Owner.Id == owner
                   select royalty;
        }
        private IEnumerable<Refund.Normal.NormalRefundRoleBill> getPlatformRefundedBill(decimal applyformId) {
            return RefundBills.Where(item => item.ApplyformId != applyformId && item.Platform != null && item.Platform.Deduction != null).Select(item => item.Platform.Deduction);
        }
        private decimal getRefundedTradeFee() {
            return RefundBills.Sum(b => b.Tradement.TradeFee);
        }

        public override IEnumerable<RoleBill> RoleBills {
            get {
                var roleBills = new List<RoleBill>();
                if(Purchaser != null) roleBills.Add(Purchaser);
                if(Supplier != null) roleBills.Add(Supplier);
                if(Provider != null) roleBills.Add(Provider);
                if(_royalties != null) roleBills.AddRange(_royalties);
                if(Platform != null && Platform.Deduction != null) roleBills.Add(Platform.Deduction);
                return roleBills;
            }
        }
        public override Tradement.Tradement TradementBase {
            get { return Tradement; }
        }
        public override PlatformBasicProfit PlatformBasicProfit {
            get {
                if(Platform != null) {
                    return new PlatformBasicProfit {
                        TradeFee = Platform.TradeFee,
                        Premium = Platform.Premium,
                        Account = Platform.Account,
                        Success = Platform.Success
                    };
                }
                return null;
            }
        }
        public override DateTime? TradeTime {
            get { return Purchaser.Time; }
        }
    }
}