using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.Core;
using ChinaPay.Data;

 namespace ChinaPay.B3B.Service.Foundation.Domain {
   internal class BAFCollection : RepositoryCache<Guid, BAF> {
        private static BAFCollection _instance;
        private static object _locker = new object();
        public static BAFCollection Instance {
            get {
                if (null == _instance) {
                    lock (_locker) {
                        if (null == _instance) {
                            _instance = new BAFCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private BAFCollection()
            : base(Repository.Factory.CreateBAFRepository(), 5 * 60 * 1000) {
                RefreshService.BAFChanged += Refresh;
        }
        /// <summary>
        /// 查询燃油附加税
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="mileage">里程数</param>
        /// <param name="etdzDate">出票日期</param>
        public BAF QueryBAF(UpperString airline, decimal mileage, DateTime etdzDate) {
            var bafs = from item in this.Values
                        where (airline.Value == item.AirlineCode.Value || item.AirlineCode.IsNullOrEmpty())
                            && mileage >= item.Mileage
                            && etdzDate.Date >= item.EffectiveDate.Date
                            && (!item.ExpiredDate.HasValue || etdzDate.Date <= item.ExpiredDate.Value.Date)
                        orderby item.AirlineCode.Value descending, item.Mileage descending, item.EffectiveDate descending
                        select item;
            return bafs.FirstOrDefault();
        }
    }
}