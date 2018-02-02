using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service {
    public static class RecommendService {
        /// <summary>
        /// 保存航班最低价
        /// </summary>
        /// <param name="departure">出发地</param>
        /// <param name="arrival">到达地</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="fare">价格</param>
        /// <param name="discount">折扣</param>
        /// <param name="product">产品类型</param>
        public static void SaveFlightLowerFare(string departure, string arrival, DateTime flightDate, decimal fare, decimal discount, ProductType product) {
            if(string.IsNullOrWhiteSpace(departure))
                throw new ArgumentNullException("departure");
            if(string.IsNullOrWhiteSpace(arrival))
                throw new ArgumentNullException("arrival");
            if(fare <= 0)
                throw new InvalidOperationException("fare");
            if(discount <= 0)
                throw new InvalidOperationException("discount");
            var model = new Recommend.Domain.FareInfo(departure, arrival, flightDate, fare, discount, product);
            Recommend.Domain.FareDataCenter.Instance.Save(model);
        }
        /// <summary>
        /// 查询航班最低价
        /// </summary>
        /// <param name="departure">出发地</param>
        /// <param name="arrival">到达地</param>
        /// <param name="flightDate">航班日期</param>
        public static decimal QueryFlightLowerFare(string departure, string arrival, DateTime flightDate) {
            var model = Recommend.Domain.FareDataCenter.Instance.Query(departure, arrival, flightDate);
            return model == null ? 0 : model.Fare;
        }
        /// <summary>
        /// 查询低价信息
        /// </summary>
        /// <param name="departure">出发地</param>
        public static IEnumerable<Recommend.Domain.FareInfo> QueryFlightLowerFares(string departure) {
            if(string.IsNullOrWhiteSpace(departure))
                throw new ArgumentNullException("departure");
            return from item in Recommend.Domain.FareDataCenter.Instance.Values
                   where item.Departure == departure
                   orderby item.FlightDate
                   select item;
        }
    }
}