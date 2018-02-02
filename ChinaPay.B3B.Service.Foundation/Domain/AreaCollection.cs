using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    class AreaCollection  : RepositoryCache<string, Area> {
        private static AreaCollection _instance;
        private static object _locker = new object();
        public static AreaCollection Instance {
            get {
                if (null == _instance) {
                    lock (_locker) {
                        if (null == _instance) {
                            _instance = new AreaCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private AreaCollection()
            : base(Repository.Factory.CreateAreaRepository(), 5 * 60 * 1000) {
        }
    }
}