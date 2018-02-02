using System.Linq;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal {
    public class NormalRefundRoleBillSource : RefundRoleBillSource<NormalRefundDetailBill>{
        decimal? _releasedFare = null, _fare = null, _baf = null, _airportFee = null, _refundFee = null, _commission = null, _increasing = null, _serviceCharge = null;

        /// <summary>
        /// 发布的票面总价
        /// </summary>
        public decimal ReleasedFare {
            get {
                if(!_releasedFare.HasValue) {
                    _releasedFare = Details.Sum(item => item.Flight.ReleasedFare);
                }
                return _releasedFare.Value;
            }
            internal set {
                _releasedFare = value;
            }
        }
        /// <summary>
        /// 票面总价
        /// </summary>
        public decimal Fare {
            get {
                if(!_fare.HasValue) {
                    _fare = Details.Sum(item => item.Flight.Fare);
                }
                return _fare.Value;
            }
            internal set {
                _fare = value;
            }
        }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public decimal BAF {
            get {
                if(!_baf.HasValue) {
                    _baf = Details.Sum(item => item.Flight.BAF);
                }
                return _baf.Value;
            }
            internal set {
                _baf = value;
            }
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get {
                if(!_airportFee.HasValue) {
                    _airportFee = Details.Sum(item => item.Flight.AirportFee);
                }
                return _airportFee.Value;
            }
            internal set {
                _airportFee = value;
            }
        }
        /// <summary>
        /// 退/废票手续费
        /// </summary>
        public decimal RefundFee {
            get {
                if(!_refundFee.HasValue) {
                    _refundFee = Details.Sum(item => item.RefundFee);
                }
                return _refundFee.Value;
            }
            internal set {
                _refundFee = value;
            }
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission {
            get {
                if(!_commission.HasValue) {
                    _commission = Details.Sum(item => item.Commission);
                }
                return _commission.Value;
            }
            internal set {
                _commission = value;
            }
        }
        /// <summary>
        /// 加价金额
        /// </summary>
        public decimal Increasing {
            get {
                if(!_increasing.HasValue) {
                    _increasing = Details.Sum(item => item.Increasing);
                }
                return _increasing.Value;
            }
            internal set {
                _increasing = value;
            }
        }
        /// <summary>
        /// 服务费
        /// </summary>
        public decimal ServiceCharge {
            get {
                if(!_serviceCharge.HasValue) {
                    _serviceCharge = Details.Sum(item => item.ServiceCharge);
                }
                return _serviceCharge.Value;
            }
            internal set {
                _serviceCharge = value;
            }
        }

        protected override System.Collections.Generic.IEnumerable<NormalRefundDetailBill> GetDetailBills() {
            return DistributionQueryService.QueryNormalRefundDetailBills(this.BillId);
        }
    }
}