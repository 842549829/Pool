using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.XAPI.Service.Pid.Domain;
using ChinaPay.DataAccess;

namespace ChinaPay.XAPI.Service.Pid.Repository.SqlServer
{
    class CommandRepository : SqlServerRepository, ICommandRepository
    {
        public CommandRepository(string connectionString)
            : base(connectionString) { }

        public Command Query(string commandName)
        {
            Command command = null;
            string sql = @"select Name, Description, OperationId from Commands " +
                 @" where Name = @Name";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", commandName);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string description = reader.GetString(1);
                        int operationId = reader.GetInt16(2);
                        command = new Command(name, description, operationId);
                    }
                }
            }
            return command;
        }
        
        public IEnumerable<Command> Query()
        {
            List<Command> result = null;
            string sql = @"select Name, Description, OperationId from Commands";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    result = new List<Command>();
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string description = reader.GetString(1);
                        int operationId = reader.GetInt16(2);
                        Command command = new Command(name, description, operationId);
                        result.Add(command);
                    }
                }
            }
            return result;
        }
        
        public int Update(Command command)
        {
            string sql = @"UPDATE Commands SET  Description=@Description, OperationId=@OperationId" +
                @"WHERE Name = @Name";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Description", command.Description);
                dbOperator.AddParameter("OperationId", command.OperationId);
                dbOperator.AddParameter("Name", command.Name);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Detele(string name)
        {
            string sql = @"DETELE FROM Commands WHERE Name = @Name";

            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            { 
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
