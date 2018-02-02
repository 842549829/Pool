using System;
using System.Collections.Generic;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Announce.Reposity.SqlServer {
    public class AnnounceReposity : SqlServerRepository, IAnnounceReposity {
        public AnnounceReposity(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<DataTransferObject.Announce.AnnounceListView> QueryAnnounceList(Guid company, AnnounceQueryCondition condition )
        {
            var result = new List<DataTransferObject.Announce.AnnounceListView>();
            string sql = "SELECT [Id],[Title],[PublishTime],[PublishAccount],[AnnounceType],[AduiteStatus],[AnnounceScope] FROM [dbo].[T_Announce] WHERE [Company]=@Company";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                StringBuilder strWhere = new StringBuilder();
                dbOperator.AddParameter("Company", company);
                if(!string.IsNullOrWhiteSpace(condition.Title)) {
                    strWhere.Append(" AND [Title]=@Title");
                    dbOperator.AddParameter("Title", condition.Title);
                }
                if(!string.IsNullOrWhiteSpace(condition.PublishAccount)) {
                    strWhere.Append(" AND [PublishAccount]=@PublishAccount");
                    dbOperator.AddParameter("PublishAccount", condition.PublishAccount);
                }
                if(condition.AduiteStatus.HasValue) {
                    strWhere.Append(" AND [AduiteStatus]=@AduiteStatus");
                    dbOperator.AddParameter("AduiteStatus", (int)condition.AduiteStatus);
                }
                if(condition.PublishTime.Lower.HasValue) {
                    strWhere.Append(" AND [PublishTime]>@PublishLowerTime");
                    dbOperator.AddParameter("PublishLowerTime", condition.PublishTime.Lower.Value.Date);
                }
                if(condition.PublishTime.Upper.HasValue) {
                    strWhere.Append(" AND [PublishTime]<@PublishUpperTime");
                    dbOperator.AddParameter("PublishUpperTime", condition.PublishTime.Upper.Value.Date);
                }
                if(strWhere.Length > 0) {
                    sql += strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DataTransferObject.Announce.AnnounceListView view = new DataTransferObject.Announce.AnnounceListView();
                        view.Id = reader.GetGuid(0);
                        view.Title = reader.GetString(1);
                        view.PublishTime = reader.GetDateTime(2);
                        view.PublishAccount = reader.GetString(3);
                        view.AnnounceLevel = (AnnounceLevel)reader.GetInt32(4);
                        view.AduiteStatus = (AduiteStatus)reader.GetInt32(5);
                        view.AnnounceScope = (AnnounceScope)reader.GetByte(6);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public IEnumerable<DataTransferObject.Announce.AnnounceListView> QueryAnnounceList(Guid company, AnnounceQueryCondition condition, Pagination pagination)
        {
            var result = new List<DataTransferObject.Announce.AnnounceListView>();
            var fields = "[Id],[Title],[PublishTime],[PublishAccount],[AnnounceType],[AduiteStatus],[AnnounceScope]";
            var catelog = "[dbo].[T_Announce]";
            var orderbyFiled = "[PublishTime] DESC";
            var where = new StringBuilder();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                where.AppendFormat("[Company] = '{0}' AND ", company);
                if (!string.IsNullOrWhiteSpace(condition.Title))
                {
                    where.AppendFormat("[Title]= '{0}' AND ", condition.Title.Trim());
                }
                if (!string.IsNullOrWhiteSpace(condition.PublishAccount))
                {
                    where.AppendFormat("[PublishAccount]= '{0}' AND ", condition.PublishAccount.Trim());
                }
                if (condition.AduiteStatus.HasValue)
                {
                    where.AppendFormat("[AduiteStatus]= {0}  AND ", (int)condition.AduiteStatus);
                }
                if (condition.PublishTime.Lower.HasValue)
                {
                    where.AppendFormat("[PublishTime] > '{0}'  AND ", condition.PublishTime.Lower.Value);
                }
                if (condition.PublishTime.Upper.HasValue)
                {
                    where.AppendFormat("[PublishTime] < '{0}'  AND ", condition.PublishTime.Upper.Value.AddDays(1).AddMilliseconds(-3));
                }
                if (where.Length > 0)
                {
                    where.Remove(where.Length - 5, 5);
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
                        result.Add(new AnnounceListView()
                        {
                            Id = reader.GetGuid(0),
                            Title = reader.GetString(1),
                            PublishTime = reader.GetDateTime(2),
                            PublishAccount = reader.GetString(3),
                            AnnounceLevel = (AnnounceLevel)reader.GetInt32(4),
                            AduiteStatus = (AduiteStatus)reader.GetInt32(5),
                             AnnounceScope = (AnnounceScope)reader.GetByte(6)
                        });
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

        public DataTransferObject.Announce.AnnounceView QueryAnnounce(Guid id) {
            DataTransferObject.Announce.AnnounceView view = null;
            string sql = "SELECT [Title],[Content],[AnnounceType],[PublishTime],[AnnounceScope] FROM [dbo].[T_Announce] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new DataTransferObject.Announce.AnnounceView();
                        view.Title = reader.GetString(0);
                        view.Content = reader.GetString(1);
                        view.AnnounceType = (AnnounceLevel)reader.GetInt32(2);
                        view.PublishTime = reader.GetDateTime(3);
                        view.AnnounceScope = (AnnounceScope)reader.GetByte(4);
                    }
                }
            }
            return view;
        }

        public int Insert(Domain.Announce announce) {
            string sql = "INSERT INTO [dbo].[T_Announce]([Id],[Title],[Content],[PublishTime],[PublishAccount],[AnnounceType],[PublishRole],[AduiteStatus],[Company],[AnnounceScope]) " +
                          "VALUES(@Id,@Title,@Content,@PublishTime,@PublishAccount,@AnnounceType,@PublishRole,@AduiteStatus,@Company,@AnnounceScope)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", announce.Id);
                dbOperator.AddParameter("Title", announce.Title);
                dbOperator.AddParameter("Content", announce.Content);
                dbOperator.AddParameter("PublishTime", announce.PublishTime);
                dbOperator.AddParameter("PublishAccount", announce.PublishAccount);
                dbOperator.AddParameter("AnnounceType", (int)announce.AnnounceType);
                dbOperator.AddParameter("PublishRole", (int)announce.PublishRole);
                dbOperator.AddParameter("AduiteStatus", (int)announce.AduiteStatus);
                dbOperator.AddParameter("Company", announce.Company);
                dbOperator.AddParameter("AnnounceScope",(int)announce.AnnunceScope);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Update(Domain.Announce announce) {
            string sql = "UPDATE [dbo].[T_Announce] SET [Title]=@Title,[Content]=@Content," +
                         "[AnnounceType]=@AnnounceType,[AnnounceScope]=@AnnounceScope WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Title", announce.Title);
                dbOperator.AddParameter("Content", announce.Content);
                dbOperator.AddParameter("AnnounceType", (int)announce.AnnounceType);
                dbOperator.AddParameter("AnnounceScope",(int)announce.AnnunceScope);
                dbOperator.AddParameter("Id", announce.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateStatus(Guid id, AduiteStatus status) {
            string sql = "UPDATE [dbo].[T_Announce] SET [AduiteStatus]=@Status WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Status", (int)status);
                dbOperator.AddParameter("Id", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateStatuses(IEnumerable<Guid> ids, AduiteStatus status)
        {
            string sql = string.Format("UPDATE [dbo].[T_Announce] SET [AduiteStatus]= {0} WHERE [Id] IN ({1})", (int)status, ids.Join(",", item => "'" + item.ToString() + "'"));
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(IEnumerable<Guid> ids) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                string sql = string.Format("DELETE FROM [dbo].[T_Announce] WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<AnnounceListView> Query(Guid company,bool domainIsOem,bool companyIsOem, Pagination pagination) 
        {
            var result = new List<DataTransferObject.Announce.AnnounceListView>();
            var fields = "[Id],[Title],[PublishTime],[PublishAccount],[AnnounceType]";
            var catelog = "[dbo].[T_Announce]";
            var orderbyFiled = "[PublishTime] DESC,[AnnounceType] DESC";
            var where = new StringBuilder();
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                where.AppendFormat("(PublishRole= {0} AND [AduiteStatus]={1}",
                   (byte)PublishRole.平台, (int)ChinaPay.B3B.DataTransferObject.Announce.AduiteStatus.Audited);
                if (companyIsOem)
                    where.Append(")");
                else
                {
                    where.AppendFormat(" AND {0}=[AnnounceScope]&{0})", (byte)(domainIsOem ? AnnounceScope.OEM : AnnounceScope.B3B));
                    where.AppendFormat(" OR ([Company] = '{0}' AND [AduiteStatus] = {1})",
                    company, (int)ChinaPay.B3B.DataTransferObject.Announce.AduiteStatus.Audited);
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
                        result.Add(new AnnounceListView()
                        {
                            Id = reader.GetGuid(0),
                            Title = reader.GetString(1),
                            PublishTime = reader.GetDateTime(2),
                            PublishAccount = reader.GetString(3),
                            AnnounceLevel = (AnnounceLevel)reader.GetInt32(4)
                        });
                    }
                }
                if (pagination.GetRowCount)
                {
                    pagination.RowCount = (int)totalCount.Value;
                }
            }
            return result;
        }

        public AduiteStatus QueryAduiteStatus(Guid id) {
            var result = AduiteStatus.UnAudit;
            string sql = "SELECT [AduiteStatus] FROM [dbo].[T_Announce] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result = (AduiteStatus)reader.GetInt32(0);
                    }
                }
            }
            return result;
        }

        public Guid QueryPlatForm()
        {
            Guid platform = Guid.NewGuid();
            string sql = "SELECT [Company] FROM [dbo].[T_Announce] WHERE PublishRole = @PublishRole";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("PublishRole", PublishRole.平台);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        platform = reader.GetGuid(0);
                    }
                }
            }
            return platform;
        }


        public IEnumerable<Guid> QueryEmergencyIds()
        {
            var result = new List<Guid>();
            string sql = "SELECT [Id] FROM [dbo].[T_Announce]  WHERE [PublishRole]=@PublishRole AND [AduiteStatus]=@AuditStatus AND [AnnounceType]=@AnnounceType ORDER BY [PublishTime] DESC";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("PublishRole", PublishRole.平台);
                dbOperator.AddParameter("AuditStatus", (int)AduiteStatus.Audited);
                dbOperator.AddParameter("AnnounceType", (int)AnnounceLevel.Emergency);
                using (var reader = dbOperator.ExecuteReader(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetGuid(0));
                    }
                }
            }
            return result;
        }


        public IEnumerable<Guid> QueryEmergencyIdsByOem(Guid company, bool domainIsOem, bool companyIsOem)
        {
            var result = new List<Guid>();
            StringBuilder sql = new StringBuilder();
            
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                sql.AppendFormat(@"SELECT [Id],PublishRole,PublishTime FROM [dbo].[T_Announce] 
 WHERE (PublishRole= {0} AND [AnnounceType] ={1} AND [AduiteStatus]={2}",
(byte)PublishRole.平台, (byte)AnnounceLevel.Emergency, (byte)AduiteStatus.Audited);
                if (companyIsOem)
                    sql.Append(")");
                else
                {
                    sql.AppendFormat(" AND {0}=[AnnounceScope]&{0}) OR ", (byte)(domainIsOem ? AnnounceScope.OEM : AnnounceScope.B3B));
                    sql.AppendFormat(@"([Company]='{0}' AND [AduiteStatus]={1} AND [AnnounceType]={2})", company, (byte)AduiteStatus.Audited, (byte)AnnounceLevel.Emergency);
                }
                sql.Append(" ORDER BY PublishRole,PublishTime DESC");
                using (var reader = dbOperator.ExecuteReader(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetGuid(0));
                    }
                }
            }
            return result;
        }
    }
}
