namespace ChinaPay.B3B.Service.FlightQuery.Repository {
    interface IRepository {
        void SaveFlightRecord(Domain.FlightRecord record);
        Domain.FlightRecord Query(string departure, string arrival, System.DateTime flightDate, bool exactQuery);
    }
}