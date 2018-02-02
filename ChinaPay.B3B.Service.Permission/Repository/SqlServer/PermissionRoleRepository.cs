using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Permission.Repository.SqlServer {
    class PermissionRoleRepository : SqlServerRepository, IPermissionRoleRepository {
        public PermissionRoleRepository(string connectionString)
            : base(connectionString) {
        }

        public void Register(Guid company, PermissionRoleView permissionRole) {
            string sql = "INSERT INTO dbo.T_PermissionRole ([Id],[Company],[Name],[Remark],[Valid]) VALUES (@ID,@COMPANY,@NAME,@REMARK,@VALID)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", permissionRole.Id);
                dbOperator.AddParameter("COMPANY", company);
                dbOperator.AddParameter("NAME", permissionRole.Name);
                dbOperator.AddParameter("REMARK", permissionRole.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", permissionRole.Valid);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(PermissionRoleView permissionRole) {
            string sql = "UPDATE dbo.T_PermissionRole SET [Name]=@NAME,[Remark]=@REMARK,[Valid]=@VALID WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", permissionRole.Id);
                dbOperator.AddParameter("NAME", permissionRole.Name);
                dbOperator.AddParameter("REMARK", permissionRole.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", permissionRole.Valid);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Delete(Guid permissionRole) {
            string sql = "DELETE FROM dbo.T_PermissionRole WHERE [Id]=@ID;" +
                        "DELETE FROM dbo.[T_RolePermission] WHERE [Role]=@Id";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", permissionRole);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void UpdateStatus(Guid permissionRole, bool enabled) {
            string sql = "UPDATE dbo.T_PermissionRole SET [Valid]=@VALID WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", permissionRole);
                dbOperator.AddParameter("VALID", enabled);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Guid permissionRole, Website website, List<PermissionView.MenuView> menuViews) {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.T_RolePermission WHERE [Role]=@ROLEID AND EXISTS(SELECT NULL FROM dbo.T_Menu WHERE Id=Menu AND Website=@WEBSITE);");
            if(menuViews != null) {
                var menusSql = new StringBuilder();
                foreach(var menu in menuViews) {
                    if(menu != null && menu.Children != null) {
                        foreach(var subMenu in menu.Children) {
                            if(subMenu != null) {
                                menusSql.AppendFormat(" SELECT @ROLEID,'{0}' UNION ALL", subMenu.Id);
                            }
                        }
                    }
                }
                if(menusSql.Length > 0) {
                    sql.Append("INSERT INTO dbo.T_RolePermission ([Role],Menu)");
                    sql.Append(menusSql.ToString(), 0, menusSql.Length - 10);
                }
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ROLEID", permissionRole);
                dbOperator.AddParameter("WEBSITE", (byte)website);
                dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public PermissionRoleView QueryPermissionRole(Guid permissionRole) {
            var sql = "SELECT [Id],[Name],[Remark],[Valid] FROM dbo.T_PermissionRole WHERE Id=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", permissionRole);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        return constructPermissionRoleView(reader);
                    }
                }
            }
            return null;
        }

        public PermissionRoleView QueryPermissionRole(Guid company, string permissionRoleName) {
            var sql = "SELECT [Id],[Name],[Remark],[Valid] FROM dbo.T_PermissionRole WHERE Company=@COMPANY AND Name=@NAME";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", company);
                dbOperator.AddParameter("NAME", permissionRoleName);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    if(reader.Read()) {
                        return constructPermissionRoleView(reader);
                    }
                }
            }
            return null;
        }

        public IEnumerable<PermissionRoleView> QueryPermissionRoles(Guid company) {
            var result = new List<PermissionRoleView>();
            var sql = "SELECT Id,Name,Remark,Valid FROM dbo.T_PermissionRole WHERE Company=@COMPANY";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("COMPANY", company);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(constructPermissionRoleView(reader));
                    }
                }
            }
            return result;
        }

        public List<PermissionView.MenuView> QueryPremissionRolePermissions(Guid permissionRole, Website website) {
            var result = new List<DataTransferObject.Permission.PermissionView.MenuView>();
            var sql = "SELECT [Website],[MenuId],[MenuName],[SubMenuId],[SubMenuName] FROM dbo.V_PermissionRole" +
                      " WHERE RoleId=@ROLEID AND Website=@WEBSITE ORDER BY MenuSortLevel,MenuId,SubMenuSortLevel";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ROLEID", permissionRole);
                dbOperator.AddParameter("WEBSITE", (byte)website);
                DataTransferObject.Permission.PermissionView.MenuView menuView = null;
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var currentMenuId = reader.GetGuid(1);
                        if(menuView == null || menuView.Id != currentMenuId) {
                            menuView = new PermissionView.MenuView() {
                                Id = currentMenuId,
                                Name = reader.GetString(2),
                                Children = new List<PermissionView.SubMenuView>()
                            };
                            result.Add(menuView);
                        }
                        menuView.Children.Add(new PermissionView.SubMenuView() {
                            Id = reader.GetGuid(3),
                            Name = reader.GetString(4)
                        });
                    }
                }
            }
            return result;
        }

        public void UpdateUsersInRole(Guid permissionRole, IEnumerable<Guid> users) {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.T_UserPermission WHERE [Role]=@ROLE;");
            if(users.Count() > 0) {
                sql.Append("INSERT INTO dbo.T_UserPermission ([Role],[User])");
                foreach(var item in users) {
                    sql.AppendFormat(" SELECT @ROLE,'{0}' UNION ALL", item);
                }
                sql.Remove(sql.Length - 10, 10);
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ROLE", permissionRole);
                dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public void UpdateRolesOfUser(Guid user, IEnumerable<Guid> permissionRoles) {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.T_UserPermission WHERE [User]=@USER;");
            if(permissionRoles.Count() > 0) {
                sql.Append("INSERT INTO dbo.T_UserPermission ([Role],[User])");
                foreach(var item in permissionRoles) {
                    sql.AppendFormat(" SELECT '{0}',@USER UNION ALL", item);
                }
                sql.Remove(sql.Length - 10, 10);
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("USER", user);
                dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public IEnumerable<KeyValuePair<Guid, string>> QueryPermissionRolesOfUser(Guid user) {
            var result = new List<KeyValuePair<Guid, string>>();
            var sql = "SELECT Id,Name FROM dbo.T_PermissionRole INNER JOIN dbo.T_UserPermission ON Id=[Role] WHERE [User]=@USER";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("USER", user);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        result.Add(new KeyValuePair<Guid, string>(reader.GetGuid(0), reader.GetString(1)));
                    }
                }
            }
            return result;
        }

        private PermissionRoleView constructPermissionRoleView(DbDataReader reader) {
            return new PermissionRoleView(reader.GetGuid(0)) {
                Name = reader.GetString(1),
                Remark = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Valid = reader.GetBoolean(3)
            };
        }
    }
}