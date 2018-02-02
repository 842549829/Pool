using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.XAPI.Service.Pid.Domain;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class GroupResourceRepository : SqlServerRepository, IGroupResourceRepository
    {
        public GroupResourceRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<GroupResource> Query()
        {
            List<GroupResource> result = null;
            string sql = @"select GroupId, ResourceId from GroupResources";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<GroupResource>();
                    while (reader.Read())
                    {
                        short groupId = reader.GetInt16(0);
                        short resourceId = reader.GetInt16(1);
                        GroupResource item = new GroupResource(groupId, resourceId);
                        result.Add(item);
                    }
                }
            }
            return result;


        }

        public GroupResource Query(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(GroupResource resource)
        {
            throw new NotImplementedException();
        }

        public int Update(GroupResource resource)
        {
            throw new NotImplementedException();
        }

        public int Delete(int[] id)
        {
            throw new NotImplementedException();
        }
    }
}
