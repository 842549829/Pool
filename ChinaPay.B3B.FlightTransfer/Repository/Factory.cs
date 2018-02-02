using ChinaPay.B3B.Service.FlightTransfer.Repository.SqlServer;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.FlightTransfer.Repository {
    static class Factory {
        public static DbOperator CreateCommand() {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }

        public static ITransferRepository CreateTransfeRepository(DbOperator command)
        {
            return new TransferRepository(command);
        }

        
    }
}
