using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Organization.Repository {
    static class Factory {
        public static IAccountRepository CreateAccountRepository() {
            return new Organization.Repository.SqlServer.AccountRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICustomerRepository CreateCustomerRepository() {
            return new Organization.Repository.SqlServer.CustomerRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyDocmentRepository CreateCompanyDocumentRepository(){
            return new Organization.Repository.SqlServer.CompanyDocumentRepository(ConnectionManager.B3BConnectionString);
        }
        public static IApplyPosRepository CreateApplyPosRepository() {
            return new Organization.Repository.SqlServer.ApplyPosRepository(ConnectionManager.B3BConnectionString);
        }
        public static IRegisterIpRepository CreateRegisterIpRepository() {
            return new Organization.Repository.SqlServer.RegisterIpRepository(ConnectionManager.B3BConnectionString);
        }
        public static IVerfiCodeRepository CreateVerfiCodeRepository() {
            return new Organization.Repository.SqlServer.VerfiCodeRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyUpgradeRepository CreateCompanyUpgradeRepository()  {
            return new Organization.Repository.SqlServer.CompanyUpgradeRepository(ConnectionManager.B3BConnectionString);
        }
        public static IExternalInterfaceSettingRepository CreateExternalInterfaceRepository() {
            return new Organization.Repository.SqlServer.ExternalInterfaceSettingRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyRepository CreateCompanyRepository() {
            return new Organization.Repository.SqlServer.CompanyRepository(ConnectionManager.B3BConnectionString);
        }
        public static IEmployeeRepository CreateEmployeeRepository() {
            return new Organization.Repository.SqlServer.EmployeeRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyGroupsRepository CreateCompanyGroupsRepository() {
            return new Organization.Repository.SqlServer.CompanyGroupsRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICustomNumberRepository CreateCustomNumberRepository() {
            return new Organization.Repository.SqlServer.CustomNumberRepository(ConnectionManager.B3BConnectionString);
        }
        public static IOfficeNumberRepository CreateOfficeNumberRepository() {
            return new Organization.Repository.SqlServer.OfficeNumberRepository(ConnectionManager.B3BConnectionString);
        }
        public static IConfigurationRepository CreateConfigurationRepository() {
            return new Organization.Repository.SqlServer.ConfigurationRepository(ConnectionManager.B3BConnectionString);
        }
        public static IDistributionOEMRepository CreateDistributionOEMRepository(){
            return new Organization.Repository.SqlServer.DistributionOEMRepository(ConnectionManager.B3BConnectionString);
        }
        public static IIncomeGroupRepository CreateIncomeGroupRepository(){
            return new Organization.Repository.SqlServer.IncomeGroupRepository(ConnectionManager.B3BConnectionString);
        }
        public static IIncomeGroupDeductSettingRepository CreateIncomeGroupDeductGlobalRepository(){
            return new Organization.Repository.SqlServer.IncomeGroupDeductSettingRepository(ConnectionManager.B3BConnectionString);
        }
        public static IOEMStyleRepository CreateOEMStyleRepository(){
            return new Organization.Repository.SqlServer.OEMStyleRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyDrawditionRepository CreateCompanyDrawditionRepository(){
            return new Organization.Repository.SqlServer.CompanyDrawditionRepository(ConnectionManager.B3BConnectionString);
        }
        public static IPurchaseLimitationRepository CreatePurchaseRestrictionSettingRepository(){
            return new Organization.Repository.SqlServer.PurchaseRestrictionSettingRepository(ConnectionManager.B3BConnectionString);
        }
        public static IIncomeGroupLimitRespository CreateIncomeGroupLimitRespository(){
            return new Organization.Repository.SqlServer.IncomeGroupLimitRespository(ConnectionManager.B3BConnectionString);
        }
    }
}
