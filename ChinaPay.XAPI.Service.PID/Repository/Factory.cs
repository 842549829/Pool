using System.Configuration;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    class Factory
    {
        public static readonly string url = ConfigurationManager.ConnectionStrings["PidManage"].ConnectionString;
        
        public static IGroupRepository CreateGroupRepository()
        {
            return new Pid.Repository.SqlServer.GroupRepository(url);
        }
        public static IAgentRepository CreateAgentRepository()
        {
            return new Pid.Repository.SqlServer.AgentRepository(url);
        }
        public static IHistoryRepository CreateHistoryRepository()
        {
            return new Pid.Repository.SqlServer.HistoryRepository(url);
        }
        public static IOperationRepository CreateOperationRepository()
        {
            return new Pid.Repository.SqlServer.OperationRepository(url);
        }
        public static IResourceRepository CreateResourceRepository()
        {
            return new Pid.Repository.SqlServer.ResourceRepository(url);
        }
        public static IRouteRepository CreateRouteRepository()
        {
            return new Pid.Repository.SqlServer.RouteRepository(url);
        }
        public static ICommandRepository CreateCommandRepository()
        {
            return new Pid.Repository.SqlServer.CommandRepository(url);
        }
        public static IGroupResourceRepository CreateGroupResourceRepository()
        {
            return new Pid.Repository.SqlServer.GroupResourceRepository(url);
        }
    }
}