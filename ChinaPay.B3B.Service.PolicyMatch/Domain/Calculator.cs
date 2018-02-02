using System;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Statistic;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    using System.Collections.Generic;
    using Common.Enums;
    using DataTransferObject.Organization;
    using DataTransferObject.Policy;
    using Organization;
    using Izual;
    using System.Linq;
    using FlightQuery.Domain;

    public static class Calculator {

        /// <summary>
        /// 计算普通政策的票面价
        /// </summary>
        /// <param name="standardPrice">基础价格</param>
        /// <param name="adultDiscount">舱位折扣(成人)</param>
        /// <param name="passengerType">乘客类型</param>
        /// <returns>票面价</returns>
        private static decimal ComputeNormalParValue(decimal standardPrice, decimal adultDiscount, PassengerType passengerType) {
            // 由于折扣已经在外面处理了，所以这里就不再处理折扣了
            var adultFare = Utility.Calculator.Round(standardPrice * adultDiscount, 1);
            if(passengerType == PassengerType.Child) return Utility.Calculator.Round((adultDiscount >= 1 ? adultFare : standardPrice)/2, 1);
            return adultFare;
        }

        /// <summary>
        /// 计算特殊政策的票面价
        /// </summary>
        /// <remarks>
        /// 2012-10-19 deng.zhao edit
        /// 单程控位：取得其结算价用于展示，其后会根据其情况改变后出票；
        /// 散冲团：同上；
        /// 免票：直接取0；
        /// 集团票：1000  H 850(票面) 
        /// 商旅卡：同上
        /// 2012-11-01 deng.zhao 需求变动，单程控位和散冲需要根据关系显示票面价，因此增加参数；
        /// 2013-01-14 deng.zhao 为适应集团票的低价低返，对票面价的计算公式作出调整；
        /// 调整为，直减下的低价低返时，不再按直减计算，票面价直接取舱位折扣后的票面；
        /// 2013-01-19 deng.zhao 由于14日的更改导致退票出现问题，修正了需求。
        /// 现在的需求为，设置一个区间值，若票价不在此区间内，不出票。
        /// </remarks>
        public static decimal ComputeSpecialParValue(SpecialPolicyInfo policy, decimal standardPrice, decimal discount, DeductionType deductionType) {
            switch(policy.Type) {
                case SpecialProductType.Singleness:
                case SpecialProductType.Disperse:
                // 2012-11-20 其它特殊，指的是弃程Q舱
                case SpecialProductType.OtherSpecial:
                case SpecialProductType.CostFree:
                    return 0;
                case SpecialProductType.Bloc:
                case SpecialProductType.Business:
                    decimal price;
                    switch(policy.PriceType) {
                        case PriceType.Price:
                            // 1、按价格发布，这里的price是价格信息；
                            price = policy.Price;
                            break;
                        case PriceType.Subtracting:
                            if(policy.IsBargainBerths) {
                                throw new NotSupportedException();
                            } else {
                                var orginalFare = ComputeNormalParValue(standardPrice, discount, PassengerType.Adult);
                                // 这里稍令人费解，是因为此时的price存放的是减少的百分比；
                                price = orginalFare * (1 - policy.Price);
                            }
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                    return Utility.Calculator.Round(price, 1);
                case SpecialProductType.LowToHigh:
                    return ComputeNormalParValue(standardPrice, discount, PassengerType.Adult);
                default:
                    throw new NotSupportedException();
            }
        }


        public static decimal ComputeNotchParValue(decimal standardPrice, decimal adultDiscount, PassengerType passengerType)
        {
            // 由于折扣已经在外面处理了，所以这里就不再处理折扣了
            var adultFare = Utility.Calculator.Round(standardPrice * adultDiscount, 1);
            if (passengerType == PassengerType.Child) return Utility.Calculator.Round((adultDiscount >= 1 ? adultFare : standardPrice) / 2, 1);
            return adultFare;
        }

        /// <summary>
        /// 计算特价政策票面价
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="standardPrice"></param>
        /// <param name="patPrice">贴入的PAT价格</param>
        /// <returns></returns>
        private static decimal ComputeBargainParValue(BargainPolicyInfo policy, decimal standardPrice, decimal? patPrice) {
            switch(policy.VoyageType) {
                case VoyageType.RoundTrip:
                    if(policy.Price > 0) return policy.Price;  
                    if(!patPrice.HasValue) throw new InvalidOperationException("缺少pat价格");
                    return patPrice.Value;
                case VoyageType.TransitWay:
                    if(!patPrice.HasValue) throw new InvalidOperationException("缺少pat价格");
                    return patPrice.Value;
                case VoyageType.OneWay:
                    decimal price;
                    switch(policy.PriceType) {
                        case PriceType.Price:
                            price = policy.Price;
                            break;
                        case PriceType.Discount:
                            price = standardPrice * policy.Price;
                            break;
                        case PriceType.Commission:
                            if(!patPrice.HasValue) throw new InvalidOperationException("缺少pat价格");
                            price = patPrice.Value;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    return Utility.Calculator.Round(price, 1);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 计算团队政策票面价
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="patPrice"></param>
        /// <returns></returns>
        private static decimal ComputeTeamParValue(TeamPolicyInfo policy, decimal? patPrice) {
            if(!patPrice.HasValue) throw new InvalidOperationException("缺少pat价格");
            return patPrice.Value;
        }

        /// <summary>
        /// 计算非普通政策的票面价
        /// </summary>
        /// <param name="policy">政策</param>
        /// <param name="standardPrice">基础价格</param>
        /// <param name="discount">折扣</param>
        /// <returns>票面价</returns>
        private static decimal ComputeNonNormalParValue(PolicyInfoBase policy, decimal standardPrice, decimal discount, decimal? patPrice, DeductionType deductionType) {
            var bp = policy as BargainPolicyInfo;
            if(bp != null)
                return ComputeBargainParValue(bp, standardPrice, patPrice);
            var sp = policy as SpecialPolicyInfo;
            if(sp != null)
                return ComputeSpecialParValue(sp, standardPrice, discount, deductionType);
            var tp = policy as TeamPolicyInfo;
            if(tp != null)
                return ComputeTeamParValue(tp, patPrice);

            return -1;
        }

        /// <summary>
        /// 计算所有政策的票面（若政策类型不明）
        /// </summary>
        private static decimal ComputeParValue(PolicyInfoBase policy, BunkType bunkType, decimal standardPrice, decimal discount, decimal? patPrice, SuperiorInfo superior, PassengerType passengerType) {
            return (bunkType == BunkType.Economic || bunkType == BunkType.FirstOrBusiness) && (policy.PolicyType == PolicyType.NormalDefault || policy.PolicyType == PolicyType.Normal)
                       ? ComputeNormalParValue(standardPrice, discount, passengerType)
                       : ComputeNonNormalParValue(policy, standardPrice, discount, patPrice, GetDeductionType(policy.Owner, superior));
        }

        /// <summary>
        /// 计算特殊政策的结算价
        /// </summary>
        /// <param name="policy">特殊政策</param>
        /// <param name="deductionType">返佣类型</param>
        /// <remarks>
        /// 2012-10-31 修改了结算价的计算规则，按照
        /// 票面价：
        /// 单程控位：以用户发布为准；
        /// 散冲团：以用户发布为准；
        /// 免票：以用户发布为准；
        /// 集团票：1000 * 0.85 * (1- 0.15)
        /// 商旅卡：同上
        /// 结算价 = 票面价 - 佣金 
        /// 2013-01-14 集团票的规则修改，增加了低价低返的规则，具体规则见计算返佣的方法说明
        /// 对于集团票中的直减发布，若选取了低价低返，那么票面价低于给定值时，同行的返点有变化。
        /// 变化的规则是：此时不再按直减计算，而是直接在票面基础上计算。
        /// 
        /// 
        /// </remarks>
        public static decimal ComputeSpecialSettlementPrice(SpecialPolicyInfo policy, decimal standardPrice, decimal discount, DeductionType deductionType) {
            switch(policy.PriceType) {
                case PriceType.Price:
                    return Utility.Calculator.Round(GetSettleAmount(policy, deductionType), 0);
                case PriceType.Subtracting:
                    // 获取舱位折扣后的票面价；
                    var orginalFare = ComputeNormalParValue(standardPrice, discount, PassengerType.Adult);
                    // 计算出结算价；
                    var settlementPrice = Utility.Calculator.Round(orginalFare * (1 - GetSettleAmount(policy, deductionType)), 0);

                    // 若为同行的集团政策，且设定了低价限制，则判断其价格区间；
                    if (DeductionType.Profession == deductionType && policy.Type == SpecialProductType.Bloc &&
                        policy.LowNoType == LowNoType.LowInterval)
                    {
                        // 获取集团票直减后的票面价，用于判断价格区间；
                        decimal afterSubtractinPirce = orginalFare*(1 - policy.Price);
                        // 若不在区间内；
                        if (!(afterSubtractinPirce >= policy.LowNoMinPrice &&
                            (policy.LowNoMaxPrice == -1 || afterSubtractinPirce <= policy.LowNoMaxPrice)))
                        {
                            settlementPrice = 0;
                        }
                    }

                    return settlementPrice;
                case PriceType.Commission://Xie.  2013-03-06
                    return Utility.Calculator.Round((1-GetSettleAmount(policy, deductionType)) * standardPrice * discount, 0);
                default:
                    throw new NotSupportedException();
            }
        }

 
        /// <summary>
        /// 计算返点（这个方法没有被调用）
        /// </summary>
        internal static decimal ComputeDeduction(DeductionType deductionType, decimal saleCommission, IEnumerable<PolicySettingInfo> deductions) {
            // 贴扣点都只针对同行
            if(deductionType != DeductionType.Profession) return 0;

            var p = (from d in deductions
                     let period = d.Periods.FirstOrDefault(pm => pm.PeriodStart <= saleCommission && pm.PeriodEnd >= saleCommission)
                     where period != null
                     select new { deduction = d, period });

            return p.Any() ? p.MaxOrDefault(o => (o.deduction.LastModifyTime ?? o.deduction.CreateTime)).period.Rebate : 0;
        }

        /// <summary>
        /// 获取最新的全局贴扣点数据；
        /// </summary>
        /// <remarks>负数表示贴点，正数表示扣点，0表示不处理</remarks>
        internal static PolicySettingPeriod GetGlobalPolicySettingPeriod(DeductionType deductionType, decimal saleCommission, IEnumerable<PolicySettingInfo> policySettings)
        {
            // 贴扣点都只针对普通政策，且只针对同行；
            if(deductionType == DeductionType.Profession) {
                // 取得贴点（Rebate <0 或者扣点 Rebate> 0 的数据）
                var pss = (from ps in policySettings
                           let period = ps.Periods.FirstOrDefault(pm => pm.Rebate < 0 || (pm.Rebate > 0 && pm.PeriodStart < saleCommission && pm.PeriodEnd >= saleCommission))
                           where ps.Enable && period != null
                           select new { Setting = ps, period }).ToList();
                // 取得最新发布的数据；
                if(pss.Any())
                {
                    return pss.MaxOrDefault(o => (o.Setting.LastModifyTime ?? o.Setting.CreateTime)).period;
                }
            }
            return null;
        }
        
        ///// <summary>
        ///// 获取最新的全局贴扣点数据；
        ///// </summary>
        ///// <remarks>负数表示贴点，正数表示扣点，0表示不处理</remarks>
        //internal static decimal GetGlobalPolicySettingValue(DeductionType deductionType, decimal saleCommission, IEnumerable<PolicySettingInfo> policySettings)
        //{
        //    // 贴扣点都只针对普通政策，且只针对同行；
        //    if (deductionType == DeductionType.Profession)
        //    {
        //        // 取得贴点（Rebate <0 或者扣点 Rebate> 0 的数据）
        //        var pss = (from ps in policySettings
        //                   let period = ps.Periods.FirstOrDefault(pm => pm.Rebate < 0 || (pm.Rebate > 0 && pm.PeriodStart <= saleCommission && pm.PeriodEnd >= saleCommission))
        //                   where ps.Enable && period != null
        //                   select new { Setting = ps, period }).ToList();

        //        // 取得最新发布的数据；
        //        if (pss.Any())
        //        {
        //            return pss.MaxOrDefault(o => (o.Setting.LastModifyTime ?? o.Setting.CreateTime)).period.Rebate;
        //        }
        //    }
        //    return 0;
        //}
        
        internal static decimal GetGlobalPolicySettingValue(DeductionType deductionType, decimal saleCommission, IEnumerable<PolicySettingInfo> policySettings)
        {
            var period = GetGlobalPolicySettingPeriod(deductionType, saleCommission, policySettings);

            return period != null ? period.Rebate : 0;
        }

        /// <summary>
        /// 获取OEM返点
        /// </summary>
        /// <param name="saleCommission">采购得到的返点</param>
        /// <param name="incomeLimitation"> </param>
        /// <returns></returns>
        internal static decimal GetIncomePolicyDeduction(decimal saleCommission, IncomeGroupLimit incomeLimitation)
        {
            IncomeGroupPeriod result = null;
            switch (incomeLimitation.Type)
            {
                case PeriodType.Interval:
                    result = (from p in incomeLimitation.Period
                              where p.StartPeriod < saleCommission && p.EndPeriod >= saleCommission
                              select p).FirstOrDefault();
                    break;
                case PeriodType.Unite:
                    result = incomeLimitation.Period.FirstOrDefault();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result != null ? result.Period : 0;
        }
        

        /// <summary>
        /// 获取单条普通政策上的贴扣点值，正数为贴点，负数为扣点；
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="airportPair"></param>
        /// <param name="berth"></param>
        /// <param name="flightDate"></param>
        /// <param name="flag">这个用于标识对于舱位的判断是包含在内，还是相等</param>
        /// <returns></returns>
        /// <remarks>
        /// 标志值为真，则采用的方式是完全相等，标志值为假，则为包含。
        /// </remarks>
        internal static decimal GetSinglePolicySubsidyAndDeduction(IEnumerable<NormalPolicySetting> allSettingOnPolicies, Guid policyId, AirportPair airportPair, string berth, DateTime flightDate)
        {
            var normalPolicySettings = from p in allSettingOnPolicies
                                       where p.PolicyId == policyId && p.Enable && string.Equals(p.Berths, berth, StringComparison.CurrentCultureIgnoreCase)
                                        && p.StartTime.Date <= flightDate.Date && flightDate.Date <= p.EndTime.Date
                                        && (p.FlightsFilter == "**" || p.FlightsFilter.Contains(airportPair.Departure + airportPair.Arrival) ||
                                            p.FlightsFilter.Contains(airportPair.Departure + "*") || p.FlightsFilter.Contains("*" + airportPair.Arrival))
                                       select p;

            var policySettings = normalPolicySettings as List<NormalPolicySetting>  ?? normalPolicySettings.ToList();
            if (policySettings.Any())
            {
                var policySetting = policySettings.First();
                if (policySetting.Type)
                {
                    return policySetting.Commission;
                }
                else
                {
                    return -policySetting.Commission;
                }
            }
            return 0;
        }
        
        /// <summary>
        /// 获取协调设置值
        /// </summary>
        internal static decimal GetHarmonyValue(PolicyInfoBase policy, DeductionType deductionType, IEnumerable<PolicyHarmonyInfo> harmonies) {
            var harmony = harmonies.Where(ph => (ph.PolicyType & policy.PolicyType) == policy.PolicyType
                                                && ph.DeductionType == deductionType).MaxOrDefault(h => h.LastModifyTime);
            return harmony == null ? 0 : harmony.HarmonyValue;
        }

        /// <summary>
        /// 获取协调设置值
        /// </summary>
        /// <param name="policyType"></param>
        /// <param name="deductionType"></param>
        /// <param name="harmonies"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2103-01-10 deng,zhao 为方便协调值的计算，新增；
        /// </remarks>
        internal static decimal GetHarmonyValue(PolicyType policyType, DeductionType deductionType, IEnumerable<PolicyHarmonyInfo> harmonies)
        {
            var harmony = harmonies.Where(ph => (ph.PolicyType & policyType) == policyType
                                                && ph.DeductionType == deductionType).MaxOrDefault(h => h.LastModifyTime);
            return harmony == null ? 0 : harmony.HarmonyValue;
        }

        /// <summary>
        /// 根据返佣类型，获取政策上的返点（针对普通和特价）；
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="deductionType"></param>
        /// <returns></returns>
        internal static decimal GetCommission(PolicyInfoBase policy, DeductionType deductionType) {
            switch(deductionType) {
                case DeductionType.Internal:
                    return policy.InternalCommission;
                case DeductionType.Subordinate:
                    return policy.SubordinateCommission;
                default:
                    return policy.ProfessionCommission;
            }
        }

        /// <summary>
        /// 根据返佣类型，计算所给出政策的返佣值。
        /// </summary>
        /// <param name="policy">政策</param>
        /// <param name="deductionType">返佣类型</param>
        /// <returns>返佣值</returns>
        /// <remarks>
        /// 随返佣的方式不同，此结果有可能为价格，有可能为百分比。
        /// 但现在几种产品的计算都一样了，应该可以和GetCommission合并了吧。 
        /// 此方法针对特殊政策中的按价格发布；
        /// </remarks>
        private static decimal GetSettleAmount(SpecialPolicyInfo policy, DeductionType deductionType) {
            //2012-12-31 去掉了此句的限制，所有的特殊政策都需要根据返佣类型计算；
            //if(policy.Type != SpecialProductType.Bloc && policy.Type != SpecialProductType.Business) return 0;
            switch(deductionType) {
                case DeductionType.Internal:
                    return policy.InternalCommission;
                case DeductionType.Subordinate:
                    return policy.SubordinateCommission;
                 case DeductionType.Profession:
                    return policy.ProfessionCommission;
                default:
                    throw  new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// 根据上下级关系，获得其返点类型（上级、同行、内部）
        /// </summary>
        internal static DeductionType GetDeductionType(Guid sup, SuperiorInfo superior) {
            if(superior == null || sup != superior.Id) return DeductionType.Profession;

            switch(superior.Type) {
                case RelationshipType.Organization:
                    return DeductionType.Internal;
                case RelationshipType.Distribution:
                    return DeductionType.Subordinate;
                default:
                    return DeductionType.Profession;
            }
        }

        public static DeductionType GetDeductionType(RelationType relation) {
            switch(relation) {
                case RelationType.Brother:
                    return DeductionType.Profession;
                case RelationType.Junion:
                    return DeductionType.Subordinate;
                case RelationType.Interior:
                    return DeductionType.Internal;
                default:
                    return DeductionType.Profession;
            }
        }
        /// <summary>
        /// 根据上下级信息，获取关系类型（下级、内部机构，同行）
        /// </summary>
        internal static RelationType GetRelationType(Guid sup, SuperiorInfo superior) {
            if(superior == null || sup != superior.Id) return RelationType.Brother;
            switch(superior.Type) {
                case RelationshipType.Organization:
                    return RelationType.Interior;
                case RelationshipType.Distribution:
                    return RelationType.Junion;
                default:
                    return RelationType.Brother;
            }
        }

        /// <summary>
        /// 计算协调后的返点
        /// </summary>

        /// <summary>
        /// 根据现有返点和协调值，得到协调过后的返点
        /// </summary>
        /// <param name="rebate">现有返点</param>
        /// <param name="harmonyValue">协调值</param>
        /// <returns>协调过后的返点</returns>
        internal static decimal GetHarmonyCommission(decimal rebate, decimal harmonyValue) {
            return (harmonyValue == 0 || harmonyValue > rebate )? rebate : harmonyValue;
        }

        /// <summary>
        /// 计算政策的匹配信息
        /// </summary>
        /// <param name="policy">待处理政策</param>
        /// <param name="bunk">舱位</param>
        /// <param name="patPrice">PAT价格</param>
        /// <param name="purchaser">购买方编号</param>
        /// <param name="superior">上级编号</param>
        /// <param name="deductions">政策设置列表</param>
        /// <param name="harmonies">协调值</param>
        /// <param name="needSubsidize">是否需要贴点</param>
        /// <returns></returns>
        internal static MatchedPolicy ComputePolicy(PolicyInfoBase policy, Bunk bunk, decimal? patPrice, Guid purchaser, SuperiorInfo superior, IEnumerable<PolicySettingInfo> deductions, IEnumerable<PolicyHarmonyInfo> harmonies, bool needSubsidize, PassengerType passengerType, decimal single) {
            return ComputePolicy(policy, superior, deductions, harmonies, bunk.Code, bunk.Type, bunk.Owner.StandardPrice, (bunk is GeneralBunk) ? (bunk as GeneralBunk).Discount : 0, patPrice, purchaser, needSubsidize, passengerType, single);
        }

        /// <summary>
        /// 计算返佣时也会调用的，当单程航班查询时，贴点为true；
        /// </summary>
        internal static MatchedPolicy ComputePolicy(PolicyInfoBase policy, SuperiorInfo superior, IEnumerable<PolicySettingInfo> dedcutions, IEnumerable<PolicyHarmonyInfo> harmonies, string bunkCode, BunkType bunkType, decimal standardPrice, decimal discount, decimal? patPrice, Guid purchaser, bool needSubsidize, PassengerType passengerType, decimal spsd) {
            var deductionType = GetDeductionType(policy.Owner, superior);
            // 票面价
            var pv = ComputeParValue(policy, bunkType, standardPrice, discount, patPrice, superior, passengerType);
            if(pv < 0) return null;

            // 卖出返点（出票方返点）
            var scmm = GetCommission(policy, deductionType);
            // 平台需贴扣出去的点（给出票方的，正数为贴，负数为扣）
            decimal dd = 0M;
            var global = 0M;
            var single = 0M;
            var hasSubsidized = false;

            // 1、获取单条政策贴扣点，已由spsd传入；负数表扣点，正数表贴点；这里需要注意，只要在普通政策下同行关系时，才贴点；
            // 这里的贴点只对普通单程起作用；
            single = policy is NormalPolicyInfo && deductionType == DeductionType.Profession ? spsd : 0;
            // 2、获取全局贴扣点设置值，这个东西是反的，加个负号把它倒过来；
            global = -(policy is NormalPolicyInfo ? GetGlobalPolicySettingValue(deductionType, scmm + single, dedcutions.Where(d => d.Berths.Split(',').Contains(bunkCode))) : 0);

            // 3、单条贴扣点一起计算了；
            dd = single;
            // 4、全局贴点（此处不计算对最高的政策的贴点，在所有价格没有出来之前，无法计算），注意此处的贴点值是贴到；
            // 不论是单条是扣还是贴，只要满足条件，就贴到此值；
            if(needSubsidize && global > scmm) {
                dd = global - scmm;
                hasSubsidized = true;
            }

            // 6、全局扣点；
            if(global < 0) {
                dd = single + global;
                // 防止过度扣点，扣点的上限为卖出返点；
                if(scmm < dd) dd = scmm;
            }

            // 买入返点（给采购的）
            var cmm = scmm + dd;

            // 2013-04-01 deng.zhao 重大变动，将政策协调从此处移除，在最后统一操作；
            // 政策协调
            //var harmonyValue = GetHarmonyValue(policy, deductionType, harmonies);
            //var hasHarmony = false;
            //if(harmonyValue > 0) {
            //    var cmm2 = GetHarmonyCommission(cmm, harmonyValue);
            //    hasHarmony = cmm != cmm2;
            //    cmm = cmm2;
            //}

            // 保证返点在[0, 1]这个区间内
            if(cmm < 0) cmm = 0;
            if(cmm > 1) cmm = 1;
            // 由于处理了贴扣点和协调，需要反过来处理平台的点
            dd = scmm - cmm;
            // 结算价
            var settleAmount = pv * (1 - cmm);
            return new MatchedPolicy {
                Id = policy.Id,
                Provider = policy.Owner,
                RelationType = GetRelationType(policy.Owner, superior),
                Commission = cmm,
                Deduction = dd,
                OriginalPolicy = policy,
                PolicyType = policy.PolicyType,
                Rebate = scmm,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                ParValue = pv,
                WorkStart = policy.WorkStart,
                WorkEnd = policy.WorkEnd,
                RefundStart = policy.RefundStart,
                RefundEnd = policy.RefundEnd,
                HasSubsidized = hasSubsidized
                //,HarmonyValue = harmonyValue,
                //HasHarmony = hasHarmony
            };
        }

        internal static MatchedPolicy ComputePolicy(PolicyInfoBase policy, SuperiorInfo superior, IEnumerable<PolicySettingInfo> deductions, IEnumerable<PolicyHarmonyInfo> harmonies, IEnumerable<VoyageFilterInfo> voyageFilterInfos, decimal? patPrice, Guid purchaser, bool needSubsidize, PassengerType passengerType, decimal spsd) {
            var deductionType = GetDeductionType(policy.Owner, superior);
            // 卖出返点
            var scmm = GetCommission(policy, deductionType);
            // 平台扣点
            decimal dd = 0;
            // 2013-05-20 这里增加了条件；
            if(policy is NormalPolicyInfo || policy is NotchPolicyInfo) {
                var global = 0M;
                var single = 0M;
                var matchedDeductions = deductions.ToList();
                voyageFilterInfos.ForEach(item => {
                    matchedDeductions = matchedDeductions.Where(d => d.Berths.Split(',').Contains(item.Bunk.Code)).ToList();
                });

                // 1、获取单条政策贴扣点，已由spsd传入；负数表扣点，正数表贴点；这里需要注意，只要在普通政策下同行关系时，才贴点；
                single =deductionType == DeductionType.Profession ? spsd : 0;
                // 2、获取全局贴扣点设置值，这个东西是反的，加个负号把它倒过来；
                // 此时计算的值，应根据加上贴扣点之后的值去计算；
                global = -GetGlobalPolicySettingValue(deductionType, scmm + single, matchedDeductions);

                // 3、单条贴扣点一起计算了；
                dd = single;
                // 4、全局贴点（此处不计算对最高的政策的贴点，在所有价格没有出来之前，无法计算），注意此处的贴点值是贴到；
                // 不论是单条是扣还是贴，只要满足条件，就贴到此值；
                if (needSubsidize && global > scmm)
                {
                    dd = global - scmm;
                }

                // 6、全局扣点；
                if (global < 0)
                {
                    dd = single + global;
                    // 防止过度扣点，扣点的上限为卖出返点；
                    if (scmm < dd) dd = scmm;
                }
            }
            // 买入返点
            var cmm = scmm  + dd;

            //// 2013-04-01 deng.zhao 重大变动，将政策协调从此处移除，在最后统一操作；
            //// 政策协调
            //var hasHarmony = false;
            //var harmonyValue = GetHarmonyValue(policy, deductionType, harmonies);
            //if(harmonyValue > 0) {
            //    var cmm2 = GetHarmonyCommission(cmm, harmonyValue);
            //    hasHarmony = cmm != cmm2;
            //    cmm = cmm2;
            //}

            // 保证返点在[0, 1]这个区间内
            if(cmm < 0) cmm = 0;
            if(cmm > 1) cmm = 1;
            // 由于处理了贴扣点和协调，需要反过来处理平台的点
            dd = scmm - cmm;

            // 票面价
            var pv = 0M;
            // 结算价
            var settleAmount = 0M;

            // 特价政策中的往返（针对往返产品舱），单独处理(票面价取法不一样)
            var bargainPolicy = policy as BargainPolicyInfo;
            if(bargainPolicy != null && bargainPolicy.VoyageType == VoyageType.RoundTrip) {
                pv = ComputeBargainParValue(bargainPolicy, 0, patPrice);
                settleAmount = Utility.Calculator.Round((pv / 2) * (1 - cmm), -2) * 2;
            } else {
                // 特价中的单程 与 无pat价格时的处理方式 一样
                if(bargainPolicy != null && bargainPolicy.VoyageType == VoyageType.OneWay) {
                    // 无pat价格时，由于特价增加了返佣类型，此处传入了价格；
                    voyageFilterInfos.ForEach(item => {
                        var currentParValue = ComputeBargainParValue(bargainPolicy, item.Flight.StandardPrice, patPrice);
                        pv += currentParValue;
                        settleAmount += currentParValue * (1 - cmm);
                    });
                } else {
                    if(patPrice.HasValue) {
                        // 有pat价格时,需要判断将差价分摊到各航段上去
                        var originalTotalParValue = voyageFilterInfos.Sum(item => ComputeParValue(policy, item.Bunk.Type, item.Flight.StandardPrice, item.Bunk.Discount, patPrice, superior, passengerType));
                        var totalBalance = originalTotalParValue - patPrice.Value;
                        var flightCount = voyageFilterInfos.Count();
                        var averageBalance = Utility.Calculator.Round(totalBalance / flightCount, 1);
                        var currentIndex = 0;
                        voyageFilterInfos.ForEach(item => {
                            currentIndex++;
                            var currentParValue = ComputeParValue(policy, item.Bunk.Type, item.Flight.StandardPrice, item.Bunk.Discount, patPrice, superior, passengerType);
                            if(currentIndex == flightCount) {
                                currentParValue -= totalBalance - averageBalance * (flightCount - 1);
                            } else {
                                currentParValue -= averageBalance;
                            }
                            pv += currentParValue;
                            settleAmount += currentParValue * (1 - cmm);
                        });
                    } else {
                        // 无pat价格时
                        voyageFilterInfos.ForEach(item => {
                            var currentParValue = ComputeParValue(policy, item.Bunk.Type, item.Flight.StandardPrice, item.Bunk.Discount, patPrice, superior, passengerType);
                            pv += currentParValue;
                            settleAmount += currentParValue * (1 - cmm);
                        });
                    }
                }
            }
            if(pv < 0) return null;
            return new MatchedPolicy {
                Id = policy.Id,
                Provider = policy.Owner,
                RelationType = GetRelationType(policy.Owner, superior),
                Commission = cmm,
                Deduction = dd,
                OriginalPolicy = policy,
                PolicyType = policy.PolicyType,
                Rebate = scmm,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                ParValue = pv,
                WorkStart = policy.WorkStart,
                WorkEnd = policy.WorkEnd,
                RefundStart = policy.RefundStart,
                RefundEnd = policy.RefundEnd
                //,
                //HasHarmony = hasHarmony,
                //HarmonyValue = harmonyValue
            };
        }

        /// <summary>
        /// 对给出的外部政策进行计算，给出相应的匹配政策；
        /// </summary>
        /// <param name="policy">外部政策</param>
        /// <param name="policyType">政策类型</param>
        /// <param name="superior">上级信息</param>
        /// <param name="harmonies">协调内容列表</param>
        /// <returns>匹配政策</returns>
        internal static MatchedPolicy ComputePolicy(ExternalPolicyView policy, PolicyType policyType, SuperiorInfo superior, IEnumerable<PolicyHarmonyInfo> harmonies)
        {
            // 获取销售关系；
            var relationType = Calculator.GetRelationType(policy.Provider, superior);
            // 获取返点类型；
            var deductionType = GetDeductionType(relationType);

            //// 根据政策类型、返点类型、协调内容，计算协调值；
            //var harmonyValue = GetHarmonyValue(policyType, deductionType, harmonies);
            //// 根据协调值，调整采购返点；
            //var commission = GetHarmonyCommission(policy.Rebate, harmonyValue);
            //// 根据协调后的采购返点和原采购返点是否一致，判断政策是否协调；
            //var hasHarmony = (commission != policy.Rebate);
            //// 根据采购返点，计算结算价；
            //var settleAmount = policy.ParValue * (1 - commission);
            var settleAmount = policy.ParValue * (1 - policy.Rebate);
            // 获取客户等级；
            var grade = CompanyService.GetCompanyParameter(policy.Provider).Creditworthiness.HasValue
                                ? CompanyService.GetCompanyParameter(policy.Provider).Creditworthiness.Value
                                : 5;

            return new MatchedPolicy()
                       {
                           // 直接生成的，无任何作用；
                           Id = Guid.NewGuid(),
                           IsExternal = true,
                           OriginalExternalPolicy = policy,
                           Provider = policy.Provider,
                           PolicyType = GetExternalPolicyType(policy.PolicyType, policyType),
                           // 原始返点；
                           Rebate = policy.Rebate,
                           // 采购得到的返点
                           Commission = policy.Rebate,
                           // 关系类型，根据编号计算；
                           RelationType = relationType,
                           Deduction = 0,
                           SettleAmount = settleAmount,
                           ParValue = policy.ParValue,
                           WorkEnd = policy.WorkEnd,
                           WorkStart = policy.WorkStart,
                           RefundStart = policy.ScrapStart,
                           RefundEnd = policy.ScrapEnd,
                           OfficeNumber = policy.OfficeNo,
                           NeedAUTH = policy.RequireAuth,
                           Speed = new GeneralProductSpeedInfo.Item(policy.ETDZSpeed, null),
                           CompannyGrade =grade
                           //,
                           //HarmonyValue = harmonyValue,
                           //HasHarmony = hasHarmony
                       };
        }

        internal static PolicyType GetExternalPolicyType(PolicyType? externalPolicyType, PolicyType requirePolicyType)
        {
            if(externalPolicyType.HasValue) return externalPolicyType.Value;
            if(Enum.IsDefined(typeof(PolicyType), requirePolicyType))
            {
                return requirePolicyType;
            }else
            {
                var policyTypes = Enum.GetValues(typeof (PolicyType)) as PolicyType[];
                return policyTypes.First(pt => pt != PolicyType.Unknown && (requirePolicyType & pt) == pt);
            }
        }
        
        /// <summary>
        /// 计算儿童政策的匹配信息
        /// </summary>
        /// <param name="workingSetting"></param>
        /// <param name="voyageFilterInfos"></param>
        /// <returns></returns>
        internal static MatchedPolicy ComputeChildPolicy(Data.DataMapping.WorkingSetting workingSetting, IEnumerable<VoyageFilterInfo> voyageFilterInfos, SuperiorInfo superior) {
            var rebate = workingSetting.RebateForChild.Value;
            var totalParValue = 0M;
            var settleAmount = 0M;
            voyageFilterInfos.ForEach(voyage => {
                var parValue = ComputeNormalParValue(voyage.Flight.StandardPrice, voyage.Bunk.Discount, PassengerType.Child);
                totalParValue += parValue;
                settleAmount += parValue * (1 - rebate);
            });
            return new MatchedPolicy {
                Id = workingSetting.Company,
                Commission = rebate,
                OfficeNumber = workingSetting.DefaultOfficeNumber,
                NeedAUTH = workingSetting.IsImpower,
                ParValue = totalParValue,
                Provider = workingSetting.Company,
                PolicyType = PolicyType.NormalDefault,
                Rebate = rebate,
                SettleAmount = settleAmount,
                RelationType = GetRelationType(workingSetting.Company, superior),
            };
        }

        /// <summary>
        /// 计算特殊政策的信息
        /// </summary>
        internal static MatchedPolicy ComputeSpecialPolicy(SpecialPolicyInfo policy, decimal standardPrice, decimal discount, SuperiorInfo superior) {
            DeductionType deductionType = GetDeductionType(policy.Owner, superior);
            var settleAmount = ComputeSpecialSettlementPrice(policy, standardPrice, discount, deductionType);
            // 用于过滤掉不合理的政策；
            // 2013-01-14，为处理低价不出的政策，在前一步，将低级不出的结算价做成了0；
            if(settleAmount <= 0) return null;
            return new MatchedPolicy {
                Id = policy.Id,
                RelationType = GetRelationType(policy.Owner, superior),
                OriginalPolicy = policy,
                ParValue = ComputeSpecialParValue(policy, standardPrice, discount, deductionType),
                PolicyType = policy.PolicyType,
                Provider = policy.Owner,
                SettleAmount = settleAmount,
                WorkStart = policy.WorkStart,
                WorkEnd = policy.WorkEnd,
                RefundStart = policy.RefundStart,
                RefundEnd = policy.RefundEnd,
                IsSeat = policy.IsSeat,
                ConfirmResource = policy.ConfirmResource
                
            };
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-01-11 deng.zhao 为计算外部导入政策专用；
        /// </remarks>
        internal static MatchedPolicy ComputeSpecialPolicy(MatchedPolicy policy, decimal deductionForPlatform)
        {
            // 平台扣点；
            var commission = policy.Commission - deductionForPlatform;
            var deduction = deductionForPlatform;
            if (commission < 0)
            {
                commission = 0;
                deduction = commission;
            }
            var settleAmount = (1 - commission) * policy.ParValue;

            policy.Commission = commission;
            policy.SettleAmount = settleAmount;
            policy.Deduction = deduction;
            return policy;
        }

        internal static MatchedPolicy ComputeSpecialPolicy(NormalPolicyInfo policy, decimal deductionForProvider, decimal deductionForPlatform, decimal standardPrice, decimal discount) {
            var provideCommission = policy.ProfessionCommission - deductionForProvider;
            if(provideCommission < 0) provideCommission = 0;

            // 平台扣点；
            var commission = provideCommission - deductionForPlatform;
            if(commission < 0) {
                commission = 0;
                deductionForPlatform = provideCommission;
            }
            var parValue = ComputeNormalParValue(standardPrice, discount, PassengerType.Adult);
            var settlement = (1 - commission) * parValue;
            return new MatchedPolicy {
                Id = policy.Id,
                RelationType = RelationType.Brother,
                OriginalPolicy = policy,
                ParValue = parValue,
                Commission = commission,
                Deduction = deductionForPlatform,
                OfficeNumber = policy.OfficeCode,
                PolicyType = policy.PolicyType,
                Provider = policy.Owner,
                Rebate = provideCommission,
                SettleAmount = settlement,
                WorkStart = policy.WorkStart,
                WorkEnd = policy.WorkEnd,
                RefundStart = policy.RefundStart,
                RefundEnd = policy.RefundEnd
            };
        }

        internal static MatchedPolicy ComputeDefaultSpecialPolicy(NormalDefaultPolicyInfo policy, decimal deductionForPlatform, decimal standardPrice, decimal discount) {
            var commission = policy.AdultCommission - deductionForPlatform;
            if(commission < 0) {
                commission = 0;
                deductionForPlatform = policy.AdultCommission;
            }
            var parValue = ComputeNormalParValue(standardPrice, discount, PassengerType.Adult);
            var settlement = (1 - commission) * parValue;
            var ws = CompanyService.GetWorkingSetting(policy.AdultProviderId);
            return new MatchedPolicy {
                RelationType = RelationType.Brother,
                ParValue = parValue,
                Commission = commission,
                Deduction = deductionForPlatform,

                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(policy.AdultProviderId),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                PolicyType = PolicyType.NormalDefault,
                Provider = policy.AdultProviderId,
                Rebate = policy.AdultCommission,
                SettleAmount = settlement
            };
        }

        /// <summary>
        /// 计算默认政策信息(航班查询时使用)
        /// </summary>
        internal static MatchedPolicy ComputeNormalDefaultPolicy(NormalDefaultPolicyInfo defaultPolicy, decimal standardPrice, decimal discount, SuperiorInfo superior, PassengerType passengerType) {
            var provider = passengerType == PassengerType.Adult ? defaultPolicy.AdultProviderId : defaultPolicy.ChildProviderId;
            var commission = passengerType == PassengerType.Adult ? defaultPolicy.AdultCommission : defaultPolicy.ChildCommission;
            var par = ComputeNormalParValue(standardPrice, discount, passengerType);
            var settleAmount = par * (1 - commission);

            return new MatchedPolicy {
                // Source属性基本无用，后面废弃；
                //Id = defaultPolicy.Source,
                // 航班查询不需要Office号
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.NormalDefault,

            };
        }

        /// <summary>
        /// 计算默认政策信息（政策选择和生成订单时使用）
        /// </summary>
        /// <param name="defaultPolicy"></param>
        /// <param name="voyageFilterInfos"></param>
        /// <param name="superior"></param>
        /// <param name="patPrice"></param>
        /// <param name="passengerType"></param>
        /// <returns></returns>
        internal static MatchedPolicy ComputeNormalDefaultPolicy(NormalDefaultPolicyInfo defaultPolicy, IEnumerable<VoyageFilterInfo> voyageFilterInfos, SuperiorInfo superior, decimal? patPrice, PassengerType passengerType) {
            var provider = passengerType == PassengerType.Adult ? defaultPolicy.AdultProviderId : defaultPolicy.ChildProviderId;
            var commission = passengerType == PassengerType.Adult ? defaultPolicy.AdultCommission : defaultPolicy.ChildCommission;
            var par = 0M;
            var settleAmount = 0M;
            if(patPrice.HasValue) {
                // 有pat价格时,需要判断将差价分摊到各航段上去
                var originalTotalParValue = voyageFilterInfos.Sum(item => ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType));
                var totalBalance = originalTotalParValue - patPrice.Value;
                var flightCount = voyageFilterInfos.Count();
                var averageBalance = Utility.Calculator.Round(totalBalance / flightCount, 1);
                var currentIndex = 0;
                voyageFilterInfos.ForEach(item => {
                    currentIndex++;
                    var currentParValue = ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType);
                    if(currentIndex == flightCount) {
                        currentParValue -= totalBalance - averageBalance * (flightCount - 1);
                    } else {
                        currentParValue -= averageBalance;
                    }
                    par += currentParValue;
                    settleAmount += currentParValue * (1 - commission);
                });
            } else {
                // 无pat价格时
                voyageFilterInfos.ForEach(item => {
                    var currentParValue = ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType);
                    par += currentParValue;
                    settleAmount += currentParValue * (1 - commission);
                });
            }
            // 2013-04-16 deng.zhao
            var ws = CompanyService.GetWorkingSetting(provider);
            return new MatchedPolicy {
                //Id = defaultPolicy.Source,
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.NormalDefault
            };
        }

        /// <summary>
        /// 计算特价默认政策（航班查询时使用），航班查询的时候是用不到了；基本没用。
        /// </summary>
        /// <param name="defaultPolicy">特价默认政策</param>
        /// <param name="standardPrice">标准价</param>
        /// <param name="discount">折扣</param>
        /// <param name="superior">上级编号</param>
        /// <param name="passengerType">旅客类型</param>
        /// <returns>匹配的政策</returns>
        internal static MatchedPolicy ComputeBargainDefaultPolicy(BargainDefaultPolicyInfo defaultPolicy, decimal patPrice, SuperiorInfo superior) {
            var provider = defaultPolicy.AdultProviderId;
            var commission = defaultPolicy.AdultCommission;
            // 直接设置为Pat的价格；
            var par = patPrice;
            var settleAmount = par * (1 - commission);
            var ws = CompanyService.GetWorkingSetting(provider);
            return new MatchedPolicy {
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.BargainDefault,
            };
        }

        /// <summary>
        /// 计算特价默认政策（政策选择和生成订单时使用），不一定可以匹配到，若无法匹配到，返回一个空集合；
        /// </summary>
        /// <param name="defaultPolicy"></param>
        /// <param name="voyageFilterInfos"></param>
        /// <param name="superior"></param>
        /// <param name="patPrice"></param>
        /// <param name="passengerType"></param>
        /// <returns></returns>
        internal static MatchedPolicy ComputeBargainDefaultPolicy(BargainDefaultPolicyInfo defaultPolicy, IEnumerable<VoyageFilterInfo> voyageFilterInfos, SuperiorInfo superior, decimal patPrice) {


            var provider = defaultPolicy.AdultProviderId;
            var commission = defaultPolicy.AdultCommission;
            // 直接设置为Pat的价格；
            var par = patPrice;
            var settleAmount = par * (1 - commission);
            var ws = CompanyService.GetWorkingSetting(provider);
            return new MatchedPolicy {
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.BargainDefault
            };
        }

        /// <summary>
        /// 计算上级默认政策（航班查询时使用）
        /// </summary>
        /// <param name="defaultPolicy"></param>
        /// <param name="standardPrice"></param>
        /// <param name="discount"></param>
        /// <param name="superior"></param>
        /// <param name="passengerType"></param>
        /// <returns></returns>
        internal static MatchedPolicy ComputeOwnerDefaultPolicy(OwnerDefaultPolicyInfo defaultPolicy, decimal standardPrice, decimal discount, SuperiorInfo superior, PassengerType passengerType) {
            var provider = passengerType == PassengerType.Adult ? defaultPolicy.AdultProviderId : defaultPolicy.ChildProviderId;
            var commission = passengerType == PassengerType.Adult ? defaultPolicy.AdultCommission : defaultPolicy.ChildCommission;
            var par = ComputeNormalParValue(standardPrice, discount, passengerType);
            var settleAmount = par * (1 - commission);
            var ws = CompanyService.GetWorkingSetting(provider);
            return new MatchedPolicy {
                // 借用Id的地方，存放的是
                Id = defaultPolicy.LimitationId,
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.OwnerDefault,
            };
        }

        /// <summary>
        /// 计算上级默认政策（政策选择和生成订单时使用）
        /// </summary>
        /// <param name="defaultPolicy"></param>
        /// <param name="voyageFilterInfos"></param>
        /// <param name="superior"></param>
        /// <param name="patPrice"></param>
        /// <param name="passengerType"></param>
        /// <returns></returns>
        /// <remarks>
        /// 现在只有在获得默认政策时才会调用此方法，其它地方都没有调用，所以，此处不会处理儿童票，
        /// 所以，关于乘客类型那里是可以去掉了；
        /// </remarks>
        internal static MatchedPolicy ComputeOwnerDefaultPolicy(OwnerDefaultPolicyInfo defaultPolicy, IEnumerable<VoyageFilterInfo> voyageFilterInfos, SuperiorInfo superior, decimal? patPrice, PassengerType passengerType) {
            var provider = passengerType == PassengerType.Adult ? defaultPolicy.AdultProviderId : defaultPolicy.ChildProviderId;
            var commission = passengerType == PassengerType.Adult ? defaultPolicy.AdultCommission : defaultPolicy.ChildCommission;
            var par = 0M;
            var settleAmount = 0M;

            if(patPrice.HasValue) {
                // 有pat价格时,需要判断将差价分摊到各航段上去
                var originalTotalParValue = voyageFilterInfos.Sum(item => ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType));
                var totalBalance = originalTotalParValue - patPrice.Value;
                var flightCount = voyageFilterInfos.Count();
                var averageBalance = Utility.Calculator.Round(totalBalance / flightCount, 1);
                var currentIndex = 0;
                voyageFilterInfos.ForEach(item => {
                    currentIndex++;
                    var currentParValue = ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType);
                    if(currentIndex == flightCount) {
                        currentParValue -= totalBalance - averageBalance * (flightCount - 1);
                    } else {
                        currentParValue -= averageBalance;
                    }
                    par += currentParValue;
                    settleAmount += currentParValue * (1 - commission);
                });
            } else {
                // 无pat价格时
                voyageFilterInfos.ForEach(item => {
                    var currentParValue = ComputeNormalParValue(item.Flight.StandardPrice, item.Bunk.Discount, passengerType);
                    par += currentParValue;
                    settleAmount += currentParValue * (1 - commission);
                });
            }
            var ws = CompanyService.GetWorkingSetting(provider);
            return new MatchedPolicy {
                Id = defaultPolicy.LimitationId,
                //OfficeNumber = CompanyService.QueryDefaultOfficeNumber(provider),
                OfficeNumber = ws == null ? string.Empty : ws.DefaultOfficeNumber,
                NeedAUTH = ws != null && ws.IsImpower,

                RelationType = GetRelationType(provider, superior),
                Provider = provider,
                Commission = commission,
                SettleAmount = Utility.Calculator.Round(settleAmount, -2),
                Deduction = 0,
                OriginalPolicy = null,
                ParValue = par,
                Rebate = commission,
                PolicyType = PolicyType.OwnerDefault
            };
        }
        

        internal static MatchedPolicy ProcessIncomeDeduction(MatchedPolicy policy, IncomeGroupLimitGroup group,
                                                             Guid superior,
                                                             string carrier,
                                                             AirportPair airportPair,
                                                             DateTime flightDate)
        {
            // 政策检查，若不为下级和默认政策；
            if (policy.RelationType == RelationType.Interior ||
                policy.PolicyType == PolicyType.NormalDefault ||
                policy.PolicyType == PolicyType.BargainDefault ||
                policy.PolicyType == PolicyType.BargainDefault)
            {
                return policy;
            }

            var incomeLimitation = GetIncomeLimitation(group, carrier, policy.RelationType);

            // 根据利润类型，设置政策上的属性；
            switch (policy.OemInfo.ProfitType)
            {
                case OemProfitType.PriceMarkup:
                    // 获取设置加价值；
                    var settingPriceMarkup = incomeLimitation.Price;
                    // 获取标准票面；
                    var standarPrice =
                        FoundationService.QueryBasicPrice(carrier, airportPair.Departure, airportPair.Arrival,
                                                          flightDate).Price;
                    ComputeForIncomePriceMarkup(policy, superior, standarPrice, settingPriceMarkup);
                    break;
                case OemProfitType.Discount:
                    // 获取折扣；
                    var settingDiscount = Calculator.GetIncomePolicyDeduction(policy.Commission, incomeLimitation);
                    // 获取设置折扣值；（2013-05-14 deng.zhao 这里按需求做调整）
                    ComputeForIncomeDiscount(policy, superior, settingDiscount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return policy;
        }


        /// <summary>
        /// 根据给出的收益组，和给出的航空公司，获取收益设置；
        /// </summary>
        /// <param name="limitationGroup"></param>
        /// <param name="airline"></param>
        /// <param name="relationType"> </param>
        /// <returns></returns>
        internal static IncomeGroupLimit GetIncomeLimitation(IncomeGroupLimitGroup limitationGroup, string airline, RelationType relationType)
        {
            if (limitationGroup == null || limitationGroup.Limitation == null) return null;

            var incomeLimitation = (from il in limitationGroup.Limitation
                                    where il.Airlines.Contains(airline) && il.IsOwnerPolicy == (relationType != RelationType.Brother)
                                    select il).FirstOrDefault();

            return incomeLimitation;
        }

        
        private static MatchedPolicy ComputeForIncomePriceMarkup(MatchedPolicy policy, Guid companyId, decimal standarPrice, decimal settingPriceMarkup)
        {
            decimal value;
            // 判断此时，加价后的结算价是否大于标准票面；
            if (policy.SettleAmount + settingPriceMarkup <= standarPrice || policy.SettleAmount>standarPrice)
            {
                value = settingPriceMarkup;
                policy.SettleAmount += settingPriceMarkup;
            }
            else
            {
                value = standarPrice - policy.SettleAmount;
                policy.SettleAmount = standarPrice;
            }

            if (policy.OemInfo.Profits == null)
            {
                policy.OemInfo.Profits = new List<OemProfit>();
            }

            policy.OemInfo.Profits.Add(new OemProfit()
                                           {
                                               CompanyId = companyId,
                                               Value = value
                                           });
            return policy;
        }

        /// <summary>
        /// 计算折扣值，实际为计算收益组下的折扣值。
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="companyId"></param>
        /// <param name="maxRebate">设置的最高返点值</param>
        /// <returns></returns>
        private static MatchedPolicy ComputeForIncomeDiscount(MatchedPolicy policy, Guid companyId, decimal maxRebate)
        {
            // 若获得的返点大于设置的最高返点，则设置其为最高返点，否则不变。
            if (policy.Commission > maxRebate)
            {
                policy.Commission = policy.Commission - maxRebate;
            }

            policy.SettleAmount =
                Utility.Calculator.Round(policy.ParValue * (1 - policy.Commission), -2);

            if (policy.OemInfo.Profits == null)
            {
                policy.OemInfo.Profits = new List<OemProfit>();
            }

            policy.OemInfo.Profits.Add(new OemProfit()
            {
                CompanyId = companyId,
                Value = maxRebate
            });
            return policy;
        }


    }
}