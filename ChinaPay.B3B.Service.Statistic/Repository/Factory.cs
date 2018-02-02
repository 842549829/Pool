using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Statistic.Repository {
    class Factory {
        public static IStatisticRepository CreateStatisticRepository() {
            return new SqlServer.StatisticRepository(ConnectionManager.B3BConnectionString);
        }
    }
}