using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup;
using ChinaPay.Repository;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class CompanyGroupReposity : SqlServerRepository, ICompanyGroupRepository {
        public CompanyGroupReposity(string connectionString)
            : base(connectionString) {
        }

        public IEnumerable<DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupListView> QueryCompanyGroups(Guid company, DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupQueryCondition condition) {
            var result = new List<DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupListView>();
            string sql = "SELECT [T_CompanyGroup].[Id],[T_CompanyGroup].[Name],[T_CompanyGroup].[Description],[T_CompanyGroup].[PurchaseMyPolicyOnly],[T_CompanyGroup].[RegisterAccount],[T_CompanyGroup].[RegisterTime],[T_CompanyGroup].[UpdateAccount]," +
                         "[T_CompanyGroup].[UpdateTime],members.memberCount FROM [T_CompanyGroup] LEFT JOIN (SELECT [T_CompanyGroupRelation].[GroupId],Count([T_CompanyGroupRelation].[Company]) AS memberCount From [T_CompanyGroupRelation] GROUP BY [T_CompanyGroupRelation].[GroupId]) members" +
                         " ON [T_CompanyGroup].[Id]=members.[GroupId] WHERE [T_CompanyGroup].[Company]=@Company";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupListView view = new CompanyGroupListView();
                        view.Id = reader.GetGuid(0);
                        view.Name = reader.GetString(1);
                        view.Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        view.MemberCount = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                        view.PurchaseMyPolicyOnly = reader.GetBoolean(3);
                        view.RegisterAccount = reader.GetString(4);
                        view.RegisterTime = reader.GetDateTime(5);
                        view.UpdateAccount = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        if(!reader.IsDBNull(7)) {
                            view.LastUpdateTime = reader.GetDateTime(7);
                        }
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupView QueryCompanyGroup(Guid companyGroup) {
            var result = new DataTransferObject.SystemSetting.CompanyGroup.CompanyGroupView();
            string sql = "SELECT [T_CompanyGroup].[Name],[T_CompanyGroup].[Description],[T_CompanyGroup].[PurchaseMyPolicyOnly],[T_PurchaseMyPolicyOnlyLimitation].[Airlines]," +
                         "[T_PurchaseMyPolicyOnlyLimitation].[Departures],[T_PurchaseMyPolicyOnlyLimitation].[PurchaseNone],[T_PurchaseMyPolicyOnlyLimitation].[PolicyRebate]" +
                         "FROM [T_CompanyGroup] INNER JOIN [T_PurchaseMyPolicyOnlyLimitation] ON [T_CompanyGroup].[Id]=[T_PurchaseMyPolicyOnlyLimitation].[GroupId] WHERE [T_CompanyGroup].[Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", companyGroup);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    var model = new List<Limitation>();
                    while(reader.Read()) {
                        result.Name = reader.GetString(0);
                        result.Description = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        result.PurchaseMyPolicyOnly = reader.GetBoolean(2);
                        Limitation limit = new Limitation();
                        limit.Airlines = reader.GetString(3).Split(new char[] { ',' });
                        limit.Departures = reader.GetString(4).Split(new char[] { '，' });
                        limit.PurchaseMyPolicyOnlyForNonePolicy = reader.GetBoolean(5);
                        if(!reader.IsDBNull(6)) {
                            limit.DefaultRebateForNonePolicy = reader.GetDecimal(6);
                        }
                        model.Add(limit);
                    }
                    result.Limitations = model;
                }
            }
            return result;
        }

        public IEnumerable<DataTransferObject.SystemSetting.CompanyGroup.MemberListView> QueryMembers(Guid companyGroup, DataTransferObject.SystemSetting.CompanyGroup.MemberQueryCondition condition) {
            var result = new List<DataTransferObject.SystemSetting.CompanyGroup.MemberListView>();
            string sql = "SELECT [T_Company].[Id],[T_Company].[AbbreviateName],[T_Membership].[UserName],[T_County].[ChineseName],[T_Contact].[Name],[T_Contact].[Phone]," +
                         "[T_Company].[Status],[T_Company].[RegisterTime],[T_CompanyGroup].[Name] FROM [T_CompanyGroup]  INNER JOIN [T_CompanyGroupRelation] ON " +
                         "[T_CompanyGroup].[Id]=[T_CompanyGroupRelation].[GroupId] INNER JOIN [T_Company] ON [T_CompanyGroupRelation].[Company]=[T_Company].[Id] " +
                         "INNER JOIN [T_User] ON [T_User].[Owner]=[T_Company].[Id] AND [T_User].[IsAdmin]='true' " +
                         "INNER JOIN [T_Membership] ON [T_Membership].[User]= [T_User].[Id] " +
                         "INNER JOIN [T_Contact] ON [T_Contact].[Id]=[T_Company].[Contact] " +
                         "INNER JOIN [T_County] ON [T_County].[Code]=[T_Company].[County] ";
            StringBuilder strWhere = new StringBuilder(" WHERE [T_CompanyGroup].[Id]=@CompanyGroup");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("CompanyGroup", companyGroup);
                if(!string.IsNullOrWhiteSpace(condition.CompanyName)) {
                    strWhere.Append(" AND [T_Company].[Name]=@Name");
                    dbOperator.AddParameter("Name", condition.CompanyName);
                }
                if(!string.IsNullOrWhiteSpace(condition.Contact)) {
                    strWhere.Append(" AND [T_Contact].[Name]=@Contact");
                    dbOperator.AddParameter("Contact", condition.Contact);
                }
                if(!string.IsNullOrWhiteSpace(condition.UserName)) {
                    strWhere.Append(" AND [T_Membership].[UserName]=@UserName");
                    dbOperator.AddParameter("UserName", condition.UserName);
                }
                if(strWhere.Length > 0) {
                    sql += strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DataTransferObject.SystemSetting.CompanyGroup.MemberListView view = new MemberListView();
                        view.CompanyId = reader.GetGuid(0);
                        view.CompanyName = reader.GetString(1);
                        view.UserName = reader.GetString(2);
                        view.County = reader.GetString(3);
                        view.Contact = reader.GetString(4);
                        view.ContactPhone = reader.GetString(5);
                        view.Status = (CompanyStatus)reader.GetInt32(6);
                        view.RegisterTime = reader.GetDateTime(7);
                        view.GroupName = reader.GetString(8);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public IEnumerable<DataTransferObject.SystemSetting.CompanyGroup.MemberListView> QueryCandidateMembers(Guid company, DataTransferObject.SystemSetting.CompanyGroup.MemberQueryCondition condition) {
            var result = new List<DataTransferObject.SystemSetting.CompanyGroup.MemberListView>();
            string sql = "SELECT [T_Company].[Id],[T_Company].[AbbreviateName],[T_Membership].[UserName],[T_County].[ChineseName],[T_Contact].[Name],[T_Contact].[Phone]," +
                   "[T_Company].[Status],[T_Company].[RegisterTime],[T_CompanyGroup].[Name] FROM [T_CompanyGroup]  INNER JOIN [T_CompanyGroupRelation] ON " +
                   "[T_CompanyGroup].[Id]=[T_CompanyGroupRelation].[GroupId]" +
                   " INNER JOIN [T_Company] ON [T_CompanyGroupRelation].[Company]=[T_Company].[Id] AND [T_Company].[AssociationMode]=1 OR [T_Company].[AssociationMode]=2 " +
                   "INNER JOIN [T_User] ON [T_User].[Owner]=[T_Company].[Id] " +
                   "INNER JOIN [T_Membership] ON [T_Membership].[User]= [T_User].[Id] " +
                   "INNER JOIN [T_Contact] ON [T_Contact].[Id]=[T_Company].[Contact] " +
                   "INNER JOIN [T_County] ON [T_County].[Code]=[T_Company].[County] ";
            StringBuilder strWhere = new StringBuilder(" WHERE [T_CompanyGroup].[Company]=@Company");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                if(!string.IsNullOrWhiteSpace(condition.CompanyName)) {
                    strWhere.Append("[T_Company].[Name]=@Name");
                    dbOperator.AddParameter("Name", condition.CompanyName);
                }
                if(!string.IsNullOrWhiteSpace(condition.Contact)) {
                    strWhere.Append("[T_Contact].[Name]=@Contact");
                    dbOperator.AddParameter("Contact", condition.Contact);
                }
                if(!string.IsNullOrWhiteSpace(condition.UserName)) {
                    strWhere.Append("[T_Membership].[UserName]=@UserName");
                    dbOperator.AddParameter("UserName", condition.UserName);
                }
                if(strWhere.Length > 0) {
                    sql += strWhere;
                }
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DataTransferObject.SystemSetting.CompanyGroup.MemberListView view = new MemberListView();
                        view.CompanyId = reader.GetGuid(0);
                        view.CompanyName = reader.GetString(1);
                        view.UserName = reader.GetString(2);
                        view.County = reader.GetString(3);
                        view.Contact = reader.GetString(4);
                        view.ContactPhone = reader.GetString(5);
                        view.Status = (CompanyStatus)reader.GetInt32(6);
                        view.RegisterTime = reader.GetDateTime(7);
                        view.GroupName = reader.GetString(8);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public int RegisterMembers(Guid companyGroup, IEnumerable<Guid> members) {
            if(members.Count() == 0)
                return 0;
            StringBuilder sql = new StringBuilder("INSERT INTO [T_CompanyGroupRelation]([GroupId],[Company])");
            int index = 0;
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                foreach(var item in members) {
                    sql.AppendFormat(" SELECT @GroupId{0},@Company{0} UNION ALL", index);
                    dbOperator.AddParameter("GroupId" + index.ToString(), companyGroup);
                    dbOperator.AddParameter("Company" + index.ToString(), item);
                    index++;
                }
                sql.Remove(sql.Length - 10, 10);
                return dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public int RegisterCompanyGroup(Domain.CompanyGroup companyGroup) {
            StringBuilder sql = new StringBuilder("INSERT INTO [dbo].[T_CompanyGroup]([Id],[Name],[Description],[PurchaseMyPolicyOnly],[RegisterTime],[RegisterAccount],[UpdateTime],[UpdateAccount],[Company]) VALUES(@GroupId,@Name,@Description,@PurchaseMyPolicyOnly,@RegisterTime,@operatorAccount,@UpdateTime,@Company);");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("GroupId", companyGroup.Id);
                dbOperator.AddParameter("Name", companyGroup.Name);
                if(string.IsNullOrWhiteSpace(companyGroup.Description)) {
                    dbOperator.AddParameter("Description", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Description", companyGroup.Description);
                }
                dbOperator.AddParameter("PurchaseMyPolicyOnly", companyGroup.PurchaseMyPolicyOnly);
                dbOperator.AddParameter("RegisterTime", companyGroup.RegisterTime);
                if(string.IsNullOrWhiteSpace(companyGroup.UpdateAccount)) {
                    dbOperator.AddParameter("UpdateAccount", DBNull.Value);
                } else {
                    dbOperator.AddParameter("UpdateAccount", companyGroup.UpdateAccount);
                }
                if(companyGroup.UpdateTime.HasValue) {
                    dbOperator.AddParameter("UpdateTime", companyGroup.UpdateTime);
                } else {
                    dbOperator.AddParameter("UpdateTime", DBNull.Value);
                }
                dbOperator.AddParameter("RegisterAccount", companyGroup.RegisterAccount);
                dbOperator.AddParameter("Company", companyGroup.Company);
                if(companyGroup.Limitations != null && companyGroup.Limitations.Count() > 0) {
                    int index = 0;
                    sql.Append("INSERT INTO [dbo].[T_PurchaseMyPolicyOnlyLimitation] ([Id],[Airlines],[Departures],[PurchaseNone],[PolicyRebate],[GroupId])");
                    foreach(var item in companyGroup.Limitations) {
                        sql.AppendFormat(" SELECT @Id{0},@Airlines{0},@Departures{0},@PurchaseNone{0},@PolicyRebate{0},@GroupId{0} UNION ALL", index);
                        dbOperator.AddParameter("Id" + index.ToString(), item.Id);
                        dbOperator.AddParameter("Airlines" + index.ToString(), item.Airlines.Join("/"));
                        dbOperator.AddParameter("Departures" + index.ToString(), item.Departures.Join("/"));
                        dbOperator.AddParameter("PurchaseNone" + index.ToString(), item.PurchaseMyPolicyOnlyForNonePolicy);
                        if(item.DefaultRebateForNonePolicy.HasValue) {
                            dbOperator.AddParameter("PolicyRebate" + index.ToString(), item.DefaultRebateForNonePolicy);
                        } else {
                            dbOperator.AddParameter("PolicyRebate" + index.ToString(), DBNull.Value);
                        }
                        dbOperator.AddParameter("GroupId" + index.ToString(), companyGroup.Id);
                        index++;
                    }
                }
                sql.Remove(sql.Length - 10, 10);
                return dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public int UpdateCompanyGroup(Domain.CompanyGroup companyGroup) {
            StringBuilder sql = new StringBuilder("Update [dbo].[T_CompanyGroup] SET [Name]=@Name,[Description]=@Description,[PurchaseMyPolicyOnly]=@PurchaseMyPolicyOnly," +
                                                  "[UpdateAccount]=@Account,[UpdateTime]=@UpdateTime,[Company]=@Company WHERE [Id]=@Id;");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", companyGroup.Id);
                dbOperator.AddParameter("Name", companyGroup.Name);
                if(string.IsNullOrWhiteSpace(companyGroup.Description)) {
                    dbOperator.AddParameter("Description", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Description", companyGroup.Description);
                }
                dbOperator.AddParameter("PurchaseMyPolicyOnly", companyGroup.PurchaseMyPolicyOnly);
                if(companyGroup.UpdateTime.HasValue) {
                    dbOperator.AddParameter("UpdateTime", companyGroup.UpdateTime);
                }
                dbOperator.AddParameter("Account", companyGroup.UpdateAccount);
                dbOperator.AddParameter("Company", companyGroup.Company);
                sql.AppendFormat("DELETE FROM [dbo].[T_PurchaseMyPolicyOnlyLimitation] WHERE [GroupId]='{0}';", companyGroup.Id);
                if(companyGroup.Limitations != null && companyGroup.Limitations.Count() > 0) {
                    int index = 0;
                    sql.Append("INSERT INTO [dbo].[T_PurchaseMyPolicyOnlyLimitation]([Id],[Airlines],[Departures],[PurchaseNone],[PolicyRebate],[GroupId])");
                    foreach(var item in companyGroup.Limitations) {
                        sql.AppendFormat(" SELECT @Id{0},@Airlines{0},@Departures{0},@PurchaseNone{0},@PolicyRebate{0},@GroupId{0} UNION ALL", index);
                        dbOperator.AddParameter("Id" + index.ToString(), item.Id);
                        dbOperator.AddParameter("Airlines" + index.ToString(), item.Airlines.Join("/"));
                        dbOperator.AddParameter("Departures" + index.ToString(), item.Departures.Join("/"));
                        dbOperator.AddParameter("PurchaseNone" + index.ToString(), item.PurchaseMyPolicyOnlyForNonePolicy);
                        if(item.DefaultRebateForNonePolicy.HasValue) {
                            dbOperator.AddParameter("PolicyRebate" + index.ToString(), item.DefaultRebateForNonePolicy);
                        } else {
                            dbOperator.AddParameter("PolicyRebate" + index.ToString(), DBNull.Value);
                        }
                        dbOperator.AddParameter("GroupId" + index.ToString(), companyGroup.Id);
                        index++;
                    }
                }
                sql.Remove(sql.Length - 10, 10);
                return dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public int DeleteCompanyGroup(Guid company, Guid companyGroup) {
            string sql = "DELETE FROM [T_CompanyGroup] WHERE [Company]=@Company AND [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("Id", companyGroup);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteCompanyGroups(Guid company, IEnumerable<Guid> companyGroups) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(companyGroups.Count() > 0) {
                    string sql = string.Format("DELETE FROM [T_CompanyGroup] WHERE [Company]='{0}' AND [Id] IN({1})", company, companyGroups.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }

        public int DeleteMembers(Guid companyGroup, IEnumerable<Guid> members) {
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                if(members.Count() > 0) {
                    string sql = string.Format("DELETE FROM [dbo].[T_CompanyGroupRelation] WHERE [GroupId]='{0}' AND [Company] IN ({1})", companyGroup, members.Join(",", item => "'" + item.ToString() + "'"));
                    return dbOperator.ExecuteNonQuery(sql);
                } else {
                    return 0;
                }
            }
        }
    }
}
