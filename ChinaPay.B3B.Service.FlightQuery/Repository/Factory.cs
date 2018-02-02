namespace ChinaPay.B3B.Service.FlightQuery.Repository {
    class Factory {
        public static IRepository CreateRepository() {
            return new SqlServer.Repository(ChinaPay.Repository.ConnectionManager.B3BConnectionString);
        }
    }
}
