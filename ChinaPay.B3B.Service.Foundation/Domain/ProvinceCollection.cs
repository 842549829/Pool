using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
     internal class ProvinceCollection : RepositoryCache<string, Province> {
        private static ProvinceCollection _instance;
        private static object _locker = new object();
        public static ProvinceCollection Instance {
            get {
                if (null == _instance) {
                    lock (_locker) {
                        if (null == _instance) {
                            _instance = new ProvinceCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private ProvinceCollection()
            : base(Repository.Factory.CreateProvinceRepository(), 5 * 60 * 1000) {
        }
    }
}