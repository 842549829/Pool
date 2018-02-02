using System.Collections.Generic;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    /// <summary>
    /// 系统参数
    /// </summary>
    internal class SystemParams {
        private static SystemParams _instance = null;
        private static object _locker = new object();
        public static SystemParams Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new SystemParams();
                        }
                    }
                }
                return _instance;
            }
        }
        RepositoryCache<SystemParamType, SystemParam> _repositoryCache;
        const double _interval = 60 * 1000;
        private SystemParams() {
            var repository = Repository.Factory.CreateSystemParamRepository();
            _repositoryCache = new RepositoryCache<SystemParamType, SystemParam>(repository, _interval);
        }
        public SystemParam this[SystemParamType paramType] {
            get {
                return _repositoryCache[paramType];
            }
        }
        public IEnumerable<SystemParam> Query() {
            return _repositoryCache.Values;
        }
        public void Update(SystemParamType paramType, string value) {
            var data = this[paramType];
            data.Value = value;
            _repositoryCache.Update(paramType, data);
        }
    }
}