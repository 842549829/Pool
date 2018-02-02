using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class CompanyGroupsRepository :SqlServerRepository, ICompanyGroupsRepository
    {
        public CompanyGroupsRepository(string connectionString) : base(connectionString) { }
        public IEnumerable<CompanyGroupInfo> QueryCompanyGroups(CompanyGroupQueryParameter condition, Pagination pagination)
        {
            IList<CompanyGroupInfo> result = new List<CompanyGroupInfo>();
            using (var dbOperator = new DbOperator(Provider,ConnectionString))
            {
                dbOperator.AddParameter("@iOwner",condition.Owner);
                if (!string.IsNullOrEmpty(condition.Name)) dbOperator.AddParameter("@iName", condition.Name);
                if (!string.IsNullOrEmpty(condition.Creator)) dbOperator.AddParameter("@iCreator", condition.Creator);
                if (condition.CreateTimeStart.HasValue) dbOperator.AddParameter("@iCreateTimeStart", condition.CreateTimeStart.Value);
                if (condition.CreateTimeEnd.HasValue) dbOperator.AddParameter("@iCreateTimeEnd",condition.CreateTimeEnd.Value);
                dbOperator.AddParameter("@iPageSize",pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex",pagination.PageIndex);
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("[dbo].[P_QueryCompanyGroups]", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var info = new CompanyGroupInfo();
                        info.Id = reader.GetGuid(0);
                        info.Name = reader.GetString(1);
                        info.Description = reader.GetString(2);
                        info.Owner = reader.GetGuid(3);
                        info.Creator = reader.GetString(4);
                        info.CreateTime = reader.GetDateTime(5);
                        info.LastModifyTime = reader.GetDateTime(6);
                        info.MemberCount = reader.GetInt32(7);
                        result.Add(info);
                    }
                }
                if (pagination.GetRowCount) pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
    }
}
