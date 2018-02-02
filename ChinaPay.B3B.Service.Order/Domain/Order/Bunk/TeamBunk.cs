using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    public class TeamBunk : BaseBunk {
        decimal? _fare;
        decimal? _discount;
        string _description;

        internal TeamBunk(string code, decimal discountOrParValue, PriceType mode, string ei, string description)
            : base(code, ei) {
            if(mode == PriceType.Price) {
                _fare = Utility.Calculator.Round(discountOrParValue, 1);
            } else {
                _discount = discountOrParValue;
            }
            _description = description ?? string.Empty;
        }
        internal TeamBunk(string code, decimal discount, decimal fare, string ei, string description)
            : base(code, ei) {
            _discount = discount;
            _fare = fare;
            _description = description ?? string.Empty;
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
                if(!_fare.HasValue) {
                    _fare = GetFareByDiscount(this.Discount);
                }
                return _fare.Value;
            }
        }
        public string Description {
            get {
                return _description;
            }
        }

        public override Common.Enums.BunkType Type {
            get { return Common.Enums.BunkType.Team; }
        }

        internal override void ReviseFare(decimal fare) {
            if(fare < 0) throw new Core.CustomException("票面价不能小于0");
            _fare = fare;
            _discount = null;
        }
    }
}