using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.DataAccess;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class RouteGroupsViewRepository : SqlServerRepository, IRouteGroupsViewRepository
    {
        public RouteGroupsViewRepository(string connectionString)
            : base(connectionString) { }

        public RouteGroupsView Query(int agentId, int operationId)
        {
            RouteGroupsView routeGroupsView = null;

            string sql = @"select AgentId, OperationId, XapiName, XapiPassword, XapiAddress, OfficeNo, RuleType, IsPublic, resourceCount "
                + " from ViewRouteGroups " +
                 @" where AgentId = @AgentId and OperationId = @OperationId";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("AgentId", agentId);
                dbOperator.AddParameter("OperationId", operationId);

                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int aid = reader.GetInt32(0);
                        int oid = reader.GetInt32(1);
                        string xapiName = reader.GetString(2);
                        string xapiPassword = reader.GetString(3);
                        IPEndPoint xapiAddress = new IPEndPoint(IPAddress.Parse(reader.GetString(4)), 8080);
                        string officeNo = reader.GetString(5);
                        RuleType ruleType = (RuleType)reader.GetInt16(6);
                        bool isPublic = reader.GetBoolean(7);
                        int resourceCount = reader.GetInt32(8);
                        routeGroupsView = new RouteGroupsView(aid, oid, xapiName, xapiPassword, officeNo, xapiAddress, ruleType, isPublic, resourceCount);
                    }
                }
            }
            return routeGroupsView;
        }
    }
}
