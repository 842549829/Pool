using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class CustomNumberRepository : SqlServerRepository, ICustomNumberRepository
    {
        public CustomNumberRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Data.DataMapping.CustomNumber> Query(Guid companyId)
        {
            var result = new List<Data.DataMapping.CustomNumber>();
            string sql = @"SELECT Id,Number,Describe,Enabled FROM dbo.T_CustomNumber WHERE Company=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company",companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var customNumber = new Data.DataMapping.CustomNumber();
                        customNumber.Id = reader.GetGuid(0);
                        customNumber.Number = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                            customNumber.Describe = reader.GetString(2);
                        customNumber.Enabled = reader.GetBoolean(3);
                        customNumber.Company = companyId;
                        result.Add(customNumber);
                    }
                }
            }

            return result;
        }


        public IEnumerable<Data.DataMapping.CustomNumber> QueryCustomNumberByEmployee(Guid employeeId)
        {
            var result = new List<Data.DataMapping.CustomNumber>();
            string sql = "SELECT Id,Company,Number,Describe,[Enabled] FROM T_CustomNumber WHERE Id IN (SELECT CustomNumber FROM T_EmpowermentCustom WHERE Employee=@Employee)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Employee", employeeId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var customNumber = new Data.DataMapping.CustomNumber();
                        customNumber.Id = reader.GetGuid(0);
                        customNumber.Company = reader.GetGuid(1);
                        customNumber.Number = reader.GetString(2);
                        if (!reader.IsDBNull(3)) customNumber.Describe = reader.GetString(3);
                        customNumber.Enabled = reader.GetBoolean(4);
                        result.Add(customNumber);
                    }
                }
            }
            return result;
        }


        public IEnumerable<Data.DataMapping.EmpowermentCustom> QueryEmpowermentCustoms(Guid customNumberId)
        {
            var result = new List<Data.DataMapping.EmpowermentCustom>();
            string sql = "SELECT Employee,CustomNumber FROM T_EmpowermentCustom WHERE CustomNumber=@CustomNumber";
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                dbOperator.AddParameter("CustomNumber", customNumberId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var info = new Data.DataMapping.EmpowermentCustom();
                        info.Employee = reader.GetGuid(0);
                        info.CustomNumber = reader.GetGuid(1);
                        result.Add(info);
                    }
                }
            }
            return result;
        }
    }
}
