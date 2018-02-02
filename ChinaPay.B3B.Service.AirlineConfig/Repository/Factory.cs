using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.AirlineConfig.Repository {
    static class Factory {

        public static IAirlineConfigRepository CreateAirlineConfigRepository()
        {
            return new SqlServer.AirlineConfigRepository(ConnectionManager.B3BConnectionString);
        }
    }
}
