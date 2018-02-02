using System.Collections.Generic;
using ChinaPay.DataAccess;
using ChinaPay.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class RouteRepository : SqlServerRepository, IRouteRepository
    {

        public RouteRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<Route> Query()
        {
            List<Route> result = null;
            string sql = @"SELECT AgentId, OperationId, GroupId FROM Routes";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<Route>();
                    while (reader.Read())
                    {
                        int agentId = reader.GetInt16(0);
                        int operationId = reader.GetInt16(1);
                        int groupId = reader.GetInt16(2);
                        Route route = new Route(agentId, operationId, groupId);
                        result.Add(route);
                    }
                }
            }
            return result;
        }

        public int Insert(Route route)
        {
            string sql = @"INSERT INTO Routes(  AgentId, OperationId, GroupId)" +
                @"values( @AgentId, @OperationId, @GroupId);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("AgentId", route.AgentId);
                dbOperator.AddParameter("OperationId", route.OperationId);
                dbOperator.AddParameter("GroupId", route.GroupId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }


        public int Update(Route route)
        {
            string sql = @"UPDATE Routes SET  OperationId=@OperationId, GroupId=@GroupId " +
                @"WHERE AgentId = @AgentId";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("OperationId", route.OperationId);
                dbOperator.AddParameter("GroupId", route.GroupId);
                dbOperator.AddParameter("AgentId", route.AgentId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Detele(int[] id)
        {
            string str_ids = DomainService.GetIds(id);
            string sql = @"DETELE FROM Routes WHERE id in (" + str_ids + " )";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
