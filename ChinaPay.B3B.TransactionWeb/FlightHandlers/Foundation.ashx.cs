using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers {
    /// <summary>
    /// 预订流程的基础数据
    /// </summary>
    public class Foundation : ChinaPay.Infrastructure.WebEx.AjaxHandler.WebAjaxHandler {
        public object QueryAirlines() {
            return from item in Service.FoundationService.Airlines
                   orderby item.Code.Value
                   where item.Valid
                   select new
                   {
                       Code = item.Code.Value,
                       Name = item.ShortName
                   };
        }
        public object QueryAirports() {
            return from item in Service.FoundationService.Airports
                   orderby item.Location.HotLevel, item.IsMain descending
                   where item.Valid
                   select new
                   {
                       Code = item.Code.Value,
                       Name = item.ShortName,
                       City = item.Location.Name,
                       Spelling = item.Location.Spelling,
                       ShortSpelling = item.Location.ShortSpelling,
                       IsMain = item.IsMain,
                       IsHot = item.Location.HotLevel <= 20
                   };
        }
    }
}