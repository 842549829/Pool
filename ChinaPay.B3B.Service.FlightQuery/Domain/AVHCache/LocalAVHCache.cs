using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.FlightQuery.Domain.AVHCache {
    class LocalAVHCache : IAVHCache {
        static LocalAVHCache _instance = null;
        static object _locker = new object();
        public static LocalAVHCache Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new LocalAVHCache();
                        }
                    }
                }
                return _instance;
            }
        }

        ChinaPay.Data.Cache<string, IEnumerable<Command.Domain.FlightQuery.Flight>> _dataCache;
        private LocalAVHCache() {
            _dataCache = new Data.Cache<string, IEnumerable<Command.Domain.FlightQuery.Flight>>();
            _dataCache.Timeout = 180;
        }
        public IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate) {
            return _dataCache[GetKey(departure, arrival, flightDate)];
        }

        public void SaveFlights(string departure, string arrival, DateTime flightDate, IEnumerable<Command.Domain.FlightQuery.Flight> value) {
            _dataCache.Save(GetKey(departure, arrival, flightDate), value);
        }
        private string GetKey(string departure, string arrival, DateTime flightDate) {
            return departure + arrival + flightDate.ToString("yyMMdd");
        }
    }
}