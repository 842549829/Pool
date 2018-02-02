using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.SystemSetting.PolicyHarmony;
using ChinaPay.B3B.Service;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingHandlers
{
    /// <summary>
    /// SystemAirline 的摘要说明
    /// </summary>
    public class SystemAirline : BaseHandler {
        public object QueryAirlinesByAirlineCode(string code) {
            return from airline in FoundationService.RefundAndReschedulings.Where(item => item.AirlineCode.Value == code)
                   select new
                   {
                       ShortName = airline.Airline.ShortName,
                       Change = airline.Change,
                       Refund = airline.Refund,
                       Scrap = airline.Scrap,
                       AirlineTel = airline.AirlineTel,
                       Remark = airline.Remark
                   };
        }
        public object QueryAirlinesByAirlineCodeNew(string code)
        {
            var airline = FoundationService.QueryRefundAndReschedulingNewBase(code);
            var detail = new List<RefundAndReschedulingDetail> { 
                new RefundAndReschedulingDetail{
                     ChangeAfter = string.Empty,
                     ChangeBefore = string.Empty,
                     Endorse = string.Empty,
                     ScrapAfter = string.Empty,
                     ScrapBefore = string.Empty,
                     Bunks = string.Empty
                }
            };
            return new
            {
                ShortName = ReplaceName(airline.Airline.Name),
                Scrap = string.IsNullOrEmpty(airline.Scrap) ? string.Empty : airline.Scrap,
                AirlineTel = string.IsNullOrEmpty(airline.AirlineTel) ? string.Empty : airline.AirlineTel,
                Remark = string.IsNullOrEmpty(airline.Remark) ? string.Empty : airline.Remark,
                Condition = string.IsNullOrEmpty(airline.Condition) ? string.Empty : airline.Condition,
                RefundAndReschedulingDetail = from item in airline.RefundAndReschedulingDetail ?? detail
                                              select new
                                              {
                                                  ChangeAfter = string.IsNullOrEmpty(item.ChangeAfter) ? string.Empty : item.ChangeAfter,
                                                  ChangeBefore = string.IsNullOrEmpty(item.ChangeBefore) ? string.Empty : item.ChangeBefore,
                                                  Endorse = string.IsNullOrEmpty(item.Endorse) ? string.Empty : item.Endorse,
                                                  ScrapAfter = string.IsNullOrEmpty(item.ScrapAfter) ? string.Empty : item.ScrapAfter,
                                                  ScrapBefore = string.IsNullOrEmpty(item.ScrapBefore) ? string.Empty : item.ScrapBefore,
                                                  Bunks = string.IsNullOrEmpty(item.Bunks) ? string.Empty : item.Bunks,
                                              }
            };
        }
        private string ReplaceName(string name)
        {
            return name.Replace("中国", "").Replace("股份", "").Replace("有限", "").Replace("责任", "").Replace("公司", "");
        }
    }
}