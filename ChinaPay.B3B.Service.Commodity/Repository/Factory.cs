using System;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Commodity.Repository
{
    static class Factory
    {
        public static ICommodityReposity CreateIntegralReposity(DbOperator command)
        {
            return new ChinaPay.B3B.Service.Commodity.Repository.SqlServer.CommodityReposity(command);
        }
        public static DbOperator CreateCommand()
        {
            return new DbOperator("System.Data.SqlClient", ChinaPay.Repository.ConnectionManager.B3BConnectionString);
        }
    }
}
