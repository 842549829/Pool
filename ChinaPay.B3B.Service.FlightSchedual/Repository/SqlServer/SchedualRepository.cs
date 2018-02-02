using System;
using ChinaPay.B3B.Service.FlightSchedual.Domain;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository.SqlServer
{
    class SchedualRepository : SqlServerRepository, ISchedualRepository
    {
        public SchedualRepository(string connectionString) : base(connectionString){}
        
        public int Save(DelayedSchedual delayedSchedual)
        {
            const string sql = @"EXECUTE Flight.ProcAddDelayedSchedual @FlightNumber, @FlightDate, @DeparturAirport, @ArrivalAirport, @NewFlightDate, @NewDepartureTime, @NewArrivalTime, @NewAddDays";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("FlightNumber", delayedSchedual.Schedual.FlightNumber.ToString());
                dbOperator.AddParameter("FlightDate", delayedSchedual.FlightDate);
                dbOperator.AddParameter("DeparturAirport", delayedSchedual.Schedual.DepartureAirport);
                dbOperator.AddParameter("ArrivalAirport", delayedSchedual.Schedual.ArrivalAirport);
                dbOperator.AddParameter("NewFlightDate", delayedSchedual.NewFlightDate);
                dbOperator.AddParameter("NewDepartureTime", delayedSchedual.NewDepartureTime);
                dbOperator.AddParameter("NewArrivalTime", delayedSchedual.NewArrivalTime);
                dbOperator.AddParameter("NewAddDays", delayedSchedual.NewAddDays);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
        
        public int Save(TransferedSchedual transferedSchedual)
        {
            const string sql = @"EXECUTE Flight.ProcAddTransferedSchedual @FlightNumber, @FlightDate,  @DeparturAirport, @ArrivalAirport, @NewFlightNumber, @NewFlightDate";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("FlightNumber", transferedSchedual.Schedual.FlightNumber.ToString());
                dbOperator.AddParameter("FlightDate", transferedSchedual.FlightDate);
                dbOperator.AddParameter("DeparturAirport", transferedSchedual.Schedual.DepartureAirport);
                dbOperator.AddParameter("ArrivalAirport", transferedSchedual.Schedual.ArrivalAirport);
                dbOperator.AddParameter("NewFlightNumber", transferedSchedual.NewFlightNumber.ToString());
                dbOperator.AddParameter("NewFlightDate", transferedSchedual.NewFlightDate);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Save(CanceledSchedual canceledSchedual)
        {
            const string sql = @"EXECUTE Flight.ProcAddCanceledSchedual @FlightNumber, @FlightDate, @DeparturAirport, @ArrivalAirport;";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("FlightNumber", canceledSchedual.Schedual.FlightNumber.ToString());
                dbOperator.AddParameter("FlightDate", canceledSchedual.FlightDate);
                dbOperator.AddParameter("DeparturAirport", canceledSchedual.Schedual.DepartureAirport);
                dbOperator.AddParameter("ArrivalAirport", canceledSchedual.Schedual.ArrivalAirport);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Save(NormalSchedual normalSchedual)
        {
            const string sql = @"INSERT INTO Flight.Scheduals ( FlightNumber, FlightDate, Status ) "
                    + @"VALUES  ( @FlightNumber, @FlightDate, @Status);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("FlightNumber", normalSchedual.Schedual.FlightNumber);
                dbOperator.AddParameter("FlightDate", normalSchedual.FlightDate);
                dbOperator.AddParameter("Status", SchedualStatus.Normal);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Save(Schedual schedual)
        {
            const string sql = @"MERGE Flight.Schedual AS target " +
                               @"USING (VALUES(@FlightNumber, @DepartureAirport, @ArrivalAirport, @DepartureTime, @ArrivalTime, @AddDays)) " +
                               @"AS source(FlightNumber, DepartureAirport, ArrivalAirport, DepartureTime, ArrivalTime, AddDays) " +
                               @"ON (target.FlightNumber = source.FlightNumber) " +
                               @"WHEN NOT MATCHED THEN " +
                               @"INSERT (FlightNumber, DepartureAirport, ArrivalAirport, DepartureTime, ArrivalTime, AddDays) " +
                               @"VALUES (FlightNumber, DepartureAirport, ArrivalAirport, DepartureTime, ArrivalTime, AddDays);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("FlightNumber", schedual.FlightNumber);
                dbOperator.AddParameter("DepartureAirport", schedual.DepartureAirport);
                dbOperator.AddParameter("ArrivalAirport", schedual.ArrivalAirport);
                dbOperator.AddParameter("DepartureTime", schedual.DepartureTime);
                dbOperator.AddParameter("ArrivalTime", schedual.ArrivalTime);
                dbOperator.AddParameter("AddDays", schedual.AddDays);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Schedual Query(FlightNumber flightNumber, string departureAirport, string arrivalAirport, DateTime datetime)
        {
            throw new NotImplementedException();
        }
    }
}
