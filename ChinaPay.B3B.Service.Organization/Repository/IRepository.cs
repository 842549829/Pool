using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core;
using System.Data;

namespace ChinaPay.B3B.Service.Organization.Repository {
    interface IAccountRepository {
        int Save(Guid companyId, Domain.Account item);
        int UpdateStatus(Guid companyId, AccountType accountType, bool enabled);
        Domain.Account Query(Guid companyId, AccountType accountType);
        IEnumerable<Domain.Account> Query(Guid companyId);
        IEnumerable<Domain.Account> GetAllValidAccount(AccountType accountType);
        IEnumerable<Domain.Account> Query(string accountNo);
        IEnumerable<AccountDetailInfo> QueryAccount(Pagination pagination, PaymentsAccountQueryCondition condition);
        void SetWithholdingInfo(WithholdingView withholdingView);
        WithholdingView GetWithholding(WithholdingAccountType accountType, Guid company);
    }
    interface ICustomerRepository {
        int Save(Guid companyId, Customer customer);
        int Save(Guid companyId, IEnumerable<Customer> customers);
        int Update(Guid companyId, Customer customer);
        int Delete(Guid companyId, Guid customer);
        Customer Query(Guid customerId);
        IEnumerable<Customer> Query(CustomerQueryCondition condition, Pagination pagination);
    }

    interface ICompanyDocmentRepository {
        int Save(CompanyDocument document);
        CompanyDocument Query(Guid companyId);
        int Delete(Guid companyId);
    }

    interface IApplyPosRepository{
        int Save(Guid companyId, bool isNeedApply);
    }

    interface IRegisterIpRepository {
        int Insert(RegisterIP registerIp);
        RegisterIP Query(string ip);
        int Update(RegisterIP registerIp);
    }
    interface IVerfiCodeRepository {
        int Insert(VerfiCode verfiCode);
    }
    interface ICompanyUpgradeRepository {
        int Save(CompanyUpgrade companyUpgrade);
        int Audit(Guid companyId,bool audited,DateTime auditTime);
        CompanyUpgrade Query(Guid companyId);
        IEnumerable<CompanyUpgrade> QueryNeedAuditCompanys();
        IEnumerable<CompanyAuditInfo> QueryNeedAuditCompanies(CompanyAuditQueryCondition condition, Pagination pagination);
    }
    interface IExternalInterfaceSettingRepository
    {
        int Save(ExternalInterfaceSetting setting);
        ExternalInterfaceSetting Query(Guid companyId);
        IEnumerable<ExternalInterfaceInfo> Query(ExternalInterfaceQueryCondition condition, Pagination pagination);
    }
    interface ICompanyRepository{
         IEnumerable<CompanyListInfo> QueryCompanies(CompanyQueryParameter condition,Pagination pagination);
         IEnumerable<SpreadingView> QuerySpreadCompanies(SpreadingQueryParameter condition,Pagination pagination);
         IEnumerable<SubordinateCompanyListInfo> QuerySuordinateCompanies(SubordinateQueryParameter condition,Pagination pagination);
         IEnumerable<CompanyGroupMemberListInfo> QueryCompanyGroupMember(CompanyGroupMemberParameter condition,Pagination pagination);
         IEnumerable<CompanyGroupMemberListInfo> QueryMemberCanAdd(CompanyGroupMemberParameter condition,Pagination pagination);
         RelationInfo QuerySpreader(Guid companyId);
         CompanySettingsInfo QueryCompanySettingsInfo(Guid companyId);
         IEnumerable<CompanyInitInfo> QueryCompanyInit(CompanyType companyType, bool searchDisabledCompany);
         CompanyParameter QueryCompanyParameter(Guid id);
         WorkingHours QueryWorkingHours(Guid id);
         WorkingSetting QueryWorkingSetting(Guid id);
         bool ExistsCompanyName(string companyName);
         bool ExistsAbbreviateName(string abberviateName);
         CompanyGroupDetailInfo GetGroupInfo(Guid id);
         IEnumerable<BusinessManager> QueryBusinessManagers(Guid companyId);
         CompanyGroupLimitationInfo GetGroupLimitation(Guid id);
         SettingPolicy QuerytPolicySetting(Guid companyId);
         IEnumerable<WorkingHours> QueryWorkingHours();
         IEnumerable<WorkingSetting> QueryChildTicketProviders(string ariline);
         IEnumerable<CompanyGroupLimitationInfo> GetGroupLimitations(Guid companyId);
         Dictionary<Guid, CompanyParameter> QueryCreditworthiness(IEnumerable<Guid> company);
         Data.DataMapping.Company GetCompanyInfo(Guid companyId);
         SuperiorInfo QuerySuperiorInfo(Guid companyId);
         RelationInfo QuerySupperior(Guid companyId);
        bool ExistsAbbreviateNameOrCompanyName(string abberviateName, string companyName);
        CompanyDetailInfo GetCompanyDetailInfo(Guid companyId);
        ChinaPay.B3B.Data.DataMapping.Relationship QueryRelationship(Guid companyId);
        Contact QueryContact(Guid id);
        bool SetCompanyOperatorAccount(Guid companyId, string employeeNo);
        IEnumerable<CompanyListInfo> QueryCompanys();
        CompanyDetailInfo GetCompanyDetail(string userNo);
        /// <summary>
        /// 根据编号查询公司收益设置信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        IncomeGroupLimitType QueryGlobalIncomeGroup(Guid companyId);
        /// <summary>
        /// 根据域名查找OEM
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        OEMInfo QueryOEM(string domain);
        /// <summary>
        ///  通过公司ID查找OEM信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        OEMInfo QueryOEM(Guid companyId);
        /// <summary>
        ///  通过OEMID查找OEM信息
        /// </summary>
        /// <param name="oemId"></param>
        /// <returns></returns>
        OEMInfo QueryOEMById(Guid oemId);

