namespace ChinaPay.B3B.Data {
    using DataMapping;
    using Izual.Data.SqlClient;
    using Izual.Linq;

    public static class DataContext {
        //[ThreadStatic]
        //private static readonly SqlQueryProvider provider;
        //static DataContext() {
        //provider = new SqlQueryProvider(Environment.DatabaseConnectionString);

        //Regulations = provider.GetEntry<Regulation>();
        //Companies = provider.GetEntry<Company>();
        //CompanyGroups = provider.GetEntry<CompanyGroup>();
        //Employees = provider.GetEntry<Employee>();
        //Configurations = provider.GetEntry<Configuration>();
        //WorkingHours = provider.GetEntry<WorkingHours>();
        //WorkingSettings = provider.GetEntry<WorkingSetting>();
        //SettingPolicies = provider.GetEntry<SettingPolicy>();
        //BusinessManagers = provider.GetEntry<BusinessManager>();
        //VIPManagements = provider.GetEntry<VIPManagement>();
        //CompanyParameters = provider.GetEntry<CompanyParameter>();
        //CompanyGroupLimitations = provider.GetEntry<CompanyGroupLimitation>();
        //OemInfos = provider.GetEntry<OemInfo>();
        //Relationships = provider.GetEntry<Relationship>();
        //Accounts = provider.GetEntry<Account>();
        //Districts = provider.GetEntry<District>();
        //SpecialPolicyTypes = provider.GetEntry<SpecialPolicyType>();
        //CompanyGroupRelations = provider.GetEntry<CompanyGroupRelation>();
        //RegisterStatiscations = provider.GetEntry<RegisterStatiscation>();
        //AgentQualifications = provider.GetEntry<AgentQualification>();
        //Contacts = provider.GetEntry<Contact>();
        //Addresses = provider.GetEntry<Address>();
        //BargainPolicies = provider.GetEntry<BargainPolicy>();
        //RoundTripPolicies = provider.GetEntry<RoundTripPolicy>();
        //Providers = provider.GetEntry<Provider>();
        //SpecialPolicies = provider.GetEntry<SpecialPolicy>();
        //NormalPolicies = provider.GetEntry<NormalPolicy>();
        //OfficeNumbers = provider.GetEntry<OfficeNumber>();
        //SuspendOperations = provider.GetEntry<SuspendOperation>();
        //SuspendedPolicies = provider.GetEntry<SuspendedPolicy>();
        //FreezeOperations = provider.GetEntry<FreezeOperation>();
        //UserPermissions = provider.GetEntry<UserPermission>();
        //PermissionRoles = provider.GetEntry<PermissionRole>();
        //PolicyHarmonies = provider.GetEntry<PolicyHarmony>();
        //DefaultPolicies = provider.GetEntry<DefaultPolicy>();
        //PolicySettings = provider.GetEntry<PolicySetting>();
        //PolicySettingPeriods = provider.GetEntry<PolicySettingPeriod>();
        //}
        public static IEntry<T> GetEntry<T>() {
            return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<T>();
        }

        //public static readonly IEntry<Regulation> Regulations;
        //public static readonly IEntry<Company> Companies;
        //public static readonly IEntry<CompanyGroup> CompanyGroups;
        //public static readonly IEntry<Employee> Employees;
        //public static readonly IEntry<Configuration> Configurations;
        //public static readonly IEntry<WorkingHours> WorkingHours;
        //public static readonly IEntry<WorkingSetting> WorkingSettings;
        //public static readonly IEntry<SettingPolicy> SettingPolicies;
        //public static readonly IEntry<BusinessManager> BusinessManagers;
        //public static readonly IEntry<VIPManagement> VIPManagements;
        //public static readonly IEntry<CompanyParameter> CompanyParameters;
        //public static readonly IEntry<CompanyGroupLimitation> CompanyGroupLimitations;
        //public static readonly IEntry<OemInfo> OemInfos;
        //public static readonly IEntry<Relationship> Relationships;
        //public static readonly IEntry<Account> Accounts;
        //public static readonly IEntry<District> Districts;
        //public static readonly IEntry<SpecialPolicyType> SpecialPolicyTypes;
        //public static readonly IEntry<CompanyGroupRelation> CompanyGroupRelations;
        //public static readonly IEntry<RegisterStatiscation> RegisterStatiscations;
        //public static readonly IEntry<AgentQualification> AgentQualifications;
        //public static readonly IEntry<Contact> Contacts;
        //public static readonly IEntry<Address> Addresses;
        //public static readonly IEntry<BargainPolicy> BargainPolicies;
        //public static readonly IEntry<RoundTripPolicy> RoundTripPolicies;
        //public static readonly IEntry<Provider> Providers;
        //public static readonly IEntry<SpecialPolicy> SpecialPolicies;
        //public static readonly IEntry<NormalPolicy> NormalPolicies;
        //public static readonly IEntry<OfficeNumber> OfficeNumbers;
        //public static readonly IEntry<SuspendOperation> SuspendOperations;
        //public static readonly IEntry<SuspendedPolicy> SuspendedPolicies;
        //public static readonly IEntry<FreezeOperation> FreezeOperations;
        //public static readonly IEntry<UserPermission> UserPermissions;
        //public static readonly IEntry<PermissionRole> PermissionRoles;
        //public static readonly IEntry<PolicyHarmony> PolicyHarmonies;
        //public static readonly IEntry<DefaultPolicy> DefaultPolicies;
        //public static readonly IEntry<PolicySetting> PolicySettings;
        //public static readonly IEntry<PolicySettingPeriod> PolicySettingPeriods;

