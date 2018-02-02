using System.Configuration;
using ChinaPay.B3B.Service.Queue.Repository.SqlServer;

namespace ChinaPay.B3B.Service.Queue.Repository
{
    class Factory
    {
        //public static readonly string Url = ConfigurationManager.ConnectionStrings["B3B"].ConnectionString;
        public static readonly string Url = "server=192.168.1.253;database=B3B;uid=sa;password=123456";

        public static IQueueRepository CreateQueueRepository()
        {
            return new QueueRepository(Url);
        }
    }
}
