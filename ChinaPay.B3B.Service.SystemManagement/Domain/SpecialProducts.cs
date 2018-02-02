using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.SystemManagement;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.SystemManagement.Domain
{
    class SpecialProducts
    {
        private static SpecialProducts _instance = null;
        private static object _locker = new object();
        public static SpecialProducts Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new SpecialProducts();
                        }
                    }
                }
                return _instance;
            }
        }
        RepositoryCache<SpecialProductType, SpecialProductView> _repositoryCache;
        const double _interval = 60 * 1000;
        private SpecialProducts()
        {
            var repository = Repository.Factory.CreateSpecialProductRepository();
            _repositoryCache = new RepositoryCache<SpecialProductType, SpecialProductView>(repository, _interval);
        }
        public SpecialProductView this[SpecialProductType paramType] {
            get {
                return _repositoryCache[paramType];
            }
        }
        public IEnumerable<SpecialProductView> Query() {
            return _repositoryCache.Values;
        }
        public void Update(SpecialProductView value) {
            _repositoryCache.Update(value.SpecialProductType, value);
        }
    }
}
