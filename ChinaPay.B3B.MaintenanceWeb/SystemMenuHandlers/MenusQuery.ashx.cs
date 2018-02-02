using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.MaintenanceWeb.SystemMenuHandlers
{
    /// <summary>
    /// MenusQuery 的摘要说明(菜单接口)
    /// </summary>
    public class MenusQuery : BaseHandler
    {
        /// <summary>
        /// 查询当前单位最大有效的菜单数据
        /// </summary>
        /// <param name="website">网站</param>
        public object QueryCompanyValidMenus(Website website)
        {
            UserRole userrole = GetUserRoles(this.CurrentCompany);
            return ConstructMenuViews(PermissionService.QueryPermissionOfCompany(this.CurrentCompany.CompanyId, userrole, website));
        }

        /// <summary>
        /// 查询有效的菜单数据
        /// </summary>
        /// <param name="website">网站</param>
        public object QueryValidMenus(Website website)
        {
            return ConstructMenuViews(SystemResourceService.QueryValidMenus(website));
        }
        /// <summary>
        /// 验证公司是否过期没有
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        private bool isExpired(DataTransferObject.Organization.CompanyDetailInfo company)
        {
            return company.PeriodStartOfUse > DateTime.Now || DateTime.Now > company.PeriodEndOfUse;
        }

        private UserRole getUserRole(Guid companyId)
        {
            var result = UserRole.Purchaser;
            var company = CompanyService.GetCompanyDetail(companyId);
            if (company.Audited && !isExpired(company))
            {
                switch (company.CompanyType)
                {
                    case CompanyType.Provider:
                        result = UserRole.Provider;
                        break;
                    case CompanyType.Purchaser:
                        result = UserRole.Purchaser;
                        break;
                    case CompanyType.Supplier:
                        result = UserRole.Supplier;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return result;
        }
        private UserRole GetUserRoles(CompanyDetailInfo company)
        {
            var result = UserRole.Purchaser;
            if (company.Audited && !isExpired(company))
            {
                switch (company.CompanyType)
                {
                    case CompanyType.Provider:
                        result = UserRole.Provider;
                        break;
                    case CompanyType.Purchaser:
                        result = UserRole.Purchaser;
                        break;
                    case CompanyType.Supplier:
                        result = UserRole.Supplier;
                        break;
                    case CompanyType.Platform:
                        result = UserRole.Platform;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return result;
        }

        /// <summary>
        /// 查询设置单位的最大额外有效菜单数据 （新增菜单数据源）
        /// </summary>
        /// <param name="website">网站</param>
        public object QueryCompanyMaxValidMenus(string id)
        {
            var purchaserPermission = PermissionService.QueryPermissionOfUserRole(UserRole.Purchaser,
                                                                                  Website.Transaction);
            var providerPermission = PermissionService.QueryPermissionOfUserRole(UserRole.Provider, Website.Transaction);
            var supplierPermission = PermissionService.QueryPermissionOfUserRole(UserRole.Supplier, Website.Transaction);

            var totalPermissions = Service.Permission.Domain.PermissionCollection.Union(purchaserPermission,
                                                                                        providerPermission,
                                                                                        supplierPermission);
            IEnumerable<PermissionView.MenuView> forbiddenPermission = null;
            switch (getUserRole(Guid.Parse(id)))
            {
                case UserRole.Provider:
                    forbiddenPermission = providerPermission;
                    break;
                case UserRole.Supplier:
                    forbiddenPermission = supplierPermission;
                    break;
                case UserRole.Purchaser:
                    forbiddenPermission = purchaserPermission;
                    break;
                default:
                    throw new NotSupportedException();
            }
            return ConstructMenuViews(Service.Permission.Domain.PermissionCollection.Subtract(totalPermissions, forbiddenPermission));
        }

        /// <summary>
        /// 得到公司的已有的额外权限
        /// </summary>
        /// <returns></returns>
        public object QueryCompantValidMenusOfCompanyId(string companyId)
        {
            return ConstructMenuViews(PermissionService.QueryCompanyAllowablePermission(Guid.Parse(companyId), Website.Transaction));
        }

        /// <summary>
        /// 查询设置单位的最大禁止有效菜单数据 （禁止菜单数据源）
        /// </summary>
        public object QueryCompanyMaxValidForbiddenMenus(string id)
        {
            return ConstructMenuViews(PermissionService.QueryPermissionOfUserRole(getUserRole(Guid.Parse(id)), Website.Transaction));
        }

        private object ConstructMenuViews(IEnumerable<PermissionView.MenuView> menus)
        {
            return new
                       {
                           name = "菜单",
                           open = true,
                           id = new Guid(),
                           pId = -1,
                           children = from item in menus
                                      select new
                                                 {
                                                     id = item.Id,
                                                     pId = new Guid(),
                                                     name = item.Name,
                                                     children = from i in item.Children
                                                                select new
                                                                           {
                                                                               id = i.Id,
                                                                               pId = item.Id,
                                                                               name = i.Name
                                                                           }
                                                 }
                       };
        }

        /// <summary>
        /// 得到公司的已有的禁止权限
        /// </summary>
        /// <returns></returns>
        public object QueryCompanyMaxValidForbiddenMenusOfCompanyId(string companyId)
        {
            return ConstructMenuViews(PermissionService.QueryCompanyForbiddenPermission(Guid.Parse(companyId), Website.Transaction));
        }

        /// <summary>
        /// 得到主菜单
        /// </summary>
        /// <returns></returns>
        public object QueryFristMenus(Website website)
        {
            var munus = from item in SystemResourceService.QueryMenus(website)
                        orderby item.Value.SortLevel
                        select new
                        {
                            id = item.Key,
                            pId = 0,
                            name = item.Value.Name,
                            meun_url = "",
                            isParent = true,
                            seq = item.Value.SortLevel,
                            remark = item.Value.Remark,
                            title = item.Value.Remark,
                            statu = item.Value.Valid,
                            display = item.Value.Display
                        };
            return new { name = "菜单", open = true, id = 0, pId = -1, children = munus, title = "所有" };
        }
        /// <summary>
        /// 得到子菜单
        /// </summary>
        /// <returns></returns>
        public object QuerySecondaryMenus(string id)
        {
            var list = from item in SystemResourceService.QuerySubMenus(Guid.Parse(id))
                       orderby item.Value.SortLevel
                       select new
                       {
                           id = item.Key,
                           pId = id,
                           name = item.Value.Name,
                           meun_url = item.Value.Address,
                           isParent = true,
                           seq = item.Value.SortLevel,
                           remark = item.Value.Remark,
                           title = item.Value.Remark,
                           statu = item.Value.Valid,
                           display = item.Value.Display
                       };
            return list;
        }
        /// <summary>
        /// 查询页面资源数据
        /// </summary>
        /// <param name="submenu">子菜单Id</param>
        public object QueryResources(string id)
        {
            var list = from item in SystemResourceService.QueryResources(Guid.Parse(id))
                       select new
                       {
                           id = item.Key,
                           pId = id,
                           name = item.Value.Name,
                           meun_url = item.Value.Address,
                           remark = item.Value.Remark,
                           title = item.Value.Remark,
                           statu = item.Value.Valid
                       };
            return list;
        }
        /// <summary>
        /// 添加主菜单
        /// </summary>
        /// <param name="website">网站</param>
        /// <param name="menuView">主菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void RegisterMenu(Website website, MenuView menuView)
        {
            SystemResourceService.RegisterMenu(website, menuView, this.CurrentUser.UserName); 
        }
        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="subMenuView">子菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void RegisterSubMenu(Website website, string pid, SubMenuView subMenuView)
        {
            SystemResourceService.RegisterSubMenu(website, Guid.Parse(pid), subMenuView, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 添加页面资源
        /// </summary>
        /// <param name="subMenu">子菜单id</param>
        /// <param name="resourceView">页面资源信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void RegisterResource(string subMenu, ResourceView resourceView)
        {
            SystemResourceService.RegisterResource(Guid.Parse(subMenu), resourceView, this.CurrentUser.UserName);
        }

        /// <summary>
        /// 删除主菜单
        /// </summary>
        /// <param name="menu">主菜单id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void DeleteMenu(string menu)
        {
            SystemResourceService.DeleteMenu(Guid.Parse(menu), this.CurrentUser.UserName);
        }
        /// <summary>
        /// 删除子菜单
        /// </summary>
        /// <param name="subMenu">子菜单id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void DeleteSubMenu(string subMenu)
        {
            SystemResourceService.DeleteMenu(Guid.Parse(subMenu), this.CurrentUser.UserName);
        }
        /// <summary>
        /// 删除页面资源
        /// </summary>
        /// <param name="resource">页面资源id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void DeleteResource(string resource)
        {
            SystemResourceService.DeleteResource(Guid.Parse(resource), this.CurrentUser.UserName);
        }
        /// <summary>
        /// 修改主菜单
        /// </summary>
        /// <param name="menuId">主菜单id</param>
        /// <param name="menuView">主菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void UpdateMenu(string menuId, MenuView menuView)
        {
            SystemResourceService.UpdateMenu(Guid.Parse(menuId), menuView, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 修改子菜单
        /// </summary>
        /// <param name="subMenuId">子菜单id</param>
        /// <param name="subMenuView">子菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void UpdateSubMenu(string subMenuId, SubMenuView subMenuView)
        {
            SystemResourceService.UpdateSubMenu(Guid.Parse(subMenuId), subMenuView, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 修改页面资源
        /// </summary>
        /// <param name="resouceId">页面资源id</param>
        /// <param name="resourceView">页面资源信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public void UpdateResource(string resouceId, ResourceView resourceView)
        {
            SystemResourceService.UpdateResource(Guid.Parse(resouceId), resourceView, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 保存单位的禁止权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="permissionView">权限信息</param>
        public void SaveCompanyForbiddenPermission(string company, List<PermissionView.MenuView> menuViews)
        {
            PermissionService.SaveCompanyForbiddenPermission(Guid.Parse(company), Website.Transaction, menuViews, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 保存单位的额外权限
        /// </summary>
        /// <param name="company">单位Id</param>
        /// <param name="permissionView">权限信息</param>
        public void SaveCompanyAllowablePermission(string company, List<PermissionView.MenuView> menuViews)
        {
            PermissionService.SaveCompanyAllowablePermission(Guid.Parse(company), Website.Transaction, menuViews, this.CurrentUser.UserName);
        }
    }
}