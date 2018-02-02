using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    public class IncomeGroupLimitRespository : SqlServerRepository, IIncomeGroupLimitRespository
    {

        public IncomeGroupLimitRespository(string connectionString)
            : base(connectionString)
        {

        }
        public void InsertIncomeGroupLimit(Domain.IncomeGroupLimitGroup setting)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                InsertIncome(setting, dbOperator, "");
            }
        }

        public void UpdateIncomeGroupLimit(Domain.IncomeGroupLimitGroup setting)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                InsertIncome(setting, dbOperator, "");
            }
        }

        private static void InsertIncome(Domain.IncomeGroupLimitGroup setting, DbOperator dbOperator, string sql)
        {
            if (setting.IncomeGroupId.HasValue)
            {
                sql += @"   DELETE FROM PEROPD 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                                   WHERE LIMITGROUP.IncomeGroupId = @GroupIde;  
                            DELETE FROM  LIMIT 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   WHERE LIMITGROUP.IncomeGroupId = @GroupIds;
                            DELETE FROM T_IncomeGroupLimitGroup WHERE IncomeGroupId = @GroupIdd; ";
                dbOperator.AddParameter("GroupIde", setting.IncomeGroupId);
                dbOperator.AddParameter("GroupIds", setting.IncomeGroupId);
                dbOperator.AddParameter("GroupIdd", setting.IncomeGroupId);
            }
            else
            {
                sql += @"   DELETE FROM PEROPD 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                                   WHERE LIMITGROUP.IncomeGroupId IS NULL AND LIMITGROUP.CompanyId = @CompanyIdq;  
                            DELETE FROM  LIMIT 
                                   FROM T_IncomeGroupLimit LIMIT
                                   INNER JOIN T_IncomeGroupLimitGroup LIMITGROUP ON LIMIT.IncomeId = LIMITGROUP.Id
                                   WHERE LIMITGROUP.IncomeGroupId IS NULL AND LIMITGROUP.CompanyId = @CompanyIds;
                            DELETE FROM T_IncomeGroupLimitGroup WHERE IncomeGroupId IS NULL AND CompanyId = @CompanyIdd; ";
                dbOperator.AddParameter("CompanyIdq", setting.CompanyId);
                dbOperator.AddParameter("CompanyIds", setting.CompanyId);
                dbOperator.AddParameter("CompanyIdd", setting.CompanyId);
            }

            sql += "INSERT INTO T_IncomeGroupLimitGroup (Id,CompanyId,IncomeGroupId,Remark) VALUES(@Id,@CompanyId,@IncomeGroupId,@Remark);";
            dbOperator.AddParameter("Id", setting.Id);
            dbOperator.AddParameter("CompanyId", setting.CompanyId);
            if (setting.IncomeGroupId.HasValue)
            {
                dbOperator.AddParameter("IncomeGroupId", setting.IncomeGroupId);

            }
            else
            {
                dbOperator.AddParameter("IncomeGroupId", DBNull.Value);
            }
            dbOperator.AddParameter("Remark", setting.Remark);
            int i = 0, j = 0;
            foreach (var item in setting.Limitation)
            {
                i++;
                sql += @" INSERT INTO T_IncomeGroupLimit (Id,IncomeId,Type,Price,Airlines,IsOwnerPolicy)
                              VALUES (@Id" + i + ",@IncomeId" + i + ",@Type" + i + ",@Price" + i + ",@Airlines" + i + ",@IsOwnerPolicy" + i + "); ";
                dbOperator.AddParameter("Id" + i, item.Id);
                dbOperator.AddParameter("IncomeId" + i, item.IncomeId);
                dbOperator.AddParameter("Type" + i, item.Type);
                dbOperator.AddParameter("Price" + i, item.Price);
                dbOperator.AddParameter("Airlines" + i, item.Airlines);
                dbOperator.AddParameter("IsOwnerPolicy" + i, item.IsOwnerPolicy);
                foreach (var period in item.Period)
                {
                    j++;
                    sql += @" INSERT INTO T_OEMIncomeGroupPeriod (DeductId,StartPeriod,EndPeriod,Period)
                                  VALUES (@DeductId" + j + ",@StartPeriod" + j + ",@EndPeriod" + j + ",@Period" + j + ");";
                    dbOperator.AddParameter("DeductId" + j, item.Id);
                    dbOperator.AddParameter("StartPeriod" + j, period.StartPeriod / 100);
                    dbOperator.AddParameter("EndPeriod" + j, period.EndPeriod / 100);
                    dbOperator.AddParameter("Period" + j, period.Period / 100);
                }
            }
            dbOperator.ExecuteNonQuery(sql);
        }

        public void InsertIncomeGroupLimitGlobal(Common.Enums.IncomeGroupLimitType type, Domain.IncomeGroupLimitGroup setting)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                var sql = " UPDATE T_Company SET IncomeGroupLimitType = @IncomeGroupLimitType WHERE ID=@COMPANY; ";
                dbOperator.AddParameter("IncomeGroupLimitType", (byte)type);
                dbOperator.AddParameter("COMPANY", setting.CompanyId);
                InsertIncome(setting, dbOperator, sql);
            }
        }

        public Domain.IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByGroupId(Guid groupId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = @"SELECT INCOME.Id,INCOME.CompanyId,INCOME.IncomeGroupId,INCOME.Remark,
                               LIMIT.Id,LIMIT.IncomeId,LIMIT.Type,LIMIT.Price,
                               PEROPD.DeductId,PEROPD.StartPeriod,PEROPD.EndPeriod,PEROPD.Period,LIMIT.Airlines,LIMIT.IsOwnerPolicy
                               FROM T_IncomeGroupLimitGroup INCOME
                               INNER JOIN T_IncomeGroupLimit LIMIT ON INCOME.Id = LIMIT.IncomeId
                               INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                               WHERE INCOME.IncomeGroupId = @IncomeGroupId";
                dbOperator.AddParameter("IncomeGroupId", groupId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    return loadIncomeGroup(reader);
                }
            }
        }

        public Domain.IncomeGroupLimitGroup QueryIncomeGroupLimitGroupByCompanyId(Guid companyId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = @"SELECT INCOME.Id,INCOME.CompanyId,INCOME.IncomeGroupId,INCOME.Remark,
                               LIMIT.Id,LIMIT.IncomeId,LIMIT.Type,LIMIT.Price,
                               PEROPD.DeductId,PEROPD.StartPeriod,PEROPD.EndPeriod,PEROPD.Period ,LIMIT.Airlines,LIMIT.IsOwnerPolicy
                               FROM T_IncomeGroupLimitGroup INCOME
                               INNER JOIN T_IncomeGroupLimit LIMIT ON INCOME.Id = LIMIT.IncomeId
                               INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                               WHERE INCOME.CompanyId = @CompanyId";
                dbOperator.AddParameter("CompanyId", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    return loadIncomeGroup(reader);
                }
            }
        }

        public Domain.IncomeGroupLimitGroup IncomeGroupLimitGroup(Guid superId, Guid purchseId)
        {
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                string sql = @" SELECT INCOME.Id,INCOME.CompanyId,INCOME.IncomeGroupId,INCOME.Remark,
                                LIMIT.Id,LIMIT.IncomeId,LIMIT.Type,LIMIT.Price,
                                PEROPD.DeductId,PEROPD.StartPeriod,PEROPD.EndPeriod,PEROPD.Period ,LIMIT.Airlines,LIMIT.IsOwnerPolicy
                                FROM T_IncomeGroupLimitGroup INCOME
                                INNER JOIN T_IncomeGroupLimit LIMIT ON INCOME.Id = LIMIT.IncomeId
                                INNER JOIN T_OEMIncomeGroupPeriod PEROPD ON LIMIT.Id = PEROPD.DeductId 
                                INNER JOIN T_IncomeGroup GROUPS ON GROUPS.Id= INCOME.IncomeGroupId
                                WHERE INCOME.CompanyId ='" + superId + "' AND GROUPS.Company = '" + purchseId + "'";
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    return loadIncomeGroup(reader);
                }
            }
        }

        private static Domain.IncomeGroupLimitGroup loadIncomeGroup(System.Data.Common.DbDataReader reader)
        {
            Domain.IncomeGroupLimitGroup income = new Domain.IncomeGroupLimitGroup();
            income.Limitation = new List<Domain.IncomeGroupLimit>();
            Domain.IncomeGroupLimit limit = null;
            while (reader.Read())
            {
                var currentId = reader.GetGuid(0);
                if (income.Id != currentId)
                {
                    income.Id = currentId;
                    income.CompanyId = reader.GetGuid(1);
                    income.IncomeGroupId = reader.IsDBNull(2) ? (Guid?)null : reader.GetGuid(2);
                    income.Remark = reader.GetString(3);
                    income.Limitation = new List<Domain.IncomeGroupLimit>();
                }
                var currlimitId = reader.GetGuid(4);
                if (!income.Limitation.Any(it => it.Id == currlimitId))
                {
                    limit = new Domain.IncomeGroupLimit();
                    limit.Id = currlimitId;
                    limit.IncomeId = reader.GetGuid(5);
                    limit.Type = (Common.Enums.PeriodType)reader.GetByte(6);
                    limit.Price = reader.GetInt32(7);
                    limit.Airlines = reader.GetString(12);
                    limit.IsOwnerPolicy = reader.GetBoolean(13);
                    limit.Period = new List<Domain.IncomeGroupPeriod>();
                    income.Limitation.Add(limit);
                }
                Domain.IncomeGroupPeriod period = new Domain.IncomeGroupPeriod();
                period.DeductId = reader.GetGuid(8);
                period.StartPeriod = reader.GetDecimal(9);
                period.EndPeriod = reader.GetDecimal(10);
                period.Period = reader.GetDecimal(11);
                limit.Period.Add(period);
            }
            return income;
        }



    }
}
