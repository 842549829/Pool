using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Segment {
    /// <summary>
    /// 往返
    /// </summary>
    public abstract class RoundTrip : Segment {
        /// <param name="carrier">乘运人</param>
        /// <param name="outwardFlight">去程航班信息</param>
        /// <param name="returnFlight">回程航班信息</param>
        internal RoundTrip(Carrier carrier, Flight outwardFlight, Flight returnFlight)
            : base(carrier) {
            if(outwardFlight == null)
                throw new ArgumentNullException("outwardFlight");
            if(returnFlight == null)
                throw new ArgumentNullException("returnFlight");
            if(!((outwardFlight.Bunk is Bunk.ProductionBunk && returnFlight.Bunk is Bunk.ProductionBunk)
                || !(outwardFlight.Bunk is Bunk.GeneralBunk && returnFlight.Bunk is Bunk.GeneralBunk)))
                throw new NotSupportedException("暂只支持普通舱与普通舱组合 或 往返产品舱 的往返");
            this.OutwardFlight = outwardFlight;
            this.ReturnFlight = returnFlight;
        }
        /// <summary>
        /// 去程
        /// </summary>
        public Flight OutwardFlight {
            get;
            private set;
        }
        /// <summary>
        /// 回程
        /// </summary>
        public Flight ReturnFlight {
            get;
            private set;
        }
        internal override IEnumerable<Flight> Flights {
            get {
                return new List<Flight> { this.OutwardFlight, this.ReturnFlight };
            }
        }

        internal static RoundTrip CreateRoundTrip(FlightView outwardFlightView, FlightView returnFlightView, PassengerType passengerType, PolicyMatch.MatchedPolicy policy) {
            if(outwardFlightView.Airline != returnFlightView.Airline)
                throw new NotSupportedException("暂只支持同一航空公司的往返程");
            Carrier carrier = new Carrier(outwardFlightView.Airline);
            switch(policy.PolicyType) {
                case ChinaPay.B3B.DataTransferObject.Policy.PolicyType.General:
                    return new GeneralRoundTrip(carrier,
                            Flight.GetFlight(outwardFlightView, passengerType, policy),
                            Flight.GetFlight(returnFlightView, passengerType, policy));
                case ChinaPay.B3B.DataTransferObject.Policy.PolicyType.Prodution:
                    return new ProductionRoundTrip(carrier,
                            Flight.GetFlight(outwardFlightView, passengerType, policy),
                            Flight.GetFlight(returnFlightView, passengerType, policy),
                            ((PolicyMatch.MatchedProductionPolicy)policy).Price);
                default:
                    throw new NotSupportedException("产品类型与行程类型不匹配");
            }
        }
    }
}