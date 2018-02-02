using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class CityCollection : RepositoryCache<string, City> {
        private static CityCollection _instance;
        private static object _locker = new object();
        public static CityCollection Instance {
            get {
                if (null == _instance) {
                    lock (_locker) {
                        if (null == _instance) {
                            _instance = new CityCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private CityCollection()
            : base(Repository.Factory.CreateCityRepository(), 5 * 60 * 1000) {
        }
    }
}