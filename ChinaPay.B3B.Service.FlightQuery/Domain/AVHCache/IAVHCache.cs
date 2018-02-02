using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.FlightQuery.Domain.AVHCache {
    interface IAVHCache {
        IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate);
        void SaveFlights(string departure, string arrival, DateTime flightDate, IEnumerable<Command.Domain.FlightQuery.Flight> value);
    }
}