        //public static IEntry<Regulation> Regulations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Regulation>(); } }
        public static IEntry<Company> Companies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Company>(); } }
        public static IEntry<CompanyGroup> CompanyGroups { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<CompanyGroup>(); } }
        public static IEntry<Employee> Employees { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Employee>(); } }
        public static IEntry<Configuration> Configurations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Configuration>(); } }
        //public static IEntry<WorkingHours> WorkingHours { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<WorkingHours>(); } }
        public static IEntry<WorkingSetting> WorkingSettings { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<WorkingSetting>(); } }
        public static IEntry<SettingPolicy> SettingPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<SettingPolicy>(); } }
        public static IEntry<BusinessManager> BusinessManagers { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<BusinessManager>(); } }
        //public static IEntry<VIPManagement> VIPManagements { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<VIPManagement>(); } }
        public static IEntry<CompanyParameter> CompanyParameters { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<CompanyParameter>(); } }
        public static IEntry<CompanyGroupLimitation> CompanyGroupLimitations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<CompanyGroupLimitation>(); } }
        //public static IEntry<OemInfo> OemInfos { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<OemInfo>(); } }
        public static IEntry<Relationship> Relationships { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Relationship>(); } }
        //public static IEntry<Account> Accounts { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Account>(); } }
        //public static IEntry<District> Districts { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<District>(); } }
        //public static IEntry<SpecialPolicyType> SpecialPolicyTypes { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<SpecialPolicyType>(); } }
        public static IEntry<CompanyGroupRelation> CompanyGroupRelations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<CompanyGroupRelation>(); } }
        //public static IEntry<RegisterStatiscation> RegisterStatiscations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<RegisterStatiscation>(); } }
        //public static IEntry<AgentQualification> AgentQualifications { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<AgentQualification>(); } }
        public static IEntry<Contact> Contacts { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Contact>(); } }
        public static IEntry<Address> Addresses { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Address>(); } }
        public static IEntry<BargainPolicy> BargainPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<BargainPolicy>(); } }
        //public static IEntry<RoundTripPolicy> RoundTripPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<RoundTripPolicy>(); } }
        //public static IEntry<Provider> Providers { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Provider>(); } }
        public static IEntry<SpecialPolicy> SpecialPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<SpecialPolicy>(); } }
        public static IEntry<NormalPolicy> NormalPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<NormalPolicy>(); } }
        public static IEntry<TeamPolicy> TeamPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<TeamPolicy>(); } }
        public static IEntry<OfficeNumber> OfficeNumbers { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<OfficeNumber>(); } }
        public static IEntry<SuspendOperation> SuspendOperations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<SuspendOperation>(); } }
        public static IEntry<SuspendedPolicy> SuspendedPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<SuspendedPolicy>(); } }
        public static IEntry<FreezeOperation> FreezeOperations { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<FreezeOperation>(); } }
        //public static IEntry<UserPermission> UserPermissions { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<UserPermission>(); } }
        //public static IEntry<PermissionRole> PermissionRoles { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<PermissionRole>(); } }
        public static IEntry<PolicyHarmony> PolicyHarmonies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<PolicyHarmony>(); } }
        public static IEntry<DefaultPolicy> DefaultPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<DefaultPolicy>(); } }
        public static IEntry<PolicySetting> PolicySettings { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<PolicySetting>(); } }
        public static IEntry<PolicySettingPeriod> PolicySettingPeriods { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<PolicySettingPeriod>(); } }
        public static IEntry<EmpowermentCustom> EmpowermentCustoms { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<EmpowermentCustom>(); } }
        public static IEntry<CustomNumber> CustomNumbers { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<CustomNumber>(); } }
        public static IEntry<BargainDefaultPolicy> BargainDefaultPolicies { get { return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<BargainDefaultPolicy>(); } }
        //public static IEntry<Suggest> Suggests{get{return new SqlQueryProvider(Environment.DatabaseConnectionString).GetEntry<Suggest>();}} 
    }
}