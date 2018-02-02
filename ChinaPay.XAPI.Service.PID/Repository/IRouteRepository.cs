using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface IRouteRepository
    {
        IEnumerable<Route> Query();
        int Insert(Route route);
        int Update(Route route);
        int Detele(int[] id);
    }
}
