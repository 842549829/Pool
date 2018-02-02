using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Permission;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Permission;

namespace ChinaPay.B3B.TransactionWeb.PermissionHandlers
{
    /// <summary>
    /// PermissionRoleQuery 的摘要说明
    /// </summary>
    public class PermissionRoleQuery : BaseHandler
    {
        /// <summary>
        /// 查询公司下的角色权限角色(列表)
        /// </summary>
        public object QueryPermissionRoles(string company)
        {
            return from item in PermissionRoleService.QueryPermissionRoles(Guid.Parse(company))
                   select new
                   {
                       RoleName = item.Name,
                       Statu = item.Valid == true ? "有效" : "无效",
                       Remark = item.Remark,
                       Id = item.Id,
                       Href_Statue = item.Valid == true ? "<a href='#'>禁用</a>" : "<a href='#'>启用</a>"
                   };
        }
        /// <summary>
        /// 查询权限角色
        /// </summary>
        public object QueryPermissionRole(string permissionRole)
        {
            PermissionRoleView return_value = null;
            try
            {
                return_value = PermissionRoleService.QueryPermissionRole(Guid.Parse(permissionRole));
            }
            catch (Exception)
            {
                throw new Exception("传递的参数有误,查询不到数据");
            }

            if (return_value == null)
            {
                throw new Exception("传递的参数有误,查询不到数据");
            }
            return return_value;
        }
        /// <summary>
        /// 查询用户所有的权限角色
        /// 仅包括简单信息
        /// </summary>
        public object QueryPerssionRolesOfUser(string user)
        {
            return from item in PermissionRoleService.QueryPerssionRolesOfUser(Guid.Parse(user))
                   select new
                   {
                       id = item.Key,
                       name = item.Value
                   };
        }
        /// <summary>
        /// 查询权限角色
        /// </summary>
        public object QueryPermissionRoleInfo(string permissionRoleName)
        {
            return PermissionRoleService.QueryPermissionRole(this.CurrentCompany.CompanyId, permissionRoleName);
        }
        /// <summary>
        /// 修改权限角色里的成员用户
        /// </summary>
        public void UpdateUsersInRole(string permissionRole, IEnumerable<Guid> users)
        {
            try
            {
                PermissionRoleService.UpdateUsersInRole(Guid.Parse(permissionRole), users, this.CurrentUser.UserName);
            }
            catch (ArgumentNullException)
            {
                throw new Exception("角色名不能为空"); ;
            }
            catch (Exception)
            {
                throw new Exception("修改失败");
            }

        }
        /// <summary>
        /// 修改用户拥有的权限角色
        /// </summary>
        public void UpdateRolesOfUser(string user, IEnumerable<Guid> permissionRoles)
        {
            PermissionRoleService.UpdateRolesOfUser(Guid.Parse(user), permissionRoles, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 删除权限角色
        /// </summary>
        public void DeletePermissionRole(string permissionRole)
        {
            PermissionRoleService.DeletePermissionRole(Guid.Parse(permissionRole), this.CurrentUser.UserName);
        }
        /// <summary>
        /// 启用权限角色
        /// </summary>
        public void EnablePermissionRole(string permissionRole)
        {
            PermissionRoleService.EnablePermissionRole(Guid.Parse(permissionRole), this.CurrentUser.UserName);
        }
        /// <summary>
        /// 禁用权限角色
        /// </summary>
        public void DisablePermissionRole(string permissionRole)
        {
            PermissionRoleService.DisablePermissionRole(Guid.Parse(permissionRole), this.CurrentUser.UserName);
        }

        /// <summary>
        /// 添加权限角色
        /// </summary>
        /// <param name="permissionRoleView">角色信息</param>
        /// <param name="company">当前单位Id</param>
        public void RegisterPermissionRole(PermissionRoleView permissionRoleView)
        {
            if (!this.IsValidPermissionRoleNameOfRegister(permissionRoleView.Name))
            {
                throw new Exception("角色名称重复");
            }
            string operatorAccount = this.CurrentUser.UserName;
            PermissionRoleService.RegisterPermissionRole(permissionRoleView, this.CurrentCompany.CompanyId, operatorAccount);
        }

        /// <summary>
        /// 修改权限角色
        /// </summary>
        /// <param name="permissionRoleView">角色信息</param>
        public void UpdatePermissionRole(string id, PermissionRoleView permissionRoleView)
        {
            if (!this.IsValidPermissionRoleNameOfModify(id, permissionRoleView.Name))
            {
                throw new Exception("角色名称重复");
            }
            permissionRoleView = new PermissionRoleView(Guid.Parse(id))
            {
                Name = permissionRoleView.Name,
                Remark = permissionRoleView.Remark,
                Valid = permissionRoleView.Valid
            };
            string operatorAccount = this.CurrentUser.UserName;
            PermissionRoleService.UpdatePermissionRole(permissionRoleView, operatorAccount);
        }

        /// <summary>
        /// 检查权限角色名是否有效
        /// 用于新增时
        /// </summary>
        /// <param name="company">当前单位Id</param>
        /// <param name="permissionRoleName">权限角色名</param>
        public bool IsValidPermissionRoleNameOfRegister(string permissionRoleName)
        {
            return PermissionRoleService.IsValidPermissionRoleName(this.CurrentCompany.CompanyId,permissionRoleName);
        }
        /// <summary>
        /// 检查权限角色名是否有效
        /// 用于修改时
        /// </summary>
        /// <param name="company">当前单位Id</param>
        /// <param name="permissionRole">当前权限角色Id</param>
        /// <param name="permissionRoleName">权限角色名</param>
        public bool IsValidPermissionRoleNameOfModify(string permissionRole, string permissionRoleName)
        {
            return PermissionRoleService.IsValidPermissionRoleName(this.CurrentCompany.CompanyId, Guid.Parse(permissionRole), permissionRoleName);
        }

        /// <summary>
        /// 修改权限角色的权限
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="website">网站</param>
        /// <param name="menuViews">权限信息</param>
        public void UpdatePermissionRolePermissions(string permissionRole, Website website, List<PermissionView.MenuView> menuViews)
        {
            string operatorAccount = this.CurrentUser.UserName;
            PermissionRoleService.UpdatePermissionRolePermissions(Guid.Parse(permissionRole), website, menuViews, operatorAccount);
        }
        /// <summary>
        /// 查询权限角色的权限
        /// </summary>
        /// <param name="permissionRole">权限角色Id</param>
        /// <param name="website">网站</param>
        public List<PermissionView.MenuView> QueryPremissionRolePermissions(string permissionRole, Website website)
        {
                return PermissionRoleService.QueryPremissionRolePermissions(Guid.Parse(permissionRole), website);
        }
    }
}