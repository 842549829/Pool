using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class OperationService
    {
        public static int Save(Operation operation)
        {
            IOperationRepository repository = Factory.CreateOperationRepository();
            return repository.Insert(operation);
        }

        public static IEnumerable<Operation> Query()
        {
            IOperationRepository repository = Factory.CreateOperationRepository();
            return repository.Query();
        }        
    }
}
