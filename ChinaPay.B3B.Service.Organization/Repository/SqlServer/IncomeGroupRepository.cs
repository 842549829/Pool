using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class IncomeGroupRepository : SqlServerRepository, IIncomeGroupRepository
    {
        public IncomeGroupRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void Insert(Domain.IncomeGroup group)
        {
            string sql = @"INSERT INTO [dbo].[T_IncomeGroup] ([Id],[Company],[Name],[Description],[Creator],[CreateTime])
             VALUES (@Id,@Company ,@Name,@Description,@Creator,@CreateTime)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Id", group.Id);
                dbOperator.AddParameter("Company", group.Company);
                dbOperator.AddParameter("Name", group.Name);
                if (string.IsNullOrWhiteSpace(group.Description))
                {
                    dbOperator.AddParameter("Description", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Description", group.Description);
                }
                dbOperator.AddParameter("Creator", group.Creator);
                dbOperator.AddParameter("CreateTime", group.CreateTime);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<IncomeGroupListView> Query(Guid company, Pagination pagination)
        {
            var result = new List<IncomeGroupListView>();
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@i_Owner", company);
                if (pagination != null)
                {
                    dbOperator.AddParameter("@i_pageSize", pagination.PageSize);
                    dbOperator.AddParameter("@i_pageIndex", pagination.PageIndex);
                }
                System.Data.Common.DbParameter totalCount = dbOperator.AddParameter("@o_rowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader =  dbOperator.ExecuteReader("dbo.P_IncomeGroupListPagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var view =new IncomeGroupListView();
                        view.Id = reader.GetGuid(0);
                        view.Company = reader.GetGuid(1);
                        view.Name = reader.GetString(2);
                        view.Description =reader.IsDBNull(3)?string.Empty: reader.GetString(3);
                        view.CreateTime = reader.GetDateTime(4);
                        view.UserCount = reader.GetInt32(5);
                        result.Add(view);
                    }
                }
                if (pagination != null && pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }


        public void Delete(Guid groupId)
        {
            //string sql = "DELETE FROM [dbo].[T_IncomeGroup] WHERE Id=@GroupId;";
            string sql = @"	DELETE FROM T_IncomeGroup WHERE Id = @GroupId
	                        DELETE FROM T_IncomeGroupRelation WHERE IncomeGroup = @GroupId;
                            DELETE FROM PEROPD 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                                   WHERE LIMITGROUP.IncomeGroupId = @GroupId;  
                            DELETE FROM  LIMIT 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   WHERE LIMITGROUP.IncomeGroupId = @GroupId;
                            DELETE FROM T_IncomeGroupLimitGroup WHERE IncomeGroupId = @GroupId;
                            DELETE FROM  Rebate
                                   FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                   INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                   INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON  Limitation.Id = Rebate.LimitationId
                                   WHERE IncomeGroupId= @GroupId;
                             DELETE FROM  Limitation
                                    FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                    INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                    WHERE IncomeGroupId= @GroupId;
                             DELETE FROM dbo.T_PurchaseLimitationGroup WHERE IncomeGroupId=@GroupId;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("GroupId", groupId);
                try
                {
                    dbOperator.BeginTransaction();
                    dbOperator.ExecuteNonQuery(sql);
                    dbOperator.CommitTransaction();
                }
                catch (Exception)
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }


        public void Update(Domain.IncomeGroup group)
        {
            string sql = @"UPDATE [dbo].[T_IncomeGroup] SET [Name] = @Name,[Description] = @Description WHERE [Id] = @Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Name", group.Name);
                if (string.IsNullOrWhiteSpace(group.Description))
                {
                    dbOperator.AddParameter("Description", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Description", group.Description);
                }
                dbOperator.AddParameter("Id", group.Id);
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        public Domain.IncomeGroup Query(Guid groupId)
        {
            Domain.IncomeGroup group = null;
            string sql = @"SELECT [Company],[Name],[Description],[Creator],[CreateTime] FROM [dbo].[T_IncomeGroup] WHERE [Id]=@GroupId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("GroupId", groupId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        group = new Domain.IncomeGroup();
                        group.Company = reader.GetGuid(0);
                        group.Name = reader.GetString(1);
                        group.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        group.Creator = reader.GetString(3);
                        group.CreateTime = reader.GetDateTime(4);
                    }
                }
            }
            return group;
        }


        public void UpdateIncomeGroupRelation(Guid? orginalIncomeGroupId, Guid? newIncomeGroupId, Guid companyId)
        {
            string sql = "";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (orginalIncomeGroupId.HasValue)
                {
                    sql += @"DELETE FROM [dbo].[T_IncomeGroupRelation] WHERE IncomeGroup=@OrginalIncomeGroupId AND Company=@CompanyId;";
                    dbOperator.AddParameter("OrginalIncomeGroupId", orginalIncomeGroupId);
                }
                if (newIncomeGroupId.HasValue)
                {
                    sql += @"INSERT INTO [dbo].[T_IncomeGroupRelation] ([IncomeGroup],[Company]) VALUES(@NewIncomeGroupId,@CompanyId)";
                    dbOperator.AddParameter("NewIncomeGroupId", newIncomeGroupId);
                }
                dbOperator.AddParameter("CompanyId", companyId);
                if (!string.IsNullOrEmpty(sql)) dbOperator.ExecuteNonQuery(sql);
            }
        }


        public void Delete(IEnumerable<Guid> groupIds)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string deleteGroupIds = groupIds.Join(",", item => "'" + item.ToString() + "'");
                string sql = string.Format(@"DELETE FROM T_IncomeGroup WHERE Id IN ({0})
	                                         DELETE FROM T_IncomeGroupRelation WHERE IncomeGroup  IN ({0});
	                                         DELETE FROM PEROPD 
                                                    FROM T_IncomeGroupLimit LIMIT
                                                    INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                                    INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                                                    WHERE LIMITGROUP.IncomeGroupId IN ({0});  
                                             DELETE FROM  LIMIT 
                                                    FROM T_IncomeGroupLimit LIMIT
                                                    INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                                    WHERE LIMITGROUP.IncomeGroupId IN ({0});
                                             DELETE FROM T_IncomeGroupLimitGroup WHERE IncomeGroupId IN ({0});
                                             DELETE FROM  Rebate
                                                     FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                                     INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                                     INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON  Limitation.Id = Rebate.LimitationId
                                                     WHERE IncomeGroupId IN ({0});
 
                                             DELETE FROM  Limitation
                                                    FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                                    INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                                    WHERE IncomeGroupId IN ({0});
 
                                             DELETE FROM dbo.T_PurchaseLimitationGroup WHERE IncomeGroupId IN ({0});", deleteGroupIds);
                try
                {
                    dbOperator.BeginTransaction();
                    dbOperator.ExecuteNonQuery(sql);
                    dbOperator.CommitTransaction();
                }
                catch (Exception)
                {
                    dbOperator.RollbackTransaction();
                    throw;
                }
            }
        }


        public DataTransferObject.Organization.IncomeGroupView QueryIncomeGroup(Guid groupId)
        {
            DataTransferObject.Organization.IncomeGroupView groupView = null;
            string sql = @"SELECT 
	    Id,Name,[Description],COUNT(TRel.Company) AS UserCount
	    FROM dbo.T_IncomeGroup TIncome LEFT JOIN dbo.T_IncomeGroupRelation TRel
	    ON TIncome.Id = TRel.IncomeGroup
	WHERE TIncome.Id = @i_Owner
	group by Id,Name,[Description]";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("i_Owner", groupId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        groupView = new DataTransferObject.Organization.IncomeGroupView();
                        groupView.IncomeGroupId = reader.GetGuid(0);
                        groupView.Name = reader.GetString(1);
                        groupView.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        groupView.UserCount = reader.GetInt32(3);
                    }
                }
            }
            return groupView;
        }


        public IEnumerable<DataTransferObject.Organization.IncomeGroupView> QueryIncomeGroup(IEnumerable<Guid> groupId)
        {
            var result = new List<DataTransferObject.Organization.IncomeGroupView>();
            string sql = string.Format(@"SELECT 
	    Id,Name,[Description],COUNT(TRel.Company) AS UserCount
	    FROM dbo.T_IncomeGroup TIncome LEFT JOIN dbo.T_IncomeGroupRelation TRel
	    ON TIncome.Id = TRel.IncomeGroup
	WHERE TIncome.Id IN ({0})
	group by Id,Name,[Description]", groupId.Join(",", item => "'" + item.ToString() + "'"));
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        DataTransferObject.Organization.IncomeGroupView groupView = new DataTransferObject.Organization.IncomeGroupView();
                        groupView.IncomeGroupId = reader.GetGuid(0);
                        groupView.Name = reader.GetString(1);
                        groupView.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        groupView.UserCount = reader.GetInt32(3);
                        result.Add(groupView);
                    }
                }
            }
            return result;
        }


        public void UpdateIncomeGroupRelation(Guid newIncomeGroupId, IEnumerable<Guid> companyId)
        {
            string sql = "";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                try
                {
                    sql = string.Format(@"DELETE FROM [dbo].[T_IncomeGroupRelation] WHERE Company IN ({0});", companyId.Join(",", item => "'" + item.ToString() + "'"));
                    int i = 0;
                    foreach (var item in companyId)
                    {
                        sql += @"INSERT INTO [dbo].[T_IncomeGroupRelation] ([IncomeGroup],[Company]) VALUES(@NewIncomeGroupId_"+i+",@CompanyId_"+i+");";
                        dbOperator.AddParameter("CompanyId_"+i, item);
                        dbOperator.AddParameter("NewIncomeGroupId_"+i, newIncomeGroupId);
                        i++;
                    }
                    if (!string.IsNullOrEmpty(sql))
                        dbOperator.ExecuteNonQuery(sql);
                }
                catch(Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
