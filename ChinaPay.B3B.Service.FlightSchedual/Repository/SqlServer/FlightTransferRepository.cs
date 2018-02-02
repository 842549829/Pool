using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository.SqlServer
{
    class FlightTransferRepository: SqlServerRepository, IFlightTransferRepository
    {
        public FlightTransferRepository(string connectionString) : base(connectionString){}

        public int Synchronous()
        {
            const string sql = @"EXECUTE P_ProductFlightTransferRecord";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
