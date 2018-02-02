using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class RouteService
    {
        public static int Save(Route agent)
        {
            IRouteRepository repository = Factory.CreateRouteRepository();
            return repository.Insert(agent);
        }

        public static IEnumerable<Route> Query()
        {
            IRouteRepository repository = Factory.CreateRouteRepository();
            return repository.Query();
        }

    }
}
