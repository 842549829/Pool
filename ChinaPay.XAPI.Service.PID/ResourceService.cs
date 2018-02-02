using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class ResourceService
    {
        public static int Save(Resource resource)
        {
            var repository = Factory.CreateResourceRepository();
            return repository.Insert(resource);
        }
         
        public static Resource Query(int resourceId)
        {
            var repository = Factory.CreateResourceRepository();
            return repository.Query(resourceId);
        }
        
        public static IEnumerable<Resource> Query()
        {
            var repository = Factory.CreateResourceRepository();
            return repository.Query();
        }


    }
}