        IEnumerable<KeyValuePair<Guid, bool>> QueryOEMContractSettings(IEnumerable<Guid> oemIds);
        PurchaseLimitationType QueryGlobalPurchase(Guid companyId);
    }
    interface IEmployeeRepository {
        IEnumerable<EmployeeListView> QueryEmployeeInfo(EmployeeQueryParameter condition, Pagination pagination);
        bool ExistsUserName(string userName);
        EmployeeDetailInfo QueryEmployee(Guid id);
        EmployeeDetailInfo QueryEmployee(string userName);
        Employee QueryCompanyAdmin(Guid companyId);
        int SetAllIpLimitation(Guid companyId,string ipLimitation);
        int SetSingleIpLimitation(Guid employeeId,string ipLimitation);
    }
    interface ICompanyGroupsRepository {
        IEnumerable<CompanyGroupInfo> QueryCompanyGroups(CompanyGroupQueryParameter condition,Pagination pagination);
    }
    interface ICustomNumberRepository {
        IEnumerable<CustomNumber> Query(Guid companyId);
        IEnumerable<CustomNumber> QueryCustomNumberByEmployee(Guid employeeId);
        IEnumerable<EmpowermentCustom> QueryEmpowermentCustoms(Guid customNumberId);
    }
    interface IOfficeNumberRepository {
        IEnumerable<OfficeNumber> Query(Guid companyId);
    }
    interface IConfigurationRepository {
        IEnumerable<Configuration> QueryConfigurations(Guid companyId);
        Configuration QueryConfiguration(Guid id);
    }
    interface IDistributionOEMRepository{
        void Insert(Domain.DistributionOEM oem, string abbreviateName);
        void Update(Domain.DistributionOEM oem);
        void InsertSetting(OemSetting setting, Guid oemid);
        void UpdateSetting(OemSetting setting);
        void InsertOEMLinks(Guid settingId, IEnumerable<Links> links);
        OemSetting QuerySetting(Guid id);
        void UpdateOemInfo(Domain.DistributionOEM oem);
        DistributionOEM QueryDistributionOEM(Guid companyId);
        DistribtionOEMUserCompanyDetailInfo QueryDistributionOEMUserDetail(Guid companyId);
        DataTable QueryDistributionOEMList(DistributionOEMCondition condition, Pagination pagination);
        IEnumerable<DistributionOEMUserListView> QueryDistributionOEMUserList(DistributionOEMUserCondition condition, Pagination pagination);
        bool CheckExsistDomainName(string domainName);
        bool CheckInitiatorIsOem(Guid companyId);
        IEnumerable<Guid> QueryOrginationCompany();
        void SvaeOEMContract(OEMInfo oemInfo);
        void ChooiceStyle(Guid oemId,Guid styleId);
        IEnumerable<DistributionOEMView> QueryDistributionOEM();
    }
    interface IIncomeGroupRepository {
        void Insert(Domain.IncomeGroup group);
        IEnumerable<IncomeGroupListView> Query(Guid company,Pagination pagination);
        void Delete(Guid groupId);
        void Update(Domain.IncomeGroup group);
        Domain.IncomeGroup Query(Guid groupId);
        IncomeGroupView QueryIncomeGroup(Guid groupId);
        IEnumerable<IncomeGroupView> QueryIncomeGroup(IEnumerable<Guid> groupId);
        void UpdateIncomeGroupRelation(Guid? orginalIncomeGroupId, Guid? newIncomeGroupId, Guid companyId);
        void UpdateIncomeGroupRelation(Guid newIncomeGroupId, IEnumerable<Guid> companyId);
        void Delete(IEnumerable<Guid> groupIds);
    }
    interface IIncomeGroupDeductSettingRepository {
        void Insert(IncomeGroupDeductGlobal setting);
        void Update(IncomeGroupDeductGlobal setting);
        IncomeGroupDeductGlobal Query(Guid incomeId);
        IncomeGroupDeductGlobal QueryByCompanyId(Guid companyId); 
        void UpdateIsGlobal(Guid companyId, bool isGlobal) ;
        IncomeGroupDeductGlobal GetIncomeGroupDeductGlobalByPurchaser(Guid ownerId, Guid purchaserId);
    }
    interface IOEMStyleRepository{
        void Insert(OEMStyle style);
        void Delete(Guid styleId);
        void Update(OEMStyle style);
        void UpdateEnable(Guid styleId,bool enable);
        OEMStyle Query(Guid styleId);
        IEnumerable<OEMStyle> Query();
    }
    interface ICompanyDrawditionRepository {
        void Insert(CompanyDrawdition dition);
        void Update(CompanyDrawdition dition);
        void Delete(Guid id);
        CompanyDrawdition QueryById(Guid id);
        List<CompanyDrawdition> QueryByOwerId(Guid owerId);
    }
    interface IPurchaseLimitationRepository {
        void InsertPurchaseRestrictionSetting(PurchaseLimitationGroup setting);
        void UpdatePurchaseRestrictionSetting(PurchaseLimitationGroup setting);
        void UpdatePurchaseRestrictionSettingGlobal(PurchaseLimitationGroup setting);
        void UpdatePurchaseLimitationGroup(Guid companyId);
        PurchaseLimitationGroup QueryPurchaseRestrictionSettingView(Guid groupId);
        PurchaseLimitationGroup QueryPurchaseRestrictionSetting(Guid companyId);
        PurchaseLimitationGroup QueryPurchaseRestrictionSettingList(Guid superId, Guid purchseId);
    }
    interface IIncomeGroupLimitRespository {
        void InsertIncomeGroupLimit(IncomeGroupLimitGroup setting);
        void UpdateIncomeGroupLimit(IncomeGroupLimitGroup setting);
        void InsertIncomeGroupLimitGlobal(IncomeGroupLimitType type, IncomeGroupLimitGroup setting);
        IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByGroupId(Guid groupId);
        IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByCompanyId(Guid companyId);
        IncomeGroupLimitGroup IncomeGroupLimitGroup(Guid superId, Guid purchseId);
    }
}