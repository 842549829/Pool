using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Command;
using ChinaPay.Core;
using ChinaPay.B3B.Service.FlightQuery.Domain.AVHCache;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    static class FlightDatasCenter {
        static IAVHCache avhCache = LocalAVHCache.Instance;
        public static IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate,Guid oemId) {
            if(string.IsNullOrWhiteSpace(departure)) throw new ArgumentNullException("departure");
            if(string.IsNullOrWhiteSpace(departure)) throw new ArgumentNullException("arrival");
            if(flightDate.Date < DateTime.Today) return new List<Command.Domain.FlightQuery.Flight>();

            // 从缓存中获取航班数据
            var cacheData = avhCache.GetFlights(departure, arrival, flightDate);
            if(null != cacheData) return cacheData.FilterExpiredFlight();

            var queryResult = CommandService.QueryFlight(departure, arrival, flightDate, oemId);
            if(queryResult.Success) {
                var datas = queryResult.Result.FilterShareFlight().FilterAirport(departure, arrival).FilterExpiredFlight().FilterRepeatFlight().ToList();
                var sortedDatas = datas.OrderBy(item => item.TakeoffTime).ToList();
                avhCache.SaveFlights(departure, arrival, flightDate, sortedDatas);
                if(sortedDatas.Any()) {
                    // 保存航班查询原始数据
                    var repository = Repository.Factory.CreateRepository();
                    repository.SaveFlightRecord(new FlightRecord {
                        Departure = departure,
                        Arrival = arrival,
                        FlightDate = flightDate,
                        Content = queryResult.Message
                    });
                }
                return sortedDatas;
            } else {
                LogService.SaveTextLog("航班查询失败：" + queryResult.Message);
                throw new CustomException(queryResult.Message);
            }
        }
        public static IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate, Time flightBeginTime, Guid oemId)
        {
            var fullData = GetFlights(departure, arrival, flightDate,oemId);
            return fullData.Where(item => item.TakeoffTime >= flightBeginTime);
        }
        public static IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate, string airline, Guid oemId)
        {
            var fullData = GetFlights(departure, arrival, flightDate,oemId);
            if(string.IsNullOrWhiteSpace(airline)) return fullData;
            return fullData.Where(item => item.Airline == airline);
        }
        public static IEnumerable<Command.Domain.FlightQuery.Flight> GetFlights(string departure, string arrival, DateTime flightDate, string airline, Time flightBeginTime, Guid oemId)
        {
            if(string.IsNullOrWhiteSpace(airline)) {
                return GetFlights(departure, arrival, flightDate, flightBeginTime,oemId);
            } else {
                var fullData = GetFlights(departure, arrival, flightDate,oemId);
                return fullData.Where(item => item.Airline == airline && item.TakeoffTime >= flightBeginTime);
            }
        }
        /// <summary>
        /// 过滤共享航班
        /// </summary>
        public static IEnumerable<Command.Domain.FlightQuery.Flight> FilterShareFlight(this IEnumerable<Command.Domain.FlightQuery.Flight> flights) {
            return flights.Where(f => !f.IsShareFlight);
        }
        /// <summary>
        /// 过滤已失效的航班
        /// </summary>
        public static IEnumerable<Command.Domain.FlightQuery.Flight> FilterExpiredFlight(this IEnumerable<Command.Domain.FlightQuery.Flight> flights) {
            var expiredTime = getFlightExpiredTime();
            return flights.Where(f => f.FlightDate.Date > DateTime.Today || (f.FlightDate.Date == DateTime.Today && f.TakeoffTime >= expiredTime));
        }
        /// <summary>
        /// 过滤已失效的航班
        /// </summary>
        public static IEnumerable<Flight> FilterExpiredFlight(this IEnumerable<Flight> flights) {
            var expiredTime = getFlightExpiredTime();
            return flights.Where(f => f.FlightDate.Date > DateTime.Today || (f.FlightDate.Date == DateTime.Today && f.TakeoffTime >= expiredTime));
        }
        /// <summary>
        /// 过滤同城机场
        /// </summary>
        public static IEnumerable<Command.Domain.FlightQuery.Flight> FilterAirport(this IEnumerable<Command.Domain.FlightQuery.Flight> flights, string departure, string arrival) {
            return flights.Where(item => item.Departure == departure && item.Arrival == arrival);
        }
        /// <summary>
        /// 过滤重复航班
        /// </summary>
        public static IEnumerable<Command.Domain.FlightQuery.Flight> FilterRepeatFlight(this IEnumerable<Command.Domain.FlightQuery.Flight> flights) {
            var result = new Dictionary<string, Command.Domain.FlightQuery.Flight>();
            foreach(var flight in flights) {
                if(!result.ContainsKey(flight.FlightNo)) {
                    result.Add(flight.FlightNo, flight);
                }
            }
            return result.Values;
        }
        private static Time getFlightExpiredTime() {
            DateTime dateTime = DateTime.Now.AddMinutes(SystemManagement.SystemParamService.FlightDisableTime);
            return new Time(dateTime);
        }
    }
}