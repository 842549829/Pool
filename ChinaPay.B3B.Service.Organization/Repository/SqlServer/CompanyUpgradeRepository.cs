using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Organization.Repository.SqlServer
{
    class CompanyUpgradeRepository : SqlServerRepository, ICompanyUpgradeRepository
    {
        public CompanyUpgradeRepository(string connectionString)
            : base(connectionString)
        {
        }

        public int Save(CompanyUpgrade companyUpgrade)
        {
            string sql = @"IF EXISTS(SELECT NULL FROM [dbo].[T_CompanyUpgradeApplication ] WHERE Company =@Company)
                         UPDATE [dbo].[T_CompanyUpgradeApplication] SET [UserNo] = @UserNo,[Name] = @Name,[AbbreviateName] = @AbbreviateName,[OrginationCode] = @OrginationCode,
                         [OfficePhones] = @OfficePhones,[ManagerName] = @ManagerName,[ManagerPhone] = @ManagerPhone,[EmergencyName] = @EmergencyName,[EmergencyPhone] = @EmergencyPhone,
                         [Type] = @Type,[AccountType] = @AccountType,[Audited] =@Audited,[AuditTime]=@AuditTime,[ApplyTime] = @ApplyTime WHERE [Company] = @Company
                         ELSE INSERT INTO [B3B].[dbo].[T_CompanyUpgradeApplication ]([Company],[UserNo],[Name],[AbbreviateName],[OrginationCode],
                         [OfficePhones],[ManagerName],[ManagerPhone],[EmergencyName] ,[EmergencyPhone],[Type],[AccountType],[Audited],[AuditTime],[ApplyTime])
                         VALUES(@Company,@UserNo,@Name,@AbbreviateName,@OrginationCode,@OfficePhones,@ManagerName,@ManagerPhone,@EmergencyName,@EmergencyPhone,
                         @Type,@AccountType,@Audited,@AuditTime,@ApplyTime)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyUpgrade.Company);
                dbOperator.AddParameter("UserNo", companyUpgrade.UserNo);
                if (string.IsNullOrWhiteSpace(companyUpgrade.Name))
                {
                    dbOperator.AddParameter("Name", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("Name", companyUpgrade.Name);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.AbbreviateName))
                {
                    dbOperator.AddParameter("AbbreviateName", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("AbbreviateName", companyUpgrade.AbbreviateName);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.OrginationCode))
                {
                    dbOperator.AddParameter("OrginationCode", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("OrginationCode", companyUpgrade.OrginationCode);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.OfficePhones))
                {
                    dbOperator.AddParameter("OfficePhones", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("OfficePhones", companyUpgrade.OfficePhones);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.ManagerName))
                {
                    dbOperator.AddParameter("ManagerName", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("ManagerName", companyUpgrade.ManagerName);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.ManagerPhone))
                {
                    dbOperator.AddParameter("ManagerPhone", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("ManagerPhone", companyUpgrade.ManagerPhone);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.EmergencyName))
                {
                    dbOperator.AddParameter("EmergencyName", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("EmergencyName", companyUpgrade.EmergencyName);
                }
                if (string.IsNullOrWhiteSpace(companyUpgrade.EmergencyPhone))
                {
                    dbOperator.AddParameter("EmergencyPhone", DBNull.Value);
                }
                else
                {
                    dbOperator.AddParameter("EmergencyPhone", companyUpgrade.EmergencyPhone);
                }
                dbOperator.AddParameter("Type", (int)companyUpgrade.Type);
                dbOperator.AddParameter("AccountType", (int)companyUpgrade.AccountType);
                dbOperator.AddParameter("Audited", companyUpgrade.Audited);
                if (companyUpgrade.AuditTime.HasValue)
                {
                    dbOperator.AddParameter("AuditTime", companyUpgrade.AuditTime);
                }
                else
                {
                    dbOperator.AddParameter("AuditTime", DBNull.Value);
                }
                dbOperator.AddParameter("ApplyTime", companyUpgrade.ApplyTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Audit(Guid companyId, bool audited, DateTime auditTime)
        {
            string sql = @"UPDATE dbo.T_CompanyUpgradeApplication SET Audited = @Audited,AuditTime = @AuditTime WHERE Company = @companyId";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Audited", audited);
                dbOperator.AddParameter("AuditTime", auditTime);
                dbOperator.AddParameter("companyId", companyId);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        //        public int Update(Domain.CompanyUpgrade companyUpgrade) {
        //            string sql = @"UPDATE [dbo].[T_CompanyUpgradeApplication] SET [UserNo] = @UserNo,[Name] = @Name,[AbbreviateName] = @AbbreviateName,[OrginationCode] = @OrginationCode,
        //                         [OfficePhones] = @OfficePhones,[ManagerName] = @ManagerName,[ManagerPhone] = @ManagerPhone,[EmergencyName] = @EmergencyName,[EmergencyPhone] = @EmergencyPhone,
        //                         [Type] = @Type,[AccountType] = @AccountType,[Audited] =@Audited,[AuditTime]=@AuditTime,[ApplyTime] = @ApplyTime WHERE [Company] = @Company";
        //            using (var dbOperator = new DbOperator(Provider, ConnectionString)) {
        //                dbOperator.AddParameter("Company", companyUpgrade.CompanyId);
        //                dbOperator.AddParameter("UserNo", companyUpgrade.UserNo);
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.CompanyName)) {
        //                    dbOperator.AddParameter("Name", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("Name", companyUpgrade.CompanyName);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.AbbreviateName)) {
        //                    dbOperator.AddParameter("AbbreviateName", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("AbbreviateName", companyUpgrade.AbbreviateName);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.OrginationCode)) {
        //                    dbOperator.AddParameter("OrginationCode", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("OrginationCode", companyUpgrade.OrginationCode);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.CompanyPhone)) {
        //                    dbOperator.AddParameter("OfficePhones", DBNull.Value);
        //                }  else {
        //                    dbOperator.AddParameter("OfficePhones", companyUpgrade.CompanyPhone);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.ManagerName)) {
        //                    dbOperator.AddParameter("ManagerName", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("ManagerName", companyUpgrade.ManagerName);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.ManagerPhone)) {
        //                    dbOperator.AddParameter("ManagerPhone", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("ManagerPhone", companyUpgrade.ManagerPhone);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.EmergencyName)) {
        //                    dbOperator.AddParameter("EmergencyName", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("EmergencyName", companyUpgrade.EmergencyName);
        //                }
        //                if (string.IsNullOrWhiteSpace(companyUpgrade.EmergencyPhone)) {
        //                    dbOperator.AddParameter("EmergencyPhone", DBNull.Value);
        //                } else {
        //                    dbOperator.AddParameter("EmergencyPhone", companyUpgrade.EmergencyPhone);
        //                }
        //                dbOperator.AddParameter("Type", (int)companyUpgrade.CompanyType);
        //                dbOperator.AddParameter("AccountType", (int)companyUpgrade.AccountType);
        //                dbOperator.AddParameter("Audited",companyUpgrade.Audited);
        //                if (companyUpgrade.AuditTime.HasValue) {
        //                    dbOperator.AddParameter("AuditTime", companyUpgrade.AuditTime);
        //                } else  {
        //                    dbOperator.AddParameter("AuditTime",DBNull.Value);
        //                }
        //                dbOperator.AddParameter("ApplyTime", companyUpgrade.ApplyTime);
        //                return dbOperator.ExecuteNonQuery(sql);
        //            }
        //        }

        public CompanyUpgrade Query(Guid companyId)
        {
            CompanyUpgrade companyUpgrade = null;
            string sql = @"SELECT [UserNo],[Name],[AbbreviateName],[OrginationCode],[OfficePhones],[ManagerName],[ManagerPhone],
                         [EmergencyName],[EmergencyPhone],[Type],[AccountType],[Audited],[AuditTime],[ApplyTime] FROM [dbo].[T_CompanyUpgradeApplication]
                          WHERE  [Company] = @Company";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Company", companyId);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        companyUpgrade = new CompanyUpgrade();
                        companyUpgrade.Company = companyId;
                        companyUpgrade.UserNo = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                        {
                            companyUpgrade.Name = reader.GetString(1);
                        }
                        if (!reader.IsDBNull(2))
                        {
                            companyUpgrade.AbbreviateName = reader.GetString(2);
                        }
                        if (!reader.IsDBNull(3))
                        {
                            companyUpgrade.OrginationCode = reader.GetString(3);
                        }
                        if (!reader.IsDBNull(4))
                        {
                            companyUpgrade.OfficePhones = reader.GetString(4);
                        }
                        if (!reader.IsDBNull(5))
                        {
                            companyUpgrade.ManagerName = reader.GetString(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            companyUpgrade.ManagerPhone = reader.GetString(6);
                        }
                        if (!reader.IsDBNull(7))
                        {
                            companyUpgrade.EmergencyName = reader.GetString(7);
                        }
                        if (!reader.IsDBNull(8))
                        {
                            companyUpgrade.EmergencyPhone = reader.GetString(8);
                        }
                        companyUpgrade.Type = (Common.Enums.CompanyType)reader.GetByte(9);
                        companyUpgrade.AccountType = (Common.Enums.AccountBaseType)reader.GetByte(10);
                        companyUpgrade.Audited = reader.GetBoolean(11);
                        if (!reader.IsDBNull(12))
                        {
                            companyUpgrade.AuditTime = reader.GetDateTime(12);
                        }
                        companyUpgrade.ApplyTime = reader.GetDateTime(13);
                    }
                }
            }
            return companyUpgrade;
        }

        public IEnumerable<CompanyUpgrade> QueryNeedAuditCompanys()
        {
            var result = new List<CompanyUpgrade>();
            string sql = @"SELECT [Company],[ApplyTime] FROM [B3B].[dbo].[T_CompanyUpgradeApplication] WHERE [AuditTime] IS NULL";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var companyUpgrade = new CompanyUpgrade();
                        companyUpgrade.Company = reader.GetGuid(0);
                        companyUpgrade.ApplyTime = reader.GetDateTime(1);
                        result.Add(companyUpgrade);
                    }
                }
            }
            return result;
        }

        public IEnumerable<DataTransferObject.Organization.CompanyAuditInfo> QueryNeedAuditCompanies(CompanyAuditQueryCondition condition, Pagination pagination)
        {
            var result = new List<DataTransferObject.Organization.CompanyAuditInfo>();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                if (!string.IsNullOrWhiteSpace(condition.UserNo))
                    dbOperator.AddParameter("@i_UserNo", condition.UserNo);
                if (condition.CompanyType.HasValue)
                    dbOperator.AddParameter("@i_CompanyType", (byte)condition.CompanyType);
                if (condition.AccountType.HasValue)
                    dbOperator.AddParameter("@i_AccountType", (byte)condition.AccountType);
                if (condition.ApplyTimeStart.HasValue)
                    dbOperator.AddParameter("@i_ApplyTimeStart", condition.ApplyTimeStart);
                if (condition.ApplyTimeEnd.HasValue)
                    dbOperator.AddParameter("@i_ApplyTimeEnd", condition.ApplyTimeEnd);

                if (!string.IsNullOrWhiteSpace(condition.CompanyName))
                    dbOperator.AddParameter("@i_CompanyName", condition.CompanyName);
                if (!string.IsNullOrWhiteSpace(condition.AuditType))
                    dbOperator.AddParameter("@i_AuditType", condition.AuditType);
                if (!string.IsNullOrWhiteSpace(condition.SourceType))
                    dbOperator.AddParameter("@i_SourceType", condition.SourceType);
                dbOperator.AddParameter("@i_Pagesize", pagination.PageSize);
                 dbOperator.AddParameter("@i_PageIndex", pagination.PageIndex);
                var totalCount = dbOperator.AddParameter("@o_RowCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_CompanyAuditPagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        result.Add(new CompanyAuditInfo()
                        {
                            CompanyId = reader.GetGuid(0),
                             UserNo= reader.GetString(1),
                            AbbreviateName = reader.GetString(2),
                            CompanyType = (CompanyType)reader.GetByte(3),
                            AccountType = (AccountBaseType)reader.GetByte(4),
                            ApplyTime = reader.GetDateTime(5),
                            AuditType = reader.GetString(6),
                            SourceType = reader.GetString(7),
                             SpreadId = reader.GetGuid(8)
                        });
                    }
                }
                if(pagination.GetRowCount)
                    pagination.RowCount = (int)totalCount.Value;
            }
            return result;
        }
    }
}
