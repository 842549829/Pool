using System;

namespace ChinaPay.B3B.Service.Distribution.Domain {
    public class Flight {
        public Flight(Guid id) {
            Id = id;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 发布的票面价
        /// </summary>
        public decimal ReleasedFare {
            get;
            internal set;
        }
        /// <summary>
        /// 票面价
        /// 实际价格
        /// </summary>
        public decimal Fare {
            get;
            internal set;
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            internal set;
        }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public decimal BAF {
            get;
            internal set;
        }

        internal static Flight GetFlight(Order.Domain.Flight flight)
        {
            var fare = flight.Bunk.Fare;
            var baf = flight.BAF;
            var airportFee = flight.AirportFee;
            var releasedFare = 0M;
            if(flight.Bunk is Order.Domain.Bunk.SpecialBunk) {
                var specialBunk = (Order.Domain.Bunk.SpecialBunk)flight.Bunk;
                releasedFare = specialBunk.ReleasedFare;
            } else {
                releasedFare = fare;
            }
            return new Flight(flight.ReservateFlight) {
                Fare = fare,
                AirportFee = airportFee,
                BAF = baf,
                ReleasedFare = releasedFare
            };
        }
    }
}
