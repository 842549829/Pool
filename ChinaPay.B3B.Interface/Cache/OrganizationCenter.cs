using System;

namespace ChinaPay.B3B.Interface.Cache {
    internal class OrganizationCenter {
        private static OrganizationCenter _instance;
        private static object _locker = new object();

        public static OrganizationCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new OrganizationCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private InterfaceCache<Guid, DataTransferObject.Organization.CompanyDetailInfo> _cache = null;
        private OrganizationCenter() {
            _cache = new InterfaceCache<Guid, DataTransferObject.Organization.CompanyDetailInfo> {
                Timeout = 3
            };
        }

        public DataTransferObject.Organization.CompanyDetailInfo this[Guid id] {
            get {
                var organization = _cache[id];
                if(organization == null) {
                    organization = Service.Organization.CompanyService.GetCompanyDetail(id);
                    _cache.Save(id, organization);
                }
                return organization;
            }
        }
    }
}