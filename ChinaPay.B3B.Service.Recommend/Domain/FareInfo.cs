using System;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Recommend.Domain {
    public class FareInfo {
        internal FareInfo(string departure, string arrival, DateTime flightDate, decimal fare, decimal discount, ProductType product)
            : this(departure, arrival, flightDate, fare, discount, product, true) {
        }
        internal FareInfo(string departure, string arrival, DateTime flightDate, decimal fare, decimal discount, ProductType product, bool isNew) {
            this.Departure = departure;
            this.Arrival = arrival;
            this.FlightDate = flightDate;
            this.Fare = fare < 0 ? 0 : fare;
            this.Discount = discount < 0 ? 0 : discount;
            this.Product = product;
            this.Changed = isNew;
        }
        public string Departure { get; private set; }
        public string Arrival { get; private set; }
        public DateTime FlightDate { get; private set; }
        public decimal Fare { get; private set; }
        public decimal Discount { get; private set; }
        public ProductType Product { get; private set; }
        internal bool Changed {
            get;
            private set;
        }

        internal void Update(decimal fare, decimal discount, ProductType product) {
            if(fare <= 0 || (this.Fare == fare && this.Product == product)) return;
            this.Fare = fare;
            this.Discount = discount;
            this.Product = product;
            this.Changed = true;
        }
    }
}