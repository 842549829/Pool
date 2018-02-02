using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class OperationRepository : SqlServerRepository,  IOperationRepository
    {
        public OperationRepository(string connectionString)
            : base(connectionString) { }

        public IEnumerable<Operation> Query()
        {
            List<Operation> result = null;
            string sql = @"select Id, Name, ConfigurationType, RuleType, Description from Operations";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<Operation>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt16(0);
                        string name = reader.GetString(1);
                        string configurationType = reader.GetString(2);
                        short a = reader.GetByte(3);
                        RuleType ruleType = (RuleType)a;
                        string description = reader.GetString(4);                        
                        Operation operation = new Operation(id, name, configurationType, ruleType, description);
                        result.Add(operation);
                    }
                }
            }
            return result;
        }

        public Operation Query(int operarionId)
        {
            Operation operation = null;
            string sql = @"select Id, Name, ConfigurationType, RuleType, Description from Operations " +
                @" where Id = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", operarionId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string configurationType = reader.GetString(2);
                        RuleType ruleType = (RuleType)reader.GetInt32(3);
                        string description = reader.GetString(4);
                        operation = new Operation(id, name, configurationType, ruleType, description);
                    }
                }
            }
            return operation;
        }

        public int Insert(Operation operation)
        {
            string sql = @"insert into Operations(Name, ConfigurationType, RuleType, Description)" +
                   @"values(@Name, @ConfigurationType, @RuleType, @Description)";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", operation.Name);
                dbOperator.AddParameter("ConfigurationType", operation.ConfigurationType);
                dbOperator.AddParameter("RuleType", operation.RuleType);
                dbOperator.AddParameter("Description", operation.Description);                
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Operation operation)
        {
            string sql = @"update Operations set  Name=@Name, ConfigurationType=@ConfigurationType, RuleType=@RuleType, Description=@Description WHERE id=@id";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", operation.Name);
                dbOperator.AddParameter("ConfigurationType", operation.ConfigurationType);
                dbOperator.AddParameter("RuleType", operation.RuleType);
                dbOperator.AddParameter("Description", operation.Description);
                dbOperator.AddParameter("id", operation.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(int[] id)
        {
            string str_ids = DomainService.GetIds(id);
            string sql = @"DETELE FROM Operations WHERE id in (" + str_ids + " )";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            } 
        }
    }
}