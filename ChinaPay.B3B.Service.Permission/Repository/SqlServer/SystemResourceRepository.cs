using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.Repository;
using ChinaPay.DataAccess;

namespace ChinaPay.B3B.Service.Permission.Repository.SqlServer {
    class SystemResourceRepository : SqlServerRepository, ISystemResourceRepository {
        public SystemResourceRepository(string connectionString)
            : base(connectionString) {
        }

        public void Register(Website website, Domain.Menu menu) {
            var sql = "INSERT INTO dbo.T_Menu ([Id],[Name],[Address],[SortLevel],[Depth],[Remark],[Valid],Display,[Parent],[Website])" +
                " VALUES (@ID,@NAME,@ADDRESS,@SORTLEVEL,@DEPTH,@REMARK,@VALID,@Display,@PARENT,@WEBSITE)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", menu.Id);
                dbOperator.AddParameter("NAME", menu.Name);
                dbOperator.AddParameter("ADDRESS", DBNull.Value);
                dbOperator.AddParameter("SORTLEVEL", menu.SortLevel);
                dbOperator.AddParameter("DEPTH", 1);
                dbOperator.AddParameter("REMARK", menu.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", menu.Valid);
                dbOperator.AddParameter("Display", menu.Display);
                dbOperator.AddParameter("PARENT", DBNull.Value);
                dbOperator.AddParameter("WEBSITE", (byte)website);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Register(Website website, Guid menuId, Domain.SubMenu subMenu) {
            var sql = "INSERT INTO dbo.T_Menu ([Id],[Name],[Address],[SortLevel],[Depth],[Remark],[Valid],Display,[Parent],[Website])" +
                " VALUES (@ID,@NAME,@ADDRESS,@SORTLEVEL,@DEPTH,@REMARK,@VALID,@Display,@PARENT,@WEBSITE)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", subMenu.Id);
                dbOperator.AddParameter("NAME", subMenu.Name);
                dbOperator.AddParameter("ADDRESS", subMenu.Address ?? string.Empty);
                dbOperator.AddParameter("SORTLEVEL", subMenu.SortLevel);
                dbOperator.AddParameter("DEPTH", 2);
                dbOperator.AddParameter("REMARK", subMenu.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", subMenu.Valid);
                dbOperator.AddParameter("Display", subMenu.Display);
                dbOperator.AddParameter("PARENT", menuId);
                dbOperator.AddParameter("WEBSITE", (byte)website);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Register(Guid subMenuId, Domain.Resource resource) {
            var sql = "INSERT INTO dbo.T_Resource ([Id],[Name],[Address],[Remark],[Valid],[Menu]) VALUES (@ID,@NAME,@ADDRESS,@REMARK,@VALID,@MENU)";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", resource.Id);
                dbOperator.AddParameter("NAME", resource.Name);
                dbOperator.AddParameter("ADDRESS", resource.Address ?? string.Empty);
                dbOperator.AddParameter("REMARK", resource.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", resource.Valid);
                dbOperator.AddParameter("MENU", subMenuId);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.Menu menu) {
            var sql = "UPDATE dbo.T_Menu SET [Name]=@NAME,[Address]=@ADDRESS,[SortLevel]=@SORTLEVEL,[Remark]=@REMARK,[Valid]=@VALID,Display=@Display WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", menu.Id);
                dbOperator.AddParameter("NAME", menu.Name);
                dbOperator.AddParameter("ADDRESS", DBNull.Value);
                dbOperator.AddParameter("SORTLEVEL", menu.SortLevel);
                dbOperator.AddParameter("REMARK", menu.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", menu.Valid);
                dbOperator.AddParameter("Display", menu.Display);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.SubMenu subMenu) {
            var sql = "UPDATE dbo.T_Menu SET [Name]=@NAME,[Address]=@ADDRESS,[SortLevel]=@SORTLEVEL,[Remark]=@REMARK,[Valid]=@VALID,Display=@Display WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", subMenu.Id);
                dbOperator.AddParameter("NAME", subMenu.Name);
                dbOperator.AddParameter("ADDRESS", subMenu.Address ?? string.Empty);
                dbOperator.AddParameter("SORTLEVEL", subMenu.SortLevel);
                dbOperator.AddParameter("REMARK", subMenu.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", subMenu.Valid);
                dbOperator.AddParameter("Display", subMenu.Display);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void Update(Domain.Resource resource) {
            string sql = "UPDATE dbo.T_Resource SET [Name]=@NAME,[Address]=@ADDRESS,[Remark]=@REMARK,[Valid]=@VALID WHERE [Id]=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", resource.Id);
                dbOperator.AddParameter("NAME", resource.Name);
                dbOperator.AddParameter("ADDRESS", resource.Address ?? string.Empty);
                dbOperator.AddParameter("REMARK", resource.Remark ?? string.Empty);
                dbOperator.AddParameter("VALID", resource.Valid);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteMenu(Guid menu) {
            string sql = "DELETE FROM dbo.T_Menu WHERE Id=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", menu);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteSubMenu(Guid subMenu) {
            string sql = "DELETE FROM dbo.T_Menu WHERE Id=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", subMenu);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public void DeleteResource(Guid resource) {
            string sql = "DELETE FROM dbo.T_Resource WHERE Id=@ID";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("ID", resource);
                dbOperator.ExecuteNonQuery(sql);
            }
        }

        public IEnumerable<KeyValuePair<Guid, MenuView>> QueryMenus(Website website) {
            var result = new List<KeyValuePair<Guid, MenuView>>();
            var sql = "SELECT Id,Name,SortLevel,Remark,Valid,Display FROM dbo.T_Menu WHERE Website=@WEBSITE AND Depth=1";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("WEBSITE", (byte)website);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var menuId = reader.GetGuid(0);
                        var menuView = new MenuView() {
                            Name = reader.GetString(1),
                            SortLevel = reader.GetInt32(2),
                            Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Valid = reader.GetBoolean(4),
                            Display = reader.IsDBNull(5) || reader.GetBoolean(5)
                        };
                        result.Add(new KeyValuePair<Guid,MenuView>(menuId, menuView));
                    }
                }
            }
            return result;
        }

