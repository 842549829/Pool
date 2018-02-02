namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 往返产品舱
    /// </summary>
    public class ProductionBunk : BaseBunk {
        decimal _fare;
        decimal? _discount = null;

        internal ProductionBunk(string code, decimal fare, string ei)
            : base(code, ei) {
            this._fare = fare;
        }
        internal ProductionBunk(string code, decimal discount, decimal fare, string ei)
            : base(code, ei) {
            this._discount = discount;
            this._fare = fare;
        }

        public override decimal Discount {
            get {
                if(!_discount.HasValue) {
                    _discount = GetDiscountByFare(this.Fare);
                }
                return _discount.Value;
            }
        }
        public override decimal Fare {
            get { return _fare; }
        }
        public override Common.Enums.BunkType Type {
            get { return Common.Enums.BunkType.Production; }
        }
        internal override void ReviseFare(decimal fare) {
            if(fare < 0) throw new Core.CustomException("票面价不能小于0");
            _fare = fare;
            _discount = null;
        }
    }
}