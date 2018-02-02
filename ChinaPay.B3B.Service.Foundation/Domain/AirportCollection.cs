using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class AirportCollection : RepositoryCache<UpperString, Airport> {
        private static AirportCollection _instance;
        private static object _locker = new object();
        public static AirportCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new AirportCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private AirportCollection()
            : base(Repository.Factory.CreateAirportRepository(), 5 * 60 * 1000) {
        }
    }
}