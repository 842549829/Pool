using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace ChinaPay.B3B.Service.Recommend.Domain {
    class FareDataCenter {
        static FareDataCenter m_instance = null;
        static object m_instanceLocker = new object();
        public static FareDataCenter Instance {
            get {
                if(m_instance == null) {
                    lock(m_instanceLocker) {
                        if(m_instance == null) {
                            m_instance = new FareDataCenter();
                        }
                    }
                }
                return m_instance;
            }
        }

        Dictionary<string, FareInfo> m_fares = null;
        object m_locker = null;
        Timer m_timer = null;
        private FareDataCenter() {
            m_fares = new Dictionary<string, FareInfo>();
            m_locker = new object();
            query();
            m_timer = new Timer(60000);
            m_timer.Elapsed += (sender, e) => refresh();
            m_timer.Start();
        }

        public IEnumerable<FareInfo> Values {
            get {
                IEnumerable<FareInfo> result = null;
                result = m_fares.Values;
                return result;
            }
        }

        public FareInfo Query(string departure, string arrival, DateTime flightDate) {
            FareInfo result;
            m_fares.TryGetValue(getKey(departure, arrival, flightDate), out result);
            return result;
        }
        public void Save(FareInfo item) {
            if(item == null)
                throw new ArgumentNullException("item");
            if(item.FlightDate.Date >= DateTime.Today) {
                var key = getKey(item.Departure, item.Arrival, item.FlightDate);
                FareInfo obj;
                lock(m_locker) {
                    if(m_fares.TryGetValue(key, out obj)) {
                        obj.Update(item.Fare, item.Discount, item.Product);
                    } else {
                        m_fares.Add(key, item);
                    }
                }
            }
        }
        private void refresh() {
            saveAll();
            query();
        }
        private void saveAll() {
            var repository = Repository.Factory.CreateFlightLowerFareRepository();
            foreach(var item in m_fares.Values) {
                if(item.Changed) {
                    repository.Save(item);
                }
            }
        }
        private void query() {
            var repository = Repository.Factory.CreateFlightLowerFareRepository();
            var fares = repository.Query();
            m_fares = fares.ToDictionary(item => getKey(item.Departure, item.Arrival, item.FlightDate));
        }
        private string getKey(string departure, string arrival, DateTime flightDate) {
            return departure + arrival + flightDate.ToString("yyMMdd");
        }
    }
}