using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository
{
    interface ICommandRepository
    {
        Command Query(string name);
        IEnumerable<Command> Query();
        int Update(Command command);
        int Detele(string name);
    }
}
