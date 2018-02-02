using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.AnnounceHandlers
{
    /// <summary>
    /// 各航空公司退废票规定
    /// </summary>
    public class AirlineDevilBackRule : BaseHandler
    {
        /// <summary>
        /// 查询所有的航空公司
        /// </summary>
        /// <returns></returns>
        public object GetAirLine() 
        {
            return from item in FoundationService.Airlines
                   where item.Valid
                   select new
                   {
                       Code = item.Code.Value,
                       Name = item.Name
                   };
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object BackProvisions(string code)
        {
            return from item in FoundationService.RefundAndReschedulings
                   where item.AirlineCode.Value == code && item.Airline.Valid
                   select new {
                        Name = item.Airline.ShortName,
                        //Refund = item.Refund,
                        Scrap = item.Scrap,
                        //Change = item.Change,
                        Remark = item.Remark
                   };
        }
    }
}