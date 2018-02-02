using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.SystemManagement.Repository {
    static class Factory {
        public static ISystemParamRepository CreateSystemParamRepository() {
            return new SqlServer.SystemParamRepository(ConnectionManager.B3BConnectionString);
        }
        public static ISystemDictionaryRepository CreateSystemDictionaryRepository() {
            return new SqlServer.SystemDictionaryRepository(ConnectionManager.B3BConnectionString);
        }
        public static ISpecialProductRepository CreateSpecialProductRepository() {
            return new SqlServer.SpecialProductRepository(ConnectionManager.B3BConnectionString);
        }
    }
}
