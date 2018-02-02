using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class Route
    {
        public Route(int agentId, int operationId, int groupId)
        {
            this.AgentId = agentId;
            this.OperationId = operationId;
            this.GroupId = groupId;
            
        }
        public int AgentId { get; set; }        
        public int OperationId { get; set; }
        public int GroupId { get; set; }
    }
}
