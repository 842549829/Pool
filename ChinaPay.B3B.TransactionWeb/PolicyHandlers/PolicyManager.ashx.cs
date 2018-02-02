using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.SystemManagement;


namespace ChinaPay.B3B.TransactionWeb.PolicyHandlers
{
    /// <summary>
    /// PolicyManager 的摘要说明
    /// </summary>
    public class PolicyManager : BaseHandler
    {
        /// <summary>
        /// 政策设置管理修改中的获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QueryBunks(string airline, string startTime, string endTime)
        {
            var flightStartDate = DateTime.Parse(startTime).Date;
            var flightEndDate = DateTime.Parse(endTime).Date;
            var result = new Dictionary<string, decimal>();
            var bunks = (from item in FoundationService.Bunks
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= flightStartDate
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= flightEndDate)
                             && normalBunk.ETDZDate.Date <= flightStartDate
                         orderby normalBunk.Discount descending
                         select normalBunk).ToList();
            foreach (var bunk in bunks)
            {
                if (!result.ContainsKey(bunk.Code.Value))
                {
                    result.Add(bunk.Code.Value, bunk.Discount);
                }
                foreach (var extend in bunk.Extended)
                {
                    if (!result.ContainsKey(extend.Code.Value))
                    {
                        result.Add(extend.Code.Value, extend.Discount);
                    }
                }
            }
            return (from bunk in result
                    orderby bunk.Value descending
                    select bunk.Key).ToList();
        }
        /// <summary>
        /// 普通政策发布和修改获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QueryNormalBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyage, bool isOneWay)
        {
            if (isOneWay && voyage != VoyageTypeValue.OneWayOrRound)
            {
                var result = new Dictionary<string, decimal>();
                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & voyage) == voyage)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();
            }
            else if (isOneWay && voyage == VoyageTypeValue.OneWayOrRound)
            {
                var result = new Dictionary<string, decimal>();
                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                bunks = (from item in FoundationService.Bunks
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                             && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                             && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                         orderby normalBunk.Discount descending
                         select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();
            }
            else
            {
                var result = new Dictionary<string, decimal>();

                var bunks = (from item in FoundationService.Bunks
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                bunks = (from item in FoundationService.Bunks
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.RoundTrip) == VoyageTypeValue.RoundTrip)
                             && ((normalBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                             && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                         orderby normalBunk.Discount descending
                         select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                return (from bunk in result
                        orderby bunk.Value descending
                        select bunk.Key).ToList();
            }

        }
        /// <summary>
        /// 特价政策发布和修改获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QueryBargainBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyageType)
        {
            var bunkslist = FoundationService.Bunks;
            var result = new List<string>();
            if (voyageType == VoyageTypeValue.OneWay)
            {
                var bunks = (from item in bunkslist
                             let bargainBunk = item as PromotionBunk
                             where bargainBunk != null
                                 && (bargainBunk.Valid
                                 && bargainBunk.AirlineCode.Value == airline
                                 && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                 && ((bargainBunk.VoyageType & voyageType) == voyageType)
                                 && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             select bargainBunk);
                foreach (var item in bunks)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                    foreach (var extended in item.Extended)
                    {
                        if (!result.Contains(extended))
                        {
                            result.Add(extended);
                        }
                    }
                }
            }
            else if (voyageType == VoyageTypeValue.RoundTrip)
            {
                var bunks = (from item in bunkslist
                             let bargainBunk = item as PromotionBunk
                             where bargainBunk != null
                                 && (bargainBunk.Valid
                                 && bargainBunk.AirlineCode.Value == airline
                                 && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                 && ((bargainBunk.VoyageType & voyageType) == voyageType)
                                 && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             select bargainBunk);
                foreach (var item in bunks)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                    foreach (var extended in item.Extended)
                    {
                        if (!result.Contains(extended))
                        {
                            result.Add(extended);
                        }
                    }
                }
                var bunks1 = (from item in bunkslist
                              let bargainBunk = item as ProductionBunk
                              where bargainBunk != null
                                  && (bargainBunk.Valid
                                  && bargainBunk.AirlineCode.Value == airline
                                  && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                  && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                  && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                  && ((bargainBunk.VoyageType & voyageType) == voyageType)
                                  && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                  && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                              select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
            }
            else if (voyageType == VoyageTypeValue.TransitWay)
            {
                var bunks = (from item in bunkslist
                             let bargainBunk = item as PromotionBunk
                             where bargainBunk != null
                                 && (bargainBunk.Valid
                                 && bargainBunk.AirlineCode.Value == airline
                                 && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                 && ((bargainBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                 && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                 && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             select bargainBunk);
                foreach (var item in bunks)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                    foreach (var extended in item.Extended)
                    {
                        if (!result.Contains(extended))
                        {
                            result.Add(extended);
                        }
                    }
                }
                bunks = (from item in bunkslist
                         let bargainBunk = item as PromotionBunk
                         where bargainBunk != null
                             && (bargainBunk.Valid
                             && bargainBunk.AirlineCode.Value == airline
                             && bargainBunk.FlightBeginDate.Date <= startTime.Date
                             && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                             && ((bargainBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                             && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                             && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                         select bargainBunk);
                foreach (var item in bunks)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                    foreach (var extended in item.Extended)
                    {
                        if (!result.Contains(extended))
                        {
                            result.Add(extended);
                        }
                    }
                }
                var bunks1 = (from item in bunkslist
                              let bargainBunk = item as TransferBunk
                              where bargainBunk != null
                                  && (bargainBunk.Valid
                                  && bargainBunk.AirlineCode.Value == airline
                                  && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                  && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                  && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                  && ((bargainBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                  && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                  && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                              select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                bunks1 = (from item in bunkslist
                          let bargainBunk = item as TransferBunk
                          where bargainBunk != null
                              && (bargainBunk.Valid
                              && bargainBunk.AirlineCode.Value == airline
                              && bargainBunk.FlightBeginDate.Date <= startTime.Date
                              && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                              && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                              && ((bargainBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                              && ((bargainBunk.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                              && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                          select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
            }
            return (from bunk in result
                    orderby bunk
                    select bunk).ToList();
        }

        /// <summary>
        /// 特殊政策发布和修改获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QuerySpecialBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, SpecialProductType specialProductType)
        {
            var bunkslist = FoundationService.Bunks;
            var result = new List<string>();
            if (specialProductType == SpecialProductType.CostFree)
            {
                var list = (from item in bunkslist
                            where item.Valid
                                && item.AirlineCode.Value == airline
                                && item is FreeBunk
                                && item.FlightBeginDate.Date <= startTime.Date
                                && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                                && item.ETDZDate.Date <= startETDZDate.Date
                                && ((item.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay)
                                && ((item.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                            select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                return result;
            }
            if (specialProductType == SpecialProductType.Bloc)
            {
                var query = bunkslist;
                var list = (from item in query
                            where item.Valid
                                && item.AirlineCode.Value == airline
                                && item.FlightBeginDate.Date <= startTime.Date
                                && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                                && item.ETDZDate.Date <= startETDZDate.Date
                                && item is GeneralBunk
                                && ((item.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay)
                                && ((item.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                                && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                            select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                list = (from item in query
                        where item.Valid
                            && item.AirlineCode.Value == airline
                            && item.FlightBeginDate.Date <= startTime.Date
                            && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                            && item.ETDZDate.Date <= startETDZDate.Date
                            && item is PromotionBunk
                            && ((item.VoyageType & VoyageTypeValue.OneWay) == VoyageTypeValue.OneWay)
                            && ((item.TravelType & TravelTypeValue.Individual) == TravelTypeValue.Individual)
                            && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                        select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                return result;
            }
            return result;
        }

        /// <summary>
        /// 团队政策发布和修改获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QueryTeamBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyage)
        {
            var bunkslist = FoundationService.Bunks;
            if (voyage == VoyageTypeValue.TransitWay)
            {
                var result = new List<string>();

                var list = (from item in bunkslist
                            where item.Valid
                                && item.AirlineCode.Value == airline
                                && item.FlightBeginDate.Date <= startTime.Date
                                && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                                && item.ETDZDate.Date <= startETDZDate.Date
                                && item is TeamBunk
                                && ((item.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                && ((item.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                            select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                list = (from item in bunkslist
                        where item.Valid
                            && item.AirlineCode.Value == airline
                            && item.FlightBeginDate.Date <= startTime.Date
                            && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                            && item.ETDZDate.Date <= startETDZDate.Date
                            && item is TeamBunk
                            && ((item.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                            && ((item.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                            && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                        select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                return result;
            }
            else
            {
                var result = new List<string>();

                var list = (from item in bunkslist
                            where item.Valid
                                && item.AirlineCode.Value == airline
                                && item.FlightBeginDate.Date <= startTime.Date
                                && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                                && item.ETDZDate.Date <= startETDZDate.Date
                                && item is TeamBunk
                                && ((item.VoyageType & voyage) == voyage)
                                && ((item.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                && ((item.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                            select item);
                foreach (var item in list)
                {
                    if (!result.Contains(item.Code.Value))
                    {
                        result.Add(item.Code.Value);
                    }
                }
                return result;
            }

        }
        /// <summary>
        /// 查询团队政策发布修改的普通舱位
        /// </summary>
        /// <returns></returns>
        public object QueryTeamNormalBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate, VoyageTypeValue voyage)
        {
            var bunkslist = FoundationService.Bunks;
            if (voyage != VoyageTypeValue.OneWayOrRound)
            {
                var result = new Dictionary<string, decimal>();
                var result1 = new List<string>();
                var bunks = (from item in bunkslist
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & voyage) == voyage)
                                 && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                if (voyage == VoyageTypeValue.OneWay)
                {
                    var bunks1 = (from item in bunkslist
                                  let bargainBunk = item as PromotionBunk
                                  where bargainBunk != null
                                      && (bargainBunk.Valid
                                      && bargainBunk.AirlineCode.Value == airline
                                      && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                      && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                      && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                      && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                      && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                                  select bargainBunk);
                    foreach (var item in bunks1)
                    {
                        if (!result1.Contains(item.Code.Value))
                        {
                            result1.Add(item.Code.Value);
                        }
                        foreach (var extended in item.Extended)
                        {
                            if (!result1.Contains(extended))
                            {
                                result1.Add(extended);
                            }
                        }
                    }
                }
                else if (voyage == VoyageTypeValue.RoundTrip)
                {
                    var bunks1 = (from item in bunkslist
                                  let bargainBunk = item as ProductionBunk
                                  where bargainBunk != null
                                      && (bargainBunk.Valid
                                      && bargainBunk.AirlineCode.Value == airline
                                      && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                      && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                      && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                      && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                      && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                                  select bargainBunk);
                    foreach (var item in bunks1)
                    {
                        if (!result1.Contains(item.Code.Value))
                        {
                            result1.Add(item.Code.Value);
                        }
                    }
                }
                var result2 = (from bunk in result
                               orderby bunk.Value descending
                               select bunk.Key).ToList();
                foreach (var item in result1)
                {
                    if (!result2.Contains(item))
                    {
                        result2.Add(item);
                    }
                }
                return result2;
            }
            else
            {
                var result = new Dictionary<string, decimal>();
                var result1 = new List<string>();
                var bunks = (from item in bunkslist
                             let normalBunk = item as GeneralBunk
                             where normalBunk != null
                                 && normalBunk.Valid
                                 && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                                 && normalBunk.FlightBeginDate.Date <= startTime.Date
                                 && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                                 && normalBunk.ETDZDate.Date <= startETDZDate.Date
                                 && ((normalBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                 && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                 && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                             orderby normalBunk.Discount descending
                             select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                bunks = (from item in bunkslist
                         let normalBunk = item as GeneralBunk
                         where normalBunk != null
                             && normalBunk.Valid
                             && (normalBunk.AirlineCode.IsNullOrEmpty() || normalBunk.AirlineCode.Value == airline)
                             && normalBunk.FlightBeginDate.Date <= startTime.Date
                             && (!normalBunk.FlightEndDate.HasValue || normalBunk.FlightEndDate.Value.Date >= endTime.Date)
                             && normalBunk.ETDZDate.Date <= startETDZDate.Date
                             && ((normalBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                             && ((normalBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                             && ((normalBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                         orderby normalBunk.Discount descending
                         select normalBunk).ToList();
                foreach (var bunk in bunks)
                {
                    if (!result.ContainsKey(bunk.Code.Value))
                    {
                        result.Add(bunk.Code.Value, bunk.Discount);
                    }
                    foreach (var extend in bunk.Extended)
                    {
                        if (!result.ContainsKey(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value, extend.Discount);
                        }
                    }
                }
                var bunks1 = (from item in bunkslist
                              let bargainBunk = item as TransferBunk
                              where bargainBunk != null
                                  && (bargainBunk.Valid
                                  && bargainBunk.AirlineCode.Value == airline
                                  && bargainBunk.FlightBeginDate.Date <= startTime.Date
                                  && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                                  && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                                  && ((bargainBunk.VoyageType & VoyageTypeValue.TransitWay) == VoyageTypeValue.TransitWay)
                                  && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                                  && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                              select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result1.Contains(item.Code.Value))
                    {
                        result1.Add(item.Code.Value);
                    }
                }
                bunks1 = (from item in bunkslist
                          let bargainBunk = item as TransferBunk
                          where bargainBunk != null
                              && (bargainBunk.Valid
                              && bargainBunk.AirlineCode.Value == airline
                              && bargainBunk.FlightBeginDate.Date <= startTime.Date
                              && (!bargainBunk.FlightEndDate.HasValue || bargainBunk.FlightEndDate.Value.Date >= endTime.Date)
                              && bargainBunk.ETDZDate.Date <= startETDZDate.Date)
                              && ((bargainBunk.VoyageType & VoyageTypeValue.OneWayOrRound) == VoyageTypeValue.OneWayOrRound)
                              && ((bargainBunk.TravelType & TravelTypeValue.Team) == TravelTypeValue.Team)
                              && ((bargainBunk.PassengerType & PassengerTypeValue.Adult) == PassengerTypeValue.Adult)
                          select bargainBunk);
                foreach (var item in bunks1)
                {
                    if (!result1.Contains(item.Code.Value))
                    {
                        result1.Add(item.Code.Value);
                    }
                }

                var result2 = (from bunk in result
                               orderby bunk.Value descending
                               select bunk.Key).ToList();

                foreach (var item in result1)
                {
                    if (!result2.Contains(item))
                    {
                        result2.Add(item);
                    }
                }
                return result2;
            }
        }
        /// <summary>
        /// 查询特殊政策类型
        /// </summary>
        /// <returns></returns>
        public object QuerySpecialProductTypeList()
        {
            var companyParamer = CompanyService.GetCompanyParameter(CurrentCompany.CompanyId) ?? new CompanyParameter { CostFree = false, Bloc = false, Business = false, Disperse = false, Singleness = false, LowToHigh = false };
            return from item in SpecialProductService.Query()
                   where item.Enabled && ((companyParamer.Singleness && item.SpecialProductType == SpecialProductType.Singleness)
                                       || (companyParamer.CostFree && item.SpecialProductType == SpecialProductType.CostFree)
                                       || (companyParamer.Bloc && item.SpecialProductType == SpecialProductType.Bloc)
                                       || (companyParamer.Business && item.SpecialProductType == SpecialProductType.Business)
                                       || (companyParamer.Disperse && item.SpecialProductType == SpecialProductType.Disperse)
                                       || (companyParamer.OtherSpecial && item.SpecialProductType == SpecialProductType.OtherSpecial)
                                       || (companyParamer.LowToHigh && item.SpecialProductType == SpecialProductType.LowToHigh))
                   select new
                   {
                       item.Name,
                       Value = item.SpecialProductType,
                       item.Description
                   };

        }
        /// <summary>
        /// 查看公司是否可以发布内部返点
        /// </summary>
        /// <returns></returns>
        public bool CanHaveSubordinate()
        {
            var company = CompanyService.GetCompanySettingsInfo(CurrentCompany.CompanyId);
            return company != null && company.Parameter.CanHaveSubordinate;
        }
        /// <summary>
        /// 查看公司是否可以发布同行返点
        /// </summary>
        /// <returns></returns>
        public bool AllowBrotherPurchase()
        {
            var company = CompanyService.GetCompanySettingsInfo(CurrentCompany.CompanyId);
            return company != null && company.Parameter.AllowBrotherPurchase;
        }
        /// <summary>
        /// 缺口政策发布和修改获取舱位查询
        /// </summary>
        /// <returns></returns>
        public object QueryNotchBunksPolicy(string airline, DateTime startTime, DateTime endTime, DateTime startETDZDate )
        {

            var result = new List<string>();

            var bunks = (from item in FoundationService.Bunks 
                         where  item.Valid
                             && (item.AirlineCode.IsNullOrEmpty() || item.AirlineCode.Value == airline)
                             && item.FlightBeginDate.Date <= startTime.Date
                             && (!item.FlightEndDate.HasValue || item.FlightEndDate.Value.Date >= endTime.Date)
                             && item.ETDZDate.Date <= startETDZDate.Date
                             && ((item.VoyageType & VoyageTypeValue.Notch) == VoyageTypeValue.Notch)
                         select item).ToList();
            foreach (var bunk in bunks)
            {
                if (!result.Contains(bunk.Code.Value))
                {
                    result.Add(bunk.Code.Value );
                }
                if (bunk is GeneralBunk)
                {
                    var nor = bunk as GeneralBunk;
                    foreach (var extend in nor.Extended)
                    {
                        if (!result.Contains(extend.Code.Value))
                        {
                            result.Add(extend.Code.Value);
                        }
                    }
                }
                else if (bunk is PromotionBunk)
                {
                    var nor = bunk as PromotionBunk;
                    foreach (var extend in nor.Extended)
                    {
                        if (!result.Contains(extend ))
                        {
                            result.Add(extend );
                        }
                    }
                }
            }
            return result;
        }



        public CompanySettingsInfo GetCompanySetting()
        {
            return CompanyService.GetCompanySettingsInfo(CurrentCompany.CompanyId) ?? new CompanySettingsInfo();
        }
    }
}