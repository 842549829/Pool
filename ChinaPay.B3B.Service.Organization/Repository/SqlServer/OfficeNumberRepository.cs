using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class OfficeNumberRepository:SqlServerRepository,IOfficeNumberRepository
    {
        public OfficeNumberRepository(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Data.DataMapping.OfficeNumber> Query(Guid companyId)
        {
            var result = new List<Data.DataMapping.OfficeNumber>();
            string sql = @"SELECT Id,Number,Enabled,Impower FROM dbo.T_OfficeNumber WHERE Company=@Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company",companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var officeNumber = new Data.DataMapping.OfficeNumber();
                        officeNumber.Id = reader.GetGuid(0);
                        officeNumber.Company = companyId;
                        officeNumber.Number = reader.GetString(1);
                        officeNumber.Enabled = reader.GetBoolean(2);
                        officeNumber.Impower = reader.GetBoolean(3);
                        result.Add(officeNumber);
                    }
                }
            }
            return result;
        }
    }
}
