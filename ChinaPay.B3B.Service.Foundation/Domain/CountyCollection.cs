using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    class CountyCollection : RepositoryCache<string, County> {
        private static CountyCollection _instance;
        private static object _locker = new object();
        public static CountyCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new CountyCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private CountyCollection()
            : base(Repository.Factory.CreateCountyRepository(), 5 * 60 * 1000) {
        }
    }
}
