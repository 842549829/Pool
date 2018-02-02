using System;
using System.Collections.Generic;
using System.Data;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Policy;

namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    class DataCenter {
        private static DataCenter _instance;
        private static object _locker = new object();
        public static DataCenter Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new DataCenter();
                        }
                    }
                }
                return _instance;
            }
        }

        private ChinaPay.Data.Cache<Guid, SuperiorInfo> _superiorCache;
        private ChinaPay.Data.Cache<Dictionary<Guid, Organization.Domain.Account>> _validReceiveAccountCache;
        private DataCenter() {
            // 上下级关系，过期时间为24小时
            _superiorCache = new ChinaPay.Data.Cache<Guid, SuperiorInfo> {
                Timeout = 24 * 60 * 60
            };
            // 有效收款账号，过期时间为5分钟
            _validReceiveAccountCache = new ChinaPay.Data.Cache<Dictionary<Guid, Organization.Domain.Account>> {
                Timeout = 300
            };
        }

        public IEnumerable<PolicyInfoBase> QueryPolicies(string departure, DateTime flightDate, VoyageType voyageType, PolicyType policyType) {
            return PolicyManageService.QueryPolicies(departure, flightDate, flightDate, voyageType, policyType, null);
        }
        public IEnumerable<PolicyInfoBase> QueryPolicies(string airline, string departure, DateTime flightDate, VoyageType voyageType, PolicyType policyType) {
            return PolicyManageService.QueryPolicies(departure, flightDate, flightDate, voyageType, policyType, airline);
        }

        /// <summary>
        /// 根据行程条件、行程类型及政策类型，获取相应的政策；
        /// </summary>
        /// <param name="airline">航空公司</param>
        /// <param name="voyages">行程条件</param>
        /// <param name="voyageType">行程类型</param>
        /// <param name="policyType">政策类型</param>
        /// <returns></returns>
        public IEnumerable<PolicyInfoBase> QueryPolicies(string airline, DataTable voyages, VoyageType voyageType, PolicyType policyType)
        {
            if (airline == null) throw new ArgumentNullException("airline");
            if (voyages == null || voyages.Columns.Count == 0) throw new ArgumentNullException("voyages");

            return PolicyManageService.QueryPolicies(airline, voyages, voyageType, policyType);
        }

        public IEnumerable<PolicyInfoBase> QueryPolicies(string departure, DateTime flightStartDate, DateTime flightEndDate, VoyageType voyageType, PolicyType policyType) {
            return PolicyManageService.QueryPolicies(departure, flightStartDate, flightEndDate, voyageType, policyType, null);
        }
        /// <summary>
        /// 根据用户编号查询其发展者信息
        /// </summary>
        public SuperiorInfo QuerySuperior(Guid purchaser) {
            //var superior = _superiorCache[purchaser];
            //if(superior == null) {
            //    superior = Service.Organization.CompanyService.QuerySuperiorInfo(purchaser);
            //    _superiorCache.Add(purchaser, superior);
            //}
            //return superior;
            return Service.Organization.CompanyService.QuerySuperiorInfo(purchaser);
        }
        /// <summary>
        /// 查询所有有效的收款账号
        /// </summary>
        public Dictionary<Guid, Organization.Domain.Account> QueryAllValidReceiveAccount() {
            var result = _validReceiveAccountCache.Value;
            if(result == null) {
                result = Organization.AccountService.GetAllValidAccount(AccountType.Receiving);
                _validReceiveAccountCache.Value = result;
            }
            return result;
        }
        /// <summary>
        /// 查询公司组限制信息
        /// </summary>
        public IEnumerable<CompanyGroupLimitationInfo> QueryCompanyLimmitations(Guid purchaser, SuperiorInfo superior) {
            return (superior != null && superior.Enable && !superior.Expired)
                       ? Organization.CompanyService.GetGroupLimits(purchaser)
                       : Izual.EnumerableHelper.GetEmpty<CompanyGroupLimitationInfo>();
        }
    }
}