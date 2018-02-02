namespace ChinaPay.B3B.Service.Recommend.Repository {
    class Factory {
        public static IFlightLowerFareRepository CreateFlightLowerFareRepository() {
            return new SqlServer.FlightLowerFareRepository(ChinaPay.Repository.ConnectionManager.B3BConnectionString);
        }
    }
}