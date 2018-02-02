using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.DataAccess;
using ChinaPay.Repository;

namespace ChinaPay.B3B.Service.Itinerary.Repository
{
  static class Factory
    {

        public static DbOperator CreateCommand()
        {
            return new DbOperator("System.Data.SqlClient", ConnectionManager.B3BConnectionString);
        }
        public static IItineraryReposity CreateItineraryReposity(DbOperator dbOperator)
        {
            return new ChinaPay.B3B.Service.Itinerary.Repository.SqlServer.ItineraryReposity(dbOperator);
        }
    }
}
