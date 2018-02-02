using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class PolicyHarmonyRepository : SqlServerRepository, IPolicyHarmonyRepository {
        public PolicyHarmonyRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView> Query(DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyQueryCondition condition) {
            var result = new List<DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView>();
            string sql = "SELECT [Id],[Airlines],[Departure],[Arrival],[PolicyType],[CityLimit],[EffectiveLowerDate],[EffectiveUpperDate]," +
                         "[IsVIP],[DeductionType],[HarmonyValue],[Account],[AddTime] FROM [dbo].[T_PolicyHarmony] WHERE [PolicyType]=@PolicyType";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                StringBuilder strWhere = new StringBuilder();
                dbOperator.AddParameter("PolicyType", (int)condition.PolicyType);
                if(!string.IsNullOrWhiteSpace(condition.Airline)) {
                    strWhere.Append(" AND [Airlines] LIKE '%'+@AIRLINE+'%'");
                    dbOperator.AddParameter("AIRLINE", condition.Airline);
                }
                if(!string.IsNullOrWhiteSpace(condition.Departure)) {
                    strWhere.Append(" AND [Departure] LIKE '%'+@DEPARTURE+'%'");
                    dbOperator.AddParameter("DEPARTURE", condition.Departure);
                }
                if(!string.IsNullOrWhiteSpace(condition.Arrival)) {
                    strWhere.Append(" AND [Arrival] LIKE '%'+@ARRIVAL+'%'");
                    dbOperator.AddParameter("ARRIVAL", condition.Arrival);
                }
                if(condition.EffectiveDate.Lower.HasValue) {
                    strWhere.Append(" AND [EffectiveLowerDate]=@EffectiveLowerDate");
                    dbOperator.AddParameter("EffectiveLowerDate", condition.EffectiveDate.Lower.Value.Date);
                }
                if(condition.EffectiveDate.Upper.HasValue) {
                    strWhere.Append(" AND [EffectiveUpperDate]=@EffectiveUpperDate");
                    dbOperator.AddParameter("EffectiveUpperDate", condition.EffectiveDate.Upper.Value.Date);
                }
                if(strWhere.Length > 0) {
                    sql += strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView view = new DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView(reader.GetGuid(0));
                        view.Account = reader.GetString(11);
                        view.AddTime = reader.GetDateTime(12);
                        view.DeductionType = (DeductionType)reader.GetInt32(9);
                        view.HarmonyValue = reader.GetDecimal(10);
                        view.IsVIP = reader.GetBoolean(8);
                        view.PolicyType = (Common.Enums.PolicyType)reader.GetInt32(4);
                        view.EffectiveDate = new Core.Range<DateTime>(reader.GetDateTime(6), reader.GetDateTime(7));
                        view.Airlines = reader.GetString(1).Split(new char[] { ',' });
                        view.Arrival = reader.GetString(3).Split(new char[] { ',' });
                        view.Departure = reader.GetString(2).Split(new char[] { ',' });
                        view.CityLimit = reader.GetString(5).Split(new char[] { ',' });
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public int Insert(Domain.PolicyHarmony harmony) {
            string sql = "INSERT INTO [dbo].[T_PolicyHarmony]([Id],[Airlines],[Departure],[Arrival],[PolicyType],[CityLimit],[EffectiveLowerDate],[EffectiveUpperDate],[IsVIP],[DeductionType],[HarmonyValue],[Remark],[Account],[AddTime]) " +
                         "VALUES (@Id,@Airlines,@Departure,@Arrival,@PolicyType,@CityLimit,@EffectiveLowerDate,@EffectiveUpperDate,@IsVIP,@DeductionType,@HarmonyValue,@Remark,@Account,@AddTime)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", harmony.Id);
                dbOperator.AddParameter("Airlines", harmony.Airlines.Join("/"));
                dbOperator.AddParameter("Departure", harmony.Departure.Join("/"));
                dbOperator.AddParameter("Arrival", harmony.Arrival.Join("/"));
                dbOperator.AddParameter("PolicyType", (int)harmony.PolicyType);
                dbOperator.AddParameter("CityLimit", harmony.CityLimit.Join("/"));
                dbOperator.AddParameter("EffectiveLowerDate", harmony.EffectiveDateRange.Lower);
                dbOperator.AddParameter("EffectiveUpperDate", harmony.EffectiveDateRange.Upper);
                dbOperator.AddParameter("Isvip", harmony.IsVIP);
                dbOperator.AddParameter("DeductionType", (int)harmony.DeductionType);
                dbOperator.AddParameter("HarmonyValue", harmony.HarmonyValue);
                if(string.IsNullOrWhiteSpace(harmony.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", harmony.Remark);
                }
                dbOperator.AddParameter("Account", harmony.Account);
                dbOperator.AddParameter("AddTime", harmony.AddTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Domain.PolicyHarmony harmony) {
            string sql = "UPDATE [dbo].[T_PolicyHarmony] SET [Airlines]=@Airlines,[Departure]=@Departure,[Arrival]=@Arrival,[PolicyType]=@PolicyType,[CityLimit]=@CityLimit," +
                        "[EffectiveLowerDate]=@EffectiveLowerDate,[EffectiveUpperDate]=@EffectiveUpperDate," +
                        "[IsVIP]=@IsVIP,[DeductionType]=@DeductionType,[HarmonyValue]=@HarmonyValue,[Remark]=@Remark,[Account]=@Account,[AddTime]=@AddTime WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", harmony.Id);
                dbOperator.AddParameter("Airlines", harmony.Airlines.Join("/"));
                dbOperator.AddParameter("Departure", harmony.Departure.Join("/"));
                dbOperator.AddParameter("Arrival", harmony.Arrival.Join("/"));
                dbOperator.AddParameter("PolicyType", (int)harmony.PolicyType);
                dbOperator.AddParameter("CityLimit", harmony.CityLimit.Join("/"));
                dbOperator.AddParameter("EffectiveLowerDate", harmony.EffectiveDateRange.Lower);
                dbOperator.AddParameter("EffectiveUpperDate", harmony.EffectiveDateRange.Upper);
                dbOperator.AddParameter("IsVIP", harmony.IsVIP);
                dbOperator.AddParameter("DeductionType", (int)harmony.DeductionType);
                dbOperator.AddParameter("HarmonyValue", harmony.HarmonyValue);
                if(string.IsNullOrWhiteSpace(harmony.Remark)) {
                    dbOperator.AddParameter("Remark", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Remark", harmony.Remark);
                }
                dbOperator.AddParameter("Account", harmony.Account);
                dbOperator.AddParameter("AddTime", harmony.AddTime);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(Guid id) {
            string sql = "DELETE FROM [dbo].[T_PolicyHarmony] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(IEnumerable<Guid> ids) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(ids.Count() > 0) {
                    string sql = string.Format("DELETE FROM [dbo].[T_PolicyHarmony] WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }


        public DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView Query(Guid id) {
            DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView view = null;
            string sql = "SELECT [Id],[Airlines],[Departure],[Arrival],[PolicyType],[CityLimit],[EffectiveLowerDate],[EffectiveUpperDate]," +
                         "[IsVIP],[DeductionType],[HarmonyValue],[Account],[AddTime],[Remark] FROM [dbo].[T_PolicyHarmony] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new DataTransferObject.SystemSetting.PolicyHarmony.PolicyHarmonyView(id);
                        view.Account = reader.GetString(11);
                        view.AddTime = reader.GetDateTime(12);
                        view.DeductionType = (DeductionType)reader.GetInt32(9);
                        view.HarmonyValue = reader.GetDecimal(10);
                        view.IsVIP = reader.GetBoolean(8);
                        view.PolicyType = (PolicyType)reader.GetInt32(4);
                        view.EffectiveDate = new Core.Range<DateTime>(reader.GetDateTime(6), reader.GetDateTime(7));
                        view.Airlines = reader.GetString(1).Split(new char[] { ',' });
                        view.Arrival = reader.GetString(3).Split(new char[] { ',' });
                        view.Departure = reader.GetString(2).Split(new char[] { ',' });
                        view.CityLimit = reader.GetString(5).Split(new char[] { ',' });
                        view.Remark = reader.IsDBNull(13) ? string.Empty : reader.GetString(13);
                    }
                }
            }
            return view;
        }
    }
}
