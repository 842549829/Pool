
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    using System;
    using System.Text.RegularExpressions;
    using Common.Enums;
    using System.Linq;
    using System.Collections.Generic;
    using DataTransferObject.Organization;
    using DataTransferObject.Policy;
    using Izual;
    using Account = Organization.Domain.Account;

    internal static class PolicyFilter {
        private static readonly IEnumerable<PolicyInfoBase> empty = EnumerableHelper.GetEmpty<PolicyInfoBase>();

        internal static IEnumerable<PolicyInfoBase> FilterById(this IEnumerable<PolicyInfoBase> policies, Guid id) {
            return id == Guid.Empty ? policies : policies.Where(p => p.Id == id);
        }
        
        /// <summary>
        /// 根据政策提供者过滤类型，过滤政策，可选择包含该提供者政策或是不包含
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="providerFilterType">政策提供者过滤类型</param>
        /// <param name="providers">政策提供者ID</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByProvider(this IEnumerable<PolicyInfoBase> policies, ProviderFilterType providerFilterType, params Guid[] providers) {
            if(providers == null || !providers.Any()) return policies;
            return policies.Where(p => providerFilterType == ProviderFilterType.Include ? providers.Contains(p.Owner) : !providers.Contains(p.Owner));
        }

        /// <summary>
        /// 根据行程，过滤政策，取得的是政策列表中与行程相符的政策；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="voyageType">行程类型</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByVoyageType(this IEnumerable<PolicyInfoBase> policies, VoyageType voyageType) {
            switch(voyageType) {
                case VoyageType.OneWay:
                case VoyageType.RoundTrip:
                    return policies.Where(p => p.VoyageType == voyageType || p.VoyageType == VoyageType.OneWayOrRound);
                case VoyageType.OneWayOrRound:
                case VoyageType.TransitWay:
                case VoyageType.Notch:
                    return policies.Where(p => p.VoyageType == voyageType);
                default:
                    return empty;
            }
        }

        /// <summary>
        /// 根据政策类型，过滤政策
        /// </summary>
        internal static IEnumerable<PolicyInfoBase> FilterByPolicyType(this IEnumerable<PolicyInfoBase> policies, PolicyType policyType) {
            return policyType == PolicyType.Unknown ? policies : policies.Where(p => (p.PolicyType & policyType) == p.PolicyType);
        }

        /// <summary>
        /// 判断机场编码是否在出港地字串中；
        /// </summary>
        /// <param name="airport">机场编码</param>
        /// <param name="filter">出港地字串（多个）</param>
        /// <param name="separator">分隔符</param>
        /// <returns>是否存在</returns>
        private static bool MatchAirport(string airport, string filter, string separator) {
            if(string.IsNullOrWhiteSpace(airport) || string.IsNullOrWhiteSpace(filter)) return false;

            var deps = filter.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return deps.Contains(airport);
        }

        /// <summary>
        /// 通过出港地过滤政策，取得的是和当前程数和出港地匹配的政策列表；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="voyageIndex">程数</param>
        /// <param name="departure">出港地（单个）</param>
        /// <param name="separator">出港地字符串分隔符</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByDeparture(this IEnumerable<PolicyInfoBase> policies, int voyageIndex, string departure, string separator = "/") {
            if(policies == null) throw new ArgumentNullException("policies");
            return string.IsNullOrWhiteSpace(departure)
                       ? empty
                       : policies.Where(p => MatchAirport(departure, p.GetDeparture(voyageIndex), separator));
        }

        /// <summary>
        /// 通过到港地过滤政策，取得的是和当前程数和到港地匹配的政策列表；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="voyageIndex">程数</param>
        /// <param name="arrival">到港地（单个）</param>
        /// <param name="separator">到港地字符串分隔符</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByArrival(this IEnumerable<PolicyInfoBase> policies, int voyageIndex, string arrival, string separator = "/") {
            if(policies == null) throw new ArgumentNullException("policies");
            return string.IsNullOrWhiteSpace(arrival)
                       ? empty
                       : policies.Where(p => MatchAirport(arrival, p.GetArrival(voyageIndex), separator));
        }

        internal static IEnumerable<PolicyInfoBase> FilterByAirline(this IEnumerable<PolicyInfoBase> policies, string airline) {
            if(policies == null) throw new ArgumentNullException("policies");
            return string.IsNullOrWhiteSpace(airline) ? empty : policies.Where(p => p.Airline == airline);
        }

        private static bool MatchFlightNumber(string flightNumber, string filter, string separator, LimitType limitType) {
            if(string.IsNullOrWhiteSpace(filter) || limitType == LimitType.None) return true;
            if(filter.Trim() == "*") return limitType != LimitType.Exclude;

            var flightNoPatters =(from pattern in filter.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries)
                                  where Regex.IsMatch(pattern.Trim(), "^[0-9]+\\*?$")
                                  select pattern.Trim().Replace("*", ".*?")).ToList();
            if(limitType == LimitType.Include) {
                return flightNoPatters.Any(flightPattern => Regex.IsMatch(flightNumber, "^" + flightPattern + "$", RegexOptions.IgnoreCase));
            } else if(limitType == LimitType.Exclude) {
                return !flightNoPatters.Any(flightPattern => Regex.IsMatch(flightNumber, "^" + flightPattern + "$", RegexOptions.IgnoreCase));
            } else {
                return true;
            }
        }

        internal static IEnumerable<PolicyInfoBase> FilterByFlightNumber(this IEnumerable<PolicyInfoBase> policies, int voyageIndex, string flightNumber, string separator = "/") {
            return policies.Where(p => MatchFlightNumber(flightNumber, p.GetFlightNumberFilter(voyageIndex), separator, p.GetFlightNumberFilterType(voyageIndex)));
        }

        /// <summary>
        /// 判断在从起始日期起，到结束日期止的时间段内，在排除掉以分隔符分隔的排除日期字串转换的日期后，起飞日期是否包含在内，即有效；
        /// </summary>
        /// <param name="flightDate">航行日期</param>
        /// <param name="start">起始日期</param>
        /// <param name="end">结束日期</param>
        /// <param name="filter">排除日期字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns>是否包含</returns>
        private static bool MatchFlightDate(DateTime flightDate, DateTime start, DateTime end, string filter, string separator) {
            var excludeDates = DateResolver.GetDates(start, end, filter, separator);
            return !excludeDates.Contains(flightDate);
        }
        /// <summary>
        /// 匹配班期
        /// </summary>
        private static bool MatchFlightWeek(DateTime flightDate, string weekFilter, string separator) {
            var includeWeeks = DateResolver.GetWeeks(weekFilter, separator);
            // 2012-11-02 deng.zhao修改 
            // return !includeWeeks.Any() || includeWeeks.Contains(flightDate.DayOfWeek);
            return includeWeeks.Contains(flightDate.DayOfWeek);
        }

        /// <summary>
        /// 根据航班日期进行过滤
        /// </summary>
        /// <remarks>
        /// 1、政策上去程航班日期
        /// 2、排除航班日期
        /// 3、适用班期
        /// </remarks>
        internal static IEnumerable<PolicyInfoBase> FilterByFlightDate(this IEnumerable<PolicyInfoBase> policies, DateTime flightDate, string separator = ",") {
            return policies.Where(p => p.DepartureDateStart.Date <= flightDate.Date && flightDate.Date <= p.DepartureDateEnd.Date   // 过滤航班日期
                 && MatchFlightDate(flightDate.Date, p.DepartureDateStart, p.DepartureDateEnd, p.DepartureDateFilter, separator) // 排除不适用航班日期
                 && MatchFlightWeek(flightDate, p.DepartureWeekFilter, separator)); // 过滤适用班期
        }

        /// <summary>
        /// 根据给出的起飞机场、到达机场、排除航线字串，以及此字串的分隔符，
        /// 判断对于此政策是否保留。
        /// </summary>
        /// <param name="departure">出发机场</param>
        /// <param name="arrival">到达机场</param>
        /// <param name="filter">以分隔符分隔的多条排除航线</param>
        /// <param name="separator">排除航线分隔符</param>
        /// <returns>是否保留</returns>
        private static bool MatchAirway(string departure, string arrival, string filter, string separator) {
            if(string.IsNullOrWhiteSpace(departure)) return false;
            if(string.IsNullOrWhiteSpace(arrival)) return false;
            if(string.IsNullOrWhiteSpace(filter)) return true;

            var airportPair = new AirportPair(departure, arrival);
            var list = new List<AirportPair> { airportPair };

            return MatchAirway(list, filter, separator);
        }

        /// <summary>
        /// 根据给出的起飞机场、中转机场、到达机场、排除航线字串，以及此字串的分隔符，
        /// 判断对于此政策是否保留。
        /// </summary>
        /// <param name="departure">出发机场</param>
        /// <param name="transit">中转机场</param>
        /// <param name="arrival">到达机场</param>
        /// <param name="filter">以分隔符分隔的多条排除航线</param>
        /// <param name="separator">排除航线分隔符</param>
        /// <returns>是否保留</returns>
        private static bool MatchAirway(string departure, string transit, string arrival, string filter, string separator) {
            if(string.IsNullOrWhiteSpace(departure)) return false;
            if(string.IsNullOrWhiteSpace(transit)) return false;
            if(string.IsNullOrWhiteSpace(arrival)) return false;
            if(string.IsNullOrWhiteSpace(filter)) return true;

            var first = new AirportPair(departure, transit);
            var second = new AirportPair(transit, arrival);
            var list = new List<AirportPair> { first, second };

            return MatchAirway(list, filter, separator);
        }

        /// <summary>
        /// 根据给出旅客行程的机场对列表、与排除航线字串，以及此字串的分隔符对比，判断对于此政策是否保留。
        /// </summary>
        /// <param name="airportPairs">机场对列表</param>
        /// <param name="excludeAirportsString">以分隔符分隔的多条排除航线</param>
        /// <param name="separator">排除航线分隔符</param>
        /// <returns>是否保留</returns>
        /// <remarks>
        /// 2013-01-28 deng.zhao
        /// 请保持传入的机场对列表与旅客的行程一致，内部将不处理其顺序，缺口程将被搭桥为连续行程；
        /// 排除航线的格式为：
        ///     若需要排除单个空港（如昆明长水）：格式为：KMG
        ///     如需要排除其中的单个航段（如昆明到重庆），格式为：KMGCKG
        ///     如需要排除其中的连续航段（如昆明到重庆到上海），格式为：KMGCKGSHA
        /// </remarks>
        private static bool MatchAirway(List<AirportPair> airportPairs, string excludeAirportsString, string separator)
        {
            // 参数判断；
            if (airportPairs == null) return false;
            if (string.IsNullOrWhiteSpace(excludeAirportsString)) return true;

            // 获取行程的字符串表达形式；
            var itineraryString = GetItineraryString(airportPairs);

            // 获取所有的排除航线设置列表，以字符串数组形式存储；
            string[] excludeAirports = excludeAirportsString.Split(new[] {separator},
                                                                   StringSplitOptions.RemoveEmptyEntries);

            return excludeAirports.All(excludeAirport => !itineraryString.ToUpper().Contains(excludeAirport.ToUpper()));
        }

        /// <summary>
        /// 根据给出旅客行程的机场对列表，获取行程的字符串表达形式。
        /// </summary>
        /// <param name="airportPairs">行程的机场对列表</param>
        /// <returns>行程的字符串表达</returns>
        private static string GetItineraryString(List<AirportPair> airportPairs)
        {
            var itineraryString = airportPairs[0].Departure;

            for (int i = 0; i < airportPairs.Count; i++)
            {
                if (i > 0 && airportPairs[i].Departure != airportPairs[i - 1].Arrival)
                {
                    itineraryString += airportPairs[i].Departure;
                }
                itineraryString += airportPairs[i].Arrival;
            }

            return itineraryString;
        }
        
        /// <summary>
        /// 处理单程的排除航线
        /// </summary>
        /// <param name="policies">待过滤政策</param>
        /// <param name="departure">出发机场</param>
        /// <param name="arrival">到达机场</param>
        /// <returns>过滤后政策</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByAirway(this IEnumerable<PolicyInfoBase> policies,
                                                                   string departure, string arrival)
        {
            return
                policies.Where(
                    p =>
                    MatchAirway(departure, arrival, string.IsNullOrEmpty(p.ExceptAirways) ? "" : p.ExceptAirways, "/"));
        }

        /// <summary>
        /// 处理中转联程排除航线（没有被调用了）
        /// </summary>
        internal static IEnumerable<PolicyInfoBase> FilterByAirway(this IEnumerable<PolicyInfoBase> policies,
                                                                   string departure, string transit, string arrival)
        {
            return
                policies.Where(
                    p =>
                    MatchAirway(departure, transit, arrival,
                                string.IsNullOrEmpty(p.ExceptAirways) ? "" : p.ExceptAirways, "/"));
        }

        /// <summary>
        /// 处理排除航线
        /// </summary>
        /// <param name="policies">待过滤政策</param>
        /// <param name="airportPairs">机场对</param>
        /// <returns>过滤后政策</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByAirway(this IEnumerable<PolicyInfoBase> policies, List<AirportPair> airportPairs)
        {
            return policies.Where(p => MatchAirway(airportPairs, string.IsNullOrEmpty(p.ExceptAirways) ? "" : p.ExceptAirways, "/"));
        }

        internal static bool MatchBunk(string bunkCode, string filter, string separator)
        {
            if(string.IsNullOrWhiteSpace(bunkCode) || string.IsNullOrWhiteSpace(filter)) return false;

            var berths = filter.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return berths.Contains(bunkCode);
        }

        internal static bool MatchBunk(PolicyInfoBase policy, string bunkCode, string separator = ",")
        {
            if(policy is SpecialPolicyInfo) {
                var specialPolicy = policy as SpecialPolicyInfo;
                switch(specialPolicy.Type) {
                    case SpecialProductType.Singleness:
                    case SpecialProductType.Disperse:
                        return true;
                    case SpecialProductType.CostFree:
                        return !specialPolicy.SynBlackScreen || MatchBunk(bunkCode, specialPolicy.Berths, separator);
                    case SpecialProductType.Bloc:
                    // 2012-11-20 新增
                    case SpecialProductType.OtherSpecial:
                    case SpecialProductType.Business:
                    //2013-03-01 Xie.
                    case SpecialProductType.LowToHigh:
                        return MatchBunk(bunkCode, specialPolicy.Berths, separator);
                    default:
                        throw new NotSupportedException();
                }
            } else {
                return MatchBunk(bunkCode, policy.Berths, separator);
            }
        }

        /// <summary>
        /// 通过提前天数过滤政策，取得的是符合提前天数规定的政策。若提前天数为3，则如果订票日期和航班日期的差小于此值，则不允许，
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="flightDate">航班日期</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByBeforehandDays(this IEnumerable<PolicyInfoBase> policies, DateTime flightDate) {
            // 最小提前天数，如果不限制，默认值为0，不需变更逻辑；最大提前天数，如果不限制，则默认值为-1，下面需做判断；
            return from p in policies
                   let hp = p as IHasBeforehandDays
                    where hp == null || 
                    (
                        (flightDate.Date - DateTime.Today).TotalDays >= hp.BeforehandDays &&
                        ((flightDate.Date - DateTime.Today).TotalDays <= hp.MaxBeforehandDays || hp.MaxBeforehandDays == -1)
                    )
                   select p;
        }

        /// <summary>
        /// 根据去程和回程时间，取得政策列表中出行天数与之相符的政策列表。
        /// </summary>
        /// <param name="policies">待过滤政策列表</param>
        /// <param name="flightDate1">去程时间</param>
        /// <param name="flightDate2">回程时间</param>
        /// <returns>过滤后的政策列表</returns>
        /// <remarks>
        ///  目前只有特价中的往返才有出行天数
        /// </remarks>
        internal static IEnumerable<PolicyInfoBase> FilterByTravelDays(this IEnumerable<PolicyInfoBase> policies, DateTime flightDate1, DateTime flightDate2) {
            if(policies == null) throw new ArgumentNullException("policies");
            return from p in policies
                   let bp = p as BargainPolicyInfo
                   where bp == null || bp.VoyageType != VoyageType.RoundTrip || flightDate2.Date >= flightDate1.Date.AddDays(bp.TravelDays)
                   select p;
        }

        /// <summary>
        /// 通过出票时间过滤政策，取得的是今天可以出票的政策；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByPrintDate(this IEnumerable<PolicyInfoBase> policies) {
            if(policies == null) throw new ArgumentNullException("policies");
            return policies.Where(policy => policy != null && policy.StartProcessDate.Date <= DateTime.Today);
        }

        internal static IEnumerable<PolicyInfoBase> FilterByReduce(this IEnumerable<PolicyInfoBase> policies, bool hasReduce) {
            if(!hasReduce) return policies;
            return policies.Where(p => {
                if(p is NormalPolicyInfo) {
                    return (p as NormalPolicyInfo).SuitReduce;
                } else if(p is TeamPolicyInfo) {
                    return (p as TeamPolicyInfo).SuitReduce;
                }
                return true;
            });
        }

        /// <summary>
        /// 根据处于工作时间内的发布者列表，过滤政策，取得的是政策的发布者处于工作时间的政策；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="workingPublishers">处于工作时间内的发布者列表</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByWorkingHours(this IEnumerable<PolicyInfoBase> policies, Dictionary<Guid, WorkingHoursView> workingPublishers) {
            policies = policies.Where(p => workingPublishers.Keys.Contains(p.Owner)).ToList();
            policies.ForEach(p => {
                var wp = workingPublishers[p.Owner];
                p.WorkStart = wp.WorkStart;
                p.WorkEnd = wp.WorkEnd;
                p.RefundStart = wp.RefundStart;
                p.RefundEnd = wp.RefundEnd;
            });
            return policies;
        }

        internal static Dictionary<Guid, WorkingHoursView> FilterWorkingPublishers(this IEnumerable<Data.DataMapping.WorkingHours> workingHours) {
            var now = DateTime.Now;
            var week = now.DayOfWeek;
            var time = (Time)now;
            var rest = week == DayOfWeek.Saturday || week == DayOfWeek.Sunday;
            return (from wh in workingHours
                    let workStart = rest ? wh.RestdayWorkStart : wh.WorkdayWorkStart
                    let workEnd = rest ? wh.RestdayWorkEnd : wh.WorkdayWorkEnd
                    where time >= workStart && time <= workEnd
                    select new WorkingHoursView() {
                        Company = wh.Company,
                        WorkStart = workStart,
                        WorkEnd = workEnd,
                        RefundStart = rest ? wh.RestdayRefundStart : wh.WorkdayRefundStart,
                        RefundEnd = rest ? wh.RestdayRefundEnd : wh.WorkdayRefundEnd
                    }).ToDictionary(o => o.Company);
        }

        /// <summary>
        /// 根据可用账户列表，过滤政策，取得的是政策的发布者有收款帐号的政策；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="validAccounts">可用账户列表</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByAccount(this IEnumerable<PolicyInfoBase> policies, Dictionary<Guid, Account> validAccounts) {
            return policies.Where(p => validAccounts.ContainsKey(p.Owner));
        }

        /// <summary>
        /// 根据价格过滤政策
        /// </summary>
        /// <remarks>
        /// 1、特殊政策价格不能高于标准价
        /// 2、特价政策折算下来的折扣不能超过标准价的10倍
        /// </remarks>
        internal static IEnumerable<PolicyInfoBase> FilterByPrice(this IEnumerable<PolicyInfoBase> policies, decimal standardPrice) {
            return policies.Where(p => {
                // 暂时不处理特殊政策的价格，并且要处理，也是按结算价处理
                //var specialPolicy = p as SpecialPolicyInfo;
                //if(specialPolicy != null) {
                //    return specialPolicy.Price < standardPrice;
                //}

                var bargainPolicy = p as BargainPolicyInfo;
                if(bargainPolicy != null) {
                    var discount = bargainPolicy.Price;
                    if(bargainPolicy.PriceType == PriceType.Price) discount = bargainPolicy.Price / standardPrice;
                    return discount < (bargainPolicy.VoyageType == VoyageType.RoundTrip ? 6 : 3);
                }
                return true;
            });
        }

        /// <summary>
        /// 根据舱位的相关性，过滤政策
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="hasContact">是否相关（是否有舱位信息比较好理解）</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByRelationWithBunk(this IEnumerable<PolicyInfoBase> policies, bool hasContact) {
            return policies.Where(p => {
                if(p is NormalPolicyInfo || p is BargainPolicyInfo)
                    return hasContact;
                if(p is SpecialPolicyInfo) {
                    var sp = p as SpecialPolicyInfo;
                    switch(sp.Type) {
                        case SpecialProductType.Singleness:
                        case SpecialProductType.Disperse:
                            return !hasContact;
                        case SpecialProductType.CostFree:
                            // 如果是选取和舱位相关（true）的，则应选取黑屏同步(true)的；
                            // 如果是选取和舱位无关（false）的，则应选取黑屏不同步(false)的；
                            // 综合起来，就是返回是否黑屏同步和舱位相关相同的；
                            return sp.SynBlackScreen == hasContact;
                        case SpecialProductType.Bloc:
                        case SpecialProductType.Business:
                        // 2012-11-20
                        case SpecialProductType.OtherSpecial:
                        // 2013-03-01 Xie.
                        case SpecialProductType.LowToHigh:
                            return hasContact;
                        default:
                            return false;
                    }
                }
                return false;
            });
        }

        /// <summary>
        /// 对是否有资源进行过滤
        /// </summary>
        /// <param name="policies"></param>
        /// <returns></returns>
        internal static IEnumerable<PolicyInfoBase> FilterResourceCount(this IEnumerable<PolicyInfoBase> policies) {
            return policies.Where(p => {
                var sp = p as SpecialPolicyInfo;
                if(sp != null) {
                    switch(sp.Type) {
                        case SpecialProductType.Singleness:
                        case SpecialProductType.Disperse:
                            return sp.ResourceAmount > 0;
                        case SpecialProductType.CostFree:
                            return sp.SynBlackScreen || sp.ResourceAmount > 0;
                        default:
                            return true;
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// 过滤相应结算价不存在（数据库中保存为-1）的政策。
        /// </summary>
        /// <param name="policies">待过滤政策</param>
        /// <param name="superior">上级</param>
        /// <returns>过滤后政策</returns>
        /// <remarks>
        /// 这个方法实际上是用于过滤掉没有设置同行，下级或是内部机构，
        /// 先根据上级信息获取关系，然后通过关系获取到结算价，如果结算价小于0（数据库里是-1），则被过滤
        /// 在解决原来的产品方没有下级，而OEM中的产品方有下级时，就是从这里着手，
        /// 方法没有改变，但在数据库中存储值时，在下级结算价时存入数据；
        /// 还有一处相关的地方，就是ComputePolicy()方法中的结算价的判断；
        /// </remarks>
        internal static IEnumerable<PolicyInfoBase> FilterByPermission(this IEnumerable<PolicyInfoBase> policies, SuperiorInfo superior) {
            return from p in policies
                   let deductionType = Calculator.GetDeductionType(p.Owner, superior)
                   let commission = Calculator.GetCommission(p, deductionType)
                   where commission >= 0
                   select p;
        }

        internal static IEnumerable<PolicyInfoBase> FilterByNoPublishPrice(this IEnumerable<PolicyInfoBase> policies) {
            return policies.Where(p => {
                var bp = p as BargainPolicyInfo;
                if(bp != null) {
                    if(bp.VoyageType == VoyageType.RoundTrip) {
                        return bp.Price >= 0;
                    } else if(bp.VoyageType == VoyageType.RoundTrip) {
                        return false;
                    }
                }
                return true;
            });
        }

        /// <summary>
        /// 根据出票方限制信息，过滤政策列表
        /// </summary>
        /// <param name="policies">待过滤政策列表</param>
        /// <param name="limit">出票方限制</param>
        /// <returns>过滤后政策列表</returns>
        /// <remarks>
        /// 2012-11-06 deng.zhao
        /// </remarks>
        [Obsolete]
        internal static IEnumerable<PolicyInfoBase> FilterByGroupLimitation(this IEnumerable<PolicyInfoBase> policies, CompanyGroupLimitationInfo limit)
        {
            // 若limit为空，表示不受限；
            if (limit == null || !policies.Any()) return policies;

            // 特殊政策不受公司组限制，若为非特殊政策，选取出受出票方限制的政策；
            return policies.Where(p => p is SpecialPolicyInfo || p.Owner == limit.Company);
        }

        internal static IEnumerable<PolicyInfoBase> FilterByGroupLimitation(this IEnumerable<PolicyInfoBase> policies, PurchaseLimitationGroup limitationGroup, Guid superior)
        {
            if (!policies.Any()) return policies;

            return (from p in policies
                    let limitation =
                        PolicyMatchServcie.GetPurchaseLimitationInfo(limitationGroup, p.Airline, p.Departure,
                                                                     p.PolicyType)
                    where limitation == null || p is SpecialPolicyInfo || p.Owner == superior
                    select p);
        }

        /// <summary>
        /// 通过航程，对给出的政策进行筛选。
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="voyages">航程</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByVoyages(this IEnumerable<PolicyInfoBase> policies, IEnumerable<VoyageFilterInfo> voyages) {
            var voyageCount = voyages.Count();

            // 循环，一段段地过滤航程；
            for(var i = 0; i < voyageCount; i++) {
                var voyage = voyages.ElementAt(i);
                var flight = voyage.Flight;
                var bunk = voyage.Bunk;
                var voyageIndex = i + 1;

                // 过滤航空公司
                policies = policies.FilterByAirline(flight.Airline);
                // 如果是多段，则从第三段开始，不再检查出发、到达 和 航班号
                if(voyageIndex <= 2) {
                    policies = policies.FilterByDeparture(voyageIndex, flight.Departure) // 出发城市
                        .FilterByArrival(voyageIndex, flight.Arrival) // 到达城市
                        .FilterByFlightNumber(voyageIndex, flight.FlightNumber); // 航班号
                }
                else
                {
                    // 判断此政策是否适用于多段，除特殊外，都可能有多段联程，
                    policies = from p in policies
                               let mcs = p as IMultiConjunctionSuitable
                               where mcs == null || mcs.MultiConjunctionSuitable
                               select p;
                }

                // 航班日期只检查第一程
                // 对于每一程都适用 wangshiling 2013.03.05
                //if(voyageIndex == 1) {
                    policies = policies.FilterByFlightDate(flight.FlightDate);
                //}
                // 过滤舱位
                if(bunk != null) {
                    policies = policies.FilterByBunk(bunk.Code);
                }
                // 只有特价中的往返政策，才过滤出行天数
                if(voyageCount == 2 && voyageIndex == 1) {
                    var departureDate = flight.FlightDate;
                    var returnDate = voyages.ElementAt(voyageIndex).Flight.FlightDate;
                    policies = policies.FilterByTravelDays(departureDate, returnDate);
                }

                policies = policies.ToList();
            }

            return policies;
        }
        
        internal static IEnumerable<PolicyInfoBase> FilterByBunk(this IEnumerable<PolicyInfoBase> policies, string bunkCode, string separator = ",") {
            return policies.Where(p => MatchBunk(p, bunkCode, separator)).ToList();
        }
        internal static IEnumerable<PolicyInfoBase> FilterByBunk(this IEnumerable<PolicyInfoBase> policies, FlightQuery.Domain.Bunk bunk, string separator = ",") {
            var result = policies.Where(p => {
                if(bunk is FlightQuery.Domain.GeneralBunk && p is BargainPolicyInfo) {
                    return false;
                }
                if(bunk is FlightQuery.Domain.ProductionBunk && p is NormalPolicyInfo) {
                    return false;
                }
                return MatchBunk(p, bunk.Code, separator);
            }).ToList();

            return result;
        }
        
        /// <summary>
        /// 为特价政策过滤掉价格类型为按返佣的政策；
        /// </summary>
        /// <param name="policies">待过滤的政策列表</param>
        /// <returns>过滤后的政策列表</returns>
        internal static IEnumerable<PolicyInfoBase> FilterByPriceTypeForbargin(this IEnumerable<PolicyInfoBase> policies)
        {
            var result = (from p in policies
                            let bp = p as BargainPolicyInfo
                            where bp == null || bp != null && bp.PriceType != PriceType.Commission
                            select p
                           );
            return result;
        }

        /// <summary>
        /// 根据过滤条件，过滤给出的政策。
        /// </summary>
        /// <param name="policies"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        internal static IEnumerable<PolicyInfoBase> Filter(this IEnumerable<PolicyInfoBase> policies, PolicyFilterConditions filter) {
            var firstFlight = filter.Voyages.First().Flight;
            // 过滤买家的政策
            policies = policies.FilterByProvider(ProviderFilterType.Exclude, filter.Purchaser);
            // 过滤政策 Id
            if(filter.PolicyId.HasValue) {
                policies = policies.FilterById(filter.PolicyId.Value);
            }
            // 指定出票方
            if(filter.Provider.HasValue) {
                policies = policies.FilterByProvider(ProviderFilterType.Include, filter.Provider.Value);
            }
            // 过滤排除的出票方
            policies = policies.FilterByProvider(ProviderFilterType.Exclude, filter.ExcludeProviders.ToArray());
            // 过滤出票日期
            policies = policies.FilterByPrintDate();
            // 过滤降舱
            policies = policies.FilterByReduce(filter.SuitReduce);

            if (filter.VoyageType != VoyageType.Notch)
            {
                // 过滤行程类型
                policies = policies.FilterByVoyageType(filter.VoyageType);
                // 过滤政策类型
                policies = policies.FilterByPolicyType(filter.PolicyType);
                // 排除航线的过滤；
                var airportPairs =
                    filter.Voyages.Select(p => new AirportPair(p.Flight.Departure, p.Flight.Arrival)).ToList();
                policies = policies.FilterByAirway(airportPairs);
                // 过滤行程
                policies = policies.FilterByVoyages(filter.Voyages);
            }

            // 如果指定的政策类型包含 特价、特殊，过滤提前天数
            if((filter.PolicyType & PolicyType.Bargain) == PolicyType.Bargain || (filter.PolicyType & PolicyType.Special) == PolicyType.Special)
                policies = policies.FilterByBeforehandDays(firstFlight.FlightDate);


            return policies.ToList();
        }

        /// <summary>
        /// 根据给出的政策列表，在允许的客票类型上过滤政策。
        /// </summary>
        /// <param name="policies">待过滤政策列表</param>
        /// <param name="allowTicketType">允许的客票类型</param>
        /// <returns>过滤后的政策列表</returns>
        /// <remarks>
        /// 这个方法只用于阻止订单提交，则在导入时，只需考虑承认，而不考虑默认政策和换出票方；
        /// 而在航班查询时，在显示航班时无需调用，而只需要在选择舱位时才会和政策相关，才需要调用此方法。
        /// </remarks>
        internal static IEnumerable<PolicyInfoBase> FilterByAllowTicketType(this IEnumerable<PolicyInfoBase> policies, AllowTicketType allowTicketType)
        {
            var result = policies.Where(p =>
                                            {
                                                switch (allowTicketType)
                                                {
                                                    case AllowTicketType.BSP:
                                                        return p.TicketType == TicketType.BSP;
                                                    case AllowTicketType.B2B:
                                                        return p.TicketType == TicketType.B2B;
                                                    case AllowTicketType.Both:
                                                        return true;
                                                    case AllowTicketType.None:
                                                        return false;
                                                    case AllowTicketType.B2BOnPolicy:
                                                        return p.PrintBeforeTwoHours || p.TicketType == TicketType.BSP;
                                                    default:
                                                        throw new ArgumentOutOfRangeException("allowTicketType");
                                                }
                                            });
            return result;
        }
        
        /// <summary>
        /// 对匹配的特殊政策，按照价格分组后，每组取一个；
        /// </summary>
        /// <param name="policies"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2012-11-01 deng.zhao 修改p.ParValue为p.SettleAmount
        /// </remarks>
        internal static IEnumerable<MatchedPolicy> DistinctSpecialPolicyForFlightQuery(this IEnumerable<MatchedPolicy> policies) {
            if(policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null && p.OriginalPolicy is SpecialPolicyInfo
                    orderby p.SettleAmount
                    group p by p.SettleAmount into g
                    select g.First()).ToList();
        }

        internal static IEnumerable<MatchedPolicy> DistinctNotchPolicyForChoosePolicy(this IEnumerable<MatchedPolicy> policies)
        {
            if (policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null
                    let np = p.OriginalPolicy as NotchPolicyInfo
                    where np != null
                    orderby p.Commission descending, p.Rebate descending
                    let gKey = new { np.Owner, np.TicketType }
                    group p by gKey into g
                    select g.First()).ToList();
        }

        internal static IEnumerable<MatchedPolicy> DistinctNormalPolicyForChoosePolicy(this IEnumerable<MatchedPolicy> policies) {
            if(policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null
                    let np = p.OriginalPolicy as NormalPolicyInfo
                    where np != null
                    orderby p.Commission descending, p.Rebate descending
                    let gKey = new { np.Owner, np.TicketType }
                    group p by gKey into g
                    select g.First()).ToList();
        }
        internal static IEnumerable<MatchedPolicy> DistinctTeamPolicyForChoosePolicy(this IEnumerable<MatchedPolicy> policies) {
            if(policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null
                    let rtp = p.OriginalPolicy as TeamPolicyInfo
                    where rtp != null
                    orderby p.Commission descending, p.Rebate descending
                    let gKey = new { rtp.Owner, rtp.TicketType }
                    group p by gKey into g
                    select g.First()).ToList();
        }
        internal static IEnumerable<MatchedPolicy> DistinctBargainPolicyForChoosePolicy(this IEnumerable<MatchedPolicy> policies) {
            if(policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null
                    let bp = p.OriginalPolicy as BargainPolicyInfo
                    where bp != null
                    orderby p.Commission descending, p.Rebate descending
                    let gKey = new { bp.Owner, bp.TicketType, p.ParValue }
                    group p by gKey into g
                    select g.First()).ToList();
        }
        internal static IEnumerable<MatchedPolicy> DistinctSpecialPolicyForChoosePolicy(this IEnumerable<MatchedPolicy> policies) {
            if(policies.Count() < 2) return policies;
            return (from p in policies
                    where p != null
                    let sp = p.OriginalPolicy as SpecialPolicyInfo
                    where sp != null
                    orderby p.SettleAmount
                    let gKey = new { sp.Owner, p.SettleAmount }
                    group p by gKey into g
                    select g.First()).ToList();
        }
    }
}