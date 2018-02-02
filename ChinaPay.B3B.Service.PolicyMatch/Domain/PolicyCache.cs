
namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    using System;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using DataTransferObject.Policy;
    using Izual;
    using Policy;
    using PolicyComparer = Izual.EqualityComparer<DataTransferObject.Policy.PolicyInfoBase>;
    using ChinaPay.B3B.Common.Enums;

    public class FlatPolicyCache : IEnumerable<PolicyInfoBase> {
        private HashSet<PolicyInfoBase> policies;
        private readonly object locker = new object();
        private DateTime lastUpdateTime;
        private TimeSpan timeout;
        private bool refreshing;

        private bool IsExpired() {
            return lastUpdateTime.Add(timeout) <= DateTime.Now;
        }
        private HashSet<PolicyInfoBase> LoadPolicies() {
            var data = new HashSet<PolicyInfoBase>(new PolicyComparer("Id"));
            data.AddRange(PolicyManageService.QueryPolicies<NormalPolicyInfo>(p => p.Audited && !p.Freezed && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            data.AddRange(PolicyManageService.QueryPolicies<BargainPolicyInfo>(p => p.Audited && !p.Freezed && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            data.AddRange(PolicyManageService.QueryPolicies<SpecialPolicyInfo>(p => p.Audited && p.PlatformAudited && !p.Freezed && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            // 2012-10-21 增加，团队政策；
            data.AddRange(PolicyManageService.QueryPolicies<TeamPolicyInfo>(p => p.Audited && !p.Freezed && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            
            // 2012-10-18 修改，修改第一行,去掉了资源张数的限制，在最后再去判断；去掉了第两行；
            //data.AddRange(PolicyManageService.QueryPolicies<SpecialPolicyInfo>(p => p.Audited && p.PlatformAudited && !p.Freezed && p.ResourceAmount > 0 && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            //data.AddRange(PolicyManageService.QueryPolicies<RoundTripPolicyInfo>(p => p.Audited && !p.Freezed && p.OwnerAudited && p.OwnerEnabled).ToList().Where(p => !p.Suspended && !p.OwnerIsExpired));
            
            return data;
        }
        private void ReplaceCacheContent(object state) {
            try {
                var data = LoadPolicies();
                lock (policies) {
                    policies = data;
                }
            }
            finally {
                lastUpdateTime = DateTime.Now;
                refreshing = false;
            }
        }

        public FlatPolicyCache() {
            policies = new HashSet<PolicyInfoBase>(new PolicyComparer("Id"));
        }
        public FlatPolicyCache(IEnumerable<PolicyInfoBase> items) {
            policies = new HashSet<PolicyInfoBase>(items, new PolicyComparer("Id"));
        }

        public void Clear() {
            lock (policies) {
                policies.Clear();
            }
        }
        public void Add(PolicyInfoBase item) {
            lock (policies) {
                policies.Add(item);
            }
        }
        public void AddRange(IEnumerable<PolicyInfoBase> items) {
            lock (policies) {
                items.ForEach(Add);
            }
        }
        public void Delete(PolicyInfoBase item) {
            lock (policies) {
                policies.Remove(item);
            }
        }
        public int Count { get { return policies.Count; } }

        public PolicyInfoBase Get(Guid id) {
            return this.SingleOrDefault(p => p.Id == id);
        }
        /// <summary>
        /// 刷新政策缓存
        /// </summary>
        public void Refresh() {
            if (IsExpired() && !refreshing) {
                ThreadPool.QueueUserWorkItem(ReplaceCacheContent);
            }
        }
        public void AsyncData() {
            ReplaceCacheContent(null);
        }

        public TimeSpan Timeout {
            get { return timeout; }
            set { timeout = value; }
        }

        #region Implementation of IEnumerable

        public IEnumerator<PolicyInfoBase> GetEnumerator() {
            //if (IsExpired() && !refreshing) {
            //    lock (locker) {
            //        if (!refreshing) {
            //            refreshing = true;
            //            //Refresh();
            //            ReplaceCacheContent(null);
            //        }
            //    }
            //}
            policies = LoadPolicies();
            return policies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

    public class CalculateEnvironmentCache {
        private readonly object locker = new { };
        private DateTime lastUpdateTime;
        private TimeSpan timeout;
        private bool refreshing;
        private bool IsExpired() {
            return lastUpdateTime.Add(timeout) <= DateTime.Now;
        }

        private Dictionary<string, NormalDefaultPolicyInfo> defaultPolicies = new Dictionary<string, NormalDefaultPolicyInfo>();
        private IEnumerable<PolicySettingInfo> policySettings = new List<PolicySettingInfo>();
        private IEnumerable<PolicyHarmonyInfo> policyHarmonies = new List<PolicyHarmonyInfo>();

        public IDictionary<string, NormalDefaultPolicyInfo> DefaultPolicies {
            get {
                Refresh();
                return defaultPolicies;
            }
        }
        public IEnumerable<PolicySettingInfo> PolicySettings {
            get {
                Refresh();
                return policySettings;
            }
        }
        public IEnumerable<PolicyHarmonyInfo> PolicyHarmonies {
            get {
                Refresh();
                return policyHarmonies;
            }
        }
        private void ReplaceCacheContent(object state) {
            var dps = PolicyManageService.GetAllDefaultPolicies().ToDictionary(p => p.Airline);
            var pss = PolicyManageService.GetAllPolicySettings().ToList();
            var phs = PolicyManageService.GetAllPolicyHarmonies().ToList();
            try {
                lock (defaultPolicies) {
                    defaultPolicies = dps;
                }
                lock (policySettings) {
                    policySettings = pss;
                }
                lock (policyHarmonies) {
                    policyHarmonies = phs;
                }
            }
            finally {
                lastUpdateTime = DateTime.Now;
                refreshing = false;
            }
        }
        private void Refresh() {
            if (IsExpired() && !refreshing) {
                ThreadPool.QueueUserWorkItem(ReplaceCacheContent);
            }
        }
        public TimeSpan Timeout {
            get { return timeout; }
            set { timeout = value; }
        }
        public void AsyncData() {
            ReplaceCacheContent(null);
        }
    }
}
