using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Permission;
using ChinaPay.B3B.Service.Permission.Domain;
using ChinaPay.B3B.Service.Permission.Repository;

namespace ChinaPay.B3B.Service.Permission {
    public static class SystemResourceService {
        /// <summary>
        /// 添加主菜单
        /// </summary>
        /// <param name="website">网站</param>
        /// <param name="menuView">主菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static Menu RegisterMenu(Website website, MenuView menuView, string operatorAccount) {
            var menu = Menu.GetMenu(menuView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Register(website, menu);
            LogHelper.SaveRegisterMenuLog(website, menu, operatorAccount);
            return menu;
        }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="website">网站</param>
        /// <param name="menu">主菜单id</param>
        /// <param name="subMenuView">子菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static SubMenu RegisterSubMenu(Website website, Guid menu, SubMenuView subMenuView, string operatorAccount) {
            var subMenu = SubMenu.GetSubMenu(subMenuView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Register(website, menu, subMenu);
            LogHelper.SaveRegisterSubMenuLog(menu, subMenu, operatorAccount);
            return subMenu;
        }
        /// <summary>
        /// 添加页面资源
        /// </summary>
        /// <param name="subMenu">子菜单id</param>
        /// <param name="resourceView">页面资源信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static Resource RegisterResource(Guid subMenu, ResourceView resourceView, string operatorAccount) {
            var resource = Resource.GetResource(resourceView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Register(subMenu, resource);
            LogHelper.SaveRegisterResourceLog(subMenu, resource, operatorAccount);
            return resource;
        }

        /// <summary>
        /// 修改主菜单
        /// </summary>
        /// <param name="menuId">主菜单id</param>
        /// <param name="menuView">主菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateMenu(Guid menuId, MenuView menuView, string operatorAccount) {
            var menu = Menu.GetMenu(menuId, menuView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Update(menu);
            LogHelper.SaveUpdateMenuLog(menu, operatorAccount);
        }
        /// <summary>
        /// 修改子菜单
        /// </summary>
        /// <param name="subMenuId">子菜单id</param>
        /// <param name="subMenuView">子菜单信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateSubMenu(Guid subMenuId, SubMenuView subMenuView, string operatorAccount) {
            var subMenu = SubMenu.GetSubMenu(subMenuId, subMenuView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Update(subMenu);
            LogHelper.SaveUpdateSubMenuLog(subMenu, operatorAccount);
        }
        /// <summary>
        /// 修改页面资源
        /// </summary>
        /// <param name="resouceId">页面资源id</param>
        /// <param name="resourceView">页面资源信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateResource(Guid resouceId, ResourceView resourceView, string operatorAccount) {
            var resource = Resource.GetResource(resouceId, resourceView);
            var repository = Factory.CreateSystemResourceRepository();
            repository.Update(resource);
            LogHelper.SaveUpdateResourceLog(resource, operatorAccount);
        }

        /// <summary>
        /// 删除主菜单
        /// </summary>
        /// <param name="menu">主菜单id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteMenu(Guid menu, string operatorAccount) {
            var repository = Factory.CreateSystemResourceRepository();
            repository.DeleteMenu(menu);
            LogHelper.SaveDeleteMenuLog(menu, operatorAccount);
        }
        /// <summary>
        /// 删除子菜单
        /// </summary>
        /// <param name="subMenu">子菜单id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteSubMenu(Guid subMenu, string operatorAccount) {
            var repository = Factory.CreateSystemResourceRepository();
            repository.DeleteSubMenu(subMenu);
            LogHelper.SaveDeleteSubMenuLog(subMenu, operatorAccount);
        }
        /// <summary>
        /// 删除页面资源
        /// </summary>
        /// <param name="resource">页面资源id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteResource(Guid resource, string operatorAccount) {
            var repository = Factory.CreateSystemResourceRepository();
            repository.DeleteResource(resource);
            LogHelper.SaveDeleteResourceLog(resource, operatorAccount);
        }

        /// <summary>
        /// 查询主菜单
        /// </summary>
        /// <param name="website">网站</param>
        public static IEnumerable<KeyValuePair<Guid, MenuView>> QueryMenus(Website website) {
            var repository = Factory.CreateSystemResourceRepository();
            return repository.QueryMenus(website);
        }
        /// <summary>
        /// 查询子菜单
        /// </summary>
        /// <param name="menu">主菜单Id</param>
        public static IEnumerable<KeyValuePair<Guid, SubMenuView>> QuerySubMenus(Guid menu) {
            var repository = Factory.CreateSystemResourceRepository();
            return repository.QuerySubMenus(menu);
        }
        /// <summary>
        /// 查询页面资源数据
        /// </summary>
        /// <param name="submenu">子菜单Id</param>
        public static IEnumerable<KeyValuePair<Guid, ResourceView>> QueryResources(Guid submenu) {
            var repository = Factory.CreateSystemResourceRepository();
            return repository.QueryResources(submenu);
        }
        /// <summary>
        /// 查询有效的菜单数据
        /// </summary>
        /// <param name="website">网站</param>
        public static IEnumerable<PermissionView.MenuView> QueryValidMenus(Website website) {
            var repository = Factory.CreateSystemResourceRepository();
            return repository.QueryValidMenus(website);
        }
    }
}