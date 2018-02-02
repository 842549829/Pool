using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Statistic;
using Izual;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {

    internal static class PolicySorter {
        private interface ISorter {
            IEnumerable<MatchedPolicy> Sort(IEnumerable<MatchedPolicy> policies);
        }

        private abstract class GeneralPolicySorter : ISorter {
            private class Comparer : IComparer<MatchedPolicy> {
                public int Compare(MatchedPolicy x, MatchedPolicy y) {
                    if(x.SettleAmount > y.SettleAmount)
                        return 1;
                    if(x.SettleAmount < y.SettleAmount)
                        return -1;

                    if(x.Commission > y.Commission)
                        return -1;
                    if(x.Commission < y.Commission)
                        return 1;

                    if(x.Speed.ETDZ > y.Speed.ETDZ)
                        return 1;
                    if(x.Speed.ETDZ < y.Speed.ETDZ)
                        return -1;

                    if(x.Speed.Refund > y.Speed.Refund)
                        return 1;
                    if(x.Speed.Refund < y.Speed.Refund)
                        return -1;

                    if(x.RelationType > y.RelationType)
                        return -1;
                    if(x.RelationType < y.RelationType)
                        return 1;

                    if(x.Deduction > y.Deduction)
                        return -1;
                    if(x.Deduction < y.Deduction)
                        return 1;

                    if(!(x.OriginalPolicy == null || y.OriginalPolicy == null)) {
                        if(x.OriginalPolicy.CreateTime > y.OriginalPolicy.CreateTime)
                            return -1;
                        if(x.OriginalPolicy.CreateTime < y.OriginalPolicy.CreateTime)
                            return 1;
                    }
                    return 0;
                }
            }
            private static readonly Comparer comparer = new Comparer();

            private readonly string m_carrier;
            protected GeneralPolicySorter(string carrier) {
                if(string.IsNullOrWhiteSpace(carrier)) throw new ArgumentNullException("carrier");
                m_carrier = carrier;
            }

            public IEnumerable<MatchedPolicy> Sort(IEnumerable<MatchedPolicy> policies) {
                if(policies == null) throw new ArgumentNullException("policies");

                if(!policies.Any()) return policies;
                policies = Filter(policies);
                var speeds = OrderStatisticService.QuerySpeed(policies.Select(p => p.Provider).Distinct(), m_carrier);
                policies.ForEach(p => p.SetSpeed(speeds[p.Provider]));

                if(policies.Count() < 2) return policies;
                var result = policies.ToList();
                result.Sort(comparer);
                return result;
            }

            protected virtual IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies) {
                return policies;
            }
        }

        /// <summary>
        /// deng.zhao 2013-01-21
        /// </summary>
        private class ExternalPolicySort : GeneralPolicySorter
        {
            public ExternalPolicySort(string carrier)
                : base(carrier)
            {
            }

            protected override IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies)
            {
                // 此处不做任何过滤；
                return policies.ToList();
            }
        }

        private class NormalPolicySorter : GeneralPolicySorter {
            public NormalPolicySorter(string carrier)
                : base(carrier) {
            }

            protected override IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies) {
                return policies.Where(p => p.PolicyType == PolicyType.Normal || p.PolicyType == PolicyType.NormalDefault).ToList();
            }
        }

        private class NotchPolicySorter : GeneralPolicySorter
        {
            public NotchPolicySorter(string carrier)
                : base(carrier)
            {
            }

            protected override IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies)
            {
                return policies.Where(p => p.PolicyType == PolicyType.Notch).ToList();
            }
        }

        private class TeamPolicySorter : GeneralPolicySorter {
            public TeamPolicySorter(string carrier)
                : base(carrier) {
            }

            protected override IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies) {
                return policies.Where(p => p.PolicyType == PolicyType.Team || p.PolicyType == PolicyType.NormalDefault).ToList();
            }
        }

        private class BargainPolicySorter : GeneralPolicySorter {
            public BargainPolicySorter(string carrier)
                : base(carrier) {
            }

            protected override IEnumerable<MatchedPolicy> Filter(IEnumerable<MatchedPolicy> policies) {
                return policies.Where(p => p.PolicyType == PolicyType.Bargain).ToList();
            }
        }

        private class SpecialPolicySorter : ISorter {
            private class Comparer : IComparer<MatchedPolicy> {
                public int Compare(MatchedPolicy x, MatchedPolicy y) {
                    if(x.SettleAmount > y.SettleAmount)
                        return 1;
                    if(x.SettleAmount < y.SettleAmount)
                        return -1;

                    if(x.Statistics.Total.OrderSuccessRate > y.Statistics.Total.OrderSuccessRate)
                        return -1;
                    if(x.Statistics.Total.OrderSuccessRate > y.Statistics.Total.OrderSuccessRate)
                        return 1;

                    if(x.Statistics.Total.TicketCount > y.Statistics.Total.TicketCount)
                        return -1;
                    if(x.Statistics.Total.TicketCount < y.Statistics.Total.TicketCount)
                        return 1;

                    if(x.Statistics.Voyage.TicketCount > y.Statistics.Voyage.TicketCount)
                        return -1;
                    if(x.Statistics.Voyage.TicketCount > y.Statistics.Voyage.TicketCount)
                        return 1;

                    if(x.RelationType > y.RelationType)
                        return -1;
                    if(x.RelationType < y.RelationType)
                        return 1;

                    if(x.OriginalPolicy.CreateTime > y.OriginalPolicy.CreateTime)
                        return -1;
                    if(x.OriginalPolicy.CreateTime < y.OriginalPolicy.CreateTime)
                        return 1;

                    return 0;
                }
            }
            private static readonly Comparer comparer = new Comparer();

            private readonly string m_departure;
            private readonly string m_arrival;
            public SpecialPolicySorter(string departure, string arrival) {
                if(string.IsNullOrWhiteSpace(departure)) throw new ArgumentNullException("departure");
                if(string.IsNullOrWhiteSpace(arrival)) throw new ArgumentNullException("arrival");
                this.m_departure = departure;
                this.m_arrival = arrival;
            }
            public IEnumerable<MatchedPolicy> Sort(IEnumerable<MatchedPolicy> policies) {
                if(policies == null) throw new ArgumentNullException("policies");

                policies = policies.Where(p => p.PolicyType == PolicyType.Special).ToList();
                if(!policies.Any()) return policies;
                var statisticInfos = OrderStatisticService.QuerySupplyStatisticInfo(policies.Select(p => p.Provider).Distinct(), m_departure, m_arrival);
                policies.ForEach(p => p.Statistics = statisticInfos[p.Provider]);

                if(policies.Count() < 2) return policies;
                var result = policies.ToList();
                result.Sort(comparer);
                return result;
            }
        }
        
        /// <summary>
        /// 对外部政策排序
        /// </summary>
        /// <param name="policies">待排序政策</param>
        /// <param name="carrier">承运人</param>
        /// <returns>排序后政策列表</returns>
        /// <remarks>
        /// deng.zhao 2013-01-21 新增；
        /// </remarks>
        public static IEnumerable<MatchedPolicy> SortExternalPolicy(this IEnumerable<MatchedPolicy> policies, string carrier)
        {
            return new ExternalPolicySort(carrier).Sort(policies);
        }

        public static IEnumerable<MatchedPolicy> SortNotchPolicy(this IEnumerable<MatchedPolicy> policies, string carrier)
        {
            return new NotchPolicySorter(carrier).Sort(policies);
        }

        public static IEnumerable<MatchedPolicy> SortNormalPolicy(this IEnumerable<MatchedPolicy> policies, string carrier) {
            return new NormalPolicySorter(carrier).Sort(policies);
        }
        public static IEnumerable<MatchedPolicy> SortBargainPolicy(this IEnumerable<MatchedPolicy> policies, string carrier) {
            return new BargainPolicySorter(carrier).Sort(policies);
        }
        public static IEnumerable<MatchedPolicy> SortTeamPolicy(this IEnumerable<MatchedPolicy> policies, string carrier) {
            return new TeamPolicySorter(carrier).Sort(policies);
        }
        public static IEnumerable<MatchedPolicy> SortSpecialPolicy(this IEnumerable<MatchedPolicy> policies, string departure, string arrival) {
            return new SpecialPolicySorter(departure, arrival).Sort(policies);
        }
    }
}