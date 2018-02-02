using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Domain.Segment {
    /// <summary>
    /// 单程
    /// </summary>
    public class OneWay : Segment {
        internal OneWay(Carrier carrier, Flight flight)
            : base(carrier) {
            if(!(flight.Bunk is Bunk.SoleOrderableBunk))
                throw new CustomException("该舱位不能单独预订");
            if(flight == null)
                throw new ArgumentNullException("flight");
            this.Flight = flight;
        }
        /// <summary>
        /// 航班信息
        /// </summary>
        public Flight Flight {
            get;
            private set;
        }
        public override Price Price {
            get { return this.Flight.Price; }
        }
        internal override IEnumerable<Flight> Flights {
            get {
                return new List<Flight> { this.Flight };
            }
        }

        internal static OneWay CreateOneWay(FlightView flightView, PassengerType passengerType, PolicyMatch.MatchedPolicy policy) {
            var carrier = new Carrier(flightView.Airline);
            return new OneWay(carrier, Flight.GetFlight(flightView, passengerType, policy));
        }
    }
}
