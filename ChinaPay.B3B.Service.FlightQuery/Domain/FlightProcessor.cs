using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    class FlightProcessor {
        public static IEnumerable<Flight> Execute(IEnumerable<Command.Domain.FlightQuery.Flight> originalFlights, BunkFilter bunkFilter) {
            var result = new List<Flight>();
            foreach(var flight in originalFlights) {
                var item = GetFlight(flight);
                if(item == null) continue;
                List<Bunk> filteredBunks;
                item.Bunks = getBunks(flight, item, bunkFilter, out filteredBunks);
                item.FilteredBunks = filteredBunks;
                result.Add(item);
            }
            return result;
        }
        public static IEnumerable<Flight> ExecuteWithoutBunk(IEnumerable<Command.Domain.FlightQuery.Flight> originalFlights, System.DateTime flightDate) {
            return originalFlights.Select(f => GetFlight(f, flightDate)).ToList();
        }
        public static Flight GetFlight(Command.Domain.FlightQuery.Flight originalFlight, System.DateTime? reviseFlightDate = null) {
            if(originalFlight == null) return null;
            Flight result = null;
            var airline = FoundationService.QueryAirline(originalFlight.Airline);
            if(null != airline) {
                var flightDate = reviseFlightDate ?? originalFlight.FlightDate;
                var basicPrice = FoundationService.QueryBasicPrice(originalFlight.Airline, originalFlight.Departure, originalFlight.Arrival, flightDate);
                if(null != basicPrice) {
                    result = new Flight(originalFlight.Airline, originalFlight.FlightNo) {
                        AirlineName = airline.ShortName,
                        Departure = getAirport(originalFlight.Departure, originalFlight.TerminalOfDeparture),
                        Arrival = getAirport(originalFlight.Arrival, originalFlight.TerminalOfArrival),
                        FlightDate = flightDate,
                        TakeoffTime = originalFlight.TakeoffTime,
                        LandingTime = originalFlight.LandingTime,
                        StandardPrice = basicPrice.Price,
                        AirCraft = originalFlight.AirCraft,
                        IsStop = originalFlight.IsStop,
                        DaysInterval = originalFlight.AddDays,
                        AirportFee = FoundationService.QueryAirportFee(originalFlight.AirCraft),
                        BAF = FoundationService.QueryBAF(originalFlight.Airline, basicPrice.Mileage)
                    };
                }
            }
            return result;
        }
        private static Airport getAirport(string code, string terminal) {
            var airportData = FoundationService.QueryAirport(code);
            return new Airport(code) {
                Terminal = terminal,
                Name = null == airportData ? string.Empty : airportData.Name,
                City = null == airportData ? string.Empty : airportData.Location.Name,
                AbbrivateName = null == airportData ? string.Empty : airportData.ShortName
            };
        }
        private static IEnumerable<Bunk> getBunks(Command.Domain.FlightQuery.Flight originalFlight, Flight flight, BunkFilter bunkFilter, out List<Bunk> filteredBunks) {
            var result = new List<Bunk>();
            filteredBunks = new List<Bunk>();
            foreach(var originalBunk in originalFlight.Bunks) {
                List<Bunk> fbunks;
                var bunks = getBunks(flight, originalBunk, bunkFilter, out fbunks);
                result.AddRange(bunks);
                filteredBunks.AddRange(fbunks);
            }
            return result;
        }
        private static IEnumerable<Bunk> getBunks(Flight flight, Command.Domain.FlightQuery.Bunk originalBunk, BunkFilter bunkFilter, out List<Bunk> filteredBunks) {
            var result = new List<Bunk>();
            filteredBunks = new List<Bunk>();
            var bunkDatas = FoundationService.QueryBunk(flight.Airline, flight.Departure.Code, flight.Arrival.Code, flight.FlightDate, originalBunk.Code);
            foreach(var bunkData in bunkDatas) {
                Bunk item = null;
                if(bunkData is Foundation.Domain.EconomicBunk) {
                    var discount = (bunkData as Foundation.Domain.EconomicBunk).GetDiscount(originalBunk.Code);
                    if(discount < 0) continue;
                    item = new EconomicBunk(originalBunk.Code) {
                        Discount = discount
                    };
                } else if(bunkData is Foundation.Domain.FirstBusinessBunk) {
                    var firstBusinessBunk = bunkData as Foundation.Domain.FirstBusinessBunk;
                    var discount = firstBusinessBunk.GetDiscount(originalBunk.Code);
                    if(discount < 0) continue;
                    item = new FirstOrBusinessBunk(originalBunk.Code) {
                        Discount = discount,
                        Description = firstBusinessBunk.Description
                    };
                } else if(bunkData is Foundation.Domain.PromotionBunk) {
                    item = new PromotionBunk(originalBunk.Code) {
                        Description = (bunkData as Foundation.Domain.PromotionBunk).Description
                    };
                } else if(bunkData is Foundation.Domain.ProductionBunk) {
                    item = new ProductionBunk(originalBunk.Code);
                } else if(bunkData is Foundation.Domain.TransferBunk) {
                    item = new TransferBunk(originalBunk.Code);
                } else if(bunkData is Foundation.Domain.FreeBunk) {
                    item = new FreeBunk(originalBunk.Code) {
                        Description = (bunkData as Foundation.Domain.FreeBunk).Description
                    };
                } else if(bunkData is Foundation.Domain.TeamBunk) {
                    item = new TeamBunk(originalBunk.Code);
                } else {
                    continue;
                }
                if(originalBunk.Status == "A") {
                    item.SeatCount = 10;
                } else {
                    int count;
                    if(int.TryParse(originalBunk.Status, out count)) {
                        item.SeatCount = count;
                    } else {
                        continue;
                    }
                }
                item.EI = bunkData.EI;
                item.EndorseRegulation = bunkData.EndorseRegulation;
                item.RefundRegulation = bunkData.RefundRegulation;
                item.ChangeRegulation = bunkData.ChangeRegulation;
                item.Remark = bunkData.Remarks;
                item.SuportChild = (bunkData.PassengerType & PassengerTypeValue.Child) == PassengerTypeValue.Child;
                item.Owner = flight;

                if(bunkFilter.Execute(bunkData)) {
                    result.Add(item);
                } else {
                    filteredBunks.Add(item);
                }
            }
            return result;
        }
    }
}