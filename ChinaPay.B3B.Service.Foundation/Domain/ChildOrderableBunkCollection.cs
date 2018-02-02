using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class ChildOrderableBunkCollection : RepositoryCache<Guid, ChildOrderableBunk> {
        private static ChildOrderableBunkCollection _instance;
        private static object _locker = new object();
        public static ChildOrderableBunkCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new ChildOrderableBunkCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private ChildOrderableBunkCollection()
            : base(Repository.Factory.CreateChildOrderableBunkRepository(), 5 * 60 * 1000) {
        }
        /// <summary>
        /// 查询儿童可预订舱位
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        public IEnumerable<ChildOrderableBunk> QueryChildrenOrderableBunk(UpperString airline) {
            return from item in this.Values
                   where airline.Value == item.AirlineCode.Value
                   select item;
        }
    }
}