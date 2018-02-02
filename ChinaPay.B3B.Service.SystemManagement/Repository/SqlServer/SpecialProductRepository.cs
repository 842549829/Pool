using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.SystemManagement;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.SystemManagement.Repository.SqlServer
{
    class SpecialProductRepository: SqlServerRepository, ISpecialProductRepository
    {
        public SpecialProductRepository(string connectionString)
            : base(connectionString) {
        }

        public int Delete(SpecialProductView value)
        {
            throw new NotImplementedException();
        }

        public int Insert(SpecialProductView value)
        {
            throw new NotImplementedException();
        }

        IEnumerable<KeyValuePair<SpecialProductType, SpecialProductView>> Data.RepositoryCache<SpecialProductType, SpecialProductView>.IRepository.Query()
        {
            var result = new List<KeyValuePair<SpecialProductType, SpecialProductView>>();
            string sql = "SELECT Type,Name,Explain,Description,Enabled FROM dbo.T_SpecialProduct";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var item = new SpecialProductView((SpecialProductType)reader.GetByte(0))
                        {
                            Name = reader.GetString(1),
                            Explain = reader.GetString(2),
                            Description = reader.GetString(3),
                            Enabled = reader.GetBoolean(4)
                        };
                        result.Add(new KeyValuePair<SpecialProductType, SpecialProductView>(item.SpecialProductType, item));
                    }
                }

            }
            return result;
        }

        public int Update(SpecialProductView value)
        {
            string sql = "UPDATE dbo.T_SpecialProduct SET Name=@Name,Explain=@Explain,Description=@Description,Enabled=@Enabled WHERE Type=@Type";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Type", value.SpecialProductType);
                dbOperator.AddParameter("Name", value.Name);
                dbOperator.AddParameter("Explain", value.Explain);
                dbOperator.AddParameter("Description", value.Description);
                dbOperator.AddParameter("Enabled", value.Enabled);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
