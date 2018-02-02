using ChinaPay.Repository;
using ChinaPay.B3B.Service.Policy.Repository.SqlServer;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Policy.Repository {
    static class Factory {
        public static IPolicyHoldOnRepository CreatePolicyHoldOnRepository() {
            return new Policy.Repository.SqlServer.PolicyHoldOnRepository(ConnectionManager.B3BConnectionString);
        }
        public static IPolicySetRepository CreatePolicySetRepository() {
            return new Policy.Repository.SqlServer.PolicySetRepository(ConnectionManager.B3BConnectionString);
        }
        //public static IPolicyRepository CreatePolicyRepository()
        //{
        //    return new Policy.Repository.SqlServer.PolicyRepository(ConnectionManager.B3BConnectionString);
        //}

        public static IPolicyRepository CreatePolicyRepository()
        {
            return new Policy.Repository.SqlServer.PolicyRepository(ConnectionManager.B3BConnectionString);
        }
        public static DbOperator CreateCommand()
        {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }

        public static IPolicyManageRepository CreatePolicysRepository(DbOperator command) {
            return new PolicyManageRepository(command);
        }
        public static IPolicyHarmoniesRepository CreatePolicyHarmoniesRepository() {
            return new Policy.Repository.SqlServer.PolicyHarmoniesRepository(ConnectionManager.B3BConnectionString);
        }

        public static ISuspendInfoRepository CreateSuspendInfoRepository(DbOperator dbOperator)
        {
            return new Policy.Repository.SqlServer.SuspendInfoRepository(dbOperator);
        }
        public static INormalPolicySettingRepository CreateNormalPolicySettingRepository(DbOperator dbOperator)
        {
            return new Policy.Repository.SqlServer.NormalPolicySettingRepository(dbOperator);
        } 
    }
}
