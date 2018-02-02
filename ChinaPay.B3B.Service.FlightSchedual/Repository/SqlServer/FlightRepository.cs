using System;
using ChinaPay.B3B.Service.FlightSchedual.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository.SqlServer
{
    class FlightRepository : SqlServerRepository, IFlightRepository
    {
        public FlightRepository(string connectionString) : base(connectionString){}


        public int Save(Flight flight)
        {
            throw new NotImplementedException();
        }

        public Flight Query(FlightNumber flightNumber)
        {
            throw new NotImplementedException();
        }
    }
}
