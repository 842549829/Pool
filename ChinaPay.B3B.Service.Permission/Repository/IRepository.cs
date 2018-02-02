using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission.Domain;

namespace ChinaPay.B3B.Service.Permission.Repository {
    interface ISystemResourceRepository {
        void Register(Website website, Menu menu);
        void Register(Website website, Guid menu, SubMenu subMenu);
        void Register(Guid subMenu, Resource resource);

        void Update(Menu menu);
        void Update(SubMenu subMenu);
        void Update(Resource resource);

        void DeleteMenu(Guid menu);
        void DeleteSubMenu(Guid subMenu);
        void DeleteResource(Guid resource);

        IEnumerable<KeyValuePair<Guid, MenuView>> QueryMenus(Website website);
        IEnumerable<KeyValuePair<Guid, SubMenuView>> QuerySubMenus(Guid menu);
        IEnumerable<KeyValuePair<Guid, ResourceView>> QueryResources(Guid submenu);
        IEnumerable<PermissionView.MenuView> QueryValidMenus(Website website);
    }
    interface IPermissionRoleRepository {
        void Register(Guid company, PermissionRoleView permissionRole);
        void Update(PermissionRoleView permissionRole);
        void Delete(Guid permissionRole);
        void UpdateStatus(Guid permissionRole, bool enabled);
        void Update(Guid permissionRole, Website website, List<PermissionView.MenuView> menuViews);

        PermissionRoleView QueryPermissionRole(Guid permissionRole);
        PermissionRoleView QueryPermissionRole(Guid company, string permissionRoleName);
        IEnumerable<PermissionRoleView> QueryPermissionRoles(Guid company);
        List<PermissionView.MenuView> QueryPremissionRolePermissions(Guid permissionRole, Website website);

        void UpdateUsersInRole(Guid permissionRole, IEnumerable<Guid> users);
        void UpdateRolesOfUser(Guid user, IEnumerable<Guid> permissionRoles);
        IEnumerable<KeyValuePair<Guid, string>> QueryPermissionRolesOfUser(Guid user);
    }
    interface IPermissionRepository {
        void SaveUserRolePermission(UserRole userRole, Website website, List<PermissionView.MenuView> menuViews);
        void SaveCompanyPermission(Guid company, Website website, List<PermissionView.MenuView> menuViews, PermissionType permissionType);

        List<PermissionView.MenuView> QueryWebsiteMenu(Website website);
        List<PermissionView.MenuView> QueryPermissionOfUserRole(Website website, UserRole userRole);
        List<PermissionView.MenuView> QueryCompanyPermission(Guid company, PermissionType permissionType, Website website);

        IEnumerable<Menu> QueryPermitMenusOfUserRole(UserRole userRole, Website website);
        IEnumerable<Menu> QueryCompanyPermitMenus(Guid company, Website website, PermissionType permissionType);

        IEnumerable<PermissionRole> QueryPermissionRolesOfUser(Guid user, Website website);
    }
}