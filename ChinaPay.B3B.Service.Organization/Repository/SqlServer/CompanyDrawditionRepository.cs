using System;
using System.Collections.Generic;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class CompanyDrawditionRepository : SqlServerRepository, ICompanyDrawditionRepository
    {
        public CompanyDrawditionRepository(string connectionString)
            : base(connectionString)
        {
        }
        public void Insert(Domain.CompanyDrawdition dition)
        {
            string sql = "INSERT INTO T_CompanyDrawCondition VALUES(@Id,@Type,@OwnerId,@Title,@Context);";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", dition.Id);
                dbOperator.AddParameter("Type", dition.Type);
                dbOperator.AddParameter("OwnerId", dition.OwnerId);
                dbOperator.AddParameter("Title", dition.Title);
                dbOperator.AddParameter("Context", dition.Context);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.CompanyDrawdition dition)
        {
            string sql = "UPDATE T_CompanyDrawCondition SET Title = @Title,Context = @Context,Type=@Type WHERE ID=@ID;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ID", dition.Id);
                dbOperator.AddParameter("Title", dition.Title);
                dbOperator.AddParameter("Type", dition.Type);
                dbOperator.AddParameter("Context", dition.Context);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public Domain.CompanyDrawdition QueryById(Guid id)
        {
            string sql = "SELECT Id,OwnerId,Title,Context,Type FROM T_CompanyDrawCondition WHERE ID=@ID;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ID", id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.CompanyDrawdition dition = null;
                    if (reader.Read())
                    {
                        dition = new Domain.CompanyDrawdition();
                        dition.Id = reader.GetGuid(0);
                        dition.OwnerId = reader.GetGuid(1);
                        dition.Title = reader.GetString(2);
                        dition.Context = reader.GetString(3);
                        dition.Type = reader.GetByte(4);
                    }
                    return dition;
                }
            }
        }

        public List<Domain.CompanyDrawdition> QueryByOwerId(Guid owerId)
        {
            string sql = "SELECT Id,OwnerId,Title,Context,Type FROM T_CompanyDrawCondition WHERE OwnerId=@OwnerId ";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("OwnerId", owerId); 
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    List<Domain.CompanyDrawdition> list = new List<Domain.CompanyDrawdition>();

                    while (reader.Read())
                    {
                        Domain.CompanyDrawdition dition = new Domain.CompanyDrawdition();
                        dition.Id = reader.GetGuid(0);
                        dition.OwnerId = reader.GetGuid(1);
                        dition.Title = reader.GetString(2);
                        dition.Context = reader.GetString(3);
                        dition.Type = reader.GetByte(4);
                        list.Add(dition);
                    }
                    return list;
                }
            }
        }
        public void Delete(Guid id)
        {
            string sql = "DELETE FROM T_CompanyDrawCondition  WHERE ID=@ID;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ID", id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }
    }
}
