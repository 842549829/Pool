using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill {
    public abstract class RoleBillSource {
        decimal? _anticipation = null, _tradeFee = null;
        Guid? _billId = null;

        protected Guid BillId {
            get {
                if(!_billId.HasValue)
                    throw new Core.CustomException("未设置角色账单Id");
                return _billId.Value;
            }
        }
        /// <summary>
        /// 明细
        /// </summary>
        public abstract IEnumerable<DetailBill> DetailsBase { get; }
        /// <summary>
        /// 预期金额
        /// </summary>
        public decimal Anticipation {
            get {
                if(!_anticipation.HasValue) {
                    _anticipation = DetailsBase.Sum(detail => detail.Anticipation);
                }
                return _anticipation.Value;
            }
            internal set {
                _anticipation = value;
            }
        }
        /// <summary>
        /// 交易手续费
        /// </summary>
        public decimal TradeFee {
            get {
                if(!_tradeFee.HasValue) {
                    _tradeFee = DetailsBase.Sum(detail => detail.TradeFee);
                }
                return _tradeFee.Value;
            }
            internal set {
                _tradeFee = value;
            }
        }

        internal void SetBillId(Guid billId) {
            _billId = billId;
        }
        protected void RefreshTotalAmount() {
            _anticipation = null;
            _tradeFee = null;
        }
    }
    public abstract class RoleBillSource<TDetailBill> : RoleBillSource
        where TDetailBill : DetailBill {

        EnumerableLazyLoader<TDetailBill> _detailBillLoader;

        internal RoleBillSource() {
            _detailBillLoader = new EnumerableLazyLoader<TDetailBill>(GetDetailBills);
        }

        public sealed override IEnumerable<DetailBill> DetailsBase {
            get { return Details; }
        }

        /// <summary>
        /// 明细
        /// </summary>
        public IEnumerable<TDetailBill> Details {
            get {
                return _detailBillLoader.QueryDatas();
            }
            internal set {
                if(value == null) throw new InvalidOperationException("明细信息不能为空");
                _detailBillLoader.SetDatas(value);
            }
        }

        protected abstract IEnumerable<TDetailBill> GetDetailBills();
    }
}