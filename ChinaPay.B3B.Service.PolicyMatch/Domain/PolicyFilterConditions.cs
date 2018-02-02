using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Enums;
    using FlightQuery.Domain;

    public enum ProviderFilterType {
        /// <summary>
        /// 包含该出票方政策
        /// </summary>
        Include,
        /// <summary>
        /// 排除该出票方政策
        /// </summary>
        Exclude
    }
    public class FlightFilterInfo {
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string FlightNumber { get; set; }
        public DateTime FlightDate { get; set; }
        public decimal StandardPrice { get; set; }
        public bool IsShare { get; set; }
        public DateTime TakeOffTime { get; set; }

        public decimal Fare { get; set; }
    }
    public class BunkFilterInfo {
        public string Code { get; set; }
        public BunkType Type { get; set; }
        public decimal Discount { get; set; }
    }
    public class VoyageFilterInfo {
        public FlightFilterInfo Flight { get; set; }
        public BunkFilterInfo Bunk { get; set; }
    }

    public class PolicyFilterConditions {
        public Guid? PolicyId { get; set; }
        public VoyageType VoyageType { get; set; }
        public PolicyType PolicyType { get; set; }
        public Guid? Provider { get; set; }
        public Guid Purchaser { get; set; }
        private readonly List<Guid> excludeProviders = new List<Guid>();
        public List<Guid> ExcludeProviders { get { return excludeProviders; } }
        public decimal PublishFare { get; set; }
        public bool SuitReduce { get; set; }
        private readonly List<VoyageFilterInfo> voyages = new List<VoyageFilterInfo>();
        public List<VoyageFilterInfo> Voyages { get { return voyages; } }
        public string PatContent { get; set; }
        public string PnrContent { get; set; }
        public PNRPair PnrPair { get; set; }
        
        public decimal? PatPrice { get; set; }

        /// <summary>
        /// 是否使用PAT价格
        /// </summary>
        public bool IsUsePatPrice { get; set; }

        /// <summary>
        /// 是否需要贴点
        /// </summary>
        public bool NeedSubsidize { get; set; }

        /// <summary>
        /// 允许选择的客票类型
        /// </summary>
        public AllowTicketType AllowTicketType { get; set; }

        
        public decimal MaxdRebate { get; set; }

        public bool UseBPNR { get; set; }

        public PolicyFilterConditions() {}
        private PolicyFilterConditions(IEnumerable<VoyageFilterInfo> voyages) {
            this.voyages = voyages.ToList();
        }

        internal static PolicyFilterConditions FromFlights(IEnumerable<Flight> flights) {
            return new PolicyFilterConditions(CreateVoyages(flights));
        }
        internal static FlightFilterInfo GetFlightFilters(Flight flight) {
            return new FlightFilterInfo {
                Airline = flight.Airline,
                Arrival = flight.Arrival.Code,
                Departure = flight.Departure.Code,
                FlightDate = flight.FlightDate,
                FlightNumber = flight.FlightNo,
                StandardPrice = flight.StandardPrice
            };
        }
        private static BunkFilterInfo GetBunkFilters(Bunk bunk) {
            return new BunkFilterInfo {
                Code = bunk.Code,
                Discount = (bunk is GeneralBunk) ? ((GeneralBunk)bunk).Discount : 0,
                Type = bunk.Type
            };
        }
        private static IEnumerable<VoyageFilterInfo> CreateVoyages(IEnumerable<Flight> flights) {
            return from flight in flights
                   let ffi = GetFlightFilters(flight)
                   from bunk in flight.Bunks
                   select new VoyageFilterInfo {
                       Flight = ffi,
                       Bunk = GetBunkFilters(bunk)
                   };
        }
    }
}
