using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Order.Repository {
    static class Factory {
        public static DbOperator CreateCommand() {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }

        public static IOrderRepository CreateOrderRepository(DbOperator command) {
            return new Order.Repository.SqlServer.OrderRepository(command);
        }

        public static IApplyformRepository CreateApplyformRepository(DbOperator command) {
            return new Order.Repository.SqlServer.ApplyformRepository(command);
        }

        public static Service.Distribution.Repository.IDistributionRepository CreateDistributionRepository(DbOperator command) {
            return new Service.Distribution.Repository.SqlServer.DistributionRepository(command);
        }

        public static ICoordinationRepository CreateCoordinationRepository() {
            return new Order.Repository.SqlServer.CoordinationRepository(ConnectionManager.B3BConnectionString);
        }

        public static IStatusRepository CreateStatusRepository() {
            return new Order.Repository.SqlServer.StatusRepository(ConnectionManager.B3BConnectionString);
        }

        public static IFreezeRepository CreateFreezeRepository() {
            return new SqlServer.FreezeRepository(ConnectionManager.B3BConnectionString);
        }
        
        public static IRoyaltyRepository CreateRoyaltyRepository() {
            return new SqlServer.RoyaltyRepository(ConnectionManager.B3BConnectionString);
        }
        public static IRefundRepository CreateRefundRepository() {
            return new SqlServer.RefundRepository(ConnectionManager.B3BConnectionString);
        }
        public static IAutoPayRepository CreateAutoPayRepository(DbOperator command) {
            return new SqlServer.AutoPayRepository(command);
        }

    }
}
