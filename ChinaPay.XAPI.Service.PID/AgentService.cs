using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid
{
    public class AgentService
    {
        public static int Save(Agent agent)
        {
            IAgentRepository repository = Factory.CreateAgentRepository();
            return repository.Insert(agent);
        }

        public static IEnumerable<Agent>  Query()
        {
            IAgentRepository repository = Factory.CreateAgentRepository();
            return repository.Query();
        }
    }
}
