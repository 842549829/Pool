using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Permission.Repository.SqlServer {
    class PermissionRepository : SqlServerRepository, IPermissionRepository {
        public PermissionRepository(string connectionString)
            : base(connectionString) {
        }

        public void SaveUserRolePermission(UserRole userRole, Website website, List<PermissionView.MenuView> menuViews) {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.T_UserRolePermission WHERE UserRole=@UserRole AND EXISTS(SELECT NULL FROM dbo.T_Menu WHERE Id=Menu AND Website=@Website);");
            if(menuViews != null) {
                var menusSql = new StringBuilder();
                foreach(var menu in menuViews) {
                    if(menu != null && menu.Children != null) {
                        foreach(var subMenu in menu.Children) {
                            if(subMenu != null) {
                                menusSql.AppendFormat(" SELECT @UserRole,'{0}' UNION ALL", subMenu.Id);
                            }
                        }
                    }
                }
                if(menusSql.Length > 0) {
                    sql.Append("INSERT INTO dbo.T_UserRolePermission (UserRole,Menu)");
                    sql.Append(menusSql.ToString(), 0, menusSql.Length - 10);
                }
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("UserRole", (byte)userRole);
                dbOperator.AddParameter("Website", (byte)website);
                dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public void SaveCompanyPermission(Guid company, Website website, List<PermissionView.MenuView> menuViews, Domain.PermissionType permissionType) {
            var sql = new StringBuilder();
            sql.Append("DELETE FROM dbo.T_CompanyPermission WHERE Company=@Company AND [Type]=@PermissionType AND EXISTS(SELECT NULL FROM dbo.T_Menu WHERE Id=Menu AND Website=@Website);");
            if(menuViews != null) {
                var menusSql = new StringBuilder();
                foreach(var menu in menuViews) {
                    if(menu != null && menu.Children != null) {
                        foreach(var subMenu in menu.Children) {
                            if(subMenu != null) {
                                menusSql.AppendFormat(" SELECT @Company,'{0}',@PermissionType UNION ALL", subMenu.Id);
                            }
                        }
                    }
                }
                if(menusSql.Length > 0) {
                    sql.Append("INSERT INTO dbo.T_CompanyPermission (Company,Menu,[Type])");
                    sql.Append(menusSql.ToString(), 0, menusSql.Length - 10);
                }
            }
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("PermissionType", (byte)permissionType);
                dbOperator.AddParameter("Website", (byte)website);
                dbOperator.ExecuteNonQuery(sql.ToString());
            }
        }

        public List<PermissionView.MenuView> QueryWebsiteMenu(Website website) {
            var result = new List<PermissionView.MenuView>();
            var sql = "SELECT [Website],[MenuId],[MenuName],[SubMenuId],[SubMenuName] FROM dbo.V_UserRolePermission ";
            sql += "WHERE WebSite=@WebSite ORDER BY MenuSortLevel,MenuId,SubMenuSortLevel,SubMenuId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("WebSite", (byte)website);
                PermissionView.MenuView menuView = null;
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

        public List<PermissionView.MenuView> QueryPermissionOfUserRole(Website website, UserRole userRole) {
            var result = new List<PermissionView.MenuView>();
            var sql = "SELECT [Website],[MenuId],[MenuName],[SubMenuId],[SubMenuName] FROM dbo.V_UserRolePermission ";
            sql += "WHERE UserRole&@UserRole=UserRole AND WebSite=@WebSite ORDER BY MenuSortLevel,MenuId,SubMenuSortLevel,SubMenuId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("UserRole", (byte)userRole);
                dbOperator.AddParameter("WebSite", (byte)website);
                PermissionView.MenuView menuView = null;
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

        public List<PermissionView.MenuView> QueryCompanyPermission(Guid company, Domain.PermissionType permissionType, Website website) {
            var result = new List<PermissionView.MenuView>();
            var sql = "SELECT [Website],[MenuId],[MenuName],[SubMenuId],[SubMenuName] FROM dbo.V_CompanyPermission" +
                      " WHERE Company=@Company AND PermissionType=@PermissionType AND WebSite=@WebSite ORDER BY MenuSortLevel,MenuId,SubMenuSortLevel,SubMenuId";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("PermissionType", (byte)permissionType);
                dbOperator.AddParameter("WebSite", (byte)website);
                PermissionView.MenuView menuView = null;
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

        public IEnumerable<Domain.Menu> QueryPermitMenusOfUserRole(UserRole userRole, Website website) {
            var sql = new StringBuilder();
            sql.Append("SELECT TMENU.Id AS MenuId,TMENU.Name AS MenuName,TMENU.Remark AS MenuRemark,TMENU.SortLevel AS MenuSortLevel,TMENU.Valid AS MenuValid,TMENU.Display as MenuDisplay,TSUBMENU.Display as SubMenuDisplay,");
            sql.Append("TSUBMENU.Id AS SubMenuId,TSUBMENU.Name AS SubMenuName,TSUBMENU.[Address] AS SubMenuAddress,TSUBMENU.Remark AS SubMenuRemark,");
            sql.Append("TSUBMENU.SortLevel AS SubMenuSortLevel,TSUBMENU.Valid AS SubMenuValid,");
            sql.Append("TRESOURCE.Id AS ResourceId,TRESOURCE.Name AS ResourceName,TRESOURCE.[Address] AS ResourceAddress,TRESOURCE.Remark AS ResourceRemark,TRESOURCE.Valid AS ResourceValid");
            sql.Append(" FROM dbo.T_UserRolePermission AS TURP");
            sql.Append(" INNER JOIN dbo.T_Menu TSUBMENU ON TURP.UserRole&@UserRole=TURP.UserRole AND TSUBMENU.Id=TURP.Menu AND TSUBMENU.Valid=1");
            sql.Append(" INNER JOIN dbo.T_Menu TMENU ON TMENU.Id=TSUBMENU.Parent AND TMENU.Valid=1 AND TMENU.Website=@Website");
            sql.Append(" LEFT JOIN dbo.T_Resource TRESOURCE ON TRESOURCE.Menu=TSUBMENU.Id AND TRESOURCE.Valid=1");
            sql.Append(" ORDER BY TMENU.SortLevel,TMENU.Id,TSUBMENU.SortLevel,TSUBMENU.Id");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("UserRole", (byte)userRole);
                dbOperator.AddParameter("Website", (byte)website);
                using(var reader = dbOperator.ExecuteReader(sql.ToString())) {
                    return constructMenus(reader);
                }
            }
        }

        public IEnumerable<Domain.Menu> QueryCompanyPermitMenus(Guid company, Website website, Domain.PermissionType permissionType) {
            var sql = new StringBuilder();
            sql.Append("SELECT TMENU.Id AS MenuId,TMENU.Name AS MenuName,TMENU.Remark AS MenuRemark,TMENU.SortLevel AS MenuSortLevel,TMENU.Valid AS MenuValid,TMENU.Display as MenuDisplay,TSUBMENU.Display as SubMenuDisplay,");
            sql.Append("TSUBMENU.Id AS SubMenuId,TSUBMENU.Name AS SubMenuName,TSUBMENU.[Address] AS SubMenuAddress,TSUBMENU.Remark AS SubMenuRemark,");
            sql.Append("TSUBMENU.SortLevel AS SubMenuSortLevel,TSUBMENU.Valid AS SubMenuValid,");
            sql.Append("TRESOURCE.Id AS ResourceId,TRESOURCE.Name AS ResourceName,TRESOURCE.[Address] AS ResourceAddress,TRESOURCE.Remark AS ResourceRemark,TRESOURCE.Valid AS ResourceValid");
            sql.Append(" FROM dbo.T_CompanyPermission AS TCOMPANY");
            sql.Append(" INNER JOIN dbo.T_Menu TSUBMENU ON TCOMPANY.Company=@Company AND TCOMPANY.[Type]=@PermissionType AND TSUBMENU.Id=TCOMPANY.Menu AND TSUBMENU.Valid=1");
            sql.Append(" INNER JOIN dbo.T_Menu TMENU ON TMENU.Id=TSUBMENU.Parent AND TMENU.Valid=1 AND TMENU.Website=@Website");
            sql.Append(" LEFT JOIN dbo.T_Resource TRESOURCE ON TRESOURCE.Menu=TSUBMENU.Id AND TRESOURCE.Valid=1");
            sql.Append(" ORDER BY TMENU.SortLevel,TMENU.Id,TSUBMENU.SortLevel,TSUBMENU.Id");
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("Company", company);
                dbOperator.AddParameter("PermissionType", (byte)permissionType);
                dbOperator.AddParameter("Website", (byte)website);
                using(var reader = dbOperator.ExecuteReader(sql.ToString())) {
                    return constructMenus(reader);
                }
            }
        }

        public IEnumerable<Domain.PermissionRole> QueryPermissionRolesOfUser(Guid user, Website website) {
            var result = new List<Domain.PermissionRole>();
            var sql = "dbo.P_QueryPermissionRolesOfUser";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("@iUserId", user);
                dbOperator.AddParameter("@iWebSite", (byte)website);
                using(var reader = dbOperator.ExecuteReader(sql, System.Data.CommandType.StoredProcedure)) {
                    Domain.PermissionRole role = null;
                    Domain.Menu menu = null;
                    Domain.SubMenu submenu = null;
                    while(reader.Read()) {
                        var currentRoleId = DataHelper.GetGuid(reader["RoleId"]);
                        if(role == null || role.Id != currentRoleId) {
                            role = new Domain.PermissionRole(currentRoleId) {
                                Name = DataHelper.GetString(reader["RoleName"]),
                                Remark = DataHelper.GetString(reader["RoleRemark"]),
                                Valid = true
                            };
                            menu = null;
                            result.Add(role);
                        }
                        var currentMenuId = DataHelper.GetGuid(reader["MenuId"]);
                        if(menu == null || menu.Id != currentMenuId) {
                            menu = new Domain.Menu(currentMenuId, DataHelper.GetString(reader["MenuName"])) {
                                Remark = DataHelper.GetString(reader["MenuRemark"]),
                                SortLevel = DataHelper.GetInteger(reader["MenuSortLevel"]),
                                Valid = true,
                                Display = DataHelper.GetBoolean(reader["MenuDisplay"])
                            };
                            submenu = null;
                            role.AppendMenu(menu);
                        }
                        var currentSubMenuId = DataHelper.GetGuid(reader["SubMenuId"]);
                        if(submenu == null || submenu.Id != currentSubMenuId) {
                            submenu = new Domain.SubMenu(currentSubMenuId, DataHelper.GetString(reader["SubMenuName"]), DataHelper.GetString(reader["SubMenuAddress"])) {
                                Remark = DataHelper.GetString(reader["SubMenuRemark"]),
                                SortLevel = DataHelper.GetInteger(reader["SubMenuSortLevel"]),
                                Valid = true,
                                Display = DataHelper.GetBoolean(reader["SubMenuDisplay"])
                            };
                            menu.AppendChild(submenu);
                        }
                        if(reader["ResourceId"] != DBNull.Value) {
                            submenu.AppendResource(new Domain.Resource(DataHelper.GetGuid(reader["ResourceId"])) {
                                Name = DataHelper.GetString(reader["ResourceName"]),
                                Address = DataHelper.GetString(reader["ResourceAddress"]),
                                Remark = DataHelper.GetString(reader["ResourceRemark"]),
                                Valid = true
                            });
                        }
                    }
                }
            }
            return result;
        }

        private IEnumerable<Domain.Menu> constructMenus(DbDataReader reader) {
            var result = new List<Domain.Menu>();
            Domain.Menu menu = null;
            Domain.SubMenu submenu = null;
            while(reader.Read()) {
                var currentMenuId = DataHelper.GetGuid(reader["MenuId"]);
                if(menu == null || menu.Id != currentMenuId) {
                    menu = new Domain.Menu(currentMenuId, DataHelper.GetString(reader["MenuName"])) {
                        Remark = DataHelper.GetString(reader["MenuRemark"]),
                        SortLevel = DataHelper.GetInteger(reader["MenuSortLevel"]),
                        Valid = DataHelper.GetBoolean(reader["MenuValid"]),
                        Display = reader["MenuDisplay"] == DBNull.Value || DataHelper.GetBoolean(reader["MenuDisplay"])
                    };
                    submenu = null;
                    result.Add(menu);
                }
                var currentSubMenuId = DataHelper.GetGuid(reader["SubMenuId"]);
                if(submenu == null || submenu.Id != currentSubMenuId) {
                    submenu = new Domain.SubMenu(currentSubMenuId, DataHelper.GetString(reader["SubMenuName"]), DataHelper.GetString(reader["SubMenuAddress"])) {
                        Remark = DataHelper.GetString(reader["SubMenuRemark"]),
                        SortLevel = DataHelper.GetInteger(reader["SubMenuSortLevel"]),
                        Valid = DataHelper.GetBoolean(reader["SubMenuValid"]),
                        Display = reader["SubMenuDisplay"] == DBNull.Value || DataHelper.GetBoolean(reader["SubMenuDisplay"])
                    };
                    menu.AppendChild(submenu);
                }
                if(reader["ResourceId"] != DBNull.Value) {
                    submenu.AppendResource(new Domain.Resource(DataHelper.GetGuid(reader["ResourceId"])) {
                        Name = DataHelper.GetString(reader["ResourceName"]),
                        Address = DataHelper.GetString(reader["ResourceAddress"]),
                        Remark = DataHelper.GetString(reader["ResourceRemark"]),
                        Valid = DataHelper.GetBoolean(reader["ResourceValid"])
                    });
                }
            }
            return result;
        }
    }
}