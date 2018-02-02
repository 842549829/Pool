using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class Command
    {
        public Command(string name, string description, int operationId)
        {
            this.Name = name;
            this.Description = description;
            this.OperationId = operationId;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int OperationId { get; set; }
    }
}
