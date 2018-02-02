using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface IRouteGroupsViewRepository
    {
        /// <summary>
        /// 根据代理人编号，操作编号，获取配置组视图
        /// </summary>
        /// <param name="agentId">代理人编号</param>
        /// <param name="operationId">操作编号</param>
        /// <returns></returns>
        RouteGroupsView Query(int agentId, int operationId);
    }
}