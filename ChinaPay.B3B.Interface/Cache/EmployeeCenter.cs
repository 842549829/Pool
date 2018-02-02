namespace ChinaPay.B3B.Interface.Cache {
    internal class EmployeeCenter {
        private static EmployeeCenter _instance;
        private static object _locker = new object();

        public static EmployeeCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new EmployeeCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private InterfaceCache<string, DataTransferObject.Organization.EmployeeDetailInfo> _cache = null;
        private EmployeeCenter() {
            _cache = new InterfaceCache<string, DataTransferObject.Organization.EmployeeDetailInfo> {
                Timeout = 10
            };
        }

        public DataTransferObject.Organization.EmployeeDetailInfo this[string userName] {
            get {
                var employee = _cache[userName];
                if(employee == null) {
                    employee = Service.Organization.EmployeeService.QueryEmployee(userName);
                    _cache.Save(userName, employee);
                }
                return employee;
            }
        }
    }
}