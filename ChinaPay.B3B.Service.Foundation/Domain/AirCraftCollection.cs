using System;
using System.Linq;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class AirCraftCollection : RepositoryCache<Guid, AirCraft> {
        private static AirCraftCollection _instance;
        private static object _locker = new object();
        public static AirCraftCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new AirCraftCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private AirCraftCollection()
            : base(Repository.Factory.CreateAirCraftRepository(), 5 * 60 * 1000) {
        }
    }
}