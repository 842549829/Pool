using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Distribution.Domain.Role;
using ChinaPay.B3B.Service.Distribution.Domain.Tradement;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund {
    public class NormalRefundBill : BillBase {
        LazyLoader<NormalPayBill> _payBillLoader;

        internal NormalRefundBill(decimal orderId, decimal applyformId)
            : base(applyformId) {
            OrderId = orderId;
            _payBillLoader = new LazyLoader<NormalPayBill>(() => DistributionQueryService.QueryNormalPayBill(OrderId));
        }

        /// <summary>
        /// 原支付订单号
        /// </summary>
        public decimal OrderId {
            get;
            private set;
        }
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal ApplyformId {
            get { return Id; }
        }
        /// <summary>
        /// 采购方
        /// </summary>
        public NormalRefundRoleBill Purchaser {
            get;
            internal set;
        }
        /// <summary>
        /// 出票方
        /// </summary>
        public NormalRefundRoleBill Provider {
            get;
            internal set;
        }
        /// <summary>
        /// 资源方
        /// </summary>
        public NormalRefundRoleBill Supplier {
            get;
            internal set;
        }
        private List<NormalRefundRoleBill> _royalties = null;
        /// <summary>
        /// 分润方
        /// </summary>
        public IEnumerable<NormalRefundRoleBill> Royalties {
            get { return _royalties ?? (_royalties = new List<NormalRefundRoleBill>()); }
            internal set {
                if(value == null) throw new InvalidOperationException("分润方不能为空");
                foreach(var royalty in value) {
                    AddRoyalty(royalty);
                }
            }
        }
        /// <summary>
        /// 平台
        /// </summary>
        public PlatformBill<NormalRefundRoleBill, NormalRefundRoleBillSource, NormalRefundDetailBill> Platform {
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
            get {
                return Purchaser != null && Purchaser.Success;
            }
        }
        /// <summary>
        /// 原支付账单
        /// </summary>
        public NormalPayBill PayBill {
            get {
                return _payBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("原支付账单不能为空");
                _payBillLoader.SetData(value);
            }
        }

        internal bool HasRefunded {
            get {
                if(Purchaser.Success) return true;
                if(Provider != null && Provider.Success) return true;
                if(Supplier != null && Supplier.Success) return true;
                if(_royalties != null && _royalties.Any(r => r.Success)) return true;
                return false;
            }
        }
        internal void AddRoyalty(NormalRefundRoleBill royalty) {
            if(royalty == null) throw new ArgumentNullException("royalty");
            if(_royalties == null) {
                _royalties = new List<NormalRefundRoleBill>();
            }
            _royalties.Add(royalty);
        }
        internal void RefundSuccess(IEnumerable<Service.Tradement.RefundResult> refundResults) {
            foreach(var item in refundResults) {
                if(item.Success) {
                    var refundRoleBills = getRefundRoleBill(item);
                    foreach(var refundRoleBill in refundRoleBills) {
                        if(!refundRoleBill.Success) {
                            refundRoleBill.TradeSuccess(item.RefundTime.Value);
                        }
                    }
                }
            }
            if(Platform != null && !Platform.Success) {
                var platformRefundInfo = refundResults.FirstOrDefault(item => item.Account == Platform.Account && (item.Roles == null || item.Roles.Contains(TradeRoleType.Platform)));
                if(platformRefundInfo != null && platformRefundInfo.Success) {
                    Platform.Success = true;
                }
            }
        }
        IEnumerable<NormalRefundRoleBill> getRefundRoleBill(Service.Tradement.RefundResult refundResult) {
            var result = new List<NormalRefundRoleBill>();
            if(isRoleRefund(Purchaser, refundResult)) result.Add(Purchaser);
            if(isRoleRefund(Provider, refundResult)) result.Add(Provider);
            if(isRoleRefund(Supplier, refundResult)) result.Add(Supplier);
            foreach(var royalty in Royalties) {
                if(isRoleRefund(royalty, refundResult)) result.Add(royalty);
            }
            if(Platform != null && Platform.Deduction != null && isRoleRefund(Platform.Deduction, refundResult)) result.Add(Platform.Deduction);
            return result;
        }
        bool isRoleRefund(NormalRefundRoleBill roleBill, Service.Tradement.RefundResult refundResult) {
            if(roleBill == null) return false;
            return (refundResult.Roles == null || refundResult.Roles.Contains(roleBill.Owner.RoleType)) &&
                   string.Compare(roleBill.Owner.Account, refundResult.Account, true) == 0;
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