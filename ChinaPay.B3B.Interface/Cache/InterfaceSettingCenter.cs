using System;

namespace ChinaPay.B3B.Interface.Cache {
    internal class InterfaceSettingCenter {
        private static InterfaceSettingCenter _instance;
        private static object _locker = new object();

        public static InterfaceSettingCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new InterfaceSettingCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private InterfaceCache<Guid, Service.Organization.Domain.ExternalInterfaceSetting> _cache = null;
        private InterfaceSettingCenter() {
            _cache = new InterfaceCache<Guid, Service.Organization.Domain.ExternalInterfaceSetting> {
                Timeout = 1200
            };
        }

        public Service.Organization.Domain.ExternalInterfaceSetting this[Guid id] {
            get {
                var setting = _cache[id];
                if(setting == null) {
                    setting = Service.Organization.ExternalInterfaceService.Query(id);
                    _cache.Save(id, setting);
                }
                return setting;
            }
        }
    }
}