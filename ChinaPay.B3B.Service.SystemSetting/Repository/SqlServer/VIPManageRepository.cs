using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIP;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class VIPManageRepository : SqlServerRepository, IVIPManagerRepository {
        public VIPManageRepository(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<VIPManageView> Query() {
            string sql = "SELECT [T_VIPManagement].[Id],[T_Membership].[UserName],[T_Company].[AbbreviateName],[T_Company].[Type],[T_Contact].[Name]," +
                         "[T_VIPManagement].[IsVip] FROM [T_VIPManagement] RIGHT JOIN [T_Company] ON [T_Company].[Id]=[T_VIPManagement].[Company] " +
                          "INNER JOIN [T_User] ON [T_Company].[Id] =[T_User].[Owner] AND [T_User].[IsAdmin]=1 " +
                          "INNER JOIN [T_Membership] ON [T_User].[Id]=[T_Membership].[User] " +
                          "INNER JOIN [T_Contact] ON [T_Company].[Contact]=[T_Contact].[Id]";
            var result = new List<VIPManageView>();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var view = new VIPManageView(reader.GetGuid(0));
                        view.UserName = reader.GetString(1);
                        view.AbbreviateName = reader.GetString(2);
                        view.Type = (CompanyType)reader.GetInt32(3);
                        view.Contact = reader.GetString(4);
                        view.IsVip = reader.GetBoolean(5);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public IEnumerable<VIPManageView> Query(VIPManageQueryCondition condition) {
            var result = new List<VIPManageView>();
            string sql = "SELECT [T_VIPManagement].[Id],[T_Membership].[UserName],[T_Company].[AbbreviateName],[T_Company].[Type],[T_Contact].[Name]," +
                         "[T_VIPManagement].[IsVip] FROM [T_VIPManagement] RIGHT JOIN [T_Company] ON [T_Company].[Id]=[T_VIPManagement].[Company] " +
                         "INNER JOIN [T_User] ON [T_Company].[Id] =[T_User].[Owner] AND [T_User].[IsAdmin]=1 " +
                         "INNER JOIN [T_Membership] ON [T_User].[Id]=[T_Membership].[User] " +
                         "INNER JOIN [T_Contact] ON [T_Company].[Contact]=[T_Contact].[Id]";
            StringBuilder strWhere = new StringBuilder();
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(!string.IsNullOrWhiteSpace(condition.UserName)) {
                    strWhere.Append("[T_Membership].[UserName]=@UserName AND");
                    dbOperator.AddParameter("UserName", condition.UserName);
                }
                if(!string.IsNullOrWhiteSpace(condition.AbbreviateName)) {
                    strWhere.Append("[T_Company].[AbbreviateName]=@AbbreviateName AND");
                    dbOperator.AddParameter("AbbreviateName", condition.AbbreviateName);
                }
                if(condition.Type.HasValue) {
                    strWhere.Append("[T_Company].[Type]=@Type");
                    dbOperator.AddParameter("Type", condition.Type);
                }
                if(condition.IsVip.HasValue) {
                    strWhere.Append("[T_VIPManagement].[IsVip]=@IsVip");
                    dbOperator.AddParameter("IsVip", condition.IsVip.Value);
                }
                if(strWhere.Length > 0) {
                    strWhere = strWhere.Remove(strWhere.Length - 4, 4);
                    sql += " WHERE" + strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var view = new VIPManageView(reader.GetGuid(0));
                        view.UserName = reader.GetString(1);
                        view.AbbreviateName = reader.GetString(2);
                        view.Type = (CompanyType)reader.GetInt32(3);
                        view.Contact = reader.GetString(4);
                        view.IsVip = reader.GetBoolean(5);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public int Update(Guid id, bool enabled) {
            string sql = "UPDATE [T_VIPManagement] SET [IsVip]=@IsVip WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("IsVip", enabled);
                dbOperator.AddParameter("Id", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }
        public int Update(IEnumerable<Guid> ids, bool enabled) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(ids.Count() > 0) {
                    string sql = string.Format("UPDATE [T_VIPManagement] SET [IsVip]=@IsVip WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                    dbOperator.AddParameter("IsVip", enabled);
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }

        public int Insert(Domain.VIPManagement managerment) {
            string sql = "INSERT INTO [T_VIPManagement]([Id],[Company],[IsVip]) VALUES (@Id,@Company,@IsVip)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", managerment.Id);
                dbOperator.AddParameter("Company", managerment.Company);
                dbOperator.AddParameter("IsVip", managerment.IsVip);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int Delete(Guid id) {
            string sql = "DELETE FROM [T_VIPManagement] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }


        public Domain.VIPManagement Query(Guid id) {
            Domain.VIPManagement view = null;
            string sql = "SELECT [Company],[IsVip] FROM [dbo].[T_VIPManagement] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", id);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new Domain.VIPManagement(id, reader.GetGuid(0));
                        view.IsVip = reader.GetBoolean(1);
                    }
                }
            }
            return view;
        }


        public int Delete(IEnumerable<Guid> ids) {
            if(ids.Count() > 0) {
                string sql = string.Format("DELETE FROM [T_VIPManagement] WHERE [Id] IN ({0})", ids.Join(",", item => "'" + item.ToString() + "'"));
                using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                    return dbOperator.ExecuteNonQuery(sql);
                }
            } else {
                return 0;
            }
        }
    }
}
