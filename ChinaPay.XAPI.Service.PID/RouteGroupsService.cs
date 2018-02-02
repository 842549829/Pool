using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class RouteGroupsService
    {
        public static RouteGroupsView Query(int agentid, int operationId)
        {
            var repository = Factory.CreateRouteGroupsViewRepository();
            return repository.Query(agentid, operationId);
        }
    }
}