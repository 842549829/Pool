using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.XAPI.Service.Pid.Repository;

namespace ChinaPay.XAPI.Service.Pid
{
    public class GroupService
    {
        public static int Save(Group operation)
        {
            IGroupRepository repository = Factory.CreateGroupRepository();
            return repository.Insert(operation);
        }
        
        public static IEnumerable<Group> Query()
        {
            IGroupRepository repository = Factory.CreateGroupRepository();
            return repository.Query(); 
        }

        /// <summary>
        /// 查找同线程的30秒内的最近的一次记录；
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public static Group Query(int tid)
        {
            IGroupRepository repository = Factory.CreateGroupRepository();
            return repository.Query(tid);
        }
        
        public static Group Query(string pnrCode)
        {
            IGroupRepository repository = Factory.CreateGroupRepository();
            return repository.Query(pnrCode);
        }
        
        public static IEnumerable<GroupResource> GetGroupResources()
        {
            IGroupResourceRepository repository = Factory.CreateGroupResourceRepository();
            return repository.Query();
        }
    }
}
