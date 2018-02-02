using ChinaPay.Repository;
namespace ChinaPay.B3B.Service.Report.Repository {
    static class Factory {
        public static IReportRepository CreateReportRepository() {
            return new SqlServer.ReportRepository(ConnectionManager.B3BConnectionString);
        }
    }
}
