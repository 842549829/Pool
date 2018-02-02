using System.Configuration;

namespace ChinaPay.B3B.Service.FlightSchedual.Repository
{
    class Factory
    {
        public static readonly string Url = ConfigurationManager.ConnectionStrings["B3B"].ConnectionString;

        public static IFlightRepository CreateFlightRepository()
        {
            return new SqlServer.FlightRepository(Url);
        }

        public static ISchedualRepository CreateFlightSchedualRepository()
        {
            return new SqlServer.SchedualRepository(Url);
        }

        public static IFlightTransferRepository CreateFlightTransferRepository()
        {
            return new SqlServer.FlightTransferRepository(Url);
        }
    }
}
