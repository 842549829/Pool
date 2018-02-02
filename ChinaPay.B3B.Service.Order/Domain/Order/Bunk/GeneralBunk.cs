namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 普通舱位
    /// 即明折明扣舱位
    /// </summary>
    public abstract class GeneralBunk : BaseBunk {
        private decimal _discount;
        private decimal? _fare;

        internal GeneralBunk(string code, decimal discount, string ei)
            : base(code, ei) {
            _discount = discount;
        }
        internal GeneralBunk(string code, decimal discount, decimal fare, string ei)
            : base(code, ei) {
            _discount = discount;
            _fare = fare;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public override decimal Discount {
            get {
                return _discount;
            }
        }
        /// <summary>
        /// 票面价
        /// </summary>
        public override decimal Fare {
            get {
                if(!_fare.HasValue) {
                    _fare = GetFareByDiscount(this.Discount);
                }
                return _fare.Value;
            }
        }
        /// <summary>
        /// 调整价格
        /// </summary>
        internal override void ReviseFare(decimal fare) {
            if(fare < 0) throw new Core.CustomException("票面价不能小于0");
            _fare = fare;
            _discount = GetDiscountByFare(_fare.Value);
        }
    }
}