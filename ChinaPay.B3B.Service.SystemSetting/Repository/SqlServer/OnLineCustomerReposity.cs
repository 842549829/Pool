using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.Repository;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.DataAccess;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting.Repository.SqlServer {
    class OnLineCustomerReposity : SqlServerRepository, IOnLineCustomerRepository {
        public OnLineCustomerReposity(string connectionString)
            : base(connectionString) {
        }

        public OnLineCustomer Query(Guid company, PublishRoles role)
        {
            OnLineCustomer view = null;
            string sql = "SELECT [T_OnLineCustomer].[Title],[T_OnLineCustomer].[Content],[T_DivideGroupManage].[Id],[T_DivideGroupManage].[Name],[T_DivideGroupManage].[Description]," +
                         "[T_DivideGroupManage].[SortLevel] "+
                          "FROM [T_OnLineCustomer] LEFT JOIN [T_DivideGroupManage] ON [T_DivideGroupManage].[Company]=[T_OnLineCustomer].[Company] " +
                          "WHERE [T_OnLineCustomer].[Company]=@Company AND [T_OnLineCustomer].[PublishRole]=@Role ORDER BY [T_DivideGroupManage].[SortLevel]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("Role", (int)role);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    var divideGroup = new List<DivideGroupManage>();
                    while(reader.Read()) {
                        view = new OnLineCustomer();
                        view.Title = reader.GetString(0);
                        view.Content = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                            var model = new DivideGroupManage(reader.GetGuid(2));
                            model.Name = reader.GetString(3);
                            model.Description = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                            model.SortLevel = reader.GetInt32(5);
                            divideGroup.Add(model);
                        }
                    }
                    if (divideGroup.Count > 0)
                    {
                        view.DivideGroupManage = divideGroup;
                    }
                }
            }
            return view;
        }

        public int SaveOnLine(Guid company, PublishRoles role, OnLineCustomerView view)
        {
            string sql = "IF EXISTS(SELECT NULL FROM [dbo].[T_OnLineCustomer] WHERE [Company]=@Company) UPDATE[dbo].[T_OnLineCustomer] SET [Title]=@Title," +
                         "[Content]=@Content,[PublishRole]=@PublishRole WHERE [Company]=@Company ELSE INSERT INTO [dbo].[T_OnLineCustomer](Title,Content,PublishRole,Company)" +
                         " VALUES(@Title,@Content,@PublishRole,@Company)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("Title", view.Title);
                dbOperator.AddParameter("Content", view.Content);
                dbOperator.AddParameter("PublishRole", (int)role);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<DivideGroupView> QueryDivideGroups(Guid company) {
            var result = new List<DivideGroupView>();
            string sql = "SELECT [Id],[Name],[Description],[SortLevel] FROM [dbo].[T_DivideGroupManage] WHERE [T_DivideGroupManage].[Company]=@Company ORDER BY [SortLevel]";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        DivideGroupView view = new DivideGroupView();
                        view.Id = reader.GetGuid(0);
                        view.Name = reader.GetString(1);
                        view.Description = reader.IsDBNull(2) ? String.Empty : reader.GetString(2);
                        result.Add(view);
                    }
                }
            }
            return result;
        }

        public DivideGroupView QueryDivideGroup(Guid divideGroup) {
            DivideGroupView view = null;
            string sql = "SELECT [Name],[Description],[SortLevel] FROM [dbo].[T_DivideGroupManage] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", divideGroup);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new DivideGroupView();
                        view.Id = divideGroup;
                        view.Name = reader.GetString(0);
                        view.Description = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        view.SortLevel = reader.GetInt32(2);
                    }
                }
            }
            return view;
        }

        public int InsertDivideGroup(Guid company, DivideGroupView divideGroup) {
            string sql = "INSERT INTO [dbo].[T_DivideGroupManage]([Id],[Name],[Description],[SortLevel],[Company]) VALUES(@Id,@Name,@Description,@SortLevel,@Company)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", divideGroup.Id);
                dbOperator.AddParameter("Name", divideGroup.Name);
                if(string.IsNullOrWhiteSpace(divideGroup.Description)) {
                    dbOperator.AddParameter("Description", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Description", divideGroup.Description);
                }
                dbOperator.AddParameter("SortLevel", divideGroup.SortLevel);
                dbOperator.AddParameter("Company", company);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateDivideGroup(DivideGroupView divideGroup) {
            string sql = "UPDATE [dbo].[T_DivideGroupManage] SET [Name]=@Name,[Description]=@Description,[SortLevel]=@SortLevel WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Name", divideGroup.Name);
                if(string.IsNullOrWhiteSpace(divideGroup.Description)) {
                    dbOperator.AddParameter("Description", DBNull.Value);
                } else {
                    dbOperator.AddParameter("Description", divideGroup.Description);
                }
                dbOperator.AddParameter("SortLevel", divideGroup.SortLevel);
                dbOperator.AddParameter("Id", divideGroup.Id);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteDivideGroup(Guid divideGroup) {
            string sql = "DELETE FROM [dbo].[T_DivideGroupManage] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", divideGroup);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<MemberManage> QueryMembers(Guid divideGroup) {
            var result = new List<MemberManage>();
            string sql = "SELECT [Id],[Remark],[QQ] FROM [dbo].[T_MemberManage] WHERE [DivideGroupId]=@DivideGroup ORDER BY SortLevel";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("DivideGroup", divideGroup);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        MemberManage member = new MemberManage(reader.GetGuid(0));
                        member.Remark = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        member.QQ = reader.GetString(2).Split(new char[] { ',' });
                        result.Add(member);
                    }
                }
            }
            return result;
        }

        public MemberManage QueryMember(Guid member) {
            MemberManage view = null;
            string sql = "SELECT [Remark],[QQ],[SortLevel] FROM [dbo].[T_MemberManage] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", member);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new MemberManage(member);
                        view.Remark = reader.GetString(0);
                        view.QQ = reader.GetString(1).Split(new char[] { ',' });
                        view.SortLevel = reader.GetInt32(2);
                    }
                }
            }
            return view;
        }

        public int InsertMember(Guid divideGroup, MemberManage member) {
            string sql = "INSERT INTO [dbo].[T_MemberManage]([Id],[Remark],[QQ],[DivideGroupId],[SortLevel]) VALUES(@Id,@Remark,@QQ,@DivideGroup,@SortLevel)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", member.Id);
                dbOperator.AddParameter("Remark", member.Remark);
                dbOperator.AddParameter("QQ", member.QQ.Join(","));
                dbOperator.AddParameter("DivideGroup", divideGroup);
                dbOperator.AddParameter("SortLevel", member.SortLevel);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int UpdateMember(MemberManage member) {
            string sql = "UPDATE [dbo].[T_MemberManage] SET [Remark]=@Remark,[QQ]=@QQ,[SortLevel]=@SortLevel WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Remark", member.Remark);
                dbOperator.AddParameter("QQ", member.QQ.Join(","));
                dbOperator.AddParameter("Id", member.Id);
                dbOperator.AddParameter("SortLevel",member.SortLevel);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public int DeleteMember(Guid member) {
            string sql = "DELETE FROM [dbo].[T_MemberManage] WHERE [Id]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Id", member);
                return dbOperator.ExecuteNonQuery(sql);
            }
        }

        public OnLineCustomerView Query(Guid company) {
            OnLineCustomerView view = null;
            string sql = "SELECT [Title],[Content] FROM [dbo].[T_OnLineCustomer] WHERE [Company]=@Company";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        view = new OnLineCustomerView();
                        view.Title = reader.GetString(0);
                        view.Content = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                    }
                }
            }
            return view;
        }


        public Guid QueryPlatFormCompany() {
            Guid platform = Guid.NewGuid();
            string sql = "SELECT [Company] FROM [dbo].[T_OnLineCustomer] WHERE PublishRole = @PublishRole";
            using (var dbOperator = new DbOperator(Provider, ConnectionString))
            {
                dbOperator.AddParameter("PublishRole", PublishRoles.平台);
                using(var reader = dbOperator.ExecuteReader(sql))
                {
                    while(reader.Read())
                    {
                        platform = reader.GetGuid(0);
                    }
                }
            }
            return platform;
        }
    }
}
