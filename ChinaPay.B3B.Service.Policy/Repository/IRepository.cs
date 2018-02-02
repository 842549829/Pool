using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Policy.Repository
{
    using System;
    using System.Collections.Generic;
    using ChinaPay.B3B.Data.DataMapping;
    using ChinaPay.B3B.DataTransferObject.Policy;
    using ChinaPay.B3B.DataTransferObject.Policy.HoldOn;
    using ChinaPay.B3B.Service.Policy.Domain;
    using Common.Enums;
    interface IPolicyHoldOnRepository
    {
        IEnumerable<HoldOnListView> QueryList(Guid? publisher);
        HoldOnView Query(Guid publisher);
        int HoldOn(Guid companyId, IEnumerable<HoldOnItem> items, PolicyOperatorRole role);
        int UnHoldOn(Guid companyId, IEnumerable<HoldOnItem> items, PolicyOperatorRole role);
    }
    interface IPolicySetRepository
    {
        SetPolicy QuerySetPolicy(Guid companyid);
        CompanyLimitPolicy QueryLimitPolicy(Guid companyid);
        IEnumerable<string> QueryAirlines(Guid companyid);
        int Save(SetPolicy policy);
        int Save(CompanyLimitPolicy policy);
    }
    interface IPolicyRepository
    {
        #region  删除
        void DeleteNormalPolicy(params Guid[] ids);
        void DeleteBargainPolicy(params Guid[] ids);
        void DeleteSpecialPolicy(params Guid[] ids);
        void DeleteTeamPolicy(params Guid[] ids);

        void DeleteNotchPolicy(params Guid[] ids);


        void DeleteNormalPolicyDeparture(IEnumerable<Guid> policyId);
        void DeleteBargainPolicyDeparture(IEnumerable<Guid> policyId);
        void DeleteSpecialPolicyDeparture(IEnumerable<Guid> policyId);
        void DeleteTeamPolicyDeparture(IEnumerable<Guid> policyId);

        #endregion
        #region  查询
        List<NormalPolicyInfo> QueryNormalPolicies(PolicyQueryParameter parameter, Pagination pagination);
        List<SpecialPolicyInfo> QuerySpecialPolicies(PolicyQueryParameter parameter, Pagination pagination);
        List<BargainPolicyInfo> QueryBargainPolicies(PolicyQueryParameter parameter, Pagination pagination);
        List<TeamPolicyInfo> QueryTeamPolicies(PolicyQueryParameter parameter, Pagination pagination);
        List<NotchPolicyInfo> QueryNotchPolicies(PolicyQueryParameter parameter, Pagination pagination);

        Dictionary<Guid, bool> QueryPolicyIds(PolicyQueryParameter parameter);

        IEnumerable<PolicyInfoBase> QueryPolicies(string departure, DateTime flightStartDate, DateTime flightEndDate, VoyageType voyageType, PolicyType policyType, string airline);
        IEnumerable<PolicyInfoBase> QueryPolicies(string airLine, System.Data.DataTable voyages, VoyageType voyageType, PolicyType policyType);
        NormalPolicy QueryNormalPolicy(Guid policyId);
        SpecialPolicy QuerySpecialPolicy(Guid policyId);
        BargainPolicy QueryBargainPolicy(Guid policyId);
        TeamPolicy QueryTeamPolicy(Guid policyId);
        NotchPolicy QueryNotchPolicy(Guid policyId);
        #endregion
        #region 修改

        void UpdateNormalPolicy(NormalPolicy normal);
        void UpdateBargainPolicy(BargainPolicy bargain);
        void UpdateSpecialPolicy(SpecialPolicy special);
        int UpdateSpecialPolicy(Guid id, int num);
        void UpdateTeamPolicy(TeamPolicy team);
        void UpdateNotchPolicy(NotchPolicy notch);
        bool UpdateNotchPolicyCommission(Guid id, decimal @internal, decimal subordinate, decimal profession);
        bool NotchAudit(bool audit, params Guid[] ids);
        void NotchLock(bool @lock, params Guid[] ids);
        #endregion
        #region 添加

        void InsertNormalPolicy(IEnumerable<NormalPolicy> normal);
        void InsertBargainPolicy(IEnumerable<BargainPolicy> bargain);
        void InsertSpecialPolicy(IEnumerable<SpecialPolicy> special);
        void InsertTeamPolicy(IEnumerable<TeamPolicy> team);
        void InsertNotchPolicy(IEnumerable<NotchPolicy> notch);

        void UpdateNormalPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId);
        void UpdateBargainPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId);
        void UpdateSpecialPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId);
        void UpdateTeamPolicyDeparture(IEnumerable<string> delDeparture, IEnumerable<string> addDeparture, Guid policyId);

        #endregion

        PolicyType CheckIfHasDefaultPolicy(Guid companyId);
        PolicyType CheckIfHasDefaultPolicy(Guid companyId, List<string> airlines);
    }
    interface IPolicyHarmoniesRepository
    {
        IEnumerable<PolicyHarmonyInfo> QueryPolicyHarmonyInfos(Pagination pagination, PolicyHarmonyQueryParameter condition);
    }
    interface IPolicyManageRepository
    {
        IEnumerable<BargainDefaultPolicyInfo> QueryBargainDefaultList(BargainDefaultPolicyQueryParameter parameter, Pagination pagination);

        IEnumerable<NormalDefaultPolicyInfo> QueryDefaultList(DefaultPolicyQueryParameter parameter, Pagination pagination);
        IEnumerable<PolicySettingInfo> QueryPolicySettingList(PolicySettingQueryParameter parameter, Pagination pagination);
        IEnumerable<SuspendOperation> QuerySuspendOption(SuspendOperationQueryParameter parameter, Pagination pagination);
    }
    interface ISuspendInfoRepository
    {
        SuspendInfo GetSuspendInfo(Guid companyId);
    }

    interface INormalPolicySettingRepository
    {
        void AddNormalPolicySetting(NormalPolicySetting view);
        void UpdateNormalPolicySetting(Guid Id, bool Enable);
        /// <summary>
        /// 查询政策设置信息
        /// </summary>
        /// <param name="policyId">政策编号</param>
        /// <param name="Type">null查询当前政策的下的贴扣点所有有效信息,是否贴扣点</param>
        /// <param name="Enable">是否启用</param>
        /// <returns></returns>
        IEnumerable<NormalPolicySetting> QueryNormalPolicySetting(Guid? policyId, bool? Type, bool? Enable, string FlightsFilter, string Berths, DateTime? flightDate);
    }
}
