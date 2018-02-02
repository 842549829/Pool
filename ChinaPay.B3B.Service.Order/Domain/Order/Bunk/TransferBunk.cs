using System;

namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    public class TransferBunk : BaseBunk {
        decimal _fare;
        decimal? _discount;

        internal TransferBunk(string code, decimal fare, string ei)
            : base(code, ei) {
            _fare = fare;
        }
        internal TransferBunk(string code, decimal discount, decimal fare, string ei)
            : base(code, ei) {
            _discount = discount;
            _fare = fare;
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

        public override Common.Enums.BunkType Type {
            get { return Common.Enums.BunkType.Transfer; }
        }

        internal override void ReviseFare(decimal fare) {
            if(fare < 0) throw new Core.CustomException("票面价不能小于0");
            _fare = fare;
            _discount = null;
        }
    }
}