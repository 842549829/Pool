using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Command;
using ChinaPay.Core;
using System.Text;

namespace ChinaPay.B3B.Interface.Processor
{
    class QueryFlightStop : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var airlineCode = Context.GetParameterValue("airlineCode");
            var flightNo = Context.GetParameterValue("flightNo");
            var flightDate = Context.GetParameterValue("flightDate");
            //验证
            Vaild(airlineCode, flightNo, flightDate);

            var AVHInfo = CommandService.GetTransitPoints(airlineCode + flightNo, DateTime.Parse(flightDate), Guid.Empty);
            if (AVHInfo.Success && AVHInfo.Result.First() != null)
            {
                var transitPoint = AVHInfo.Result.First();
                var arriveTime = ParseTime(flightDate, transitPoint.ArrivalTime, transitPoint.ArrivalAddDays);
                var departureTime = ParseTime(flightDate, transitPoint.DepartureTime, transitPoint.DepartureAddDays);
                if (arriveTime == DateTime.MinValue || departureTime == DateTime.MinValue) return "";
                StringBuilder str = new StringBuilder();

                str.Append("<transits>");
                str.AppendFormat("<city>{0}</city>", Service.FoundationService.QueryCityNameByAirportCode(transitPoint.AirportCode));
                str.AppendFormat("<code>{0}</code>", transitPoint.AirportCode);
                str.AppendFormat("<name>{0}</name>", Service.FoundationService.QueryAirportName(transitPoint.AirportCode));
                str.AppendFormat("<landingTime>{0}</landingTime>", transitPoint.ArrivalTime.Hour + ":" + transitPoint.ArrivalTime.Minute);
                str.AppendFormat("<takeoffTime>{0}</takeoffTime>", transitPoint.DepartureTime.Hour + ":" + transitPoint.DepartureTime.Minute);
                str.AppendFormat("<landingInterval>{0}</landingInterval>", transitPoint.ArrivalAddDays);
                str.AppendFormat("<takeoffInterval>{0}</takeoffInterval>", transitPoint.DepartureAddDays);
                str.Append("</transits>");
                return str.ToString();
            }
            else
            {
                InterfaceInvokeException.ThrowCustomMsgException("没有查询到经停信息");
            } return "";
        }
        private DateTime ParseTime(string flightDate, Time time, int addDays)
        {
            return DateTime.Parse(flightDate).AddHours(time.Hour).AddMinutes(time.Minute).AddDays(addDays);
        }
        private static void Vaild(string airlineCode, string flightNo, string flightDate)
        {
            if (string.IsNullOrWhiteSpace(airlineCode)) InterfaceInvokeException.ThrowParameterMissException("airlineCode");
            if (string.IsNullOrWhiteSpace(flightNo)) InterfaceInvokeException.ThrowParameterMissException("flightNo");
            if (string.IsNullOrWhiteSpace(flightDate)) InterfaceInvokeException.ThrowParameterMissException("flightDate");
            if (!Regex.IsMatch(airlineCode, "(\\w{2})")) InterfaceInvokeException.ThrowParameterErrorException("airlineCode");
            if (flightNo.Length >= 6) InterfaceInvokeException.ThrowParameterErrorException("flightNo");
            if (!Regex.IsMatch(flightDate, "([0-9]{4}-[0-9]{2}-[0-9]{2})")) InterfaceInvokeException.ThrowParameterErrorException("flightDate");
        }
    }
}