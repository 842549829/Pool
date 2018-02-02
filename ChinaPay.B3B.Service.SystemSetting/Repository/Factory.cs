using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.SystemSetting.Repository {
    static class Factory {
        public static DbOperator CreateCommand()
        {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }

        public static IVIPManagerRepository CreateVIPManagerRepository() {
            return new SqlServer.VIPManageRepository(ConnectionManager.B3BConnectionString);
        }
        public static IVIPHarmonyRepository CreateVIPHarmonyRepository() {
            return new SqlServer.VIPHarmonyRepository(ConnectionManager.B3BConnectionString);
        }
        public static IAreaRepository CreateAreaRepository() {
            return new SqlServer.AreaReposity(ConnectionManager.B3BConnectionString);
        }
        public static IPolicyHarmonyRepository CreatePolicyHarmonyRepository() {
            return new SqlServer.PolicyHarmonyRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICompanyGroupRepository CreateCompanyGroupRepository() {
            return new SqlServer.CompanyGroupReposity(ConnectionManager.B3BConnectionString);
        }
        public static IOnLineCustomerRepository CreateOnLineCustomerRepository() {
            return new SqlServer.OnLineCustomerReposity(ConnectionManager.B3BConnectionString);
        }

        public static ISuggestRepository CreateSuggestRepository(DbOperator dbOperator) {
            return new SqlServer.SuggestRepository(dbOperator);
        }
    }
}
