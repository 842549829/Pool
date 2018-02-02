using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class ConfigurationRepository :SqlServerRepository,IConfigurationRepository
    {
        public ConfigurationRepository(string connectionString) :
            base(connectionString)
        {
        }

        public IEnumerable<Data.DataMapping.Configuration> QueryConfigurations(Guid companyId)
        {
            var result = new List<Data.DataMapping.Configuration>();
            string sql = @"SELECT [Id],[OfficeNumber],[Type],[Login],[Password],[Host],[Port],[SI],[PrinterSN] FROM [dbo].[T_Configuration] WHERE [Company]=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId",companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                       var configuration = new Data.DataMapping.Configuration();
                        configuration.Company = companyId;
                        configuration.Id = reader.GetGuid(0);
                        configuration.OfficeNumber = reader.GetString(1);
                        configuration.Type = reader.GetInt32(2);
                        configuration.Login = reader.GetString(3);
                        configuration.Password = reader.GetString(4);
                        configuration.Host = reader.GetString(5);
                        configuration.Port = reader.GetInt32(6);
                        configuration.SI = reader.GetString(7);
                        if (!reader.IsDBNull(8))
                        {
                            configuration.PrinterSN = reader.GetInt32(8);
                        }
                        result.Add(configuration);
                    }
                }
                
            }
            return result;
        }

        public Data.DataMapping.Configuration QueryConfiguration(Guid id)
        {
            Data.DataMapping.Configuration configuration = null;
            string sql = @"SELECT [Company],[OfficeNumber],[Type],[Login],[Password],[Host],[Port],[SI],[PrinterSN] FROM [dbo].[T_Configuration] WHERE [Id]=@Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id",id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        configuration = new Data.DataMapping.Configuration();
                        configuration.Id = id;
                        configuration.Company = reader.GetGuid(0);
                        configuration.OfficeNumber = reader.GetString(1);
                        configuration.Type = reader.GetInt32(2);
                        configuration.Login = reader.GetString(3);
                        configuration.Password = reader.GetString(4);
                        configuration.Host = reader.GetString(5);
                        configuration.Port = reader.GetInt32(6);
                        configuration.SI = reader.GetString(7);
                        if (!reader.IsDBNull(8))
                        {
                            configuration.PrinterSN = reader.GetInt32(8);
                        }
                    }
                }
            }
            return configuration;
        }
    }
}
