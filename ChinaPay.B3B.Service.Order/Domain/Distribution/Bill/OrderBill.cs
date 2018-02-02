using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain {
    /// <summary>
    /// 订单账单
    /// </summary>
    public class OrderBill {
        LazyLoader<NormalPayBill> _mainPayBillLoader;
        EnumerableLazyLoader<PostponePayBill> _postponePayBillsLoader;
        List<PostponeRefundBill> _postponeRefundBills = null;

        internal OrderBill(decimal orderId) {
            this.OrderId = orderId;
            _mainPayBillLoader = new LazyLoader<NormalPayBill>(() => DistributionQueryService.QueryNormalPayBill(this.OrderId));
            _postponePayBillsLoader = new EnumerableLazyLoader<PostponePayBill>(() => DistributionQueryService.QueryPostponePayBills(this.OrderId));
        }

        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId {
            get;
            private set;
        }
        /// <summary>
        /// 订单初始支付账单
        /// </summary>
        public NormalPayBill PayBill {
            get {
                return _mainPayBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("初始支付账单信息不能为空");
                _mainPayBillLoader.SetData(value);
            }
        }
        /// <summary>
        /// 正常退款账单信息
        /// </summary>
        public IEnumerable<NormalRefundBill> NormalRefundBills {
            get {
                return this.PayBill.RefundBills;
            }
        }
        /// <summary>
        /// 改期支付账单信息
        /// </summary>
        public IEnumerable<PostponePayBill> PostponePayBills {
            get {
                return _postponePayBillsLoader.QueryDatas();
            }
        }
        /// <summary>
        /// 改期退款账单信息
        /// </summary>
        public IEnumerable<PostponeRefundBill> PostponeRefundBills {
            get {
                if(_postponeRefundBills == null) {
                    _postponeRefundBills = new List<PostponeRefundBill>();
                    foreach(var item in this.PostponePayBills) {
                        if(item.RefundBill != null) {
                            _postponeRefundBills.Add(item.RefundBill);
                        }
                    }
                }
                return _postponeRefundBills;
            }
        }
    }
}