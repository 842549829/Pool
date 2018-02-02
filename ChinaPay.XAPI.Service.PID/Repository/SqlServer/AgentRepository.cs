using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.DataAccess;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class AgentRepository : SqlServerRepository, IAgentRepository
    {
        public AgentRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<Agent> Query()
        {
            List<Agent> result = null;
            string sql = @"select [Id], [Name], [Description], [Status] from Agents";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<Agent>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);
                        int status = reader.GetInt32(3);
                        Agent agent = new Agent(id, name, description, status);
                        result.Add(agent);
                    }
                }
            }
            return result;
        }

        public Agent Query(int agentId)
        {
            Agent agent = null;
            string sql = @"select Id, Name, Description, Status from Agents" +
                 @" where Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", agentId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);
                        int status = reader.GetInt32(3);
                        agent = new Agent(id, name, description, status);
                    }
                }
            }
            return agent;
        }

        public int Insert(Agent agent)
        {
            string sql = @"INSERT INTO Agents(Name, Description, Status)" +
                @"values(@Name, @Description, @Status);";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", agent.Name);
                dbOperator.AddParameter("Description", agent.Description);
                dbOperator.AddParameter("Status", agent.Status);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Agent agent)
        {
            string sql = @"UPDATE Agents SET Name=@Name, Description=@Description, Status=@Status" +
                @"WHERE id = @id";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", agent.Name);
                dbOperator.AddParameter("Description", agent.Description);
                dbOperator.AddParameter("Status", agent.Status);
                dbOperator.AddParameter("id", agent.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(int[] ids)
        {
            string str_ids = DomainService.GetIds(ids);
            string sql = @"DETELE FROM Agents WHERE id in (" + str_ids + " )";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
