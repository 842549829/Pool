using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Data;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class FixedNavigations : RepositoryCache<string, FixedNavigationView> {
        private static FixedNavigations _instance = null;
        private static object _locker = new object();
        public static FixedNavigations Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new FixedNavigations();
                        }
                    }
                }
                return _instance;
            }
        }
        private FixedNavigations()
            : base(Repository.Factory.CreateFixedNavigationRepository(), 60 * 10000) {
        }
        public FixedNavigationView Query(UpperString departure, UpperString arrival) {
            return this[departure.Value + arrival.Value];
        }
        public void Delete(FixedNavigationView fixedNavigation) {
            Remove(fixedNavigation.Departure + fixedNavigation.Arrival);
        }
        public void Insert(FixedNavigationView fixedNavigation) {
            Add(fixedNavigation.Departure + fixedNavigation.Arrival, fixedNavigation);
        }
    }
}
