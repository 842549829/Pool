using System;
using ChinaPay.B3B.Service.Distribution.Domain.Role;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill {
    public abstract class RoleBill {
        decimal? _amount = null;

        protected RoleBill(TradeRole owner)
            : this(Guid.NewGuid(), owner) {
        }
        protected RoleBill(Guid id, TradeRole owner) {
            Id = id;
            Owner = owner;
        }

        internal Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 所属者
        /// </summary>
        public TradeRole Owner {
            get;
            private set;
        }
        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal Amount {
            get {
                if(!_amount.HasValue) {
                    _amount = GetAmount();
                }
                return _amount.Value;
            }
            internal set {
                _amount = value;
            }
        }
        /// <summary>
        /// 交易状态
        /// </summary>
        public bool Success {
            get;
            internal set;
        }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime? Time {
            get;
            internal set;
        }
        /// <summary>
        /// 明细
        /// </summary>
        public abstract RoleBillSource SourceBase { get; }

        internal void TradeSuccess(DateTime tradeTime) {
            if(!string.IsNullOrWhiteSpace(Owner.Account)) {
                Success = true;
                Time = tradeTime;
            }
        }
        protected void RefreshAmount() {
            _amount = null;
        }

        protected abstract decimal GetAmount();
    }
    /// <summary>
    /// 角色账单
    /// </summary>
    public abstract class RoleBill<TRoleBillSource, TDetailBill> : RoleBill
        where TRoleBillSource : RoleBillSource<TDetailBill>
        where TDetailBill : DetailBill {

        private TRoleBillSource _source = null;

        protected RoleBill(TradeRole owner)
            : base(owner) {
        }
        protected RoleBill(Guid id, TradeRole owner)
            : base(id, owner) {
        }

        /// <summary>
        /// 明细
        /// </summary>
        public override RoleBillSource SourceBase {
            get { return Source; }
        }

        /// <summary>
        /// 明细
        /// </summary>
        public TRoleBillSource Source {
            get { return _source; }
            internal set {
                if(value == null) throw new InvalidOperationException("账单明细不能为空");
                _source = value;
                _source.SetBillId(Id);
            }
        }

        protected override decimal GetAmount() {
            if(Source == null) throw new InvalidOperationException("缺少账单明细信息");
            return Source.Anticipation + Source.TradeFee;
        }
    }
}