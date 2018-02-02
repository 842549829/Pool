namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 特殊舱位
    /// </summary>
    public class SpecialBunk : BaseBunk {
        decimal? _discount = null;
        decimal _fare;

        internal SpecialBunk(decimal releasedFare)
            : base(string.Empty, string.Empty) {
            _fare = 0;
            ReleasedFare = releasedFare;
        }
        internal SpecialBunk(string code, decimal discount, decimal fare, decimal releasedFare, string ei)
            : base(code, ei) {
            _discount = discount;
            _fare = fare;
            ReleasedFare = releasedFare;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public override decimal Discount {
            get {
                if(!_discount.HasValue) {
                    _discount = GetDiscountByFare(this.Fare);
                }
                return _discount.Value;
            }
        }
        /// <summary>
        /// 票面价
        /// </summary>
        public override decimal Fare {
            get {
                return _fare;
            }
        }
        /// <summary>
        /// 发布的票面价
        /// </summary>
        public decimal ReleasedFare {
            get;
            private set;
        }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceCharge {
            get { return (ReleasedFare - Fare) > 0 ? ReleasedFare - Fare : 0; }
        }

        public override Common.Enums.BunkType Type {
            get { throw new System.InvalidOperationException(); }
        }

        internal override void ReviseFare(decimal parValue) {
            if(parValue != _fare) {
                if(parValue < 0) throw new Core.CustomException("票面价不能小于0");
                _fare = parValue;
                _discount = null;
            }
        }
        internal decimal ReviseReleasedFare(decimal releasedFare) {
            if(releasedFare <= 0) throw new Core.Exception.InvalidValueException("价格必须大于0");
            var originalReleasedFare = ReleasedFare;
            ReleasedFare = releasedFare;
            return originalReleasedFare;
        }
    }
}