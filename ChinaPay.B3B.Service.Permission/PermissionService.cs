using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission.Domain;
using ChinaPay.B3B.Service.Permission.Repository;

namespace ChinaPay.B3B.Service.Permission {
    public static class PermissionService {
        /// <summary>
        /// 保存用户角色的权限
        /// </summary>
        /// <param name="userRole">用户角色</param>
        /// <param name="website">站点</param>
        /// <param name="menuViews">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void SaveUserRolePermission(UserRole userRole, Website website, List<PermissionView.MenuView> menuViews, string operatorAccount) {
            if(menuViews == null) throw new ArgumentNullException("menuViews");
            var repository = Factory.CreatePermissionRepository();
            repository.SaveUserRolePermission(userRole, website, menuViews);
            LogHelper.SaveUpdateUserRolePermissionLog(userRole, website, operatorAccount);
        }
        /// <summary>
        /// 保存单位的禁止权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="website">站点</param>
        /// <param name="menuViews">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void SaveCompanyForbiddenPermission(Guid company, Website website, List<PermissionView.MenuView> menuViews, string operatorAccount) {
            if(menuViews == null) throw new ArgumentNullException("menuViews");
            var repository = Factory.CreatePermissionRepository();
            repository.SaveCompanyPermission(company, website, menuViews, PermissionType.Forbidden);
            LogHelper.SaveUpdateCompanyForbidenPermissionLog(company, website, operatorAccount);
        }
        /// <summary>
        /// 保存单位的额外权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="website">站点</param>
        /// <param name="menuViews">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void SaveCompanyAllowablePermission(Guid company, Website website, List<PermissionView.MenuView> menuViews, string operatorAccount) {
            if(menuViews == null) throw new ArgumentNullException("menuViews");
            var repository = Factory.CreatePermissionRepository();
            repository.SaveCompanyPermission(company, website, menuViews, PermissionType.Allowable);
            LogHelper.SaveUpdateCompanyAllowablePermissionLog(company, website, operatorAccount);
        }
        /// <summary>
        /// 查询单位的最大权限
        /// </summary>
        /// <param name="company">单位信息</param>
        /// <param name="userRole">用户角色</param>
        /// <param name="website">站点</param>
        public static List<PermissionView.MenuView> QueryPermissionOfCompany(Guid company, UserRole userRole, Website website) {
            var userRolePermission = QueryPermissionOfUserRole(userRole, website);
            var additionalPermission = QueryCompanyAllowablePermission(company, website);
            var forbidenPermission = QueryCompanyForbiddenPermission(company, website);
            var permitPermission = PermissionCombiner.Combine(userRolePermission, additionalPermission);
            return PermissionCombiner.Subtract(permitPermission, forbidenPermission);
        }
        /// <summary>
        /// 查询单位的额外的权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="website">站点</param>
        public static List<PermissionView.MenuView> QueryCompanyAllowablePermission(Guid company, Website website) {
            var repository = Factory.CreatePermissionRepository();
            return repository.QueryCompanyPermission(company, PermissionType.Allowable, website);
        }
        /// <summary>
        /// 查询单位的禁止的权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="website">站点</param>
        public static List<PermissionView.MenuView> QueryCompanyForbiddenPermission(Guid company, Website website) {
            var repository = Factory.CreatePermissionRepository();
            return repository.QueryCompanyPermission(company, PermissionType.Forbidden, website);
        }
        /// <summary>
        /// 查询用户角色的权限
        /// </summary>
        /// <param name="userRole">用户角色</param>
        /// <param name="website">站点</param>
        public static List<PermissionView.MenuView> QueryPermissionOfUserRole(UserRole userRole, Website website) {
            var repository = Factory.CreatePermissionRepository();
            return repository.QueryPermissionOfUserRole(website, userRole);
        }
        /// <summary>
        /// 查询所有有效权限
        /// </summary>
        /// <param name="website">站点</param>
        public static List<PermissionView.MenuView> QueryAllValidPermission(Website website) {
            var repository = Factory.CreatePermissionRepository();
            return repository.QueryWebsiteMenu(website);
        }
        /// <summary>
        /// 查询用户的权限
        /// </summary>
        /// <param name="company">单位信息</param>
        /// <param name="userRole">用户角色</param>
        /// <param name="user">用户信息</param>
        /// <param name="isAdmin">是否是管理员</param>
        /// <param name="website">网站</param>
        public static PermissionCollection QueryPermissionOfUser(Guid company, UserRole userRole, Guid user, bool isAdmin, Website website) {
            if(isAdmin) {
                return QueryPermissionOfAdmin(company, userRole, website);
            } else {
                return QueryPermissionOfCommonUser(company, userRole, user, website);
            }
        }
        private static PermissionCollection QueryPermissionOfCommonUser(Guid company, UserRole userRole, Guid user, Website website) {
            var repository = Factory.CreatePermissionRepository();
            var userPermissionRoles = repository.QueryPermissionRolesOfUser(user, website);
            var userPermissions = PermissionCollection.Union(userPermissionRoles);
            var companyPermissions = getCompanyPermissions(company, userRole, website);
            var finalPermissions = PermissionCollection.Intersact(userPermissions, companyPermissions);
            return new PermissionCollection(finalPermissions);
        }
        private static PermissionCollection QueryPermissionOfAdmin(Guid company, UserRole userRole, Website website) {
            return new PermissionCollection(getCompanyPermissions(company, userRole, website));
        }
        private static IEnumerable<Menu> getCompanyPermissions(Guid company, UserRole userRole, Website website) {
            var repository = Factory.CreatePermissionRepository();
            var userRolePermitMenus = repository.QueryPermitMenusOfUserRole(userRole, website);
            var allowableMenus = repository.QueryCompanyPermitMenus(company, website, PermissionType.Allowable);
            var forbiddenMenus = repository.QueryCompanyPermitMenus(company, website, PermissionType.Forbidden);
            var permitMenus = PermissionCollection.Union(userRolePermitMenus, allowableMenus);
            return PermissionCollection.Subtract(permitMenus, forbiddenMenus);
        }
    }
}