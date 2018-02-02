using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers
{
    /// <summary>
    /// 推荐信息处理
    /// </summary>
    public class Recommend : ChinaPay.Infrastructure.WebEx.AjaxHandler.WebAjaxHandler
    {
        public object QueryRecommendCities()
        {
            return Service.FoundationService.Airports.Where(item => item.Valid && item.IsMain).OrderBy(item => item.Location.HotLevel).Take(6).Select(item => new
            {
                Name = item.Location.Name,
                Code = item.Code.Value
            });
        }
        public object QueryRecommendInfos(string code)
        {
            if (code == null) return "[]";
            return from item in Service.RecommendService.QueryFlightLowerFares(code.ToUpper()).Where(p => p.FlightDate > DateTime.Today && p.Product != DataTransferObject.Order.ProductType.Special).Take(12)
                   join departure in Service.FoundationService.Airports on item.Departure equals departure.Code
                   join arrival in Service.FoundationService.Airports on item.Arrival equals arrival.Code
                   orderby item.Discount
                   select new
                   {
                       Date = item.FlightDate.ToString("MM-dd"),
                       Departure = departure.Location.Name,
                       DepartureCode = departure.Code.Value,
                       Arrival = arrival.Location.Name,
                       ArrivalCode = arrival.Code.Value,
                       Fare = item.Fare.ToString("F0")
                   };
        }
        public object QueryLowerFares(string departure, string arrival, DateTime startDate, DateTime endDate)
        {
            var fares = new List<KeyValuePair<DateTime, decimal>>();
            var currentDate = startDate;
            while (currentDate <= endDate)
            {
                var fare = Service.RecommendService.QueryFlightLowerFare(departure, arrival, currentDate);
                if (fare > 0)
                {
                    fares.Add(new KeyValuePair<DateTime, decimal>(currentDate, fare));
                }
                currentDate = currentDate.AddDays(1);
            }
            return fares.Select(item => new
            {
                Date = item.Key.ToString("yyyy-MM-dd"),
                Fare = item.Value.TrimInvaidZero()
            });
        }
    }
}