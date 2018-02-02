using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class AirlineCollection : RepositoryCache<UpperString, Airline> {
        private static AirlineCollection _instance;
        private static object _locker = new object();
        public static AirlineCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new AirlineCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private AirlineCollection()
            : base(Repository.Factory.CreateAirlineRepository(), 5 * 60 * 1000) {
        }
    }
}