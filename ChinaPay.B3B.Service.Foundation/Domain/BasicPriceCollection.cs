using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class BasicPriceCollection : RepositoryCache<Guid, BasicPrice> {
        private static BasicPriceCollection _instance;
        private static object _locker = new object();
        public static BasicPriceCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new BasicPriceCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private BasicPriceCollection()
            : base(Repository.Factory.CreateBasicPriceRepository(), 5 * 60 * 1000) {
                RefreshService.BasePriceChanged += Refresh;
        }
        /// <summary>
        /// 查询基础运价信息
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        public BasicPrice QueryBasicPrice(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate) {
            var prices = from item in this.Values
                         where (airline.Value == item.AirlineCode.Value || item.AirlineCode.IsNullOrEmpty())
                            && ((departure.Value == item.DepartureCode.Value && arrival.Value == item.ArrivalCode.Value)
                                || (departure.Value == item.ArrivalCode.Value && arrival.Value == item.DepartureCode.Value))
                            && flightDate.Date >= item.FlightDate.Date
                            && DateTime.Today >= item.ETDZDate.Date
                         orderby item.AirlineCode.Value descending, item.FlightDate descending,item.ModifyTime descending
                         select item;
            return prices.FirstOrDefault();
        }
    }
}