using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class CommandService
    {
        public static Command Query(string commandName)
        {
            ICommandRepository repository = Factory.CreateCommandRepository();
            return repository.Query(commandName);
        }

        public static IEnumerable<Command> Query()
        {
            ICommandRepository repository = Factory.CreateCommandRepository();
            return repository.Query();
        }
    }
}
