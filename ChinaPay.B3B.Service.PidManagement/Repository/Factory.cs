using System.Configuration;
using ChinaPay.B3B.Service.PidManagement.Repository.SqlServer;

namespace ChinaPay.B3B.Service.PidManagement.Repository
{
    public class Factory
    {
        public static readonly string Url = ConfigurationManager.ConnectionStrings["B3B"].ConnectionString;

        public static IPidManagementRepository CreatePidManagementRepository()
        {
            return new PidManagementRepository(Url);
        }
    }
}
