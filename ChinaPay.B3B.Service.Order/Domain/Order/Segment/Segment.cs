using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Segment {
    /// <summary>
    /// 航程
    /// </summary>
    public abstract class Segment {
        protected Segment(Carrier carrier) {
            this.Carrier = carrier;
        }
        /// <summary>
        /// 承运人
        /// </summary>
        public Carrier Carrier {
            get;
            private set;
        }

        /// <summary>
        /// 成人价格信息
        /// </summary>
        public abstract Price Price {
            get;
        }
        internal abstract IEnumerable<Flight> Flights {
            get;
        }

        internal static Segment CreateSegment(OrderView orderView, PassengerType passengerType, PolicyMatch.MatchedPolicy policy) {
            int flightCount = orderView.Flights.Count();
            if(flightCount == 1) {
                return OneWay.CreateOneWay(orderView.Flights.ElementAt(0), passengerType, policy);
            } else if(flightCount == 2) {
                return RoundTrip.CreateRoundTrip(orderView.Flights.ElementAt(0), orderView.Flights.ElementAt(1), passengerType, policy);
            } else {
                throw new NotSupportedException("暂只支持最多两个航段"); 
            }
        }
    }
}