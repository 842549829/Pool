using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using System.Data.Common;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class PurchaseRestrictionSettingRepository : SqlServerRepository, IPurchaseLimitationRepository
    {

        public PurchaseRestrictionSettingRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void InsertPurchaseRestrictionSetting(Domain.PurchaseLimitationGroup setting)
        {
            using(var dbOperator = new DbOperator(Provider,ConnectionString)){
                string sql = InsertPurchaseLimitationGroup(setting, dbOperator);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdatePurchaseRestrictionSetting(Domain.PurchaseLimitationGroup setting)
        {
            string sql = @"DELETE FROM  Rebate
                                   FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                   INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                   INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON  Limitation.Id = Rebate.LimitationId
                                   WHERE IncomeGroupId= @CurrentIncomeGroupId;
                             DELETE FROM  Limitation
                                    FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                    INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                    WHERE IncomeGroupId= @CurrentIncomeGroupId;
                             DELETE FROM dbo.T_PurchaseLimitationGroup WHERE IncomeGroupId=@CurrentIncomeGroupId;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CurrentIncomeGroupId", setting.IncomeGroupId);
                sql += InsertPurchaseLimitationGroup(setting, dbOperator);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public PurchaseLimitationGroup QueryPurchaseRestrictionSettingView(Guid groupId)
        {
            PurchaseLimitationGroup view = null;
            string sql = @"SELECT LimitationGroup.Id,IsGlobal,Limitation.Id,Airlines,Departures,LimitationGroupId,LimitationId,ProductType,AllowOnlySelf,Rebate
                           FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                           INNER JOIN dbo.T_PurchaseLimitation Limitation ON LimitationGroup.Id = Limitation.LimitationGroupId
                           INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON Limitation.Id = Rebate.LimitationId
                           WHERE IncomeGroupId=@IncomeGroupId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("IncomeGroupId", groupId);
                view = restructCotor(dbOperator, sql);
            }
            return view;
        }

        public PurchaseLimitationGroup QueryPurchaseRestrictionSetting(Guid companyId)
        {
            PurchaseLimitationGroup view = null;
            string sql = @"SELECT LimitationGroup.Id,IsGlobal,Limitation.Id,Airlines,Departures,LimitationGroupId,LimitationId,ProductType,AllowOnlySelf,Rebate
                           FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                           INNER JOIN dbo.T_PurchaseLimitation Limitation ON LimitationGroup.Id = Limitation.LimitationGroupId
                           INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON Limitation.Id = Rebate.LimitationId
                           WHERE CompanyId=@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", companyId);
                view = restructCotor(dbOperator,sql);
            }
            return view;
        }

        public PurchaseLimitationGroup QueryPurchaseRestrictionSettingList(Guid superId, Guid purchseId)
        {
            PurchaseLimitationGroup view = null;
            string sql = @"DECLARE @IsGlobalPurchase int;
                           SELECT @IsGlobalPurchase = PurchaseLimitationType FROM dbo.T_Company
                           WHERE Id=@CompanyId;
                           IF @IsGlobalPurchase = 2 
                             BEGIN
                              SELECT LimitationGroup.Id,IsGlobal,Limitation.Id,Airlines,Departures,LimitationGroupId,LimitationId,ProductType,AllowOnlySelf,Rebate
                           FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                           INNER JOIN dbo.T_PurchaseLimitation Limitation ON LimitationGroup.Id = Limitation.LimitationGroupId
                           INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON Limitation.Id = Rebate.LimitationId
                           WHERE CompanyId=@CompanyId;
                             END
                           ELSE IF @IsGlobalPurchase = 1
                           SELECT LimitationGroup.Id,IsGlobal,Limitation.Id,Airlines,Departures,LimitationGroupId,LimitationId,ProductType,AllowOnlySelf,Rebate
                           FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                           INNER JOIN dbo.T_PurchaseLimitation Limitation ON LimitationGroup.Id = Limitation.LimitationGroupId
                           INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON Limitation.Id = Rebate.LimitationId
                           INNER JOIN dbo.T_IncomeGroup IncomeGroup ON LimitationGroup.IncomeGroupId = IncomeGroup.Id
                           INNER JOIN dbo.T_IncomeGroupRelation Relation ON IncomeGroup.Id = Relation.IncomeGroup 
                           WHERE Relation.Company =@PurchaseId AND IncomeGroup.Company =@CompanyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CompanyId", superId);
                dbOperator.AddParameter("PurchaseId", purchseId);
                view = restructCotor(dbOperator, sql);
            }
            return view;
        }

        public void UpdatePurchaseRestrictionSettingGlobal(PurchaseLimitationGroup setting)
        {
            string sql = @"DELETE FROM  Rebate
                                   FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                   INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                   INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON  Limitation.Id = Rebate.LimitationId
                                   WHERE CompanyId= @CurrentCompanyId;
                             DELETE FROM  Limitation
                                    FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                    INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                    WHERE CompanyId= @CurrentCompanyId;
                             DELETE FROM dbo.T_PurchaseLimitationGroup WHERE CompanyId= @CurrentCompanyId;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CurrentCompanyId", setting.CompanyId);
                sql += InsertPurchaseLimitationGroup(setting, dbOperator);
                dbOperator.ExecuteNonQuery(sql);
            }
        }


        string InsertPurchaseLimitationGroup(PurchaseLimitationGroup setting, DbOperator dbOperator)
        {
            string sql = @"INSERT INTO [dbo].[T_PurchaseLimitationGroup]([Id],[IsGlobal],[CompanyId],[IncomeGroupId]) VALUES(@GroupId,@IsGlobal,@CompanyId,@IncomeGroupId);";
                dbOperator.AddParameter("GroupId", setting.Id);
                dbOperator.AddParameter("IsGlobal", setting.IsGlobal);
                if (setting.CompanyId.HasValue)
                {
                    dbOperator.AddParameter("CompanyId", setting.CompanyId);
                }
                else
                {
                    dbOperator.AddParameter("CompanyId", DBNull.Value);
                }
                if (setting.IncomeGroupId.HasValue)
                {
                    dbOperator.AddParameter("IncomeGroupId", setting.IncomeGroupId);
                }
                else
                {
                    dbOperator.AddParameter("IncomeGroupId", DBNull.Value);
                }
                sql += InsertPurchaseLimitation(setting.Limitation, dbOperator);
                return sql;
        }
        string InsertPurchaseLimitation(IList<PurchaseLimitation> limitation, DbOperator dbOperator)
        {
            string sql = "";
            int i = 0;
            int n = 0;
            foreach (var item in limitation)
            {
                sql += @"INSERT INTO [dbo].[T_PurchaseLimitation]([Id],[Airlines],[Departures],[LimitationGroupId]) VALUES(@Limitation_" + i + ",@Airlines_" + i + ",@Departures_" + i + ",@LimitationGroupId_" + i + ");";
                dbOperator.AddParameter("@Limitation_" + i, item.Id);
                dbOperator.AddParameter("@Airlines_" + i, item.Airlines);
                dbOperator.AddParameter("@Departures_" + i, item.Departures);
                dbOperator.AddParameter("@LimitationGroupId_" + i, item.LimitationGroupId);
                
                foreach (var it in item.Rebate)
                {
                    sql += @"INSERT INTO [dbo].[T_PurchaseLimitationRebate]([LimitationId],[ProductType],[AllowOnlySelf],[Rebate]) VALUES(@LimitationId_" + n + ",@ProductType_" + n + ",@AllowOnlySelf_" + n + ",@Rebate_" + n + ");";
                    dbOperator.AddParameter("@LimitationId_" + n, it.LimitationId);
                    dbOperator.AddParameter("@ProductType_" + n, (short)it.Type);
                    dbOperator.AddParameter("@AllowOnlySelf_" + n, it.AllowOnlySelf);
                    if (it.Rebate.HasValue)
                    {
                        dbOperator.AddParameter("@Rebate_" + n, it.Rebate);
                    }
                    else
                    {
                        dbOperator.AddParameter("@Rebate_" + n, DBNull.Value);
                    }
                    n++;
                }
                i++;
            }
            return sql;
        }
        PurchaseLimitationGroup restructCotor(DbOperator dbOperator,string sql)
        {
            PurchaseLimitationGroup view = null;
            List<Guid> limitationId = null;
            using (var reader = dbOperator.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    Guid currentLimitationGroupId = reader.GetGuid(0);
                    if (view == null || view.Id != currentLimitationGroupId)
                    {
                        view = new PurchaseLimitationGroup(reader.GetGuid(0));
                        if (!reader.IsDBNull(0))
                            view.Id = reader.GetGuid(0);
                        view.IsGlobal = reader.GetBoolean(1);
                        view.Limitation = new List<PurchaseLimitation>();
                        limitationId = new List<Guid>();
                    }
                    Guid currentLimitationId = reader.GetGuid(2);
                    if (!limitationId.Contains(currentLimitationId))
                    {
                        var limitation = new PurchaseLimitation()
                        {
                            Id = reader.GetGuid(2),
                            Airlines = reader.GetString(3),
                            Departures = reader.GetString(4),
                            LimitationGroupId = reader.GetGuid(5),
                            Rebate = new List<PurchaseLimitationRebate>()
                        };
                        view.Limitation.Add(limitation);
                        limitationId.Add(currentLimitationId);
                    }
                    Guid currentRebateLimitationId = reader.GetGuid(6);
                    PurchaseLimitationRateType type = (PurchaseLimitationRateType)reader.GetByte(7);
                    var currentLimitation = view.Limitation.Where(limit => limit.Id == currentRebateLimitationId).FirstOrDefault();
                    if (currentLimitation.Rebate.Count() == 0 ||
                        currentLimitation.Rebate.All(it => it.LimitationId == currentRebateLimitationId && it.Type != type))
                    {
                        var rebate = new PurchaseLimitationRebate()
                        {
                            LimitationId = currentRebateLimitationId,
                            Type = type,
                            AllowOnlySelf = reader.GetBoolean(8)
                        };
                        if (!reader.IsDBNull(9))
                            rebate.Rebate = reader.GetDecimal(9);
                        currentLimitation.Rebate.Add(rebate);
                    }
                }
            }
            return view;
        }

        public void UpdatePurchaseLimitationGroup(Guid companyId)
        {
            string sql = @"DELETE FROM  Rebate
                                   FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                   INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                   INNER JOIN dbo.T_PurchaseLimitationRebate Rebate ON  Limitation.Id = Rebate.LimitationId
                                   WHERE CompanyId= @CurrentCompanyId;
                             DELETE FROM  Limitation
                                    FROM dbo.T_PurchaseLimitationGroup LimitationGroup
                                    INNER JOIN dbo.T_PurchaseLimitation Limitation  ON Limitation.LimitationGroupId = LimitationGroup.Id
                                    WHERE CompanyId= @CurrentCompanyId;
                             DELETE FROM dbo.T_PurchaseLimitationGroup WHERE CompanyId= @CurrentCompanyId;";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("CurrentCompanyId", companyId);
                dbOperator.ExecuteNonQuery(sql);
            }

        }
    }
}
