using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Permission.Repository {
    static class Factory {
        public static Permission.Repository.ISystemResourceRepository CreateSystemResourceRepository() {
            return new Permission.Repository.SqlServer.SystemResourceRepository(ConnectionManager.B3BConnectionString);
        }

        public static Permission.Repository.IPermissionRoleRepository CreatePermissionRoleRepository() {
            return new Permission.Repository.SqlServer.PermissionRoleRepository(ConnectionManager.B3BConnectionString);
        }

        public static Permission.Repository.IPermissionRepository CreatePermissionRepository() {
            return new Permission.Repository.SqlServer.PermissionRepository(ConnectionManager.B3BConnectionString);
        }
    }
}
