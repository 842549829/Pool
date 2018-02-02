using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission.Repository;

namespace ChinaPay.B3B.Service.Permission {
    /// <summary>
    /// 权限角色服务
    /// </summary>
    public static class PermissionRoleService {
        /// <summary>
        /// 检查权限角色名是否有效
        /// 用于新增时
        /// </summary>
        /// <param name="company">当前单位Id</param>
        /// <param name="permissionRoleName">权限角色名</param>
        public static bool IsValidPermissionRoleName(Guid company, string permissionRoleName) {
            var permissionRole = QueryPermissionRole(company, permissionRoleName);
            return permissionRole == null;
        }
        /// <summary>
        /// 检查权限角色名是否有效
        /// 用于修改时
        /// </summary>
        /// <param name="company">当前单位Id</param>
        /// <param name="permissionRole">当前权限角色Id</param>
        /// <param name="permissionRoleName">权限角色名</param>
        public static bool IsValidPermissionRoleName(Guid company, Guid permissionRole, string permissionRoleName) {
            var permissionRoleData = QueryPermissionRole(company, permissionRoleName);
            return permissionRoleData == null || permissionRoleData.Id == permissionRole;
        }
        /// <summary>
        /// 添加权限角色
        /// </summary>
        /// <param name="permissionRoleView">角色信息</param>
        /// <param name="company">当前单位Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void RegisterPermissionRole(PermissionRoleView permissionRoleView, Guid company, string operatorAccount) {
            if(permissionRoleView == null)
                throw new ArgumentNullException("permissionRoleView");
            permissionRoleView.Validate();
            var repository = Factory.CreatePermissionRoleRepository();
            repository.Register(company, permissionRoleView);
            LogHelper.SaveRegisterPermissionRoleLog(permissionRoleView, company, operatorAccount);
        }
        /// <summary>
        /// 修改权限角色
        /// </summary>
        /// <param name="permissionRoleView">角色信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdatePermissionRole(PermissionRoleView permissionRoleView, string operatorAccount) {
            if(permissionRoleView == null)
                throw new ArgumentNullException("permissionRoleView");
            permissionRoleView.Validate();
            var repository = Factory.CreatePermissionRoleRepository();
            repository.Update(permissionRoleView);
            LogHelper.SaveUpdatePermissionRoleLog(permissionRoleView, operatorAccount);
        }
        /// <summary>
        /// 删除权限角色
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeletePermissionRole(Guid permissionRole, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.Delete(permissionRole);
            LogHelper.SaveDeletePermissionRoleLog(permissionRole, operatorAccount);
        }
        /// <summary>
        /// 启用权限角色
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void EnablePermissionRole(Guid permissionRole, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.UpdateStatus(permissionRole, true);
            LogHelper.SaveEnablePermissionRoleLog(permissionRole, operatorAccount);
        }
        /// <summary>
        /// 禁用权限角色
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DisablePermissionRole(Guid permissionRole, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.UpdateStatus(permissionRole, false);
            LogHelper.SaveDisablePermissionRoleLog(permissionRole, operatorAccount);
        }
        /// <summary>
        /// 修改权限角色的权限
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="website">网站</param>
        /// <param name="menuViews">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdatePermissionRolePermissions(Guid permissionRole, Website website, List<PermissionView.MenuView> menuViews, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.Update(permissionRole, website, menuViews);
            LogHelper.SaveUpdatePermissionRolePermissionsLog(permissionRole, website, operatorAccount);
        }

        /// <summary>
        /// 查询单位下权限角色列表
        /// </summary>
        /// <param name="company">单位Id</param>
        public static IEnumerable<PermissionRoleView> QueryPermissionRoles(Guid company) {
            var repository = Factory.CreatePermissionRoleRepository();
            return repository.QueryPermissionRoles(company);
        }
        /// <summary>
        /// 查询权限角色
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        public static PermissionRoleView QueryPermissionRole(Guid permissionRole) {
            var repository = Factory.CreatePermissionRoleRepository();
            return repository.QueryPermissionRole(permissionRole);
        }
        /// <summary>
        /// 查询权限角色
        /// </summary>
        /// <param name="company">当前单位Id</param>
        /// <param name="permissionRoleName">权限角色名</param>
        public static PermissionRoleView QueryPermissionRole(Guid company, string permissionRoleName) {
            var repository = Factory.CreatePermissionRoleRepository();
            return repository.QueryPermissionRole(company, permissionRoleName);
        }
        /// <summary>
        /// 查询权限角色的权限
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="website">网站</param>
        public static List<PermissionView.MenuView> QueryPremissionRolePermissions(Guid permissionRole, Website website) {
            var repository = Factory.CreatePermissionRoleRepository();
            return repository.QueryPremissionRolePermissions(permissionRole, website);
        }

        /// <summary>
        /// 修改权限角色里的成员用户
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="users">成员用户Id集合</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateUsersInRole(Guid permissionRole, IEnumerable<Guid> users, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.UpdateUsersInRole(permissionRole, users);
            LogHelper.SaveUpdateUsersInRoleLog(permissionRole, users, operatorAccount);
        }
        /// <summary>
        /// 修改用户拥有的权限角色
        /// </summary>
        /// <param name="user">用户Id</param>
        /// <param name="permissionRoles">权限角色Id集合</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateRolesOfUser(Guid user, IEnumerable<Guid> permissionRoles, string operatorAccount) {
            var repository = Factory.CreatePermissionRoleRepository();
            repository.UpdateRolesOfUser(user, permissionRoles);
            LogHelper.SaveUpdateRolesOfUserLog(user, permissionRoles, operatorAccount);
        }
        /// <summary>
        /// 查询用户所有的权限角色
        /// 仅包括简单信息
        /// </summary>
        /// <param name="user">用户Id</param>
        /// <returns>KeyValuePair  Guid:权限角色Id；string：权限角色名称</returns>
        public static IEnumerable<KeyValuePair<Guid, string>> QueryPerssionRolesOfUser(Guid user) {
            var repository = Factory.CreatePermissionRoleRepository();
            return repository.QueryPermissionRolesOfUser(user);
        }
    }
}