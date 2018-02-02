using ChinaPay.B3B.Service.FlightSchedual.Domain;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository
{
    interface IFlightRepository
    {
        int Save(Flight flight);
        Flight Query(FlightNumber flightNumber);
    }
}
