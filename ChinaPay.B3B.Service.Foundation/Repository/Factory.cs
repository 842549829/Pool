using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Foundation.Repository {
    static class Factory {
        public static IAreaRepository CreateAreaRepository() {
            return new SqlServer.AreaRepository(ConnectionManager.B3BConnectionString);
        }
        public static IProvinceRepository CreateProvinceRepository() {
            return new SqlServer.ProvinceRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICityRepository CreateCityRepository() {
            return new SqlServer.CityRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICountyRepository CreateCountyRepository() {
            return new SqlServer.CountyRepository(ConnectionManager.B3BConnectionString);
        }
        public static IAirportRepository CreateAirportRepository() {
            return new SqlServer.AirportRepository(ConnectionManager.B3BConnectionString);
        }
        public static IAirlineRepository CreateAirlineRepository() {
            return new SqlServer.AirlineRepository(ConnectionManager.B3BConnectionString);
        }
        public static IAirCraftRepository CreateAirCraftRepository() {
            return new SqlServer.AirCraftRepository(ConnectionManager.B3BConnectionString);
        }
        public static IBAFRepository CreateBAFRepository() {
            return new SqlServer.BAFRepository(ConnectionManager.B3BConnectionString);
        }
        public static IBasicPriceRepository CreateBasicPriceRepository() {
            return new SqlServer.BasicPriceRepository(ConnectionManager.B3BConnectionString);
        }
        public static IBunkRepository CreateBunkRepository() {
            return new SqlServer.BunkRepository(ConnectionManager.B3BConnectionString);
        }
        public static IChildOrderableBunkRepository CreateChildOrderableBunkRepository() {
            return new SqlServer.ChildOrderableBunkRepository(ConnectionManager.B3BConnectionString);
        }
        public static IRefundAndReschedulingRepository CreateRefundAndReschedulingRepository() {
            return new SqlServer.RefundAndReschedulingRepository(ConnectionManager.B3BConnectionString);
        }
        public static IRefundAndReschedulingNewRepository CreateRefundAndReschedulingNewRepository() {
            return new SqlServer.RefundAndReschedulingNewRepository(ConnectionManager.B3BConnectionString);
        }
        public static IFixedNavigationRepository CreateFixedNavigationRepository() {
            return new SqlServer.FixedNavigationRepository(ConnectionManager.B3BConnectionString);
        }
        public static ICheck_InRepository CreateCheck_InRepository() {
            return new SqlServer.Check_InRepository(ConnectionManager.B3BConnectionString);
        }
    }
}