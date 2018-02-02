using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums; 

namespace ChinaPay.B3B.Service.Permission {
    class LogHelper {
        public static void SaveRegisterMenuLog(Website website, Domain.Menu menu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Insert,
                operatorAccount,
                OperatorRole.Platform,
                menu.Id.ToString(),
                "添加菜单。" + menu.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveRegisterSubMenuLog(Guid menuId, Domain.SubMenu subMenu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Insert,
                operatorAccount,
                OperatorRole.Platform,
                subMenu.Id.ToString(),
                string.Format("添加子菜单。主菜单:{0} {1}", menuId, subMenu.ToString())
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveRegisterResourceLog(Guid submenuId, Domain.Resource resource, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Insert,
                operatorAccount,
                OperatorRole.Platform,
                resource.Id.ToString(),
                string.Format("添加页面资源。菜单:{0} {1}", submenuId, resource.ToString())
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateMenuLog(Domain.Menu menu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Update,
                operatorAccount,
                OperatorRole.Platform,
                menu.Id.ToString(),
                "修改菜单。" + menu.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateSubMenuLog(Domain.SubMenu subMenu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Update,
                operatorAccount,
                OperatorRole.Platform,
                subMenu.Id.ToString(),
                "修改子菜单。" + subMenu.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateResourceLog(Domain.Resource resource, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Update,
                operatorAccount,
                OperatorRole.Platform,
                resource.Id.ToString(),
                "修改页面资源。" + resource.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveDeleteMenuLog(Guid menu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Delete,
                operatorAccount,
                OperatorRole.Platform,
                menu.ToString(),
                "删除菜单。" + menu.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveDeleteSubMenuLog(Guid subMenu, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Delete,
                operatorAccount,
                OperatorRole.Platform,
                subMenu.ToString(),
                "删除子菜单。" + subMenu.ToString()
                );
            LogService.SaveOperationLog(log);
        }
        public static void SaveDeleteResourceLog(Guid resource, string operatorAccount) {
            var log = new Log.Domain.OperationLog(
                OperationModule.权限,
                OperationType.Delete,
                operatorAccount,
                OperatorRole.Platform,
                resource.ToString(),
                "删除页面资源。" + resource.ToString()
                );
            LogService.SaveOperationLog(log);
        }

        public static void SaveUpdateUserRolePermissionLog(UserRole userRole, Website website, string operatorAccount) {
            var logContent = string.Format("修改角色[{0}][{1}]权限", userRole.GetDescription(), website.GetDescription());
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.Platform,
                                                  userRole.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateCompanyForbidenPermissionLog(Guid company, Website website, string operatorAccount) {
            var logContent = string.Format("修改公司[{0}][{1}]的禁止权限", company, website.GetDescription());
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.Platform,
                                                  company.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateCompanyAllowablePermissionLog(Guid company, Website website, string operatorAccount) {
            var logContent = string.Format("修改公司[{0}][{1}]的额外权限", company, website.GetDescription());
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.Platform,
                                                  company.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }

        public static void SaveRegisterPermissionRoleLog(PermissionRoleView view, Guid company, string operatorAccount) {
            var logContent = string.Format("单位[{0}]添加权限角色。名称:{1} 备注:{2} 状态:{3}", company, view.Name, view.Remark, view.Valid);
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  view.Id.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdatePermissionRoleLog(PermissionRoleView view, string operatorAccount) {
            var logContent = string.Format("单位修改权限角色。名称:{0} 备注:{1} 状态:{2}", view.Name, view.Remark, view.Valid);
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  view.Id.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }
        public static void SaveDeletePermissionRoleLog(Guid permissionRole, string operatorAccount) {
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  permissionRole.ToString(),
                                                  "单位删除权限角色");
            LogService.SaveOperationLog(log);
        }
        public static void SaveEnablePermissionRoleLog(Guid permissionRole, string operatorAccount) {
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  permissionRole.ToString(),
                                                  "单位启用权限角色");
            LogService.SaveOperationLog(log);
        }
        public static void SaveDisablePermissionRoleLog(Guid permissionRole, string operatorAccount) {
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  permissionRole.ToString(),
                                                  "单位禁用权限角色");
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdatePermissionRolePermissionsLog(Guid permissionRole, Website website, string operatorAccount) {
            var logContent = string.Format("修改角色[{0}][{1}]权限", permissionRole.ToString(), website.GetDescription());
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.Platform,
                                                  permissionRole.ToString(),
                                                  logContent);
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateUsersInRoleLog(Guid permissionRole, IEnumerable<Guid> users, string operatorAccount) {
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  permissionRole.ToString(),
                                                  "单位修改权限角色内的用户");
            LogService.SaveOperationLog(log);
        }
        public static void SaveUpdateRolesOfUserLog(Guid user, IEnumerable<Guid> permissionRoles, string operatorAccount) {
            var log = new Log.Domain.OperationLog(OperationModule.权限,
                                                  OperationType.Update,
                                                  operatorAccount,
                                                  OperatorRole.User,
                                                  user.ToString(),
                                                  "单位修改用户的权限");
            LogService.SaveOperationLog(log);
        }
    }
}