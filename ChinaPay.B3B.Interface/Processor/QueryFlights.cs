using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Interface.Cache;

namespace ChinaPay.B3B.Interface.Processor
{
    class QueryFlights : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var departure = Context.GetParameterValue("departure");
            var arrival = Context.GetParameterValue("arrival");
            var flightDate = Context.GetParameterValue("flightDate");
            Vaild(departure, arrival, flightDate);
            DateTime fdate;
            if (DateTime.TryParse(flightDate, out fdate))
            {
                var vailTime = Service.SystemManagement.SystemParamService.FlightValidityMinutes.ToString();
                var originalFlights = Service.FlightQueryService.QueryOWFlights(departure, arrival, fdate, "", Guid.Empty);
                //departure = Service.FoundationService.QueryCityNameByAirportCode(departure);
                //arrival = Service.FoundationService.QueryCityNameByAirportCode(arrival);
                if (!originalFlights.Any()) InterfaceInvokeException.ThrowCustomMsgException(departure.ToUpper() + " 到 " + arrival.ToUpper() + " " + fdate.ToString("yyyy-MM-dd") + " 没有直达航班。");
                var matchedFlights = Service.PolicyMatch.PolicyMatchServcie.MatchOneWayFlights(originalFlights, Company.CompanyId, Employee.Id).ToList();


                StringBuilder str = new StringBuilder();
                //将匹配出来的航班信息存入缓存中 
                CustomContext context = CustomContext.NewContext();
                context[Employee.Id.ToString()] = matchedFlights;
                ContextCenter.Instance.Save(context);
                str.AppendFormat("<batchNo>{0}</batchNo><flights>", context.Id);

                foreach (var policy in matchedFlights)
                {
                    var item = policy.OriginalFlight;

                    str.AppendFormat("<airlineCode>{0}</airlineCode>", item.Airline);
                    str.AppendFormat("<airlineName>{0}</airlineName>", item.AirlineName);
                    str.AppendFormat("<flightNo>{0}</flightNo>", item.FlightNo);
                    str.AppendFormat("<aircraft>{0}</aircraft>", item.AirCraft);
                    str.AppendFormat("<hasFood>{0}</hasFood>", item.HasFood ? "1" : "0");//1：有餐食 0：无餐食
                    str.AppendFormat("<hasStop>{0}</hasStop>", item.IsStop ? "1" : "0");//1：有经停 0：无经停
                    str.AppendFormat("<departure><city>{0}</city><code>{1}</code><name>{2}</name><terminal>{3}</terminal></departure>", item.Departure.City, item.Departure.Code, item.Departure.Name, item.Departure.Terminal);
                    str.AppendFormat("<arrival><city>{0}</city><code>{1}</code><name>{2}</name><terminal>{3}</terminal></arrival>", item.Arrival.City, item.Arrival.Code, item.Arrival.Name, item.Arrival.Terminal);
                    str.AppendFormat("<takeoffTime>{0}</takeoffTime>", item.TakeoffTime.Hour + ":" + item.TakeoffTime.Minute);
                    str.AppendFormat("<landingTime>{0}</landingTime>", item.LandingTime.Hour + ":" + item.LandingTime.Minute);
                    str.AppendFormat("<daysInterval>{0}</daysInterval>", item.DaysInterval);
                    str.AppendFormat("<airportFee>{0}</airportFee>", item.AirportFee);
                    str.AppendFormat("<fuel>{0}</fuel>", item.BAF.Adult);
                    str.AppendFormat("<standardFare>{0}</standardFare>", item.StandardPrice);
                    str.AppendFormat("<amount>{0}</amount>", policy.LowestPrice);
                }
                str.Append("</flights>");
                return str.ToString();
            }
            else
            {
                InterfaceInvokeException.ThrowParameterErrorException("flightDate");
            }
            return "";
        }

        private static void Vaild(string departure, string arrival, string flightDate)
        {
            if (string.IsNullOrWhiteSpace(departure)) InterfaceInvokeException.ThrowParameterMissException("departure");
            if (string.IsNullOrWhiteSpace(arrival)) InterfaceInvokeException.ThrowParameterMissException("arrival");
            if (string.IsNullOrWhiteSpace(flightDate)) InterfaceInvokeException.ThrowParameterMissException("flightDate");
            if (!Regex.IsMatch(departure, "(\\w{3})")) InterfaceInvokeException.ThrowParameterErrorException("departure");
            if (!Regex.IsMatch(arrival, "(\\w{3})")) InterfaceInvokeException.ThrowParameterErrorException("arrival");
            if (!Regex.IsMatch(flightDate, "([0-9]{4}-[0-9]{2}-[0-9]{2})")) InterfaceInvokeException.ThrowParameterErrorException("flightDate");
        }
    }
}