        public IEnumerable<KeyValuePair<Guid, SubMenuView>> QuerySubMenus(Guid menu) {
            var result = new List<KeyValuePair<Guid, SubMenuView>>();
            var sql = "SELECT Id,Name,SortLevel,Remark,Valid,Display,[Address] FROM dbo.T_Menu WHERE Parent=@PARENT AND Depth=2";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("PARENT", menu);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var subMenuId = reader.GetGuid(0);
                        var subMenuView = new SubMenuView() {
                            Name = reader.GetString(1),
                            SortLevel = reader.GetInt32(2),
                            Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Valid = reader.GetBoolean(4),
                            Display = reader.IsDBNull(5) || reader.GetBoolean(5),
                            Address = reader.IsDBNull(6) ? string.Empty: reader.GetString(6)
                        };
                        result.Add(new KeyValuePair<Guid, SubMenuView>(subMenuId, subMenuView));
                    }
                }
            }
            return result;
        }
        
        public IEnumerable<KeyValuePair<Guid, ResourceView>> QueryResources(Guid submenu) {
            var result = new List<KeyValuePair<Guid, ResourceView>>();
            var sql = "SELECT Id,Name,[Address],Remark,Valid FROM dbo.T_Resource WHERE Menu=@MENU";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("MENU", submenu);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    while(reader.Read()) {
                        var resourceId = reader.GetGuid(0);
                        var resourceView = new ResourceView() {
                            Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Address = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Remark = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Valid = reader.GetBoolean(4)
                        };
                        result.Add(new KeyValuePair<Guid, ResourceView>(resourceId, resourceView));
                    }
                }
            }
            return result;
        }

        public IEnumerable<PermissionView.MenuView> QueryValidMenus(Website website) {
            var result = new List<PermissionView.MenuView>();
            var sql = "SELECT TFIRST.Id,TFIRST.Name,TSECOND.Id,TSECOND.Name" +
                " FROM dbo.T_Menu TFIRST LEFT JOIN dbo.T_Menu TSECOND ON TFIRST.Id=TSECOND.Parent AND TSECOND.Depth=2 AND TSECOND.Valid=1" +
                " WHERE TFIRST.Depth=1 AND TFIRST.Valid=1 AND TFIRST.Website=@WEBSITE" +
                " ORDER BY TFIRST.SortLevel,TFIRST.Id,TSECOND.SortLevel";
            using(var dbOperator = new DbOperator(Provider, ConnectionString)) {
                dbOperator.AddParameter("WEBSITE", (byte)website);
                using(var reader = dbOperator.ExecuteReader(sql)) {
                    PermissionView.MenuView menu = null;
                    while(reader.Read()) {
                        var currentMenuId = reader.GetGuid(0);
                        if(menu == null || menu.Id != currentMenuId) {
                            menu = new PermissionView.MenuView() {
                                Id = currentMenuId,
                                Name = reader.GetString(1),
                                Children = new List<PermissionView.SubMenuView>()
                            };
                            result.Add(menu);
                        }
                        if(!reader.IsDBNull(2)) {
                            menu.Children.Add(new PermissionView.SubMenuView() {
                                Id = reader.GetGuid(2),
                                Name = reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}