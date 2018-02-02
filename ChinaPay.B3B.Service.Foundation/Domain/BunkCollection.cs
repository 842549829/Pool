using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.Core;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    internal class BunkCollection : RepositoryCache<Guid, Bunk> {
        private static BunkCollection _instance;
        private static object _locker = new object();
        public static BunkCollection Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new BunkCollection();
                        }
                    }
                }
                return _instance;
            }
        }

        private BunkCollection()
            : base(Repository.Factory.CreateBunkRepository(), 5 * 60 * 1000) {
                RefreshService.BunkChaned += Refresh;
        }
        public IEnumerable<GeneralBunk> QueryValidGeneralBunks(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate) {
            return QueryValidBunk(airline, departure, arrival, flightDate).OfType<GeneralBunk>().ToList();
        }
        public IEnumerable<GeneralBunk> QueryAllGeneralBunks(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate)
        {
            return Values.Where(item => (item.AirlineCode.IsNullOrEmpty() || airline.Value == item.AirlineCode.Value)
                                        && flightDate.Date >= item.FlightBeginDate.Date
                                        && (!item.FlightEndDate.HasValue || flightDate.Date <= item.FlightEndDate.Value.Date)
                                        && DateTime.Today >= item.ETDZDate.Date
                                        && containsDepartureAndArrival(item, departure, arrival)).OfType<GeneralBunk>().OrderByDescending(b=>b.Level).ThenByDescending(b=>b.FlightBeginDate).ThenByDescending(B=>B.ModifyTime).ToList();
        }
        private IEnumerable<Bunk> QueryValidBunk(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate) {
            return Values.Where(item => item.Valid
                                        && (item.AirlineCode.IsNullOrEmpty() || airline.Value == item.AirlineCode.Value)
                                        && flightDate.Date >= item.FlightBeginDate.Date
                                        && (!item.FlightEndDate.HasValue || flightDate.Date <= item.FlightEndDate.Value.Date)
                                        && DateTime.Today >= item.ETDZDate.Date
                                        && containsDepartureAndArrival(item, departure, arrival));
        }
        /// <summary>
        /// 查询舱位信息
        /// </summary>
        /// <param name="airline">航空公司</param>
        /// <param name="departure">出发</param>
        /// <param name="arrival">到达</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="code">舱位代码</param>
        public IEnumerable<Bunk> QueryBunk(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate, UpperString code) {
            // 获取到 航空公司、出发、到达、航班日期 和 舱位都匹配的所有舱位信息
            // 根据 级别、航班日期 和 修改时间 排序
            // 根据舱位类型分组，并取每组的第一个
            var bunks = (from bunk in QueryValidBunk(airline, departure, arrival, flightDate)
                         where containsBunk(bunk, code)
                         orderby bunk.Level descending, bunk.FlightBeginDate descending, bunk.ModifyTime descending
                         group bunk by bunk.Type into bunkGroup
                         select bunkGroup.First()).ToList();



            //更改EI项
            var pattern = new Regex("^[a-zA-Z\\d/]+$");
            var details = FoundationService.QueryDetailList(airline.Value, code.Value).Where(item => pattern.IsMatch(item.Bunks));
            string refundRegulation = string.Empty;
            string changeRegulation = string.Empty;
            string endorseRegulation = string.Empty;
            foreach (var item in details)
            {
                refundRegulation += ("航班起飞前：" + item.ScrapBefore + "；航班起飞后：" + item.ScrapAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                changeRegulation += ("航班起飞前：" + item.ChangeBefore + "；航班起飞后：" + item.ChangeAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                endorseRegulation += item.Endorse.Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
            }
            if (string.IsNullOrWhiteSpace(refundRegulation))
                refundRegulation = "以航司具体规定为准";
            if (string.IsNullOrWhiteSpace(changeRegulation))
                changeRegulation = "以航司具体规定为准";
            foreach (var item in bunks)
            {
                item.ChangeRegulation = changeRegulation;
                item.EndorseRegulation = endorseRegulation;
                item.RefundRegulation = refundRegulation;
            }


            var result = new List<Bunk>();
            if(bunks.Count > 0) {
                // 如果有多个明折明扣舱位，则只取一个
                var generalBunk = filterGeneralBunk(bunks);
                if(generalBunk != null) result.Add(generalBunk);
                var nonGeneralBunks = bunks.Where(item => !(item is GeneralBunk)).ToList();
                result.AddRange(nonGeneralBunks);
            }
            return result;
        }
        /// <summary>
        /// 查询舱位信息
        /// </summary>
        /// <param name="airline">航空公司</param>
        /// <param name="departure">出发</param>
        /// <param name="arrival">到达</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="code">舱位代码</param>
        /// <param name="itineraryType">行程类型</param>
        /// <param name="travelType">旅行类型</param>
        /// <param name="passengerType">旅客类型</param>
        public IEnumerable<Bunk> QueryBunk(UpperString airline, UpperString departure, UpperString arrival, DateTime flightDate, UpperString code,
            VoyageTypeValue itineraryType, TravelTypeValue travelType, PassengerTypeValue passengerType) {
            return QueryBunk(airline, departure, arrival, flightDate, code).Where(bunk => bunk != null
                                                                                          && (bunk.VoyageType & itineraryType) == itineraryType
                                                                                          && (bunk.PassengerType & passengerType) == passengerType
                                                                                          && (bunk.TravelType & travelType) == travelType).ToList();
            //var bunk = QueryBunk(airline, departure, arrival, flightDate, code);
            //if(bunk != null
            //    && (bunk.VoyageType & itineraryType) == itineraryType
            //    && (bunk.PassengerType & passengerType) == passengerType
            //    && (bunk.TravelType & travelType) == travelType) {
            //    return bunk;
            //}
            //return null;
        }
        Bunk filterGeneralBunk(IEnumerable<Bunk> bunks) {
            return (from b in bunks
                    where b is GeneralBunk
                    orderby b.Level descending, b.FlightBeginDate descending, b.ModifyTime descending
                    select b).FirstOrDefault();
        }
        bool containsDepartureAndArrival(Bunk bunk, UpperString departure, UpperString arrival) {
            var generalBunk = bunk as GeneralBunk;
            if(generalBunk == null) return true;
            if(isValidAirport(generalBunk.DepartureCode, departure) && isValidAirport(generalBunk.ArrivalCode, arrival)) return true;
            if(isValidAirport(generalBunk.ArrivalCode, departure) && isValidAirport(generalBunk.DepartureCode, arrival)) return true;
            return false;
        }
        bool isValidAirport(UpperString repositoryAirport, UpperString requestAirport) {
            return repositoryAirport.IsNullOrEmpty() || repositoryAirport.Value == requestAirport.Value;
        }
        bool containsBunk(Bunk bunk, UpperString bunkCode) {
            return bunk != null && bunk.Code.Value == bunkCode.Value || existsInExtendedBunk(bunk, bunkCode);
        }
        bool existsInExtendedBunk(Bunk bunk, UpperString bunkCode) {
            if(bunk is GeneralBunk) {
                var generalBunk = bunk as GeneralBunk;
                return generalBunk.Extended.Any(item => item.Code.Value == bunkCode.Value);
            } else if(bunk is PromotionBunk) {
                var promotionBunk = bunk as PromotionBunk;
                return promotionBunk.Extended.Any(item => item == bunkCode.Value);
            } else {
                return false;
            }
        }
    }
}