using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Itinerary.Repository.SqlServer
{
    public class ItineraryReposity : SqlServerTransaction,IItineraryReposity
    {
        public ItineraryReposity(DbOperator dbOperator)
            : base(dbOperator)
        {
        }
    }
}
