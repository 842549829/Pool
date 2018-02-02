using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Permission;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Permission;

namespace ChinaPay.B3B.TransactionWeb.PermissionHandlers
{
    /// <summary>
    /// PermissionQuery 的摘要说明
    /// </summary>
    public class PermissionQuery : BaseHandler
    {
        /// <summary>
        /// 验证角色名称是否存在
        /// </summary>
        /// <param name="RoleName">角色名</param>
        /// <returns>存在返回true 不存在返回false</returns>
        public bool IsExistsRoleName(string RoleName)
        {
            return true;
        }

        /// <summary>
        /// 查询角色列表信息（默认的角色）
        /// </summary>
        /// <returns></returns>
        public object QueryRoleDefaultList()
        {
            var roleNames = Enum.GetNames(typeof(UserRole));
            var roles = roleNames.Select(name => (UserRole) Enum.Parse(typeof (UserRole), name)).ToList();
            return from item in roles
                   select new
                   {
                       Id = item.GetHashCode(),
                       Name = item.ToString(),
                       Description = item.GetDescription().Split('|')
                   };
        }
        /// <summary>
        /// 保存用户角色的权限
        /// </summary>
        public void SaveUserRolePermission(UserRole userRole, Website website, List<PermissionView.MenuView> strs)
        {
            PermissionService.SaveUserRolePermission(userRole, website, strs, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 查询用户角色的权限
        /// </summary>
        /// <param name="userRole">用户角色</param>
        public List<PermissionView.MenuView> QueryPermissionOfUserRole(UserRole userRole, Website website)
        {
            return PermissionService.QueryPermissionOfUserRole(userRole, website);
        }



        /// <summary>
        /// 保存单位的禁止权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="permissionView">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void SaveCompanyForbiddenPermission(string company, Website website, List<PermissionView.MenuView> menuViews)
        {
            PermissionService.SaveCompanyForbiddenPermission(Guid.Parse(company), website, menuViews, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 保存单位的额外权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="permissionView">权限信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void SaveCompanyAllowablePermission(string company, Website website, List<PermissionView.MenuView> menuViews, string operatorAccount)
        {
            PermissionService.SaveCompanyAllowablePermission(Guid.Parse(company), website, menuViews, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 查询单位的最大权限
        /// </summary>
        /// <param name="company">单位信息</param>
        public List<PermissionView.MenuView> QueryPermissionOfCompany(string company, UserRole userRole, Website website)
        {
            return PermissionService.QueryPermissionOfCompany(Guid.Parse(company), userRole, website);

        }
        /// <summary>
        /// 查询单位的额外的权限
        /// </summary>
        /// <param name="company">单位Id</param>
        public List<PermissionView.MenuView> QueryCompanyAllowablePermission(string company, Website website)
        {
            return PermissionService.QueryCompanyAllowablePermission(Guid.Parse(company), website);
        }
        /// <summary>
        /// 查询单位的禁止的权限
        /// </summary>
        /// <param name="company">单位Id</param>
        public List<PermissionView.MenuView> QueryCompanyForbiddenPermission(string company, Website website)
        {
            return PermissionService.QueryCompanyForbiddenPermission(Guid.Parse(company), website);
        }
    }
}