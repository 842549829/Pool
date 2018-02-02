using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.B3B.DataTransferObject.SystemSetting.PolicyHarmony;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIP;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIPHarmony;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.SystemSetting.Repository {
    interface IVIPManagerRepository {
        IEnumerable<VIPManageView> Query();
        IEnumerable<VIPManageView> Query(VIPManageQueryCondition condition);
        VIPManagement Query(Guid id);
        int Update(Guid id, bool enabled);
        int Insert(VIPManagement managerment);
        int Delete(Guid id);
        int Delete(IEnumerable<Guid> ids);
        int Update(IEnumerable<Guid> ids, bool enabled);
    }
    interface IVIPHarmonyRepository {
        IEnumerable<VIPHarmonyListView> Query();
        VIPHarmonyView Query(Guid id);
        int Insert(VIPHarmony harmony);
        int Update(VIPHarmony harmony);
        int Delete(Guid id);
        int Delete(IEnumerable<Guid> ids);
    }
    interface IAreaRepository {
        IEnumerable<AreaListView> Query(AreaQueryConditon condition);
        IEnumerable<AreaListView> Query(AreaQueryConditon condition, Pagination pagination);
        //IEnumerable<AreaRelationListView> Query(AreaRelationQueryCondtion condition);
        IEnumerable<AreaRelationListView> Query(AreaRelationQueryCondtion condition,Pagination pagination);
        AreaView Query(Guid id);
        AreaRelationView QueryRelation(string provinceCode);
        Guid QueryAreaCode(string name);
        int InsertArea(SellArea area);
        int DeleteArea(Guid id);
        int DeleteArea(IEnumerable<Guid> ids);
        int UpdateArea(SellArea area);
        int InsertAreaRelation(Guid area, string provinceCode);
        int UpdateAreaRelation(Guid area, string provinceCode);
        int DeleteAreaRelation(string provinceCode);
        int DeleteAreaRelation(IEnumerable<string> provinceCode);
    }
    interface IPolicyHarmonyRepository
    {
        IEnumerable<PolicyHarmonyView> Query(PolicyHarmonyQueryCondition condition);
        PolicyHarmonyView Query(Guid id);
        int Insert(PolicyHarmony harmony);
        int Update(PolicyHarmony harmony);
        int Delete(Guid id);
        int Delete(IEnumerable<Guid> ids);
    }
    interface ICompanyGroupRepository
    {
        IEnumerable<CompanyGroupListView> QueryCompanyGroups(Guid company, CompanyGroupQueryCondition condition);
        CompanyGroupView QueryCompanyGroup(Guid companyGroup);
        IEnumerable<MemberListView> QueryMembers(Guid companyGroup, MemberQueryCondition condition);
        IEnumerable<MemberListView> QueryCandidateMembers(Guid company, MemberQueryCondition condition);
        int RegisterMembers(Guid companyGroup, IEnumerable<Guid> members);
        int RegisterCompanyGroup(CompanyGroup companyGroup);
        int UpdateCompanyGroup(CompanyGroup companyGroup);
        int DeleteCompanyGroup(Guid company, Guid companyGroup);
        int DeleteCompanyGroups(Guid company, IEnumerable<Guid> companyGroups);
        int DeleteMembers(Guid companyGroup, IEnumerable<Guid> members);
    }
    interface IOnLineCustomerRepository
    {
        OnLineCustomer Query(Guid company,PublishRoles role);
        OnLineCustomerView Query(Guid company);
        Guid QueryPlatFormCompany();
        int SaveOnLine(Guid company,PublishRoles role,OnLineCustomerView view);
        DivideGroupView QueryDivideGroup(Guid divideGroup);
        IEnumerable<DivideGroupView> QueryDivideGroups(Guid company);
        int InsertDivideGroup(Guid company,DivideGroupView divideGroup);
        int UpdateDivideGroup(DivideGroupView divideGroup);
        int DeleteDivideGroup(Guid divideGroup);
        IEnumerable<MemberManage> QueryMembers(Guid divideGroup);
        MemberManage QueryMember(Guid divideGroup);
        int InsertMember(Guid divideGroup,MemberManage member);
        int UpdateMember(MemberManage member);
        int DeleteMember(Guid member);
    }

    interface ISuggestRepository
    {
        /// <summary>
        /// 添加用户建议
        /// </summary>
        /// <param name="suggest"></param>
        /// <returns></returns>
        bool Insert(Suggest suggest);
        IEnumerable<Suggest> Query(DateTime? start, DateTime? end, SuggestCategory? category, Pagination pagination);
    }
}
