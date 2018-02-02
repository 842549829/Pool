using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Permission;

namespace ChinaPay.B3B.Service.Permission.Domain {
    /// <summary>
    /// 权限集合
    /// </summary>
    public class PermissionCollection {
        readonly List<Menu> m_menus;

        internal PermissionCollection(IEnumerable<Menu> menus) {
            m_menus = new List<Menu>();
            var unitedMenus = Union(menus);
            var sortedMenus = unitedMenus.OrderBy(menu => menu.SortLevel);
            m_menus.AddRange(sortedMenus);
        }

        public IEnumerable<Menu> GetMenus() {
            return m_menus.AsReadOnly();
        }
        public bool HasPermission(string address) {
            foreach(var item in m_menus) {
                if(item.ContainValidResource(address)) {
                    return true;
                }
            }
            return false;
        }

        internal static IEnumerable<Menu> Union(IEnumerable<Menu> menus) {
            var dicMenus = new Dictionary<Guid, Menu>();
            foreach(var item in menus) {
                if(item != null) {
                    if(dicMenus.ContainsKey(item.Id)) {
                        dicMenus[item.Id] = Menu.Union(dicMenus[item.Id], item);
                    } else {
                        dicMenus.Add(item.Id, item.Clone());
                    }
                }
            }
            foreach(var item in dicMenus) {
                item.Value.Sort();
            }
            return dicMenus.Values;
        }
        internal static IEnumerable<Menu> Union(params IEnumerable<Menu>[] menus) {
            return Union(from items in menus
                         from item in items
                         where item != null
                         select item);
        }
        internal static IEnumerable<Menu> Union(IEnumerable<PermissionRole> roles) {
            IEnumerable<Menu> menus = new List<Menu>();
            if(roles != null) {
                menus = from role in roles
                        where role != null
                        from menu in role.Menus
                        where menu != null
                        select menu;
            }
            return Union(menus);
        }
        internal static IEnumerable<Menu> Subtract(IEnumerable<Menu> first, IEnumerable<Menu> second) {
            var result = new List<Menu>();
            if(second == null) {
                if(first != null) {
                    result.AddRange(first.Select(item => item.Clone()));
                }
            } else {
                if(first != null) {
                    var dicForbidenMenus = second.ToDictionary(item => item.Id);
                    foreach(var item in first) {
                        if(dicForbidenMenus.ContainsKey(item.Id)) {
                            var menu = Menu.Subtract(item, dicForbidenMenus[item.Id]);
                            if(menu != null && !menu.IsEmpty) {
                                result.Add(menu);
                            }
                        } else {
                            result.Add(item.Clone());
                        }
                    }
                }
            }
            return result;
        }
        internal static IEnumerable<Menu> Intersact(IEnumerable<Menu> first, IEnumerable<Menu> second) {
            var firstDic = Union(first).ToDictionary(item => item.Id);
            var secondDic = Union(second).ToDictionary(item => item.Id);
            return firstDic.Where(item => secondDic.ContainsKey(item.Key)).Select(item => Menu.Intersact(item.Value, secondDic[item.Key])).Where(menu => menu != null && !menu.IsEmpty);
        }

        public static IEnumerable<PermissionView.MenuView> Union(IEnumerable<PermissionView.MenuView> menus) {
            var dicMenus = new Dictionary<Guid, PermissionView.MenuView>();
            foreach(var item in menus) {
                if(item != null) {
                    if(dicMenus.ContainsKey(item.Id)) {
                        dicMenus[item.Id] = PermissionView.MenuView.Union(dicMenus[item.Id], item);
                    } else {
                        dicMenus.Add(item.Id, item.Clone());
                    }
                }
            }
            return dicMenus.Values;
        }
        public static IEnumerable<PermissionView.MenuView> Union(params IEnumerable<PermissionView.MenuView>[] menus) {
            return Union(from items in menus
                         from item in items
                         where item != null
                         select item);
        }
        public static IEnumerable<PermissionView.MenuView> Subtract(IEnumerable<PermissionView.MenuView> first, IEnumerable<PermissionView.MenuView> second) {
            var result = new List<PermissionView.MenuView>();
            if(second == null) {
                if(first != null) {
                    result.AddRange(first.Select(item => item.Clone()));
                }
            } else {
                if(first != null) {
                    var dicForbidenMenus = second.ToDictionary(item => item.Id);
                    foreach(var item in first) {
                        if(dicForbidenMenus.ContainsKey(item.Id)) {
                            var menu = PermissionView.MenuView.Subtract(item, dicForbidenMenus[item.Id]);
                            if(menu != null && !menu.IsEmpty) {
                                result.Add(menu);
                            }
                        } else {
                            result.Add(item.Clone());
                        }
                    }
                }
            }
            return result;
        }
        public static IEnumerable<PermissionView.MenuView> Intersact(IEnumerable<PermissionView.MenuView> first, IEnumerable<PermissionView.MenuView> second) {
            var firstDic = Union(first).ToDictionary(item => item.Id);
            var secondDic = Union(second).ToDictionary(item => item.Id);
            return firstDic.Where(item => secondDic.ContainsKey(item.Key)).Select(item => PermissionView.MenuView.Intersact(item.Value, secondDic[item.Key])).Where(menu => menu != null && !menu.IsEmpty);
        }
    }
}