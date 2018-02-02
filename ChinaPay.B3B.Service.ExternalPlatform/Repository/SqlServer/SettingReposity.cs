using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.DataAccess;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform.Repository.SqlServer
{
    public class SettingReposity : SqlServerRepository, ISettingReposity
    {
        public SettingReposity(string connectionString)
            : base(connectionString)
        {
        }

        public IEnumerable<Setting> QuerySettings()
        {
            var result = new List<Setting>();
            string sql = @"SELECT [Platform],[Deduct],[Provider],[RebateBalance],setting.[Enabled],[PayInterface], employee.[Login]
                           FROM [dbo].[T_Setting] setting
                           INNER JOIN dbo.T_Company company ON setting.Provider = company.Id
                           INNER JOIN dbo.T_Employee employee ON employee.IsAdministrator = 1 AND employee.[Owner] = company.Id";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        var setting = new Setting();
                        setting.Platform = (Common.Enums.PlatformType)reader.GetByte(0);
                        setting.Deduct = reader.GetDecimal(1);
                        setting.Provider = reader.GetGuid(2);
                        setting.RebateBalance = reader.GetDecimal(3);
                        setting.Enabled = reader.GetBoolean(4);
                        var strPayInterface = reader.GetString(5);
                        var list = new List<ChinaPay.B3B.DataTransferObject.Common.PayInterface>();
                        setting.StrPayInterface = "";
                        foreach (var item in strPayInterface.Split('|'))
                        {
                            list.Add((DataTransferObject.Common.PayInterface)int.Parse(item));
                            setting.StrPayInterface += ((DataTransferObject.Common.PayInterface)int.Parse(item)).GetDescription()+"|";
                        }
                        if (!string.IsNullOrWhiteSpace(setting.StrPayInterface))
                           setting.StrPayInterface = setting.StrPayInterface.Remove(setting.StrPayInterface.Length - 1);
                        setting.PayInterface = list.ToArray();
                        setting.ProviderAccount = reader.GetString(6);
                        result.Add(setting);
                    }
                }
            }
            return result;
        }

        public Setting QuerySetting(Common.Enums.PlatformType platform)
        {
            Setting setting = null;
            string sql = @"SELECT [Deduct],[Provider],[RebateBalance],setting.[Enabled],[PayInterface],employee.[login] FROM [dbo].[T_Setting] setting                           INNER JOIN dbo.T_Company company ON setting.Provider = company.Id
                           INNER JOIN dbo.T_Employee employee ON employee.IsAdministrator = 1 AND employee.[Owner] = company.Id 
WHERE [Platform]=@Platform";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Platform", (byte)platform);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        setting = new Setting()
                        {
                            Platform =platform,
                             Deduct = reader.GetDecimal(0),
                              Provider = reader.GetGuid(1),
                               RebateBalance = reader.GetDecimal(2),
                                Enabled = reader.GetBoolean(3),
                                ProviderAccount = reader.GetString(5)
                        };
                        var strPayInterface = reader.GetString(4);
                        var list = new List<ChinaPay.B3B.DataTransferObject.Common.PayInterface>();
                        foreach (var item in strPayInterface.Split('|'))
                        {
                            list.Add((DataTransferObject.Common.PayInterface)int.Parse(item));
                        }
                        setting.PayInterface = list.ToArray();
                    }
                }
            }
            return setting;
        }

        public int InsertSetting(Setting setting)
        {
            string sql = @"INSERT INTO [B3B].[dbo].[T_Setting]([Platform],[Deduct],[Provider] ,[RebateBalance],[Enabled],[PayInterface]) VALUES
           (@Platform,@Deduct,@Provider,@RebateBalance,@Enabled,@PayInterface)";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Platform", (byte)setting.Platform);
                dbOperator.AddParameter("Deduct", setting.Deduct);
                dbOperator.AddParameter("Provider", setting.Provider);
                dbOperator.AddParameter("RebateBalance", setting.RebateBalance);
                dbOperator.AddParameter("Enabled", setting.Enabled);
                string strPayInterface = "";
                var str = new List<string>();
                foreach (var item in setting.PayInterface)
                {
                    str.Add(((byte)item).ToString());
                }
                strPayInterface = string.Join("|", str);
                dbOperator.AddParameter("PayInterface", strPayInterface);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateSetting(Setting setting)
        {
            string sql = @"UPDATE [dbo].[T_Setting] SET [Deduct] = @Deduct,[Provider] = @Provider,[RebateBalance] = @RebateBalance,[PayInterface] = @PayInterface WHERE [Platform] = @Platform";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Deduct", setting.Deduct);
                dbOperator.AddParameter("Provider", setting.Provider);
                dbOperator.AddParameter("RebateBalance", setting.RebateBalance);
                string strPayInterface = "";
                var str = new List<string>();
                foreach (var item in setting.PayInterface)
                {
                    str.Add(((byte)item).ToString());
                }
                strPayInterface = string.Join("|",str);
                dbOperator.AddParameter("PayInterface", strPayInterface);
                dbOperator.AddParameter("Platform",(byte)setting.Platform);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateStatus(Common.Enums.PlatformType platform, bool enabled)
        {
            string sql = "UPDATE [dbo].[T_Setting] SET [Enabled]= @Enabled WHERE Platform = @Platform";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("Enabled", enabled);
                dbOperator.AddParameter("Platform", platform);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<DataTransferObject.Policy.ExternalPolicyLog> QueryExternalPolicys(decimal? orderId, System.DateTime stratDate, System.DateTime endDate, ChinaPay.Core.Pagination pagination)
        {
            List<DataTransferObject.Policy.ExternalPolicyLog> result = new List<DataTransferObject.Policy.ExternalPolicyLog>();
            string fields = "[OrderId],[Id],[Platform],[Provider],[TicketType],[RequireChangePNR],[Condition],[ETDZSpeed],[WorkStart],[WorkEnd],[ScrapStart],[ScrapEnd],[OriginalRebate],[Rebate],[ParValue],[OfficeNo],[RequireAuth] ,[RecordDate],[OriginalPolicyContent]";
            string catelog = "[dbo].[T_ExternalPolicyCopy]";
            string orderbyFiled = "[RecordDate] DESC";
            System.Text.StringBuilder where = new System.Text.StringBuilder();
            using (DbOperator dbOperator = new DbOperator(Provider, ConnectionString))
            {
                where.AppendFormat("[RecordDate] BETWEEN N'{0}' AND N'{1}'", stratDate, endDate.AddDays(1).AddTicks(-1));
                if (orderId.HasValue)
                {
                    where.AppendFormat(" AND [OrderId] = {0}", orderId);
                }
                dbOperator.AddParameter("@iField", fields);
                dbOperator.AddParameter("@iCatelog", catelog);
                dbOperator.AddParameter("@iCondition", where.ToString());
                dbOperator.AddParameter("@iOrderBy", orderbyFiled);
                dbOperator.AddParameter("@iPagesize", pagination.PageSize);
                dbOperator.AddParameter("@iPageIndex", pagination.PageIndex);
                dbOperator.AddParameter("@iGetCount", pagination.GetRowCount);
                var totalCount = dbOperator.AddParameter("@oTotalCount");
                totalCount.DbType = System.Data.DbType.Int32;
                totalCount.Direction = System.Data.ParameterDirection.Output;
                using (var reader = dbOperator.ExecuteReader("dbo.P_Pagination", System.Data.CommandType.StoredProcedure))
                {
                    while (reader.Read())
                    {
                        var log = new DataTransferObject.Policy.ExternalPolicyLog();
                        log.OrderId = reader.GetDecimal(0);
                        log.Id = reader.GetString(1);
                        log.Platform = (PlatformType) reader.GetByte(2);
                        log.Provider = reader.GetGuid(3);
                        log.TicketType = (TicketType)reader.GetByte(4);
                        log.RequireChangePNR = reader.GetBoolean(5);
                        log.Condition = reader.GetString(6);
                        log.ETDZSpeed = reader.GetInt32(7);
                        log.WorkStart = Izual.Time.Parse(reader.GetValue(8).ToString());
                        log.WorkEnd = Izual.Time.Parse(reader.GetValue(9).ToString());
                        log.ScrapStart = Izual.Time.Parse(reader.GetValue(10).ToString());
                        log.ScrapEnd = Izual.Time.Parse(reader.GetValue(11).ToString());
                        log.OriginalRebate = reader.GetDecimal(12);
                        log.Rebate = reader.GetDecimal(13);
                        log.ParValue = reader.GetDecimal(14);
                        log.OfficeNo = reader.IsDBNull(15) ? string.Empty : reader.GetString(15);
                        log.RequireAuth = reader.GetBoolean(16);
                        log.RecordDate = reader.GetDateTime(17);
                        log.OriginalContent = reader.IsDBNull(18)?string.Empty:reader.GetString(18);
                        result.Add(log);
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

    }
}
