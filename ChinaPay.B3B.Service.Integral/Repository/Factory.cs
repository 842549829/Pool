using System;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Integral.Repository
{
    static class Factory
    { 
        public static IReposity CreateIntegralReposity(DbOperator dbOperator)
        {
            return new ChinaPay.B3B.Service.Integral.Repository.SqlServer.IntegralResposity(dbOperator );
        }
        public static DbOperator CreateCommand()
        {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }

    }
}
