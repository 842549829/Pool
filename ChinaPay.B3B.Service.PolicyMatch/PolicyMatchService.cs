using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.B3B.Service.FlightQuery.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.PolicyMatch.Domain;
using ChinaPay.B3B.Service.Statistic;
using ChinaPay.B3B.Service.SystemManagement;
using Izual;
using Izual.Data;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.PolicyMatch
{
    using OriginalBunk = Bunk;
    using OriginalFlight = Flight;

    public static class PolicyMatchServcie
    {
        /// <summary>
        /// 获取舱位上的价格最低的政策；（将在航班查询中使用,在此会做贴点和扣点处理）
        /// 在单程、往返去程和往返回程时被调用；
        /// </summary>
        /// <param name="policies">政策列表</param>
        /// <param name="bunks">舱位列表</param>
        /// <param name="purchaser">采购编号</param>
        /// <param name="superior">上级编号</param>
        /// <param name="env">环境</param>
        /// <param name="group">采买限制组</param>
        /// <param name="allSettingOnPolicies"> </param>
        /// <param name="normalDefaultPolicies"> </param>
        /// <param name="needSubsidize">是否需要贴点</param>
        /// <param name="oemInfo"> </param>
        /// <param name="setting">收益组设置</param>
        /// <returns>
        ///     当在单程时需要贴点，而其它的不需要；
        ///     这里之所以传入舱位列表，是在处理往返程的回程时，往返程的舱位都需要传入，而对于单程或往返去程时，只传入单个舱位；
        /// </returns>
        private static MatchedPolicy GetLowestPolicyOfBunk(IEnumerable<PolicyInfoBase> policies,
                                                           IEnumerable<OriginalBunk> bunks, Guid purchaser,
                                                           SuperiorInfo superior, MatchEnvironment env,
                                                           PurchaseLimitationGroup group,
                                                           IEnumerable<NormalPolicySetting> allSettingOnPolicies,
                                                           Dictionary<string, NormalDefaultPolicyInfo>
                                                               normalDefaultPolicies, bool needSubsidize,
                                                           IncomeGroupLimitGroup setting
            )
        {
            var bunk = bunks.Last();
            var flight = bunk.Owner;
            
            var pss =
                env.PolicySettings.Where(
                    psi => psi.Airline == bunk.Owner.Airline && bunks.All(b => psi.Berths.Contains(b.Code)));
            var phs =
                env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(bunk.Owner.Airline)).ToList();

            // 2013-01-26 deng.zhao 对于单条政策的贴扣点做了调整，计算出单条政策的贴点值后，传入；
            var airportPair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
            IEnumerable<MatchedPolicy> mps = (from p in policies.FilterByBunk(bunk)
                                              let spsd =
                                                  Calculator.GetSinglePolicySubsidyAndDeduction(allSettingOnPolicies,
                                                                                                p.Id, airportPair,
                                                                                                bunk.Code,
                                                                                                flight.FlightDate)
                                              select
                                                  Calculator.ComputePolicy(p, bunk, null, purchaser, superior, pss, phs,
                                                                           false, PassengerType.Adult, spsd)
                                             ).Where(p => p != null).ToList();

            //var oemInfo = OEMService.QueryOEM(superior.Id);
            //var setting = GetIncomeGroupDeductSetting(oemInfo, purchaser);
            var carrier = flight.Airline;
            var flightDate = flight.FlightDate;

            // 处理全局贴点；
            if (needSubsidize)
            {
                mps = ProcessGlobalSubsidy(mps, pss);
            }
            // 处理OEM扣点；
            mps = ProcessIncomeDeduction(mps, setting, carrier, airportPair, flightDate).ToList();
            // 处理政策协调；
            mps = ProcessAllHarmony(mps, superior, phs);
            
            var lowest = mps.MinOrDefault(p => p.SettleAmount);
            // 若匹配不到最低价，则取默认政策，在查询时，只对普通舱位操作；
            if (lowest == null && bunk is GeneralBunk)
            {
                var limitation = GetPurchaseLimitationInfo(group, carrier, flight.Departure.Code, PolicyType.Normal);
                // 若有公司组限制，取采购的所有者（此时和上级的关系可为上级或同行）的默认政策；
                if (limitation != null)
                {
                    lowest = Calculator.ComputeOwnerDefaultPolicy(PolicyManageService.GetOwnerDefaultPolicy(limitation, superior.Id),
                                                                  bunk.Owner.StandardPrice,
                                                                  (bunk as GeneralBunk).Discount, superior,
                                                                  PassengerType.Adult);
                }
                else
                {
                    lowest =
                        Calculator.ComputeNormalDefaultPolicy(
                            normalDefaultPolicies[bunk.Owner.Airline],
                            bunk.Owner.StandardPrice, (bunk as GeneralBunk).Discount, superior, PassengerType.Adult);
                }
            }
            return lowest;
        }

        /// <summary>
        /// 获取舱位上的价格最低的政策；（在航班查询中使用，只有单程的航班显示中用到）
        /// 这个只处理特殊政策，
        /// 2013-03-28，为了适应oem扣点，这里对参数做了调整；
        /// </summary>
        /// <param name="policies"></param>
        /// <param name="bunks"></param>
        /// <param name="superior"></param>
        /// <param name="purchaser"> </param>
        /// <returns></returns>
        /// <remarks>
        /// 这里的舱位可能没有信息，所以传个参数进来吧，新增个flight；
        /// </remarks>
        private static MatchedPolicy GetLowestSpeicalPolicyOfBunk(IEnumerable<PolicyInfoBase> policies,
                                                                  IEnumerable<OriginalBunk> bunks, SuperiorInfo superior,
                                                                  Guid purchaser, Flight flight,
                                                                  IncomeGroupLimitGroup setting)
        {
            var result = (from p in policies
                          let sp = p as SpecialPolicyInfo
                          from b in bunks
                          where sp.Berths.Contains(b.Code)
                          let discount = b is GeneralBunk ? (b as GeneralBunk).Discount : 0
                          let computedPolicy =
                              Calculator.ComputeSpecialPolicy(sp, b.Owner.StandardPrice, discount, superior)
                          where computedPolicy != null
                          select computedPolicy);

            var airportpair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);

            var carrier = flight.Airline;
            var flightDate = flight.FlightDate;
            result = ProcessIncomeDeduction(result, setting, carrier, airportpair, flightDate).ToList();

            // 此处无政策协调的操作；
            return result.MinOrDefault(p => p.SettleAmount);
        }

        #region 航班查询

        private static readonly IEnumerable<MatchedPolicy> emptyPolicies = EnumerableHelper.GetEmpty<MatchedPolicy>();

        /// <summary>
        /// 根据传入的航班列表（历史记录），和当前航班的信息，查找同一航程下的，前后三天内的价格更低的航班信息。
        /// </summary>
        /// <param name="flights">航班列表</param>
        /// <param name="currentPrice">当前航班</param>
        /// <param name="purchaser">采购者</param>
        /// <param name="cachedMatchedPolicy">用户缓存匹配到的政策</param>
        /// <returns>筛选后的航班列表</returns>
        /// <remarks>
        /// 在特殊政策下无舱位的政策中匹配，需要黑屏同步的不予考虑。
        /// 按照航班分类，每个航班显示一个最低价。
        /// </remarks>
        public static Dictionary<DateTime, IEnumerable<InstructionalFlight>> MatchInstructionalFlights(
            Dictionary<DateTime, IEnumerable<Flight>> flights, decimal currentPrice, Guid purchaser,
            Action<IEnumerable<MatchedPolicy>> cachedMatchedPolicy)
        {
            if (flights == null)
            {
                throw new ArgumentNullException();
            }
            var result = new Dictionary<DateTime, IEnumerable<InstructionalFlight>>();

            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            Dictionary<Guid, WorkingHoursView> workingPublishers =
                CompanyService.GetWorkingHours().FilterWorkingPublishers();
            IEnumerable<PolicyInfoBase> policies = null;
            if(flights.Count ==0 || !flights.Any(f => f.Value.Any()))
            {
                policies = EnumerableHelper.GetEmpty<PolicyInfoBase>();
            }else
            {
                var firstDateHasFlight = flights.FirstOrDefault(f => f.Value.Any());
                var firstFlight = firstDateHasFlight.Value.First();
                policies = DataCenter.Instance.QueryPolicies(firstFlight.Departure.Code, flights.Keys.Min(), flights.Keys.Max(), VoyageType.OneWay, PolicyType.Special);
            }

            List<PolicyInfoBase> filterPolicies = policies
                .FilterByRelationWithBunk(false) // 取得与仓位无关的政策；
                .FilterByAccount(vas) // 取得有收款帐号的政策；
                .FilterByPermission(sup) // 取得有权限的政策；
                .FilterByPrintDate() // 取得今天可以出票的政策；
                .FilterResourceCount() // 取得资源数大于0的政策；
                .FilterByWorkingHours(workingPublishers) // 取得在工作时间内的政策；
                //这里没有处理对BSP和B2B的过滤；
                .ToList();

            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);

            foreach (var item in flights)
            {
                var matchedFlights = new List<InstructionalFlight>();
                if (item.Value != null && item.Value.Any())
                {
                    // 所有的信息中，以下的内容是一致的；
                    Flight firstFlight = item.Value.First();
                    string departure = firstFlight.Departure.Code;
                    string arrival = firstFlight.Arrival.Code;
                    var airportpair = new AirportPair(departure, arrival);
                    var flightDate = firstFlight.FlightDate;

                    List<PolicyInfoBase> currentDatePolicies =
                        filterPolicies.FilterByFlightDate(flightDate) // 取得飞行时间匹配的政策；
                            .FilterByBeforehandDays(flightDate) // 取得提前天数符合的政策；
                            .FilterByDeparture(1, departure) // 取得出港地匹配的政策；
                            .FilterByArrival(1, arrival) // 取得到港地匹配的政策；
                            .FilterByAirway(departure, arrival) // 取得无航线限制的政策；
                            .ToList();

                    foreach (Flight flight in item.Value)
                    {
                        // 每个航班的航空公司不一样；
                        var carrier = flight.Airline;

                        // 暂时只考虑单程；
                        List<PolicyInfoBase> currentPolicies =
                            currentDatePolicies.FilterByAirline(flight.Airline) // 取得对应当前航空公司的政策；
                                .FilterByFlightNumber(1, flight.FlightNo).ToList(); // 取得无航班限制的政策；

                        IEnumerable<MatchedPolicy> matchedPolicy = currentPolicies.Select(
                            p => Calculator.ComputeSpecialPolicy(p as SpecialPolicyInfo, flight.StandardPrice, 0, sup))
                            .Where(
                                p => p != null && p.SettleAmount < currentPrice && p.SettleAmount < flight.StandardPrice);
                        // 2013-03-28 deng.zhao OEM扣点；
                        matchedPolicy = ProcessIncomeDeduction(matchedPolicy, setting, carrier, airportpair, flight.FlightDate);
                        // 特殊政策没有政策协调，这里不用了；

                        MatchedPolicy lowest =
                            matchedPolicy.DistinctSpecialPolicyForChoosePolicy().SortSpecialPolicy(departure, arrival).
                                FirstOrDefault();

                        cachedMatchedPolicy(matchedPolicy);
                        if (lowest != null)
                        {
                            matchedFlights.Add(new InstructionalFlight
                                                   {
                                                       OriginalFlight = flight,
                                                       OriginalPolicy = lowest.OriginalPolicy as SpecialPolicyInfo,
                                                       Statistic = lowest.Statistics,
                                                       SettleAmount = lowest.SettleAmount,
                                                       ResourceAmount =
                                                           (lowest.OriginalPolicy as SpecialPolicyInfo).ResourceAmount,
                                                       CompannyGrade = 5
                                                   });
                        }
                    }
                }
                // 将按价格排序后的结果加入到集合中；
                result.Add(item.Key, matchedFlights.OrderBy(p => p.SettleAmount).ToList());
            }

            // 获取发布方星级
            IEnumerable<Guid> publishers = (from dateOfFlights in result.Values
                                            from flight in dateOfFlights
                                            select flight.OriginalPolicy.Owner).Distinct();
            if (publishers.Any())
            {
                Dictionary<Guid, CompanyParameter> companyParameters =
                    AccountCombineService.GetCreditworthiness(publishers);
                foreach (var dateOfFlights in result.Values)
                {
                    foreach (InstructionalFlight flight in dateOfFlights)
                    {
                        if (companyParameters.ContainsKey(flight.OriginalPolicy.Owner))
                        {
                            CompanyParameter companyParameter = companyParameters[flight.OriginalPolicy.Owner];
                            if (companyParameter != null)
                            {
                                decimal? starClass = companyParameter.Creditworthiness;
                                if (starClass.HasValue) flight.CompannyGrade = starClass.Value;
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 航班查询（单程，用于航班列表显示，只填充最低价格）
        /// </summary>
        /// <param name="flights">航班列表</param>
        /// <param name="purchaser">采购者 Id</param>
        /// <param name="publisher">政策发布者 Id</param>
        public static IEnumerable<MatchedFlight> MatchOneWayFlights(IEnumerable<OriginalFlight> flights, Guid purchaser,
                                                                    Guid? publisher = null)
        {
            // 若无航班信息，则直接退出；
            if (!flights.Any()) yield break;
            // 获取到航班列表里的第一个航班；
            Flight refer = flights.First();
            // 由此得到航班飞行时间；
            DateTime flightDate = refer.FlightDate;
            // 获取到对应的匹配的环境；
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(null, refer.Departure.Code,
                                                                           refer.Arrival.Code, refer.FlightDate,
                                                                           VoyageType.OneWay);
            // 获取上级机构，这个对下级能看到的政策会有影响；
            // 规则是，如果允许采购其他代理发布的政策，则不限，如果不允许，则指明规定的航线和出港中限制，
            // 但是，当公司没有发布该航线和政策设置时，可以设置默认返点（上级的默认政策）和允许采购其它的部门政策；
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            // 获取所有收款帐号，如果一个政策发布者没有收款帐号，则排除；
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            // 在工作时间内的政策发布者，除去不在工作时间内的政策发布者，得到在工作的政策发布者；
            var workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            // 获取采买限制；
            var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);
            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();

            // 获取所有普通默认政策
            var normalDefaultPolicies = PolicyManageService.GetAllDefaultPolicies().ToDictionary(p => p.Airline);

            List<PolicyInfoBase> filterPolicies = DataCenter.Instance.QueryPolicies(refer.Departure.Code, refer.FlightDate.Date, VoyageType.OneWay | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain | PolicyType.Special)
                    .FilterByProvider(ProviderFilterType.Exclude, purchaser) // 排除自己的政策；
                    .FilterByAccount(vas) // 排除没有收款账户的政策发布者的政策；
                    .FilterByWorkingHours(workingPublishers) // 排除不在工作时间内的政策发布者的政策；
                    //.FilterByPrintDate() // 排除出票日期不符合的
                    //.FilterByDeparture(1, refer.Departure.Code) // 排除出港不匹配的政策，由于是单程，这里只过滤第一程
                    .FilterByArrival(1, refer.Arrival.Code) // 排除到港不匹配的政策，由于是单程，这里只过滤第一程
                    .FilterByFlightDate(flightDate) // 排除飞行日期不匹配的政策；
                    .FilterByBeforehandDays(flightDate) // 若有提前天数，排除这部分政策；
                    .FilterByPermission(sup)
                    .FilterByAirway(refer.Departure.Code, refer.Arrival.Code) // 2012-10-22 统一去除排除航线；
                    .ToList(); // 过滤掉没有权限的政策

            #region 处理普通政策和特价政策

            List<PolicyInfoBase> generalPolicies =
                filterPolicies.FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain).ToList();
            // 若指定了发布者，则只采用他的政策；
            if (publisher.HasValue)
            {
                generalPolicies = generalPolicies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                    // 查询指定供应商政策
            }
            // 再次过滤，先按照政策类型过滤，然后按舱位和航线过滤；
            // 此处注意，特价政策的行程类型是不存在单程或往返的（原因是舱位无法公用），合在一起处理应无太大问题；
            generalPolicies = generalPolicies.FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain)
                //.FilterByVoyageType(VoyageType.OneWay) // 2012-10-17，这里把默认的参数加上了；
                //.FilterByAirway(refer.Departure.Code, refer.Arrival.Code) //2012-10-22 修改，
                .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                .ToList();

            #endregion

            // 取得特殊政策；
            List<PolicyInfoBase> specialPolicies = filterPolicies.FilterByPolicyType(PolicyType.Special).ToList();
            // 获取收益组设置；
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);

            foreach (Flight flight in flights)
            {
                MatchedPolicy matchedLowestGeneralForBunks = null;
                var takeoffTime = flightDate.AddHours(flight.TakeoffTime.Hour).AddMinutes(flight.TakeoffTime.Minute);
                if (flight.Bunks.Any())
                {
                    // 这里是普通和特价政策的处理；
                    List<PolicyInfoBase> subGeneralPolicies = generalPolicies.FilterByAirline(flight.Airline)
                        .FilterByFlightNumber(1, flight.FlightNo)
                        .FilterByPrice(flight.StandardPrice)
                        .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(takeoffTime)).ToList();
                    
                    // 通过上级机构限制的条件再过滤一次；
                    subGeneralPolicies = subGeneralPolicies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

                    // 对于普通和特价，找到舱位匹配的最低的那个（最后的参数代表需要贴点）
                    List<MatchedPolicy> matchedGeneralBunks =
                        flight.Bunks.Select(
                            bunk =>
                            GetLowestPolicyOfBunk(subGeneralPolicies, new[] { bunk }, purchaser, sup, env, limitationGroup,
                                                  allSettingOnPolicies, normalDefaultPolicies, true, setting)).
                            Where(p => p != null).ToList();
                    matchedLowestGeneralForBunks = matchedGeneralBunks.MinOrDefault(p => p.SettleAmount);
                }

                // 对于特殊政策，没有处理上下级关系；
                List<PolicyInfoBase> subSpecialPolicies = specialPolicies.FilterByAirline(flight.Airline)
                    .FilterByFlightNumber(1, flight.FlightNo)
                    .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(takeoffTime))
                    // 这句看起来没什么用处呢？
                    .FilterByPrice(flight.StandardPrice).ToList();

                // 获得跟舱位有关系的特殊政策中价格最低的那条
                MatchedPolicy matchedLowestSpecialForBunks =
                    GetLowestSpeicalPolicyOfBunk(subSpecialPolicies.FilterByRelationWithBunk(true), flight.Bunks, sup, purchaser, flight, setting);

                // 这个是处理弃程舱位 2012-11-20
                MatchedPolicy temp = GetLowestSpeicalPolicyOfBunk(subSpecialPolicies.FilterByRelationWithBunk(true),
                                                                  flight.FilteredBunks, sup, purchaser, flight, setting);
                if (matchedLowestSpecialForBunks == null ||
                    (temp != null && matchedLowestSpecialForBunks.SettleAmount > temp.SettleAmount))
                {
                    matchedLowestSpecialForBunks = temp;
                }
                
                // 获得跟舱位无关的特殊政策中价格最低的那条
                
                var tempSpecialPolicies = subSpecialPolicies.FilterByRelationWithBunk(false)
                    .FilterResourceCount() // 在此基础上，对于和舱位无关的特殊政策，需检查其资源数；
                    // 2012-10-20 dengzhao 方法签名变动，由于是和舱位无关的，折扣直接给的0；
                    .Select(p => Calculator.ComputeSpecialPolicy(p as SpecialPolicyInfo, flight.StandardPrice, 0, sup))
                    .Where(p => p != null && p.SettleAmount < flight.StandardPrice);

                // 2013-03-28 这里没有调用方法，直接对其进行oem扣点；
                var airportpair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
                var carrier = flight.Airline;
                tempSpecialPolicies = ProcessIncomeDeduction(tempSpecialPolicies, setting, carrier, airportpair, flightDate).ToList();
                // 由于是特殊政策，也不需要做政策协调；

                MatchedPolicy matchedLowestSpeicalWithoutBunk = tempSpecialPolicies.MinOrDefault(p => p.SettleAmount);

                if (matchedLowestGeneralForBunks == null && matchedLowestSpecialForBunks == null &&
                    matchedLowestSpeicalWithoutBunk == null) // 若无最低价，表示该航班没有任何可销售产品
                    yield return
                        new MatchedFlight {LowestPrice = -1, OriginalFlight = flight, PolicyType = PolicyType.Unknown};
                else
                {
                    // 假定基础政策的价格最低
                    // 把三类政策放在一起，获得最低价
                    MatchedPolicy lowest = matchedLowestGeneralForBunks;
                    if (lowest == null ||
                        (matchedLowestSpecialForBunks != null &&
                         lowest.SettleAmount > matchedLowestSpecialForBunks.SettleAmount))
                    {
                        lowest = matchedLowestSpecialForBunks;
                    }
                    if (lowest == null ||
                        (matchedLowestSpeicalWithoutBunk != null &&
                         lowest.SettleAmount > matchedLowestSpeicalWithoutBunk.SettleAmount))
                    {
                        lowest = matchedLowestSpeicalWithoutBunk;
                    }

                    yield return
                        new MatchedFlight
                            {LowestPrice = lowest.SettleAmount, OriginalFlight = flight, PolicyType = lowest.PolicyType}
                        ;
                }
            }
        }

        /// <summary>
        /// 根据起飞时间，获取允许的客票类型。
        /// </summary>
        /// <param name="takeOffTime">起飞时间</param>
        /// <returns>允许的客票类型</returns>
        public static AllowTicketType GetAllowTicketTypeByDepartureTime(DateTime takeOffTime)
        {
            double minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2*60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
        }

        /// <summary>
        /// 航班查询（显示一个航班的所有舱位）
        /// </summary>
        /// <param name="flight">航班对象</param>
        /// <param name="purchaser">采购者 Id</param>
        public static IEnumerable<MatchedBunk> MatchOneWayFlight(OriginalFlight flight, Guid purchaser,
                                                                 Guid? publisher = null)
        {
            var result = new List<MatchedBunk>();
            DateTime flightDate = flight.FlightDate;
            DateTime departureTIme = flightDate.AddHours(flight.TakeoffTime.Hour).AddMinutes(flight.TakeoffTime.Minute);
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure.Code,
                                                                           flight.Arrival.Code, flightDate,
                                                                           VoyageType.OneWay);
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            Dictionary<Guid, WorkingHoursView> workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();

            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);
            var airportpair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
            var carrier = flight.Airline;
            var phs = env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(carrier));

            // 获取所有普通默认政策
            var normalDefaultPolicy = PolicyManageService.GetNormalDefaultPolicy(flight.Airline);

            List<PolicyInfoBase> filterPolicies = DataCenter.Instance.QueryPolicies(flight.Airline, flight.Departure.Code, flight.FlightDate.Date, VoyageType.OneWay | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain | PolicyType.Special)
                    .FilterByProvider(ProviderFilterType.Exclude, purchaser) // 排除自己的政策
                    .FilterByAccount(vas)
                    .FilterByWorkingHours(workingPublishers)
                    //.FilterByAirline(flight.Airline)
                    //.FilterByDeparture(1, flight.Departure.Code)
                    .FilterByArrival(1, flight.Arrival.Code)
                    //.FilterByPrintDate()
                    .FilterByFlightDate(flightDate)
                    .FilterByBeforehandDays(flightDate)
                    .FilterByFlightNumber(1, flight.FlightNo)
                    //.FilterByVoyageType(VoyageType.OneWay)
                    .FilterByPrice(flight.StandardPrice)
                    .FilterByPermission(sup)
                    .FilterByAirway(flight.Departure.Code, flight.Arrival.Code) // 2012-10-22 统一去除排除航线；
                    .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                    .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(departureTIme))
                    .ToList();

            IEnumerable<MatchedBunk> generalBunks = null;
            if (flight.Bunks.Any())
            {
                IEnumerable<PolicyInfoBase> generalPolicies =
                    filterPolicies.FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain);
                if (publisher.HasValue)
                {
                    generalPolicies = generalPolicies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                        // 查询指定供应商政策
                }

                // 处理公司组限制
                //var limits = DataCenter.Instance.QueryCompanyLimmitations(purchaser, sup);
                //CompanyGroupLimitationInfo limit = GetGroupLimitation(sup, limits, flight.Airline, flight.Departure.Code);
                var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);
                // 这里直接获取的普通政策；
                var limitation = GetPurchaseLimitationInfo(limitationGroup , carrier, flight.Departure.Code,PolicyType.Normal); 
                
                // 这里还要写一个过滤的方法，似乎是
                generalPolicies = generalPolicies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

                // 2013-01-26 deng.zhao 贴扣点变动这里变动
                var airportPair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
                generalBunks = (from b in flight.Bunks
                                let psWithoutGlobalSubsidy = 
                                    (from p in generalPolicies.FilterByBunk(b)
                                     let spsd =
                                         Calculator.GetSinglePolicySubsidyAndDeduction(allSettingOnPolicies, p.Id, airportPair, b.Code, flight.FlightDate)
                                     select
                                         Calculator.ComputePolicy(p, b, null, purchaser, sup,env.PolicySettings,env.PolicyHarmonies,
                                                                  false, PassengerType.Adult, spsd)
                                             ).Where(p=>p!= null).ToList()
                                // 处理全局贴点
                                let pgs = ProcessGlobalSubsidy(psWithoutGlobalSubsidy, env.PolicySettings.Where(p => p.Berths.Contains(b.Code)))
                                // 2013-03-28 deng.zhao 处理OEM扣点；
                                let pde = ProcessIncomeDeduction(pgs, setting, carrier, airportpair, flightDate)
                                let ps = ProcessAllHarmony(pde, sup, phs)
                                let lowestPolicies =
                                    emptyPolicies.Concat(
                                        ps.Where(p => p.PolicyType == PolicyType.Normal).MinOrDefault(
                                            p => p.SettleAmount))
                                    .Concat(
                                        ps.Where(p => p.PolicyType == PolicyType.Bargain).MinOrDefault(
                                            p => p.SettleAmount))
                                    .Where(p => p != null)
                                select new MatchedBunk
                                           {
                                               OriginalBunk = b,
                                               Policies = lowestPolicies.Any()
                                                              ? lowestPolicies
                                                              : (
                                                                    b is GeneralBunk
                                                                        ? new[]
                                                                              {
                                                                                  limitation != null
                                                                                      ? Calculator.
                                                                                            ComputeOwnerDefaultPolicy(
                                                                                                    PolicyManageService.GetOwnerDefaultPolicy(limitation, sup.Id),
                                                                                                b.Owner.StandardPrice,
                                                                                                (b as GeneralBunk).
                                                                                                    Discount, sup,
                                                                                                PassengerType.Adult)
                                                                                      : Calculator.
                                                                                            ComputeNormalDefaultPolicy(
                                                                                                normalDefaultPolicy,
                                                                                                b.Owner.StandardPrice,
                                                                                                (b as GeneralBunk).
                                                                                                    Discount, sup,
                                                                                                PassengerType.Adult)
                                                                              }
                                                                        : emptyPolicies
                                                                )
                                           }).ToList();
                result.AddRange(generalBunks);
            }

            // 处理特殊政策
            List<PolicyInfoBase> specialPolicies = filterPolicies.FilterByPolicyType(PolicyType.Special).ToList();
            // 跟舱位有关系的特殊政策
            List<PolicyInfoBase> specialPoliciesForBunk = specialPolicies.FilterByRelationWithBunk(true).ToList();
            // 跟舱位无关系的特殊政策
            List<PolicyInfoBase> specialPoliciesWithoutBunk =
                specialPolicies.FilterByRelationWithBunk(false).FilterResourceCount().ToList();

            // 跟舱位有关系的特殊政策，分别放在不同的舱位对象上
            if (specialPoliciesForBunk.Any())
            {
                var airportPair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
                List<MatchedBunk> specialBunks = (from b in flight.Bunks
                                                  let sps = specialPoliciesForBunk.FilterByBunk(b)
                                                  where sps.Any()
                                                  let discount = b is GeneralBunk ? (b as GeneralBunk).Discount : 0
                                                  let csp =
                                                      sps.Select(
                                                          sp =>
                                                          Calculator.ComputeSpecialPolicy(sp as SpecialPolicyInfo,
                                                                                          b.Owner.StandardPrice,
                                                                                          discount, sup))
                                                      .Where(msp => msp != null)
                                                  // 2013-03-28 deng.zhao OEM
                                                  let matchedSpecialPolicies = ProcessIncomeDeduction(csp, setting, carrier, airportpair, flightDate)
                                                  select new MatchedBunk
                                                             {
                                                                 OriginalBunk = b,
                                                                 Policies =
                                                                     matchedSpecialPolicies.
                                                                     DistinctSpecialPolicyForFlightQuery().
                                                                     SortSpecialPolicy(flight.Departure.Code,
                                                                                       flight.Arrival.Code)
                                                             }).ToList();

                // 处理弃程专用
                List<MatchedBunk> otherSpecialBunks = (from b in flight.FilteredBunks.Concat(flight.Bunks)
                                                       let sps =
                                                           specialPoliciesForBunk.Where(
                                                               os =>
                                                               (os as SpecialPolicyInfo).Type ==
                                                               SpecialProductType.OtherSpecial).FilterByBunk(b)
                                                       where sps.Any()
                                                       let discount = b is GeneralBunk ? (b as GeneralBunk).Discount : 0
                                                       let csp =
                                                           sps.Select(
                                                               sp =>
                                                               Calculator.ComputeSpecialPolicy(sp as SpecialPolicyInfo,
                                                                                               b.Owner.StandardPrice,
                                                                                               discount, sup))
                                                           .Where(msp => msp != null)
                                                       // 2013-03-28 deng.zhao OEM
                                                       let matchedSpecialPolicies = ProcessIncomeDeduction(csp, setting, carrier, airportpair, flightDate)
                                                       select new MatchedBunk
                                                                  {
                                                                      OriginalBunk = b,
                                                                      Policies =
                                                                          matchedSpecialPolicies.
                                                                          DistinctSpecialPolicyForFlightQuery().
                                                                          SortSpecialPolicy(flight.Departure.Code,
                                                                                            flight.Arrival.Code)
                                                                  }).ToList();
                specialBunks.AddRange(otherSpecialBunks);

                // 以下这段代码是处理W舱的；
                var combinedSpeicalBunks = new Dictionary<string, MatchedBunk>();
                foreach (MatchedBunk sb in specialBunks)
                {
                    string key = sb.OriginalBunk.Code;
                    if (combinedSpeicalBunks.ContainsKey(key))
                    {
                        MatchedBunk existsMatchedBunk = combinedSpeicalBunks[key];
                        foreach (MatchedPolicy sbp in sb.Policies)
                        {
                            if (!existsMatchedBunk.Policies.Any(p => p.SettleAmount == sbp.SettleAmount))
                            {
                                existsMatchedBunk.Policies = existsMatchedBunk.Policies.Concat(sbp);
                            }
                        }
                    }
                    else
                    {
                        combinedSpeicalBunks.Add(key, sb);
                    }
                }

                // 最后处理特殊政策中，同舱位情况下，是否特殊政策的价格比普通政策的价格低；
                List<MatchedBunk> csb = combinedSpeicalBunks.Values.ToList();

                foreach (MatchedBunk matchedBunk in csb)
                {
                    foreach (MatchedBunk generalBunk in generalBunks)
                    {
                        // 若舱位相同，且普通政策中有数据，则比较后，去除掉特殊政策中价格较高的政策；如果不相同或普通政策无数据，则不变；
                        if (generalBunk.OriginalBunk == matchedBunk.OriginalBunk && generalBunk.Policies.Any())
                        {
                            // 普通政策的价格；
                            decimal settleAmount = generalBunk.Policies.First().SettleAmount;
                            matchedBunk.Policies = matchedBunk.Policies.Where(p => p.SettleAmount < settleAmount);//||(p.OriginalPolicy is SpecialPolicy &&((SpecialPolicyInfo)p.OriginalPolicy).Type==SpecialProductType.LowToHigh)); //不过滤低打高返的结算价 Xie. 2013-03-06
                        }
                    }
                }

                result.AddRange(csb);
            }
            // 所有跟舱位无关系的特殊政策放在一个舱位对象上
            if (specialPoliciesWithoutBunk.Any())
            {
                var airportPair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
                List<MatchedPolicy> matchedSpecialPoliciesWithoutBunk =
                    specialPoliciesWithoutBunk.Select(
                        p => Calculator.ComputeSpecialPolicy(p as SpecialPolicyInfo, flight.StandardPrice, 0, sup))
                        .Where(msp => msp != null).ToList();

                matchedSpecialPoliciesWithoutBunk = ProcessIncomeDeduction(matchedSpecialPoliciesWithoutBunk, setting, carrier, airportpair, flightDate).ToList();
                // 跟舱位无关系的特殊政策，要取价格低于非特殊政策中的最低价的特殊政策；
                // 同时，此价格不能大于Y舱价格；
                if (generalBunks != null && generalBunks.Any())
                {
                    decimal lowestSettleAmount =
                        generalBunks.Min(b => b.Policies.Any() ? b.Policies.First().SettleAmount : flight.StandardPrice);
                    matchedSpecialPoliciesWithoutBunk =
                        matchedSpecialPoliciesWithoutBunk.Where(p => lowestSettleAmount > p.SettleAmount && flight.StandardPrice >= p.SettleAmount).ToList();
                }
                result.Add(new MatchedBunk
                               {
                                   OriginalBunk = null,
                                   Policies =
                                       matchedSpecialPoliciesWithoutBunk.DistinctSpecialPolicyForFlightQuery().
                                       SortSpecialPolicy(flight.Departure.Code, flight.Arrival.Code).ToList()
                               });
            }

            return result;
        }

        /// <summary>
        /// 航班查询（往返去程，用于航班列表显示，只填充最低价格）
        /// </summary>
        /// <param name="flights">航班列表</param>
        /// <param name="returnDate">返程日期</param>
        /// <param name="purchaser">采购者 Id</param>
        public static IEnumerable<MatchedFlight> MatchRoundTripDepartureFlights(IEnumerable<OriginalFlight> flights,
                                                                                DateTime returnDate, Guid purchaser,
                                                                                Guid? publisher = null)
        {
            if (!flights.Any()) yield break;
            Flight refer = flights.First();
            var carrier = refer.Airline;
            DateTime flightDate = refer.FlightDate;


            // 此处设定为往返政策；
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(null, refer.Departure.Code,
                                                                           refer.Arrival.Code, refer.FlightDate,
                                                                           VoyageType.RoundTrip);
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);
            Dictionary<Guid, WorkingHoursView> workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);

            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();
            // 获取所有普通默认政策
            var normalDefaultPolicies = PolicyManageService.GetAllDefaultPolicies().ToDictionary(p => p.Airline);

            List<PolicyInfoBase> policies = DataCenter.Instance.QueryPolicies(refer.Departure.Code, refer.FlightDate.Date, VoyageType.RoundTrip | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain)
                .FilterByProvider(ProviderFilterType.Exclude, purchaser).ToList();
                // 排除自己的政策
            if (publisher.HasValue)
            {
                policies = policies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                    // 查询指定供应商政策
            }
            // 政策过滤；
            policies = policies.FilterByAccount(vas)
                .FilterByWorkingHours(workingPublishers)
                .FilterByVoyageType(VoyageType.RoundTrip)
                .FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain)
                .FilterByDeparture(1, refer.Departure.Code)
                .FilterByArrival(1, refer.Arrival.Code)
                .FilterByPrintDate()
                .FilterByFlightDate(flightDate)
                .FilterByBeforehandDays(flightDate)
                .FilterByAirway(refer.Departure.Code, refer.Arrival.Code)
                .FilterByTravelDays(flightDate, returnDate)
                .FilterByNoPublishPrice()
                .FilterByPermission(sup)
                .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                .ToList();

            foreach (Flight flight in flights)
            {
                var takeoffTime = flightDate.AddHours(flight.TakeoffTime.Hour).AddMinutes(flight.TakeoffTime.Minute);
                List<PolicyInfoBase> subGeneralPolicies = policies.FilterByAirline(flight.Airline)
                    .FilterByFlightNumber(1, flight.FlightNo)
                    .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(takeoffTime))
                    .FilterByPrice(flight.StandardPrice).ToList();

                // 处理公司组限制
                subGeneralPolicies = subGeneralPolicies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

                // 注意最后一个参数，由它控制是否贴点；
                IEnumerable<MatchedPolicy> matchedBunks =
                    flight.Bunks.Select(
                        bunk =>
                        GetLowestPolicyOfBunk(subGeneralPolicies, new[] { bunk }, purchaser, sup, env, limitationGroup, allSettingOnPolicies, normalDefaultPolicies, false, setting)).
                        Where(p => p != null);
                
                MatchedPolicy lowest = matchedBunks.MinOrDefault(p => p.SettleAmount);

                if (lowest == null)
                    yield return
                        new MatchedFlight {LowestPrice = -1, OriginalFlight = flight, PolicyType = PolicyType.Unknown};
                else
                {
                    yield return new MatchedFlight{LowestPrice = lowest.SettleAmount, OriginalFlight = flight, PolicyType = lowest.PolicyType};
                }
            }
        }

        /// <summary>
        /// 航班查询（往返去程，显示一个航班的所有舱位）
        /// </summary>
        /// <param name="flight">航班对象</param>
        /// <param name="returnDate">返程日期</param>
        /// <param name="purchaser">采购者 Id</param>
        public static IEnumerable<MatchedBunk> MatchRoundTripDepartureFlight(OriginalFlight flight, DateTime returnDate,
                                                                             Guid purchaser, Guid? publisher = null)
        {
            DateTime flightDate = flight.FlightDate;
            DateTime departureTIme = flightDate.AddHours(flight.TakeoffTime.Hour).AddMinutes(flight.TakeoffTime.Minute);
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure.Code,
                                                                           flight.Arrival.Code, flightDate,
                                                                           VoyageType.RoundTrip);
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            var carrier = flight.Airline;
            var phs =env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(carrier)).ToList();

            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            Dictionary<Guid, WorkingHoursView> workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var normalDefaultPolicy = PolicyManageService.GetNormalDefaultPolicy(flight.Airline);

            var airportpair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);

            List<PolicyInfoBase> policies = DataCenter.Instance.QueryPolicies(flight.Airline, flight.Departure.Code, flight.FlightDate.Date, VoyageType.RoundTrip | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain)
                .FilterByProvider(ProviderFilterType.Exclude, purchaser).ToList();
                // 排除自己的政策
            if (publisher.HasValue)
            {
                policies = policies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                    // 查询指定供应商政策
            }
            policies = policies.FilterByAccount(vas)
                .FilterByWorkingHours(workingPublishers)
                .FilterByVoyageType(VoyageType.RoundTrip)
                .FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain)
                .FilterByAirline(flight.Airline)
                .FilterByDeparture(1, flight.Departure.Code)
                .FilterByArrival(1, flight.Arrival.Code)
                .FilterByPrintDate()
                .FilterByFlightDate(flightDate)
                .FilterByBeforehandDays(flightDate)
                .FilterByAirway(flight.Departure.Code, flight.Arrival.Code)
                .FilterByTravelDays(flightDate, returnDate)
                .FilterByFlightNumber(1, flight.FlightNo)
                .FilterByPrice(flight.StandardPrice)
                .FilterByNoPublishPrice()
                .FilterByPermission(sup)
                .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(departureTIme))
                .ToList();

            // 处理公司组限制
            var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);
            var limitation = GetPurchaseLimitationInfo(limitationGroup , carrier,flight.Departure.Code, PolicyType.Normal);

            policies = policies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

            var airportPair = new AirportPair(flight.Departure.Code, flight.Arrival.Code);
            // 2013-01-26 deng.zhao 单条贴扣点的调整,往返，传入0；
            return (from b in flight.Bunks
                    let cp = 
                        policies.FilterByBunk(b).Select(
                            p =>
                            Calculator.ComputePolicy(p, b, null, purchaser, sup, env.PolicySettings, env.PolicyHarmonies,
                                                     false, PassengerType.Adult,0))
                    // 2013-03-28 deng.zhao 处理OEM扣点；
                    let pod = ProcessIncomeDeduction(cp, setting, carrier, airportpair, flightDate)
                    let ps = ProcessAllHarmony(pod, sup, phs)
                    let lowestPolicy = b is GeneralBunk
                                           ? ps.Where(p => p.PolicyType == PolicyType.Normal).MinOrDefault(
                                               p => p.SettleAmount)
                                           : ps.Where(p => p.PolicyType == PolicyType.Bargain).MinOrDefault(
                                               p => p.SettleAmount)
                    select new MatchedBunk
                               {
                                   OriginalBunk = b,
                                   Policies = lowestPolicy == null
                                                  ? (
                                                        b is GeneralBunk
                                                            ? new[]
                                                                  {
                                                                      limitation != null
                                                                          ? Calculator.ComputeOwnerDefaultPolicy(
                                                                              PolicyManageService.GetOwnerDefaultPolicy(
                                                                                  limitation, sup.Id), b.Owner.StandardPrice,
                                                                              (b as GeneralBunk).Discount, sup,
                                                                              PassengerType.Adult)
                                                                          : Calculator.ComputeNormalDefaultPolicy(
                                                                              normalDefaultPolicy,
                                                                              b.Owner.StandardPrice,
                                                                              (b as GeneralBunk).Discount, sup,
                                                                              PassengerType.Adult)
                                                                  }
                                                            : emptyPolicies
                                                    )
                                                  : new[] {lowestPolicy}
                               }).ToList();
        }

        /// <summary>
        /// 航班查询（往返回程，用于航班列表显示，只填充最低价格）
        /// </summary>
        /// <param name="departureBunk"> </param>
        /// <param name="returnFlights">回程航班列表</param>
        /// <param name="purchaser">采购者 Id</param>
        public static IEnumerable<MatchedFlight> MatchRoundTripReturnFlights(OriginalBunk departureBunk,
                                                                             IEnumerable<OriginalFlight> returnFlights,
                                                                             Guid purchaser, Guid? publisher = null)
        {
            if (!returnFlights.Any()) yield break;
            Flight departureFlight = departureBunk.Owner;
            Flight returnRefer = returnFlights.First();
            var flightDate = returnRefer.FlightDate;
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(departureFlight.Airline,
                                                                           departureFlight.Departure.Code,
                                                                           departureFlight.Arrival.Code,
                                                                           departureFlight.FlightDate,
                                                                           VoyageType.RoundTrip);
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);
            Dictionary<Guid, WorkingHoursView> workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);

            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();
            // 获取所有普通默认政策
            var normalDefaultPolicies = PolicyManageService.GetAllDefaultPolicies().ToDictionary(p => p.Airline);

            List<PolicyInfoBase> policies = DataCenter.Instance.QueryPolicies(departureFlight.Airline, departureFlight.Departure.Code, departureFlight.FlightDate.Date, VoyageType.RoundTrip | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain)
                .FilterByProvider(ProviderFilterType.Exclude, purchaser).ToList();
                // 排除自己的政策
            if (publisher.HasValue)
            {
                policies = policies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                    // 查询指定供应商政策
            }
            policies = policies.FilterByAccount(vas)
                .FilterByWorkingHours(workingPublishers)
                .FilterByVoyageType(VoyageType.RoundTrip)
                .FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain)
                .FilterByDeparture(1, departureFlight.Departure.Code)
                .FilterByArrival(1, departureFlight.Arrival.Code)
                .FilterByPrintDate()
                .FilterByFlightDate(departureFlight.FlightDate)
                .FilterByBeforehandDays(departureFlight.FlightDate)
                .FilterByAirway(departureFlight.Departure.Code, departureFlight.Arrival.Code)
                .FilterByTravelDays(departureFlight.FlightDate, returnRefer.FlightDate)
                .FilterByNoPublishPrice()
                .FilterByPermission(sup)
                .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                .ToList();

            foreach (Flight flight in returnFlights)
            {
                var takeoffTime = flightDate.AddHours(flight.TakeoffTime.Hour).AddMinutes(flight.TakeoffTime.Minute);

                List<PolicyInfoBase> subGernalPolicies = policies.FilterByAirline(flight.Airline)
                    .FilterByFlightNumber(1, flight.FlightNo)
                    .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(takeoffTime))
                    .FilterByPrice(flight.StandardPrice).ToList();

                // 处理公司组限制
                subGernalPolicies = subGernalPolicies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

                // 注意最后一个参数，由它控制是否贴点；
                IEnumerable<MatchedPolicy> matchedBunks =
                    flight.Bunks.Select(
                        bunk =>
                        GetLowestPolicyOfBunk(subGernalPolicies, new[] { departureBunk, bunk }, purchaser, sup, env, limitationGroup, allSettingOnPolicies, normalDefaultPolicies, false, setting))
                        .Where(p => p != null);
                MatchedPolicy lowest = matchedBunks.MinOrDefault(p => p.SettleAmount);

                if (lowest == null)
                    yield return
                        new MatchedFlight {LowestPrice = -1, OriginalFlight = flight, PolicyType = PolicyType.Unknown};
                else
                {
                    yield return new MatchedFlight{LowestPrice = lowest.SettleAmount, OriginalFlight = flight, PolicyType = lowest.PolicyType};
                }
            }
        }

        /// <summary>
        /// 航班查询（往返回程，显示一个航班的所有舱位）
        /// </summary>
        /// <param name="departureBunk"> </param>
        /// <param name="returnFlight">回程航班</param>
        /// <param name="purchaser">采购者 Id</param>
        public static IEnumerable<MatchedBunk> MatchRoundTripReturnFlight(OriginalBunk departureBunk,
                                                                          OriginalFlight returnFlight, Guid purchaser,
                                                                          Guid? publisher = null)
        {
            Flight departureFlight = departureBunk.Owner;
            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(departureFlight.Airline,
                                                                           departureFlight.Departure.Code,
                                                                           departureFlight.Arrival.Code,
                                                                           departureFlight.FlightDate,
                                                                           VoyageType.RoundTrip);
            var sup = DataCenter.Instance.QuerySuperior(purchaser);
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            Dictionary<Guid, WorkingHoursView> workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var normalDefaultPolicy = PolicyManageService.GetNormalDefaultPolicy(returnFlight.Airline);

            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, purchaser);
            var airportpair = new AirportPair(returnFlight.Departure.Code, returnFlight.Arrival.Code);
            var carrier = returnFlight.Airline;
            var flightDate = returnFlight.FlightDate;
            var phs = env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(carrier)).ToList();

            var departureTime = flightDate.AddHours(departureFlight.TakeoffTime.Hour).AddMinutes(departureFlight.TakeoffTime.Minute);

            List<PolicyInfoBase> policies = DataCenter.Instance.QueryPolicies(departureFlight.Airline, departureFlight.Departure.Code, departureFlight.FlightDate.Date, VoyageType.RoundTrip | VoyageType.OneWayOrRound, PolicyType.Normal | PolicyType.Bargain)
                .FilterByProvider(ProviderFilterType.Exclude, purchaser).ToList();
                // 排除自己的政策
            if (publisher.HasValue)
            {
                policies = policies.FilterByProvider(ProviderFilterType.Include, publisher.Value).ToList();
                    // 查询指定供应商政策
            }
            policies = policies.FilterByAccount(vas)
                .FilterByWorkingHours(workingPublishers)
                .FilterByVoyageType(VoyageType.RoundTrip)
                .FilterByPolicyType(PolicyType.Normal | PolicyType.Bargain)
                .FilterByAirline(departureFlight.Airline)
                .FilterByDeparture(1, departureFlight.Departure.Code)
                .FilterByArrival(1, departureFlight.Arrival.Code)
                .FilterByPrintDate()
                .FilterByFlightDate(departureFlight.FlightDate)
                .FilterByBeforehandDays(departureFlight.FlightDate)
                .FilterByAirway(departureFlight.Departure.Code, departureFlight.Arrival.Code)
                .FilterByTravelDays(departureFlight.FlightDate, returnFlight.FlightDate)
                .FilterByFlightNumber(1, departureFlight.FlightNo)
                .FilterByFlightNumber(2, returnFlight.FlightNo)
                .FilterByPrice(returnFlight.StandardPrice)
                .FilterByNoPublishPrice()
                .FilterByPermission(sup)
                .FilterByPriceTypeForbargin() // 2012-11-13 去除特价政策中的价格类型为按返佣的数据；
                .FilterByAllowTicketType(GetAllowTicketTypeByDepartureTime(departureTime))
                .ToList();

            // 处理公司组限制
            var limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(sup.Id, purchaser);
            var limitation = GetPurchaseLimitationInfo(limitationGroup, carrier, returnFlight.Departure.Code, PolicyType.Normal);

            policies = policies.FilterByGroupLimitation(limitationGroup, sup.Id).ToList();

            var airportPair = new AirportPair(departureFlight.Departure.Code, departureFlight.Arrival.Code);
            return (from b in returnFlight.Bunks
                    let pss =
                        env.PolicySettings.Where(
                            ps => ps.Berths.Contains(departureBunk.Code) && ps.Berths.Contains(b.Code))
                    //2013-01-26 deng.zhao，由于函数新增了参数，要求传入单条政策贴扣点，但往返时不处理贴扣点，传入的是0；
                    let cp =
                        policies.FilterByBunk(b).Select(
                            p =>
                            Calculator.ComputePolicy(p, b, null, purchaser, sup, pss, env.PolicyHarmonies, false,
                                                     PassengerType.Adult, 0))
                    // 2013-03-28 deng.zhao 处理OEM扣点；
                    let pod = ProcessIncomeDeduction(cp, setting, carrier, airportpair, flightDate)
                    let ps = ProcessAllHarmony(pod, sup, phs)
                    let lowestPolicy = b is GeneralBunk
                                           ? ps.Where(p => p.PolicyType == PolicyType.Normal).MinOrDefault(
                                               p => p.SettleAmount)
                                           : ps.Where(p => p.PolicyType == PolicyType.Bargain).MinOrDefault(
                                               p => p.SettleAmount)
                    select new MatchedBunk
                               {
                                   OriginalBunk = b,
                                   Policies = lowestPolicy == null
                                                  ? (
                                                        b is GeneralBunk
                                                            ? new[]
                                                                  {
                                                                      limitation != null
                                                                          ? Calculator.ComputeOwnerDefaultPolicy(
                                                                              PolicyManageService.GetOwnerDefaultPolicy(
                                                                                  limitation, sup.Id), b.Owner.StandardPrice,
                                                                              (b as GeneralBunk).Discount, sup,
                                                                              PassengerType.Adult)
                                                                          : Calculator.ComputeNormalDefaultPolicy(
                                                                              normalDefaultPolicy,
                                                                              b.Owner.StandardPrice,
                                                                              (b as GeneralBunk).Discount, sup,
                                                                              PassengerType.Adult)
                                                                  }
                                                            : emptyPolicies
                                                    )
                                                  : new[] {lowestPolicy}
                               }).ToList();
        }

        #endregion

        #region 政策查看

        public static PagedResult<MatchedPolicy> GetNormalPolicies<TKey>(Guid company, PolicyQueryParameter parameter,
                                                                         Expression<Func<MatchedPolicy, TKey>> orderBy,
                                                                         OrderMode orderMode = OrderMode.Ascending)
        {
            if (string.IsNullOrWhiteSpace(parameter.Airline)) throw new InvalidOperationException("必须输入航空公司");
            if (string.IsNullOrWhiteSpace(parameter.Departure)) throw new InvalidOperationException("必须输入出发地");
            if (string.IsNullOrWhiteSpace(parameter.Arrival)) throw new InvalidOperationException("必须输入到达地");
            if (parameter.DepartureDateStart == null) throw new InvalidOperationException("必须输入航班日期");

            string airline = parameter.Airline.Trim();
            string departure = parameter.Departure.Trim();
            string arrival = parameter.Arrival.Trim();
            DateTime flightDate = parameter.DepartureDateStart.Value;
            VoyageType voyageType;
            if(parameter.VoyageType.HasValue)
            {
                switch (parameter.VoyageType.Value)
                {
                    case VoyageType.OneWay:
                        voyageType = VoyageType.OneWay | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.RoundTrip:
                        voyageType = VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.OneWayOrRound:
                        voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    default:
                        voyageType = parameter.VoyageType.Value;
                        break;
                }
            }else {
                voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound | VoyageType.TransitWay | VoyageType.Notch;
            }
            var pager = new Pager<MatchedPolicy, TKey>
                            {
                                OrderBy = orderBy,
                                OrderMode = orderMode,
                                PageIndex = parameter.PageIndex,
                                PageSize = parameter.PageSize
                            };

            List<NormalPolicyInfo> policies = (from NormalPolicyInfo p in DataCenter.Instance.QueryPolicies(airline, departure, flightDate, voyageType, PolicyType.Normal)
                                               where p.Arrival.Contains(arrival) &&
                                                     (p.Owner == company || p.IsPeer) &&
                                                     (parameter.TicketType == null || p.TicketType == parameter.TicketType)
                                               select p).ToList();

            if (!policies.Any())
            {
                return pager.Paging(emptyPolicies.AsQueryable());
            }

            // 政策设置
            IEnumerable<PolicySettingInfo> deductions = (parameter.VoyageType == null ||
                                                         parameter.VoyageType == VoyageType.OneWay ||
                                                         parameter.VoyageType == VoyageType.OneWayOrRound)
                                                            ? PolicyManageService.GetDeductions().Where(
                                                                d =>
                                                                d.Airline == airline && d.Departure == departure &&
                                                                d.Arrivals.Contains(arrival)
                                                                && d.EffectiveTimeStart.Date <= flightDate &&
                                                                flightDate <= d.EffectiveTimeEnd)
                                                            : EnumerableHelper.GetEmpty<PolicySettingInfo>();
            // 政策协调
            IEnumerable<PolicyHarmonyInfo> harmories =
                PolicyManageService.GetAllPolicyHarmonies().Where(
                    h => h.Airlines.Contains(airline) && h.Departure.Contains(departure)
                         && h.Arrival.Contains(arrival) && (h.PolicyType & PolicyType.Normal) == PolicyType.Normal &&
                         h.DeductionType == DeductionType.Profession
                         && h.EffectiveLowerDate.Date <= flightDate && flightDate <= h.EffectiveUpperDate.Date);

            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();

            List<MatchedPolicy> matchPolicies = (from NormalPolicyInfo policy in policies
                                                 let commission =
                                                     Calculator.GetCommission(policy, DeductionType.Profession)
                                                 let splitedPolicies =
                                                     policy.Owner == company
                                                         ? new[] {policy}
                                                         : PolicySpliter.Execute(policy)
                                                 from sp in splitedPolicies
                                                 where
                                                     parameter.VoyageType == null ||
                                                     parameter.VoyageType == sp.VoyageType ||
                                                     ((parameter.VoyageType == VoyageType.OneWay ||
                                                       parameter.VoyageType == VoyageType.RoundTrip) &&
                                                      sp.VoyageType == VoyageType.OneWayOrRound) ||
                                                     (parameter.VoyageType == VoyageType.OneWayOrRound &&
                                                      (sp.VoyageType == VoyageType.OneWay ||
                                                       sp.VoyageType == VoyageType.RoundTrip))
                                                 let matchedDeductions =
                                                     (sp.Owner == company || sp.VoyageType != VoyageType.OneWay)
                                                         ? null
                                                         : deductions.Where(
                                                             d => sp.Berths.Any(b => d.Berths.Contains(b)))
                                                 let saleCommission =
                                                     commission +
                                                     (matchedDeductions == null
                                                          ? 0
                                                          : Calculator.GetSinglePolicySubsidyAndDeduction(allSettingOnPolicies, policy.Id,
                                                                                                          new AirportPair
                                                                                                              (departure,
                                                                                                               arrival),
                                                                                                          null,
                                                                                                          flightDate))
                                                 let deduction =
                                                     matchedDeductions == null
                                                         ? 0
                                                         : Calculator.GetGlobalPolicySettingValue(
                                                             DeductionType.Profession, saleCommission, matchedDeductions)
                                                 let buyCommission =
                                                     deduction < 0
                                                         ? (-deduction < saleCommission ? saleCommission : -deduction)
                                                         : (saleCommission > deduction ? saleCommission - deduction : 0)
                                                 let harmonyValue =
                                                     (sp.Owner == company || sp.VoyageType != VoyageType.OneWay)
                                                         ? 0
                                                         : Calculator.GetHarmonyValue(sp, DeductionType.Profession,
                                                                                      harmories)
                                                 select new MatchedPolicy
                                                            {
                                                                Commission =
                                                                    Calculator.GetHarmonyCommission(buyCommission,
                                                                                                    harmonyValue),
                                                                Provider = policy.Owner,
                                                                Deduction = saleCommission - buyCommission,
                                                                Id = policy.Id,
                                                                PolicyType = policy.PolicyType,
                                                                Rebate = saleCommission,
                                                                SettleAmount = -1,
                                                                OriginalPolicy = sp
                                                            }).ToList();

            return pager.Paging(matchPolicies.AsQueryable());
        }

        //团队政策查看 2012 10 19 wangshiling
        public static PagedResult<MatchedPolicy> GetTeamPolicies<TKey>(Guid company, PolicyQueryParameter parameter,
                                                                       Expression<Func<MatchedPolicy, TKey>> orderBy,
                                                                       OrderMode orderMode = OrderMode.Ascending)
        {
            if (string.IsNullOrWhiteSpace(parameter.Airline)) throw new InvalidOperationException("必须输入航空公司");
            if (string.IsNullOrWhiteSpace(parameter.Departure)) throw new InvalidOperationException("必须输入出发地");
            if(string.IsNullOrWhiteSpace(parameter.Arrival)) throw new InvalidOperationException("必须输入到达地");
            if(parameter.DepartureDateStart == null) throw new InvalidOperationException("必须输入航班日期");

            string airline = parameter.Airline.Trim();
            string departure = parameter.Departure.Trim();
            string arrival = parameter.Arrival.Trim();
            DateTime flightDate = parameter.DepartureDateStart.Value;
            VoyageType voyageType;
            if(parameter.VoyageType.HasValue) {
                switch(parameter.VoyageType.Value) {
                    case VoyageType.OneWay:
                        voyageType = VoyageType.OneWay | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.RoundTrip:
                        voyageType = VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.OneWayOrRound:
                        voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    default:
                        voyageType = parameter.VoyageType.Value;
                        break;
                }
            } else {
                voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound | VoyageType.TransitWay | VoyageType.Notch;
            }

            List<TeamPolicyInfo> policies = (from TeamPolicyInfo p in DataCenter.Instance.QueryPolicies(airline, departure, flightDate, voyageType, PolicyType.Team)
                                             where p.Arrival.Contains(arrival) &&
                                                   (p.Owner == company || p.IsPeer) &&
                                                   (parameter.TicketType == null || p.TicketType == parameter.TicketType)
                                             select p).ToList();
            IEnumerable<MatchedPolicy> matchPolicies = from TeamPolicyInfo policy in policies
                                                       let saleCommission =
                                                           Calculator.GetCommission(policy, DeductionType.Profession)
                                                       where saleCommission >= 0
                                                       select new MatchedPolicy
                                                                  {
                                                                      Commission = saleCommission,
                                                                      Provider = policy.Owner,
                                                                      Deduction = 0,
                                                                      Id = policy.Id,
                                                                      PolicyType = policy.PolicyType,
                                                                      Rebate = saleCommission,
                                                                      SettleAmount = -1,
                                                                      OriginalPolicy = policy
                                                                  };
            var pager = new Pager<MatchedPolicy, TKey>
                            {
                                OrderBy = orderBy,
                                OrderMode = orderMode,
                                PageIndex = parameter.PageIndex,
                                PageSize = parameter.PageSize
                            };
            return pager.Paging(matchPolicies.AsQueryable());
        }

        public static PagedResult<MatchedPolicy> GetBargainPolicies<TKey>(Guid company, PolicyQueryParameter parameter,
                                                                          Expression<Func<MatchedPolicy, TKey>> orderBy,
                                                                          OrderMode orderMode = OrderMode.Ascending)
        {
            if (string.IsNullOrWhiteSpace(parameter.Airline)) throw new InvalidOperationException("必须输入航空公司");
            if (string.IsNullOrWhiteSpace(parameter.Departure)) throw new InvalidOperationException("必须输入出发地");
            if(string.IsNullOrWhiteSpace(parameter.Arrival)) throw new InvalidOperationException("必须输入到达地");
            if(parameter.DepartureDateStart == null) throw new InvalidOperationException("必须输入航班日期");

            string airline = parameter.Airline.Trim();
            string departure = parameter.Departure.Trim();
            string arrival = parameter.Arrival.Trim();
            DateTime flightDate = parameter.DepartureDateStart.Value;
            VoyageType voyageType;
            if(parameter.VoyageType.HasValue) {
                switch(parameter.VoyageType.Value) {
                    case VoyageType.OneWay:
                        voyageType = VoyageType.OneWay | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.RoundTrip:
                        voyageType = VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    case VoyageType.OneWayOrRound:
                        voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                        break;
                    default:
                        voyageType = parameter.VoyageType.Value;
                        break;
                }
            } else {
                voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound | VoyageType.TransitWay | VoyageType.Notch;
            }
            var pager = new Pager<MatchedPolicy, TKey>
                            {
                                OrderBy = orderBy,
                                OrderMode = orderMode,
                                PageIndex = parameter.PageIndex,
                                PageSize = parameter.PageSize
                            };

            List<BargainPolicyInfo> policies = (from BargainPolicyInfo p in DataCenter.Instance.QueryPolicies(airline, departure, flightDate, voyageType, PolicyType.Bargain)
                                                where p.Arrival.Contains(arrival) &&
                                                      (p.Owner == company || p.IsPeer) &&
                                                      (parameter.TicketType == null || p.TicketType == parameter.TicketType)
                                                select p).ToList();

            if (!policies.Any())
            {
                return pager.Paging(emptyPolicies.AsQueryable());
            }

            // 政策协调
            IEnumerable<PolicyHarmonyInfo> harmories =
                PolicyManageService.GetAllPolicyHarmonies().Where(
                    h => h.Airlines.Contains(airline) && h.Departure.Contains(departure)
                         && h.Arrival.Contains(arrival) && (h.PolicyType & PolicyType.Bargain) == PolicyType.Bargain &&
                         h.DeductionType == DeductionType.Profession
                         && h.EffectiveLowerDate.Date <= flightDate && flightDate <= h.EffectiveUpperDate.Date);

            IEnumerable<MatchedPolicy> matchPolicies = from BargainPolicyInfo policy in policies
                                                       let saleCommission =
                                                           Calculator.GetCommission(policy, DeductionType.Profession)
                                                       let harmonyValue =
                                                           Calculator.GetHarmonyValue(policy, DeductionType.Profession,
                                                                                      harmories)
                                                       select new MatchedPolicy
                                                                  {
                                                                      Commission =
                                                                          Calculator.GetHarmonyCommission(
                                                                              saleCommission, harmonyValue),
                                                                      Provider = policy.Owner,
                                                                      Deduction = 0,
                                                                      Id = policy.Id,
                                                                      PolicyType = policy.PolicyType,
                                                                      Rebate = saleCommission,
                                                                      SettleAmount = -1,
                                                                      OriginalPolicy = policy
                                                                  };
            return pager.Paging(matchPolicies.AsQueryable());
        }

        public static PagedResult<MatchedPolicy> GetSpecialPolicies<TKey>(Guid company, PolicyQueryParameter parameter,
                                                                          Expression<Func<MatchedPolicy, TKey>> orderBy,
                                                                          OrderMode orderMode = OrderMode.Ascending)
        {
            if (string.IsNullOrWhiteSpace(parameter.Airline)) throw new InvalidOperationException("必须输入航空公司");
            if (string.IsNullOrWhiteSpace(parameter.Departure)) throw new InvalidOperationException("必须输入出发地");
            if (string.IsNullOrWhiteSpace(parameter.Arrival)) throw new InvalidOperationException("必须输入到达地");
            if(parameter.DepartureDateStart == null) throw new InvalidOperationException("必须输入航班日期");

            string airline = parameter.Airline.Trim();
            string departure = parameter.Departure.Trim();
            string arrival = parameter.Arrival.Trim();
            DateTime flightDate = parameter.DepartureDateStart.Value;

            List<SpecialPolicyInfo> policies = (from SpecialPolicyInfo p in DataCenter.Instance.QueryPolicies(airline, departure, flightDate, VoyageType.OneWay, PolicyType.Special)
                                                where p.Arrival.Contains(arrival) &&
                                                      (p.Owner == company || p.IsPeer) &&
                                                      p.PriceType == PriceType.Price
                                                select p).ToList();
            IEnumerable<MatchedPolicy> matchPolicies = from SpecialPolicyInfo p in policies
                                                       let saleCommission =
                                                           Calculator.GetCommission(p, DeductionType.Profession)
                                                       let settleAmount =
                                                           Calculator.ComputeSpecialSettlementPrice(p, 0, 0,
                                                                                                    DeductionType.
                                                                                                        Profession)
                                                       where settleAmount > 0
                                                       select new MatchedPolicy
                                                                  {
                                                                      Commission = 0,
                                                                      Provider = p.Owner,
                                                                      Deduction = 0,
                                                                      Id = p.Id,
                                                                      PolicyType = p.PolicyType,
                                                                      Rebate = 0,
                                                                      SettleAmount = settleAmount,
                                                                      OriginalPolicy = p
                                                                  };
            var pager = new Pager<MatchedPolicy, TKey>
                            {
                                OrderBy = orderBy,
                                OrderMode = orderMode,
                                PageIndex = parameter.PageIndex,
                                PageSize = parameter.PageSize
                            };
            return pager.Paging(matchPolicies.AsQueryable());
        }

        public static PagedResult<MatchedPolicy> GetNotchPolicies<TKey>(Guid company, PolicyQueryParameter parameter,
                                                                          Expression<Func<MatchedPolicy, TKey>> orderBy,
                                                                          OrderMode orderMode = OrderMode.Ascending)
        {
            if (string.IsNullOrWhiteSpace(parameter.Airline)) throw new InvalidOperationException("必须输入航空公司");
            if (string.IsNullOrWhiteSpace(parameter.Departure)) throw new InvalidOperationException("必须输入出发地");
            if (string.IsNullOrWhiteSpace(parameter.Arrival)) throw new InvalidOperationException("必须输入到达地");
            if (parameter.DepartureDateStart == null) throw new InvalidOperationException("必须输入航班日期");

            var airline = parameter.Airline.Trim();
            var departure = parameter.Departure.Trim();
            var arrival = parameter.Arrival.Trim();
            var flightDate = parameter.DepartureDateStart.Value;

            // 这里不一定对呢。
            //List<NotchPolicyInfo> policies = (from NotchPolicyInfo p in DataCenter.Instance.QueryPolicies(airline, departure, flightDate, VoyageType.Notch, PolicyType.Notch)
            //                                    where p.Arrival.Contains(arrival) &&
            //                                          (p.Owner == company || p.IsPeer) &&
            //                                          p.PriceType == PriceType.Price
            //                                    select p).ToList();

            //IEnumerable<MatchedPolicy> matchPolicies = from NotchPolicyInfo p in policies
            //                                           let saleCommission =
            //                                               Calculator.GetCommission(p, DeductionType.Profession)
            //                                           let settleAmount =
            //                                               Calculator.ComputeSpecialSettlementPrice(p, 0, 0,
            //                                                                                        DeductionType.
            //                                                                                            Profession)
            //                                           where settleAmount > 0
            //                                           select new MatchedPolicy
            //                                           {
            //                                               Commission = 0,
            //                                               Provider = p.Owner,
            //                                               Deduction = 0,
            //                                               Id = p.Id,
            //                                               PolicyType = p.PolicyType,
            //                                               Rebate = 0,
            //                                               SettleAmount = settleAmount,
            //                                               OriginalPolicy = p
            //                                           };
            //var pager = new Pager<MatchedPolicy, TKey>
            //{
            //    OrderBy = orderBy,
            //    OrderMode = orderMode,
            //    PageIndex = parameter.PageIndex,
            //    PageSize = parameter.PageSize
            //};
            //return pager.Paging(matchPolicies.AsQueryable());

            throw  new NotImplementedException();
        }


        #endregion

        #region 政策选择

        /// <summary>
        /// 根据给出的条件，获取匹配的政策。
        /// </summary>
        /// <param name="filter">政策过滤条件</param>
        /// <param name="needExternalPolicy">外部政策是否需要贴点</param>
        /// <param name="passengerType">乘客类型</param>
        /// <param name="count">返回政策数量</param>
        /// <returns>匹配后的政策列表</returns>
        /// <remarks>
        /// 航班查询、编码导入和换出票方时都会调用；
        /// </remarks>
        public static IEnumerable<MatchedPolicy> MatchBunk(PolicyFilterConditions filter, bool needExternalPolicy,
                                                           PassengerType passengerType = PassengerType.Adult,
                                                           int count = 5)
        {
            // 共享航班的选择在各自的方法里判断
            IEnumerable<MatchedPolicy> policies = null;
            switch (passengerType)
            {
                case PassengerType.Adult:
                    policies = MatchBunkForAdult(filter, needExternalPolicy, count).ToList();
                    break;
                case PassengerType.Child:
                    policies = MatchBunkForChild(filter, count).ToList();
                    break;
            }

            // 这里的价格替换，如果是特殊政策，那么其舱位可为空，
            if (!filter.IsUsePatPrice && filter.PolicyType != PolicyType.Special)
            {
                policies = ReplaceParValue(policies, filter.Voyages).ToList();
            }
            
            return policies;
        }

        /// <summary>
        /// 根据给出的条件，获取匹配的政策。
        /// </summary>
        /// <param name="filter">政策过滤条件</param>
        /// <param name="isExternalPolicy">采用内部政策还是外部政策</param>
        /// <param name="count">返回政策数量</param>
        /// <returns>匹配后的政策</returns>
        /// <remarks>
        /// 1、三方关系时，产品方提供座位后找出票方出票的专用匹配；
        /// 2、由于是特殊政策处理；
        ///     不需要做贴点操作；
        ///     不需要处理乘客类型；
        /// </remarks>
        public static IEnumerable<MatchedPolicy> MatchBunkForSpecial(PolicyFilterConditions filter, bool isExternalPolicy, int count = 5)
        {
            return isExternalPolicy
                       ? GetExternalPoliciesForTripartite(filter).Take(count)
                       : GetInternalPoliciesForTripartite(filter).Take(count);
        }

        /// <summary>
        /// 为选定政策给出默认政策（无贴点）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="flight"></param>
        /// <param name="limitation"></param>
        /// <param name="superior"></param>
        /// <param name="passengerType"> </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<MatchedPolicy> matchDefaultPolicyForChoosePolicy(PolicyFilterConditions filter,
                                                                                    FlightFilterInfo flight,
                                                                                    PurchaseLimitationInfo limitation,
                                                                                    SuperiorInfo superior,
                                                                                    PassengerType passengerType)
        {
            var result = new List<MatchedPolicy>();
            // 采买限制不为空，且旅客类型为成人；
            if (limitation != null && passengerType == PassengerType.Adult)
            {
                MatchedPolicy odp =
                    Calculator.ComputeOwnerDefaultPolicy(PolicyManageService.GetOwnerDefaultPolicy(limitation, superior.Id),
                                                         filter.Voyages, superior, filter.PatPrice, passengerType);
                result.Add(SettingMatchPolicy(odp, flight));
            }
            else
            {
                // 到这里的都是普通和特价，没有对舱位做验证了；
                BunkType bunkType = filter.Voyages.First().Bunk.Type;
                // 若无公司组设置，匹配普通或是特价默认政策
                if ((filter.PolicyType & PolicyType.Normal) == PolicyType.Normal
                    // 团队政策，根据舱位判断；
                    ||
                    ((filter.PolicyType & PolicyType.Team) == PolicyType.Team &&
                     (bunkType == BunkType.Economic || bunkType == BunkType.FirstOrBusiness))
                    || (filter.PolicyType & PolicyType.NormalDefault) == PolicyType.NormalDefault
                    || (filter.PolicyType & PolicyType.Notch) == PolicyType.Notch)
                {
                    MatchedPolicy ndp =
                        Calculator.ComputeNormalDefaultPolicy(
                            PolicyManageService.GetNormalDefaultPolicy(flight.Airline),
                            filter.Voyages, superior, filter.PatPrice, passengerType);
                    result.Add(SettingMatchPolicy(ndp, flight));
                }
                else if ((filter.PolicyType & PolicyType.Bargain) == PolicyType.Bargain
                         || ((filter.PolicyType & PolicyType.Team) == PolicyType.Team && bunkType == BunkType.Promotion)
                         || (filter.PolicyType & PolicyType.BargainDefault) == PolicyType.BargainDefault)
                {
                    // 匹配特价默认政策，不一定可以匹配得到；
                    BargainDefaultPolicyInfo b = PolicyManageService.GetBargainDefaultPolicy(flight.Airline,
                                                                                             FoundationService.
                                                                                                 QueryPrvinceCodeByAirport
                                                                                                 (flight.Departure));
                    if (b != null)
                    {
                        //针对接口预定生成订单需要判断 filter.PatPrice 是否为空 2013年4月27日 wangsl
                        if (filter.PatPrice != null)
                        {
                            MatchedPolicy bdp = Calculator.ComputeBargainDefaultPolicy(b, filter.Voyages, superior,
                                                                                       filter.PatPrice.Value);
                            result.Add(SettingMatchPolicy(bdp, flight));
                        }
                    }
                }
            }

            return result;
        }
        
        /// <summary>
        /// 为儿童匹配舱位（儿童不能购买特价或特殊票）
        /// </summary>
        /// <param name="filter">政策过滤条件</param>
        /// <param name="count">显示条数</param>
        /// <returns>匹配的政策列表</returns>
        private static IEnumerable<MatchedPolicy> MatchBunkForChild(PolicyFilterConditions filter, int count = 5)
        {
            VoyageFilterInfo first = filter.Voyages.First();
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var sup = DataCenter.Instance.QuerySuperior(filter.Purchaser);

            // 获取指定航空公司下的儿童政策
            List<WorkingSetting> pds = CompanyService.GetChildTicketProviders(first.Flight.Airline)
                .Where(pd => vas.ContainsKey(pd.Company)).ToList(); // 过滤有效收款账号；

            if (pds.Any())
            {
                IEnumerable<MatchedPolicy> matchedPolicies = pds.Select(pd =>
                                                                            {
                                                                                MatchedPolicy matchedPolicy =
                                                                                    Calculator.ComputeChildPolicy(pd,
                                                                                                                  filter
                                                                                                                      .
                                                                                                                      Voyages,
                                                                                                                  sup);
                                                                                SetWorkingHour(matchedPolicy);
                                                                                return matchedPolicy;
                                                                            });
                return matchedPolicies.SortNormalPolicy(first.Flight.Airline).Take(count);
            }
            else
            {
                // 没有政策，则取默认政策
                NormalDefaultPolicyInfo defaultPolicy = PolicyManageService.GetNormalDefaultPolicy(first.Flight.Airline);
                MatchedPolicy matchedPolicy = Calculator.ComputeNormalDefaultPolicy(defaultPolicy, filter.Voyages, sup,
                                                                                    filter.PatPrice, PassengerType.Child);
                SetWorkingHour(matchedPolicy);
                SetSpeed(matchedPolicy, first.Flight.Airline);
                return new[] {matchedPolicy};
            }
        }

        /// <summary>
        /// 为成人舱位匹配政策（所有政策）
        /// </summary>
        /// <param name="filter">政策过滤条件</param>
        /// <param name="isExternalPolicy">采用内部政策还是外部政策</param>
        /// <param name="count">显示条数</param>
        /// <returns>匹配的政策列表</returns>
        private static IEnumerable<MatchedPolicy> MatchBunkForAdult(PolicyFilterConditions filter, bool isExternalPolicy, int count = 5)
        {
            return isExternalPolicy ? GetExternalPolicies(filter).Take(count) : GetInternalPolicies(filter).Take(count);
        }
        
        /// <summary>
        /// 根据给出的过滤条件，获取外部政策；
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>外部政策</returns>
        private static IEnumerable<MatchedPolicy> GetExternalPolicies(PolicyFilterConditions filter)
        {
            var externalPolicies = new List<MatchedPolicy>();
            var voyageType = filter.VoyageType;
            // 只允许单程或往返；
            if (!(voyageType == VoyageType.OneWay || voyageType == VoyageType.OneWayOrRound || voyageType == VoyageType.RoundTrip))
            {
                return externalPolicies;
            }

            // 处理共享航班；
            if (filter.Voyages.Any(p => p.Flight.IsShare))
            {
                return externalPolicies;
            }
            // 1、获取环境变量；
            VoyageFilterInfo voyage = filter.Voyages.First();
            FlightFilterInfo flight = voyage.Flight;
            var sup = DataCenter.Instance.QuerySuperior(filter.Purchaser);
            var airportpair = new AirportPair(flight.Departure, flight.Arrival);
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, filter.Purchaser);

            MatchEnvironment env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure,
                                                                           flight.Arrival,
                                                                           flight.FlightDate, filter.VoyageType);

            // 2、对客票类型进行转换；
            TicketType? ticketType = null;
            bool ticketFlag = true;
            switch (filter.AllowTicketType)
            {
                case AllowTicketType.BSP:
                    ticketType = TicketType.BSP;
                    break;
                case AllowTicketType.B2B:
                    ticketType = TicketType.B2B;
                    break;
                case AllowTicketType.Both:
                case AllowTicketType.B2BOnPolicy:
                    break;
                case AllowTicketType.None:
                    ticketFlag = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // 对行程类型进行转换；
            ChinaPay.B3B.DataTransferObject.Command.PNR.ItineraryType itineraryType;
            switch (voyageType)
            {
                case VoyageType.OneWay:
                    itineraryType = ItineraryType.OneWay;
                    break;
                case VoyageType.RoundTrip:
                    itineraryType = ItineraryType.Roundtrip;
                    break;
                case VoyageType.TransitWay:
                    itineraryType = ItineraryType.Conjunction;
                    break;
                case VoyageType.Notch:
                    itineraryType = ItineraryType.Notch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            // 3、判断客票类型转换是否成功；
            if (ticketFlag)
            {
                // 若成功；
                var externalFliter = new ExternalPolicyFilter
                                         {
                                             B3BMaxRebate = filter.MaxdRebate,
                                             TicketType = ticketType,
                                             VoyageType = itineraryType,
                                             UseBPNR = filter.UseBPNR
                                         };

                // 根据条件，获取外部政策；
                RequestResult<IEnumerable<ExternalPolicyView>> requestResult = PolicyService.Match(filter.PnrPair, filter.PnrContent,
                                                                                               filter.PatContent, externalFliter);
                if (requestResult.Success)
                {
                    externalPolicies = (from p in requestResult.Result
                                                  select Calculator.ComputePolicy(p, filter.PolicyType, sup, env.PolicyHarmonies)).ToList();

                    // 处理往返降舱时，多条不同价格政策的问题；
                    if (filter.Voyages.Count == 2 && filter.PatPrice.HasValue)
                    {
                        externalPolicies =
                            externalPolicies.Where(p => p.ParValue == 0 || p.ParValue == filter.PatPrice.Value).ToList();
                    }

                    // 判断外部政策是否需要贴点；
                    bool needExternalGlobalSubsidy = SystemParamService.NeedExternalGlobalSubsidy;
                    if (needExternalGlobalSubsidy)
                    {
                        // 进行外部政策的全局贴点；
                        externalPolicies = ProcessGlobalSubsidy(externalPolicies, env.PolicySettings).ToList();
                    }
                }
            }

            externalPolicies = ProcessIncomeDeduction(externalPolicies,  setting, flight.Airline, airportpair, flight.FlightDate).ToList();

            return externalPolicies.SortExternalPolicy(flight.Airline);
        }
        
        /// <summary>
        /// 根据给出的过滤条件，为三方关系获取匹配后的外部政策。
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>匹配后的政策列表</returns>
        private static IEnumerable<MatchedPolicy> GetExternalPoliciesForTripartite(PolicyFilterConditions filter)
        {
            var externalPolicies = new List<MatchedPolicy>();
            var sup = DataCenter.Instance.QuerySuperior(filter.Purchaser);
            var voyageType = filter.VoyageType;
            // 只允许单程或往返；
            if (!(voyageType == VoyageType.OneWay || voyageType == VoyageType.OneWayOrRound || voyageType == VoyageType.RoundTrip))
            {
                return externalPolicies;
            }
            // 处理共享航班；
            if (filter.Voyages.Any(p => p.Flight.IsShare))
            {
                return externalPolicies;
            }

            // 对行程类型进行转换；
            ChinaPay.B3B.DataTransferObject.Command.PNR.ItineraryType itineraryType;
            switch (voyageType)
            {
                case VoyageType.OneWay:
                    itineraryType = ItineraryType.OneWay;
                    break;
                case VoyageType.RoundTrip:
                    itineraryType = ItineraryType.Roundtrip;
                    break;
                case VoyageType.TransitWay:
                    itineraryType = ItineraryType.Conjunction;
                    break;
                case VoyageType.Notch:
                    itineraryType = ItineraryType.Notch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var voyage = filter.Voyages.Last();
            var flight = voyage.Flight;
            var airportpair = new AirportPair(flight.Departure, flight.Arrival);
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(sup.Id, filter.Purchaser);

            var externalPolicyFilter = new ExternalPolicyFilter
                                           {
                                               B3BMaxRebate = filter.MaxdRebate,
                                               TicketType = null,
                                               VoyageType = itineraryType,
                                           };

            var requestResult = PolicyService.Match(filter.PnrPair, filter.PnrContent, filter.PatContent, externalPolicyFilter);

            if (requestResult.Success)
            {
                // 平台扣点；
                var deductionForPlatform = SystemParamService.PlatformDeductForSpecial;
                externalPolicies = (from policy in requestResult.Result
                                    select new MatchedPolicy
                                    {
                                        Id = Guid.NewGuid(),
                                        IsExternal = true,
                                        OriginalExternalPolicy = policy,
                                        Provider = policy.Provider,
                                        PolicyType =
                                            Calculator.GetExternalPolicyType(policy.PolicyType,
                                                                             filter.PolicyType),
                                        Rebate = policy.Rebate,
                                        Commission = policy.Rebate,
                                        RelationType = RelationType.Brother,
                                        Deduction = 0,
                                        SettleAmount = policy.ParValue * (1 - policy.Rebate),
                                        ParValue = policy.ParValue,
                                        WorkEnd = policy.WorkEnd,
                                        WorkStart = policy.WorkStart,
                                        RefundStart = policy.ScrapStart,
                                        RefundEnd = policy.ScrapEnd,
                                        OfficeNumber = policy.OfficeNo,
                                        NeedAUTH = policy.RequireAuth,
                                        Speed = new GeneralProductSpeedInfo.Item(policy.ETDZSpeed, null),
                                        CompannyGrade =
                                            CompanyService.GetCompanyParameter(policy.Provider).
                                                Creditworthiness.HasValue
                                                ? CompanyService.GetCompanyParameter(
                                                    policy.Provider).Creditworthiness.Value
                                                : 5,
                                        HarmonyValue = 0,
                                        HasHarmony = false
                                    }).ToList();

                // 对外部政策做操作（不做贴点和政策协调）；
                externalPolicies = (from p in externalPolicies
                                    where p.PolicyType == PolicyType.Normal
                                    select Calculator.ComputeSpecialPolicy(p, deductionForPlatform)).ToList();
            }

            // 2013-04-02 此处不做扣点；
            externalPolicies = ProcessIncomeDeduction(externalPolicies,  setting, flight.Airline, airportpair, flight.FlightDate).ToList();

            return externalPolicies.SortExternalPolicy(flight.Airline);
        }

        /// <summary>
        /// 根据给出的过滤条件，获取内部政策；
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>内部政策</returns>
        private static IEnumerable<MatchedPolicy> GetInternalPolicies(PolicyFilterConditions filter)
        {
            // 1、获取环境等相关信息；
            // a、以下为获取航班相关信息；
            // 取得行程中的第一程；
            var voyage = filter.Voyages.First();
            // 取得第一程的航班信息；
            var flight = voyage.Flight;
            // 取得第一程的港口对信息；
            var airportpair = new AirportPair(flight.Departure, flight.Arrival);
            // 取得贴点、协调等设置信息；
            var env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure, flight.Arrival, flight.FlightDate, filter.VoyageType);
            // 取得政策协调信息；
            var phs = env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(flight.Airline)).ToList();

            // b、以下为获取采购及上级信息；
            // 获取当前采购信息；
            var purchaserId = filter.Purchaser;
            // 取得上级信息；
            var sup = DataCenter.Instance.QuerySuperior(purchaserId);
            // 获取当前采购信息
            var superiorId = sup.Id;
            // 取得当前采购的上级的采买限制组；
            var  limitationGroup = PurchaseLimitationService.QueryPurchaseLimitation(superiorId, purchaserId);
            // 取得当前采购的上级的采买限制；
            var limitation = GetPurchaseLimitationInfo(limitationGroup, flight.Airline, flight.Departure, filter.PolicyType);
            // 取得当前采购的上级的收益设置；
            var setting = IncomeGroupLimitService.QueryIncomeGroupLimitGroup(superiorId, purchaserId);

            // 2、共享航班处理；
            // 2013-05-16 去掉了缺口程的限制，在后面处理；
            if (filter.Voyages.Any(p => p.Flight.IsShare))
            {
                return matchDefaultPolicyForChoosePolicy(filter, flight, limitation, sup, PassengerType.Adult);
            }

            // 2013-05-12 新增缺口程政策；这里传入的参数中filter.PolicyType为Normal呢？
            if (filter.PolicyType == PolicyType.Notch && 
                filter.VoyageType == VoyageType.Notch)
            {
                var notchPolicies = GetNotchPolicies(filter, limitationGroup, sup.Id);
                var npss = env.PolicySettings.Where(ps => filter.Voyages.All(v => ps.Berths.Contains(v.Bunk.Code)));
                var matchedPolicies = (from p in notchPolicies
                                       select
                                           Calculator.ComputePolicy(p, sup, npss, env.PolicyHarmonies, filter.Voyages,
                                                                    filter.PatPrice,
                                                                    filter.Purchaser, false, PassengerType.Adult, 0)).
                    Where(p => p != null).ToList();
                if (filter.NeedSubsidize)
                {
                    matchedPolicies = ProcessGlobalSubsidy(matchedPolicies, env.PolicySettings).ToList();
                }

                matchedPolicies =
                    matchedPolicies.DistinctNotchPolicyForChoosePolicy().SortNotchPolicy(flight.Airline).ToList();
                ProcessIncomeDeduction(matchedPolicies, setting, flight.Airline, airportpair,
                                    flight.FlightDate).ToList();
                matchedPolicies = ProcessAllHarmony(matchedPolicies, sup, phs).ToList();

                return matchedPolicies;
            }
            
           
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            // 转换类型；
            var voyageType = ConvertVoyeageType(filter.VoyageType);
            // 3、政策过滤；
            var policies = DataCenter.Instance.QueryPolicies(flight.Airline, flight.Departure, flight.FlightDate, voyageType, filter.PolicyType)
                .FilterByGroupLimitation(limitationGroup, sup.Id)   // 处理公司组限制
                .FilterByAccount(vas)                                   // 过滤掉无有效收款账号的用户的政策
                .FilterByWorkingHours(workingPublishers)                // 过滤掉已下班的用户的政策
                .Filter(filter)
                .FilterByPrice(flight.StandardPrice)                    // 过滤乱发的价格
                .FilterResourceCount()
                .FilterByAllowTicketType(filter.AllowTicketType)
                .ToList();

            // 4、处理有pat信息的情况（在查询时部分会没有价格，而在导入时会有价格导入）；
            if (filter.PatPrice == null)
            {
                policies = policies.FilterByNoPublishPrice()    // 过滤掉不能确定价格的政策(主要用于特价中的往返 和 联程);
                    .FilterByPriceTypeForbargin()   // 2012-11-15 也过滤掉特价里的按返佣发布；
                    .ToList();
            }

            // 2012-11-12 bug修复，
            // 下面的代码操作时，舱位为空时，将不做任何过滤，
            // 此时有一种特殊情况，即在免票情况下，黑屏同步和不同步；
            // 同步时是有舱位（不为空），过滤；而不同步情况是无舱位（为空），不过滤，那么同步的政策依然有可能进来；
            // 所以多加一个判断，在舱位为空时将同步的政策过滤掉；
            filter.Voyages.ForEach(item =>
            {
                if (item.Bunk != null)
                {
                    policies = policies.FilterByBunk(item.Bunk.Code).ToList();
                }
                else
                {
                    policies = (from p in policies
                                let sp = p as SpecialPolicyInfo
                                where sp == null || (sp != null && !sp.SynBlackScreen)
                                select p).ToList();
                }
            });
            
            // 5、处理特殊政策；
            if (filter.PolicyType == PolicyType.Special)
            {
                var discount = voyage.Bunk == null ? 0 : ((voyage.Bunk.Type == BunkType.Economic || voyage.Bunk.Type == BunkType.FirstOrBusiness) ? voyage.Bunk.Discount : 0);
                if (voyage.Bunk != null)
                {
                    policies = policies.FilterByRelationWithBunk(true).ToList();
                }
                var matchedPolicies = policies.Select(p => Calculator.ComputeSpecialPolicy(p as SpecialPolicyInfo, flight.StandardPrice, discount, sup)).Where(p => p != null).ToList();
                // 过滤结算价，若其有发布价，则需要发布价和结算价一致时，才不会过滤，
                // 这个应该是用在和查询订座时匹配使用，通过发布价（查询时）和结算价（计算后）相等，取得订座时的那条政策；
                if (filter.PublishFare > 0)
                {
                    matchedPolicies = matchedPolicies.Where(p => p.SettleAmount == filter.PublishFare).ToList();
                }
                
                matchedPolicies = ProcessIncomeDeduction(matchedPolicies, setting, flight.Airline, airportpair, flight.FlightDate).ToList();
                
                // 这里是特殊政策，就不做政策协调了；

                matchedPolicies = matchedPolicies.DistinctSpecialPolicyForChoosePolicy()
                    .SortSpecialPolicy(flight.Departure, flight.Arrival).ToList();
                matchedPolicies.SetCreditClass();
                return matchedPolicies;
            }

            // 6、判断此时是否取到了政策；
            if (!policies.Any())
            {
                // 若无政策，给出默认政策；
                return matchDefaultPolicyForChoosePolicy(filter, flight, limitation, sup, PassengerType.Adult);
            }

            // 7、以下处理非特殊政策；
            
            var matchedDeductions = env.PolicySettings;
            var allSettingOnPolicies = PolicySetService.QueryAllValidNormalPolicySetting();
            var pss = env.PolicySettings.Where(ps => filter.Voyages.All(v => ps.Berths.Contains(v.Bunk.Code)));
            var airportPair = new AirportPair(flight.Departure, flight.Arrival);
            var matched = (from p in policies
                          let spsd = filter.VoyageType == VoyageType.OneWay || filter.VoyageType == VoyageType.OneWayOrRound ?
                              Calculator.GetSinglePolicySubsidyAndDeduction(allSettingOnPolicies, p.Id, airportPair,
                                                                            voyage.Bunk.Code, flight.FlightDate):0
                          select
                              Calculator.ComputePolicy(p, sup, pss, env.PolicyHarmonies, filter.Voyages, filter.PatPrice,
                                                       filter.Purchaser, false, PassengerType.Adult, spsd)).Where(p=> p!= null).ToList();
            
            var internalPolicies = matched.DistinctBargainPolicyForChoosePolicy().SortBargainPolicy(flight.Airline).ToList()
                .Concat(matched.DistinctNormalPolicyForChoosePolicy().SortNormalPolicy(flight.Airline).ToList())
                .Concat(matched.DistinctTeamPolicyForChoosePolicy().SortTeamPolicy(flight.Airline).ToList()).OrderBy(p => p.SettleAmount).ToList();
            
            // 对非默认政策的成人票做贴点；
            //2013-01-26 deng.zhao 修改贴扣点；
            if (filter.NeedSubsidize)
            {
                internalPolicies = ProcessGlobalSubsidy(internalPolicies, matchedDeductions).ToList();
            }

            internalPolicies = ProcessIncomeDeduction(internalPolicies, setting, flight.Airline, airportpair, flight.FlightDate).ToList();
            internalPolicies = ProcessAllHarmony(internalPolicies, sup, phs).ToList();
            return internalPolicies;
        }

        /// <summary>
        /// 根据给出的过滤条件，为三方关系获取匹配后的内部政策。
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>匹配后的政策列表</returns>
        private static IEnumerable<MatchedPolicy> GetInternalPoliciesForTripartite(PolicyFilterConditions filter)
        {
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var sup = DataCenter.Instance.QuerySuperior(filter.Purchaser);
            var voyage = filter.Voyages.First();
            var flight = voyage.Flight;
            var bunk = voyage.Bunk;
            var workingPublishers = CompanyService.GetWorkingHours().FilterWorkingPublishers();
            var oemInfo = OEMService.QueryOEM(sup.Id);
            var airportpair = new AirportPair(flight.Departure, flight.Arrival);
            var setting = GetIncomeGroupDeductSetting(oemInfo, filter.Purchaser);
            var env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure, flight.Arrival, flight.FlightDate, filter.VoyageType);
            var phs = env.PolicyHarmonies.Where(ph => ph.Airlines.Contains(flight.Airline)).ToList();

            VoyageType voyageType;
            switch(filter.VoyageType) {
                case VoyageType.OneWay:
                    voyageType = VoyageType.OneWay | VoyageType.OneWayOrRound;
                    break;
                case VoyageType.RoundTrip:
                    voyageType = VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                    break;
                case VoyageType.OneWayOrRound:
                    voyageType = VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                    break;
                default:
                    voyageType = filter.VoyageType;
                    break;
            }
            // 特殊票的政策选择，只找普通政策
            filter.PolicyType = PolicyType.Normal;
            var policies = DataCenter.Instance.QueryPolicies(flight.Airline, flight.Departure, flight.FlightDate, voyageType, filter.PolicyType)
                .FilterByAccount(vas)
                .FilterByWorkingHours(workingPublishers)
                .Filter(filter)
                .FilterByBunk(bunk.Code).ToList();

            if (policies.Any())
            {
                // 平台扣点；
                var deductionForPlatform = SystemParamService.PlatformDeductForSpecial;
                // 出票留点；
                var deductionForProvider = SystemParamService.ProviderDeductForSpecial;
                var internalPolicies =
                    policies.Select(
                        p =>
                        Calculator.ComputeSpecialPolicy(p as NormalPolicyInfo, deductionForProvider,
                                                        deductionForPlatform, flight.Fare, bunk.Discount));
                internalPolicies = ProcessAllHarmony(internalPolicies, sup, phs);
                return internalPolicies.DistinctNormalPolicyForChoosePolicy().SortNormalPolicy(flight.Airline).ToList();
            }
            else
            {
                // 普通默认政策,此时不扣点
                var defaultPolicy = Calculator.ComputeDefaultSpecialPolicy(PolicyManageService.GetNormalDefaultPolicy(flight.Airline), 0, flight.Fare, bunk.Discount);
                SetWorkingHour(defaultPolicy);
                SetSpeed(defaultPolicy, flight.Airline);
                return new[] { defaultPolicy };
            }
        }
        
        /// <summary>
        /// 设置工作时间
        /// </summary>
        /// <param name="policy"></param>
        private static void SetWorkingHour(MatchedPolicy policy)
        {
            WorkingHours workingHours = CompanyService.GetWorkinghours(policy.Provider);
            bool isWeekend = DateTime.Today.DayOfWeek == DayOfWeek.Saturday ||
                             DateTime.Today.DayOfWeek == DayOfWeek.Sunday;
            policy.WorkStart = workingHours == null
                                   ? Time.MinValue
                                   : isWeekend ? workingHours.RestdayWorkStart : workingHours.WorkdayWorkStart;
            policy.WorkEnd = workingHours == null
                                 ? Time.MaxValue
                                 : isWeekend ? workingHours.RestdayWorkEnd : workingHours.WorkdayWorkEnd;
            policy.RefundStart = workingHours == null
                                     ? Time.MinValue
                                     : isWeekend ? workingHours.RestdayRefundStart : workingHours.WorkdayRefundStart;
            policy.RefundEnd = workingHours == null
                                   ? Time.MaxValue
                                   : isWeekend ? workingHours.RestdayRefundEnd : workingHours.WorkdayRefundEnd;
        }

        /// <summary>
        /// 设置出票时间
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="carrier"></param>
        private static void SetSpeed(MatchedPolicy policy, string carrier)
        {
            GeneralProductSpeedInfo statistic =
                OrderStatisticService.QuerySpeed(new[] {policy.Provider}, carrier)[policy.Provider];
            policy.SetSpeed(statistic);
        }

        /// <summary>
        /// 对匹配的政策进行复制并设置，从原来的匹配默认政策的方法中提取；
        /// </summary>
        /// <param name="refer"></param>
        /// <param name="flight"></param>
        /// <returns></returns>
        private static MatchedPolicy SettingMatchPolicy(MatchedPolicy refer, FlightFilterInfo flight)
        {
            var result = new MatchedPolicy
                             {
                                 Commission = refer.Commission,
                                 Deduction = refer.Deduction,
                                 Id = refer.Id,
                                 OfficeNumber = refer.OfficeNumber,
                                 OriginalPolicy = refer.OriginalPolicy,
                                 ParValue = refer.ParValue,
                                 Provider = refer.Provider,
                                 PolicyType = refer.PolicyType,
                                 Rebate = refer.Rebate,
                                 SettleAmount = refer.SettleAmount,
                                 Statistics = refer.Statistics,
                                 NeedAUTH = refer.NeedAUTH,
                                 RelationType = refer.RelationType
                             };

            SetWorkingHour(result);
            SetSpeed(result, flight.Airline);
            return result;
        }

        /// <summary>
        /// 为不需要使用PAT价格的政策列表，替换其票面价
        /// </summary>
        /// <param name="policies"></param>
        /// <param name="basicPrice"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        private static IEnumerable<MatchedPolicy> ReplaceParValue(IEnumerable<MatchedPolicy> policies,
                                                                  List<VoyageFilterInfo> voyageFilters)
        {
            // 对所有政策进行处理；
            policies.ForEach(item => ReplaceParValue(item, voyageFilters));

            // 再次按价格排序；
            return policies.OrderBy(p => p.SettleAmount);
        }

        /// <summary>
        /// 为不需要使用PAT价格的政策列表，替换其票面价;
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="voyageFilters"></param>
        /// <returns></returns>
        private static MatchedPolicy ReplaceParValue(MatchedPolicy policy, List<VoyageFilterInfo> voyageFilters)
        {
            // 参数验证；
            if (policy == null || voyageFilters == null || voyageFilters.Count == 0)
            {
                throw new ArgumentException("参数错误！");
            }
            if (voyageFilters.Any(p => p.Bunk == null || p.Flight == null))
            {
                throw new ArgumentException("航段信息不全！");
            }

            PolicyType policyType = policy.PolicyType;

            // 分两种情况，如果是默认政策，则其OriginalPolicy属性会为空（包括儿童），
            if ((policyType & PolicyType.NormalDefault) == PolicyType.NormalDefault && voyageFilters.Count == 1 &&
                policy.OriginalPolicy == null)
            {
                VoyageFilterInfo voyage = voyageFilters.First();
                BunkFilterInfo bunk = voyage.Bunk;
                FlightFilterInfo flight = voyage.Flight;

                // 当前航程基础运价票计算；
                decimal basicPrice = FoundationService.QueryBasicPriceValue(flight.Airline,
                                                                            flight.Departure,
                                                                            flight.Arrival,
                                                                            flight.FlightDate);
                decimal discount = bunk.Discount;
                policy.ParValue = Utility.Calculator.Round(basicPrice*discount, 1);
                policy.SettleAmount = policy.ParValue*(1 - policy.Commission);
            }
            else if ((policyType & PolicyType.Normal) == PolicyType.Normal && voyageFilters.Count == 1)
            {
                // 只对单程或单程/往返的政策起效，由于有可能有往返，所以需要计算程数；
                //VoyageType voyageType = policy.OriginalPolicy.VoyageType;

                //if ((voyageType == VoyageType.OneWay || voyageType == VoyageType.OneWayOrRound))
                //{
                VoyageFilterInfo voyage = voyageFilters.First();
                BunkFilterInfo bunk = voyage.Bunk;
                FlightFilterInfo flight = voyage.Flight;

                // 当前航程基础运价票计算；
                decimal basicPrice = FoundationService.QueryBasicPriceValue(flight.Airline,
                                                                            flight.Departure,
                                                                            flight.Arrival,
                                                                            flight.FlightDate);
                decimal discount = bunk.Discount;
                policy.ParValue = Utility.Calculator.Round(basicPrice*discount, 1);
                policy.SettleAmount = policy.ParValue*(1 - policy.Commission);
                //}
            }
            else
            {
            }

            return policy;
        }

        #endregion
        
        /// <summary>
        /// 对给出的政策列表中，返点最高（贴得最少）的政策做贴点；
        /// </summary>
        /// <param name="policies">待处理政策列表</param>
        /// <param name="policySettings"></param>
        /// <returns>贴点后的政策列表</returns>
        /// <remarks>
        /// 普通单程政策才会做贴点。
        /// </remarks>
        private static IEnumerable<MatchedPolicy> ProcessGlobalSubsidy(IEnumerable<MatchedPolicy> policies,
                                                                       IEnumerable<PolicySettingInfo> policySettings)
        {
            policies = policies.ToList();
            // 参数验证
            if (!policySettings.Any()) return policies;
            // 获得普通政策（2013-01-12）修改，由于外部政策的引入，此处的方法不再适用；
            //IEnumerable<MatchedPolicy> generalPolicies = policies.Where(p => p.OriginalPolicy is NormalPolicyInfo);

            IEnumerable<MatchedPolicy> generalPolicies = policies.Where(p => (p.PolicyType & PolicyType.Normal)== PolicyType.Normal);

            if (generalPolicies.Any())
            {
                // 获取最高返点；
                decimal maxCommission = generalPolicies.Max(p => p.Commission);
                // 获取最高返点对应的政策；
                IEnumerable<MatchedPolicy> maxGeneralPolicies = generalPolicies.Where(p => p.Commission == maxCommission);
                // 若最高返点的政策的关系为同行的，做贴点处理，其它的不做任何操作；
                if (maxGeneralPolicies.All(p => p.RelationType == RelationType.Brother))
                {
                    MatchedPolicy maxGeneralPolicy = generalPolicies.First(p => p.Commission == maxCommission);
                    // 仅处理没有协调过的政策
                    if (!maxGeneralPolicy.HasHarmony)
                    {
                        // 获取贴点区间信息；
                        PolicySettingPeriod policySettingPeriod =
                            Calculator.GetGlobalPolicySettingPeriod(DeductionType.Profession,
                                                                    maxGeneralPolicy.Commission,
                                                                    policySettings);
                        decimal policySettingValue = policySettingPeriod.Rebate;
                        decimal policySettingMaxValue = policySettingPeriod.MaxRebate;
                        // 2012-12-22 新增条件，若贴点值和返点值之间的差额大于最大贴点，则此政策完全不贴点；
                        // 注意现在的最大贴点值仍然是负值；
                        // 即，只有在 贴点值 - 返点值 <= 最大贴点值时，才进行操作；
                        if (policySettingValue < 0 && Math.Abs(policySettingValue) > maxGeneralPolicy.Commission &&
                            Math.Abs(policySettingValue) - maxGeneralPolicy.Commission <=
                            Math.Abs(policySettingMaxValue))
                        {
                            // 贴点值
                            decimal subsidy = policySettingValue + maxGeneralPolicy.Commission;
                            // 买入返点
                            decimal rebateForPurchaser = maxGeneralPolicy.Commission - subsidy;
                            // 政策协调
                            if (maxGeneralPolicy.HarmonyValue > 0)
                            {
                                decimal rebateForPurchaser2 = Calculator.GetHarmonyCommission(rebateForPurchaser,
                                                                                              maxGeneralPolicy.
                                                                                                  HarmonyValue);
                                maxGeneralPolicy.HasHarmony = rebateForPurchaser != rebateForPurchaser2;
                                rebateForPurchaser = rebateForPurchaser2;
                            }
                            maxGeneralPolicy.Commission = rebateForPurchaser;
                            maxGeneralPolicy.SettleAmount =
                                Utility.Calculator.Round(maxGeneralPolicy.ParValue*(1 - maxGeneralPolicy.Commission), -2);
                            maxGeneralPolicy.HasSubsidized = true;
                            maxGeneralPolicy.Deduction = maxGeneralPolicy.Rebate - maxGeneralPolicy.Commission;
                        }
                    }
                }
            }
            return policies;
        }

        /// <summary>
        /// 设置单个政策的协调值。
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="deductionType"></param>
        /// <param name="harmonies"></param>
        /// <returns></returns>
        private static MatchedPolicy ProcessSingleHarmony(MatchedPolicy policy, SuperiorInfo superior,
                                                          IEnumerable<PolicyHarmonyInfo> harmonies)
        {
            // 当作参数的判断，当政策为默认政策或者是外部政策时，才有可能为空；
            if (policy.OriginalPolicy == null)
            {
                return policy;
            }

            // 只有在普通和特殊政策时，才做协调；
            if ((policy.PolicyType & PolicyType.Normal) == PolicyType.Normal || (policy.PolicyType & PolicyType.Bargain) == PolicyType.Bargain)
            {
                var deductionType = Calculator.GetDeductionType(policy.OriginalPolicy.Owner, superior);
                var harmonyValue = Calculator.GetHarmonyValue(policy.OriginalPolicy, deductionType, harmonies);

                if (harmonyValue > 0)
                {
                    // 获得协调后的返点；
                    var currentCommission = Calculator.GetHarmonyCommission(policy.Commission, harmonyValue);
                    // 设置是否协调；
                    policy.HasHarmony = policy.Commission != currentCommission;
                    // 设置政策的返点值为协调后的返点；
                    policy.Commission = currentCommission;
                    policy.SettleAmount = policy.ParValue * (1 - policy.Commission);
                }
            }
            
            return policy;
        }

        /// <summary>
        /// 为所有匹配到的政策，设置协调值；
        /// </summary>
        /// <param name="policies">待处理的政策列表</param>
        /// <param name="superior"> </param>
        /// <param name="harmonies">政策协调信息</param>
        /// <returns>处理后的政策列表</returns>
        private static IEnumerable<MatchedPolicy> ProcessAllHarmony(IEnumerable<MatchedPolicy> policies,
                                                                    SuperiorInfo superior,
                                                                    IEnumerable<PolicyHarmonyInfo> harmonies)
        {
            var results = policies.Where(p=>p!=null).ToList();
            for (int i = 0; i < results.Count; i++)
            {
                results[i] = ProcessSingleHarmony(results[i], superior, harmonies);
            }
            return results;
        }
        
        /// <summary>
        /// 处理收益组设置。
        /// </summary>
        /// <param name="policies"></param>
        /// <param name="group"></param>
        /// <param name="carrier"></param>
        /// <param name="airportPair"></param>
        /// <param name="flightDate"></param>
        /// <returns></returns>
        private static IEnumerable<MatchedPolicy> ProcessIncomeDeduction(IEnumerable<MatchedPolicy> policies,
                                                                      IncomeGroupLimitGroup group,
                                                                      string carrier,
                                                                      AirportPair airportPair,
                                                                      DateTime flightDate)
        {
            var results = policies.Where(p => p != null).ToList();
            
            for (int i = 0; i < results.Count; i++)
            {
                results[i] = InitialIncomeSettingForPolicy(results[i]);
                if (group != null)
                {
                    results[i] = Calculator.ProcessIncomeDeduction(results[i], group, group.CompanyId, carrier, airportPair, flightDate);
                }
            }

            return results;
        }
        
        private static CompanyGroupLimitationInfo GetGroupLimitation(SuperiorInfo superior,
                                                                     IEnumerable<CompanyGroupLimitationInfo> limits,
                                                                     string airline, string departure)
        {
            if (superior == null) throw new ArgumentNullException("superior");
            if (string.IsNullOrWhiteSpace(airline)) throw new ArgumentNullException("airline");
            if (string.IsNullOrWhiteSpace(departure)) throw new ArgumentNullException("departure");

            // 若当前航线不包含在挂起列表中，进行处理；若当前航线包含在挂起列表中，返回空值，则在此后操作中，忽略其公司组限制；
            return limits.Any() && !superior.SuspendedCarirer.Contains(airline)
                       ? limits.FirstOrDefault(
                           l => l.Airlines.Split('/').Contains(airline) && l.Departures.Split('/').Contains(departure))
                       : null;
        }
        
        /// <summary>
        /// 设置信用级别
        /// </summary>
        internal static void SetCreditClass(this IEnumerable<MatchedPolicy> policies)
        {
            IEnumerable<Guid> publishers = policies.Select(p => p.Provider).Distinct();
            if (publishers.Any())
            {
                Dictionary<Guid, CompanyParameter> companyParameters =
                    AccountCombineService.GetCreditworthiness(publishers);
                policies.ForEach(p =>
                                     {
                                         if (companyParameters.ContainsKey(p.Provider))
                                         {
                                             CompanyParameter companyParameter = companyParameters[p.Provider];
                                             if (companyParameter != null)
                                             {
                                                 decimal? creditClass = companyParameter.Creditworthiness;
                                                 p.CompannyGrade = creditClass.HasValue ? creditClass.Value : 5;
                                             }
                                         }
                                     });
            }
        }
        
        /// <summary>
        /// 根据当前采购信息及其对应的上级OEM信息，给出当前采购的设置信息；
        /// </summary>
        /// <param name="oemInfo"></param>
        /// <param name="purchaser"></param>
        /// <returns></returns>
        /// <remarks>
        /// 现在的不再只针对OEM 2013-05-16 deng.zhao
        /// 这个方法也需要废除；
        /// </remarks>
        private static IncomeGroupDeductGlobal GetIncomeGroupDeductSetting(OEMInfo oemInfo, Guid purchaser)
        {
            // 若OEM不可用，则无需再获取设置；
            if (oemInfo == null || !oemInfo.Valid) { return null; }

            // 获取当前采购的组信息；谢说这句有问题；
            var purchaserGroupInfo = DistributionOEMService.QueryDistributionOEMUserDetailInfo(purchaser);
            if (purchaserGroupInfo == null) { return null; }

            // 获取收益组信息；
            var incomeGroup = oemInfo.IncomeGroupList.FirstOrDefault(item => item.Id == purchaserGroupInfo.IncomeGroupId);
            if (incomeGroup == null) { return null; }
            
            // 获取设置信息；
            var setting = incomeGroup.Setting.FirstOrDefault(item => item.IncomeGroupId == purchaserGroupInfo.IncomeGroupId);
            return setting;
        }
        
        /// <summary>
        /// 对匹配到政策设置做收益组设置
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        internal static MatchedPolicy InitialIncomeSettingForPolicy(MatchedPolicy policy)
        {
            if (policy!=null)
            {
                // 获取类型；
                var profitType = OemProfitType.Discount;
                if (policy.OriginalPolicy is SpecialPolicyInfo)
                {
                    var specialPolicy = policy.OriginalPolicy as SpecialPolicyInfo;
                    if (specialPolicy.Type != SpecialProductType.LowToHigh)
                    {
                        profitType = OemProfitType.PriceMarkup;
                    }
                }
                // 只初始一次；
                if (policy.OemInfo == null)
                {
                    policy.OemInfo = new OemPolicyInfo
                    {
                        ProfitType = profitType,
                        Profits = new List<OemProfit>()
                    };
                }
                
            }return policy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IEnumerable<PolicyInfoBase> GetNotchPolicies(PolicyFilterConditions filter, PurchaseLimitationGroup limitationGroup, Guid superiorId)
        {
            // 1、获取环境等相关信息；
            var voyage = filter.Voyages.First();
            var flight = voyage.Flight;
            var env = PolicyManageService.GetMatchEnvironment(flight.Airline, flight.Departure, flight.Arrival,
                                                              flight.FlightDate, filter.VoyageType);
            var vas = DataCenter.Instance.QueryAllValidReceiveAccount();
            var workingPublishers = env.WorkingHours.FilterWorkingPublishers();
            var airline = flight.Airline;
            
            var dt = new DataTable();
            dt.TableName = "Voyage";
            dt.Columns.Add("Id", System.Type.GetType("System.Int16"));
            dt.Columns.Add("Departure", System.Type.GetType("System.String"));
            dt.Columns.Add("Arrival", System.Type.GetType("System.String"));
            dt.Columns.Add("Date", System.Type.GetType("System.DateTime"));
            
            for (int i = 0; i < filter.Voyages.Count; i++)
            {
                var f = filter.Voyages[i].Flight;

                var dr = dt.NewRow();
                dr["Id"] = i + 1;
                dr["Departure"] = f.Departure;
                dr["Arrival"] = f.Arrival;
                dr["Date"] = f.FlightDate;

                dt.Rows.Add(dr);
            }

            //var voyageCondition = filter.Voyages.Select((v, index) => new VoyageCondition
            //                                                              {
            //                                                                  VoyageNumber = index + 1,
            //                                                                  Departure = v.Flight.Departure,
            //                                                                  Arrial = v.Flight.Arrival,
            //                                                                  Date = v.Flight.FlightDate
            //                                                              }).ToList();

            var notchPolicies = DataCenter.Instance.QueryPolicies(airline , dt, VoyageType.Notch, filter.PolicyType)
                .FilterByGroupLimitation(limitationGroup, superiorId) // 处理公司组限制
                .FilterByAccount(vas) // 过滤掉无有效收款账号的用户的政策
                .FilterByWorkingHours(workingPublishers) // 过滤掉已下班的用户的政策
                .Filter(filter)
                .FilterByPrice(flight.StandardPrice) // 过滤乱发的价格
                .FilterResourceCount()
                .FilterByAllowTicketType(filter.AllowTicketType)
                .ToList();

            filter.Voyages.ForEach(item =>
                                       {
                                           if (item.Bunk != null)
                                           {
                                               notchPolicies = notchPolicies.FilterByBunk(item.Bunk.Code).ToList();
                                           }
                                       });

            return notchPolicies;
        }
        
        /// <summary>
        /// 对行程类型进行匹配政策前的转换
        /// </summary>
        /// <param name="voyageType">待转换行程类型</param>
        /// <returns>转换后的行程类型</returns>
        private static VoyageType ConvertVoyeageType(VoyageType voyageType)
        {
            switch (voyageType)
            {
                case VoyageType.OneWay:
                    return VoyageType.OneWay | VoyageType.OneWayOrRound;
                case VoyageType.RoundTrip:
                    return VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                case VoyageType.OneWayOrRound:
                    return VoyageType.OneWay | VoyageType.RoundTrip | VoyageType.OneWayOrRound;
                default:
                    return voyageType;
            }
        }

        /// <summary>
        /// 根据给出的采买限制组，得到对应航空公司和出港城市的采买限制；
        /// </summary>
        /// <param name="limitationGroup">采买限制组</param>
        /// <param name="airline">航空公司</param>
        /// <param name="departure">出港城市</param>
        /// <param name="policyType"> </param>
        /// <returns>采买限制</returns>
        /// <remarks>
        /// 1、若返回值为空，则说明对此航空公司和出港城市没有采买限制；
        /// 2、在符合条件的集合中，现有的规则是取得适用范围最小，返点最高的限制信息；
        /// 3、此采买限制只有成人的政策；
        /// </remarks>
        internal static PurchaseLimitationInfo GetPurchaseLimitationInfo(PurchaseLimitationGroup limitationGroup, string airline, string departure, PolicyType policyType)
        {
            if (limitationGroup == null || limitationGroup.Limitation == null) return null;

            PurchaseLimitationRateType? type = null;
            if (policyType == PolicyType.Normal || policyType == PolicyType.NormalDefault)
            {
                type = PurchaseLimitationRateType.Normal;
            }
            else if (policyType == PolicyType.Bargain || policyType == PolicyType.BargainDefault)
            {
                type =  PurchaseLimitationRateType.Bargain;
            }
            else
            {
                type = null;
            }

            //var limitation = (limitationGroup.Limitation.Where(
            //    pl => pl.Airlines.Contains(airline) && pl.Departures.Contains(departure) &&
            //          pl.Rebate.Any(r => r.Type == type && r.AllowOnlySelf)).Select(pl => new
            //                                                                                  {
            //                                                                                      Limitation = pl,
            //                                                                                      Rebate =
            //                                                                                  pl.Rebate.Where(
            //                                                                                      t => t.Type == type)
            //                                                                                  })).MaxOrDefault(p=>p.Rebate.First().Rebate.Value);

            PurchaseLimitation limitation = null;
            decimal maxRebate = -1;

            // 若航班和出港地匹配，同时对应的政策类型为只允许采购自身政策，则取得其中的最大的返点对应的限制信息；
            foreach (var item in limitationGroup.Limitation)
            {
                if (item.Departures.Contains(departure) && item.Airlines.Contains(airline))
                {
                    foreach (var r in item.Rebate)
                    {
                        if (r.Type == type && r.AllowOnlySelf)
                        {
                            // 若最大值比当前值都小，则重新赋值；
                            if (r.Rebate != null && maxRebate < r.Rebate.Value)
                            {
                                maxRebate = r.Rebate.Value;
                                limitation = item;
                            }
                        }
                    }
                }
            }

            if (limitation == null)
            {
                return null;
            }
            else
            {
                return new PurchaseLimitationInfo()
                           {
                               LimitationId = limitation.Id,
                               Debate = maxRebate
                           };
            }
        }
    }
}