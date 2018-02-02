using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class IncomeGroupDeductSettingRepository : SqlServerRepository, IIncomeGroupDeductSettingRepository
    {
        public IncomeGroupDeductSettingRepository(string connectionString)
            : base(connectionString)
        {

        }
        public void Insert(Domain.IncomeGroupDeductGlobal setting)
        {
            string sql = "";
            if (setting.IncomeGroupId.HasValue)
            {
                sql += "DELETE FROM [dbo].[T_OEMIncomeGroupDeductGlobal] WHERE IncomeGroupId=@IncomeGroups;";
            }
            sql += "INSERT INTO [dbo].[T_OEMIncomeGroupDeductGlobal] (id,[IncomeGroupId],[Price],[Remark],[Type],CompanyId,IsGlobal)  VALUES (@id,@IncomeGroupId,@Price,@Remark,@Type,@CompanyId,@IsGlobal);";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("id", setting.Id);
                if (setting.IncomeGroupId.HasValue)
                {
                    dbOperator.AddParameter("IncomeGroups", setting.IncomeGroupId);
                    dbOperator.AddParameter("IncomeGroupId", setting.IncomeGroupId);
                }
                else
                {
                    dbOperator.AddParameter("IncomeGroupId", DBNull.Value);
                }

                dbOperator.AddParameter("Price", setting.Price);
                dbOperator.AddParameter("Remark", setting.Remark);
                dbOperator.AddParameter("CompanyId", setting.CompanyId);
                dbOperator.AddParameter("IsGlobal", setting.IsGlobal);
                dbOperator.AddParameter("Type", (byte)setting.Type);
                dbOperator.ExecuteNonQuery(sql);
                InsertPeriod(setting.Period);
            }
        }

        public Domain.IncomeGroupDeductGlobal Query(Guid incomeId)
        {
            string sql = @"SELECT [IncomeGroupId],[Price],[Remark],[Type],id,CompanyId,IsGlobal FROM T_OEMIncomeGroupDeductGlobal WHERE IncomeGroupId=@IncomeId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@IncomeId", incomeId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.IncomeGroupDeductGlobal setting = null;
                    if (reader.Read())
                    {
                        setting = new Domain.IncomeGroupDeductGlobal();
                        setting.IncomeGroupId = reader.IsDBNull(0) ? (Guid?)null : reader.GetGuid(0);
                        setting.Price = reader.GetInt32(1);
                        setting.Remark = reader.GetString(2);
                        setting.Type = (Common.Enums.PeriodType)reader.GetByte(3);
                        setting.Id = reader.GetGuid(4);
                        setting.CompanyId = reader.GetGuid(5);
                        setting.IsGlobal = reader.IsDBNull(6) ? (bool?)null : reader.GetBoolean(6);
                        LoadIncomeGroupPeriod(setting);
                    }
                    return setting;
                }
            }
        }

        private void LoadIncomeGroupPeriod(Domain.IncomeGroupDeductGlobal setting)
        {
            string sql = @"SELECT [DeductId],[StartPeriod],[EndPeriod],[Period] FROM T_OEMIncomeGroupPeriod WHERE DeductId=@IncomeId1";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                List<Domain.IncomeGroupPeriod> list = new List<Domain.IncomeGroupPeriod>();
                dbOperator.AddParameter("@IncomeId1", setting.Id);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.IncomeGroupPeriod period = null;
                    while (reader.Read())
                    {
                        period = new Domain.IncomeGroupPeriod();
                        period.DeductId = reader.GetGuid(0);
                        period.StartPeriod = reader.GetDecimal(1);
                        period.EndPeriod = reader.GetDecimal(2);
                        period.Period = reader.GetDecimal(3);
                        list.Add(period);
                    }
                    setting.Period = list;
                }
            }
        }

        private void InsertPeriod(IEnumerable<Domain.IncomeGroupPeriod> periodlist)
        {
            string sql = @"DELETE FROM [dbo].[T_OEMIncomeGroupPeriod] WHERE DeductId=@IncomeGroup;";
            int index = 0;
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IncomeGroup", periodlist.FirstOrDefault().DeductId);
                foreach (var item in periodlist)
                {
                    index++;
                    sql += @" INSERT INTO [dbo].[T_OEMIncomeGroupPeriod] ([DeductId],[StartPeriod],[EndPeriod],[Period])
            VALUES (@DeductId" + index + ",@StartPeriod" + index + ",@EndPeriod" + index + ",@Period" + index + ");";
                    dbOperator.AddParameter("DeductId" + index, item.DeductId);
                    dbOperator.AddParameter("StartPeriod" + index, item.StartPeriod);
                    dbOperator.AddParameter("EndPeriod" + index, item.EndPeriod);
                    dbOperator.AddParameter("Period" + index, item.Period);
                }
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        public Domain.IncomeGroupDeductGlobal QueryByCompanyId(Guid companyId)
        {
            string sql = @"SELECT [IncomeGroupId],[Price],[Remark],[Type],id,CompanyId,IsGlobal FROM T_OEMIncomeGroupDeductGlobal WHERE IncomeGroupId IS NULL AND CompanyId=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("@CompanyId", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.IncomeGroupDeductGlobal setting = null;
                    if (reader.Read())
                    {
                        setting = new Domain.IncomeGroupDeductGlobal();
                        setting.IncomeGroupId = (Guid?)null;
                        setting.Price = reader.GetInt32(1);
                        setting.Remark = reader.GetString(2);
                        setting.Type = (Common.Enums.PeriodType)reader.GetByte(3);
                        setting.Id = reader.GetGuid(4);
                        setting.CompanyId = reader.GetGuid(5);
                        setting.IsGlobal = reader.IsDBNull(6) ? (bool?)null : reader.GetBoolean(6);
                        LoadIncomeGroupPeriod(setting);
                    }
                    return setting;
                }
            }
        }
        public void UpdateIsGlobal(Guid companyId, bool isGlobal)
        {
            string sql = @" UPDATE [dbo].[T_OEMIncomeGroupDeductGlobal] SET IsGlobal = " + (isGlobal ? "1" : "0") + "  WHERE CompanyId = '" + companyId + "'";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        public void Update(Domain.IncomeGroupDeductGlobal setting)
        {
            string sql = "UPDATE [dbo].[T_OEMIncomeGroupDeductGlobal] SET [Price]=@Price,[Remark]=@Remark,[Type]=@Type,IsGlobal=@IsGlobal WHERE ID=@ID;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("ID", setting.Id);
                dbOperator.AddParameter("Price", setting.Price);
                dbOperator.AddParameter("Remark", setting.Remark);
                dbOperator.AddParameter("IsGlobal", setting.IsGlobal);
                dbOperator.AddParameter("Type", (byte)setting.Type);
                dbOperator.ExecuteNonQuery(sql);
                InsertPeriod(setting.Period);
            }
        }

        public Domain.IncomeGroupDeductGlobal GetIncomeGroupDeductGlobalByPurchaser(Guid ownerId, Guid purchaserId)
        {
            string sql = @" SELECT  groups.IncomeGroupId,groups.Price,groups.Remark,groups.Type,groups.id,groups.CompanyId,groups.IsGlobal 
                            FROM  [dbo].[T_IncomeGroup] groups
                            INNER JOIN T_IncomeGroupRelation relation ON groups.Id= relation.IncomeGroup
                            INNER JOIN T_OEMIncomeGroupDeductGlobal globals ON groups.Id = globals.IncomeGroupId
                            WHERE groups.Company ='" + ownerId + "' AND relation.Company = '" + purchaserId + "'";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            { 
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    Domain.IncomeGroupDeductGlobal setting = null;
                    if (reader.Read())
                    {
                        setting = new Domain.IncomeGroupDeductGlobal();
                        setting.IncomeGroupId = (Guid?)null;
                        setting.Price = reader.GetInt32(1);
                        setting.Remark = reader.GetString(2);
                        setting.Type = (Common.Enums.PeriodType)reader.GetByte(3);
                        setting.Id = reader.GetGuid(4);
                        setting.CompanyId = reader.GetGuid(5);
                        setting.IsGlobal = reader.IsDBNull(6) ? (bool?)null : reader.GetBoolean(6);
                        LoadIncomeGroupPeriod(setting);
                    }
                    return setting;
                }
            }
        }

    }
}
