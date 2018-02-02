using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.FlightQuery.Domain;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 航班查询服务
    /// </summary>
    public static class FlightQueryService {
        /// <summary>
        /// 航班查询
        /// 单程
        /// </summary>
        /// <param name="departure">出发机场 三字码</param>
        /// <param name="arrival">到达机场 三字码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="airline">航空公司 二字码</param>
        public static IEnumerable<Flight> QueryOWFlights(UpperString departure, UpperString arrival, DateTime flightDate, UpperString airline, Guid oemId)
        {
            var originalFlightDatas = FlightDatasCenter.GetFlights(departure.Value, arrival.Value, flightDate, airline.Value,oemId);
            return FlightProcessor.Execute(originalFlightDatas, OWBunkFilter.Instance);
        }
        /// <summary>
        /// 航班查询
        /// 往返去程
        /// </summary>
        /// <param name="departure">出发机场 三字码</param>
        /// <param name="arrival">到达机场 三字码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="airline">航空公司 二字码</param>
        public static IEnumerable<Flight> QueryRTFirstTripFlights(UpperString departure, UpperString arrival, DateTime flightDate, UpperString airline, Guid oemId)
        {
            var originalFlightDatas = FlightDatasCenter.GetFlights(departure.Value, arrival.Value, flightDate, airline.Value,oemId);
            return FlightProcessor.Execute(originalFlightDatas, RTFirstTripBunkFilter.Instance);
        }
        /// <summary>
        /// 航班查询
        /// 往返回程
        /// </summary>
        /// <param name="departure">出发机场 三字码</param>
        /// <param name="arrival">到达机场 三字码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="firstTripAirline">去程航空公司 二字码</param>
        /// <param name="firstTripFlightArrivalTime">去程航班降落时间</param>
        public static IEnumerable<Flight> QueryRTSecondTipFlights(UpperString departure, UpperString arrival, DateTime flightDate, UpperString firstTripAirline, DateTime firstTripFlightArrivalTime, Common.Enums.PolicyType firstTripPolicyType, Bunk firstTripBunk, Guid oemId)
        {
            var flightBeginTime = Time.Min;
            if(flightDate.Date <= firstTripFlightArrivalTime.Date) {
                var tempFlightBeginTime = firstTripFlightArrivalTime.AddHours(2);
                if(tempFlightBeginTime.Date > flightDate.Date) return new List<Flight>();
                flightBeginTime = new Time(tempFlightBeginTime);
            }
            var originalFlightDatas = FlightDatasCenter.GetFlights(departure.Value, arrival.Value, flightDate, firstTripAirline.Value, flightBeginTime,oemId);
            return FlightProcessor.Execute(originalFlightDatas, new RTSecondTripBunkFilter(firstTripPolicyType, firstTripBunk));
        }
        /// <summary>
        /// 从历史数据中查询航班信息
        /// </summary>
        public static Dictionary<DateTime, IEnumerable<Flight>> QueryFlightFromHistory(UpperString departure, UpperString arrival, IEnumerable<DateTime> flightDates) {
            if(departure.IsNullOrEmpty()) throw new ArgumentNullException("departure");
            if(arrival.IsNullOrEmpty()) throw new ArgumentNullException("arrival");

            var result = new Dictionary<DateTime, IEnumerable<Flight>>();
            if(flightDates.Any()) {
                //var voyageSetting = FoundationService.QueryFixedNavigation(departure, arrival);
                var repository = FlightQuery.Repository.Factory.CreateRepository();
                //if(voyageSetting == null) {
                //    var flightHistoryRecord = repository.Query(departure.Value, arrival.Value, flightDates.First(), false);
                //    var flights = processFlights(flightHistoryRecord);
                //    foreach(var item in flightDates) {
                //        result.Add(item, flights);
                //    }
                //} else {
                foreach(var flightDate in flightDates) {
                    var flightHistoryRecord = repository.Query(departure.Value, arrival.Value, flightDate, true);
                    result.Add(flightDate, processFlights(flightHistoryRecord));
                }
                //}
            }
            return result;
        }
        public static Flight QueryFlight(UpperString departure, UpperString arrival, DateTime flightTime, UpperString airline, string flightNo, Guid oemId)
        {
            if (departure.IsNullOrEmpty()) throw new ArgumentNullException("departure");
            if (arrival.IsNullOrEmpty()) throw new ArgumentNullException("arrival");
            if (arrival.IsNullOrEmpty()) throw new ArgumentNullException("airline");
            if (string.IsNullOrWhiteSpace(flightNo)) throw new ArgumentNullException("flightNo");

            // 查询历史数据
            var repository = FlightQuery.Repository.Factory.CreateRepository();
            var flightHistoryRecord = repository.Query(departure.Value, arrival.Value, flightTime, true);

            IEnumerable<Command.Domain.FlightQuery.Flight> commandFlights = null;
            if (flightHistoryRecord == null || string.IsNullOrWhiteSpace(flightHistoryRecord.Content))
            {
                // 如果历史数据中没有，则查询航班
                //var execResult = CommandService.QuerySingleFlight(departure.Value, arrival.Value, flightTime);

                //if(execResult.Success) {
                //    commandFlights = execResult.Result;
                //} else {
                //    throw new CustomException("查询航班失败");
                //}
                commandFlights = FlightDatasCenter.GetFlights(departure.Value, arrival.Value, flightTime.Date, airline.Value,oemId);
            }
            else
            {
                commandFlights = Command.Domain.Utility.Parser.GetFlights(flightHistoryRecord.Content);
                if (!commandFlights.Any(f => f.Airline == airline.Value && f.FlightNo == flightNo))
                {
                    commandFlights = FlightDatasCenter.GetFlights(departure.Value, arrival.Value, flightTime.Date, airline.Value,oemId);
                }
            }
            var flight = commandFlights.FirstOrDefault(f => f.Airline == airline.Value && f.FlightNo == flightNo);
            return FlightProcessor.GetFlight(flight, flightTime.Date);
        }

        private static IEnumerable<Flight> processFlights(FlightRecord flightRecord) {
            if(flightRecord == null || string.IsNullOrWhiteSpace(flightRecord.Content)) return new Flight[] { };
            var commandFlights = Command.Domain.Utility.Parser.GetFlights(flightRecord.Content);
            var flights = commandFlights.FilterShareFlight().FilterAirport(flightRecord.Departure, flightRecord.Arrival).FilterRepeatFlight();
            return FlightProcessor.ExecuteWithoutBunk(flights, flightRecord.FlightDate).FilterExpiredFlight().ToList();
        }
    }
}