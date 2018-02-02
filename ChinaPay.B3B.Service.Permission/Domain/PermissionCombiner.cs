using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Permission;

namespace ChinaPay.B3B.Service.Permission.Domain {
    class PermissionCombiner {
        public static PermissionView Combine(params PermissionView[] permissions) {
            var originalTransactionMenuViews = new List<PermissionView.MenuView>();
            var originalMaintenanceMenuViews = new List<PermissionView.MenuView>();
            foreach(var item in permissions) {
                if(item != null) {
                    if(item.TransactionMenus != null) {
                        originalTransactionMenuViews.AddRange(item.TransactionMenus);
                    }
                    if(item.MaintenanceMenus != null) {
                        originalMaintenanceMenuViews.AddRange(item.MaintenanceMenus);
                    }
                }
            }
            return new PermissionView() {
                TransactionMenus = Combine(originalTransactionMenuViews),
                MaintenanceMenus = Combine(originalMaintenanceMenuViews)
            };
        }
        public static List<PermissionView.MenuView> Combine(params IEnumerable<PermissionView.MenuView>[] menus) {
            var originalMenus = new List<PermissionView.MenuView>();
            foreach(var item in menus) {
                originalMenus.AddRange(item);
            }
            return Combine(originalMenus);
        }
        public static List<PermissionView.MenuView> Combine(IEnumerable<PermissionView.MenuView> menus) {
            var dicMenus = new Dictionary<Guid, PermissionView.MenuView>();
            foreach(var item in menus) {
                if(dicMenus.ContainsKey(item.Id)) {
                    dicMenus[item.Id] = Combine(dicMenus[item.Id], item);
                } else {
                    dicMenus.Add(item.Id, item);
                }
            }
            return dicMenus.Values.ToList();
        }
        public static PermissionView Subtract(PermissionView originalPermission, PermissionView forbidenPermission) {
            if(originalPermission == null) {
                return null;
            }
            if(forbidenPermission == null) {
                return originalPermission;
            }
            return new PermissionView() {
                TransactionMenus = Subtract(originalPermission.TransactionMenus, forbidenPermission.TransactionMenus),
                MaintenanceMenus = Subtract(originalPermission.MaintenanceMenus, forbidenPermission.MaintenanceMenus)
            };
        }
        
        private static PermissionView.MenuView Combine(PermissionView.MenuView first, PermissionView.MenuView second) {
            var children = new Dictionary<Guid, PermissionView.SubMenuView>();
            if(first.Children != null) {
                foreach(var item in first.Children) {
                    children.Add(item.Id, item);
                }
            }
            if(second.Children != null) {
                foreach(var item in second.Children) {
                    if(!children.ContainsKey(item.Id)) {
                        children.Add(item.Id, item);
                    }
                }
            }
            return new PermissionView.MenuView() {
                Id = first.Id,
                Name = first.Name,
                Children = children.Values.ToList()
            };
        }
        public static List<PermissionView.MenuView> Subtract(IEnumerable<PermissionView.MenuView> permitMenus, IEnumerable<PermissionView.MenuView> forbidenMenus) {
            var result = new List<PermissionView.MenuView>();
            if(forbidenMenus == null) {
                if(permitMenus != null) {
                    result.AddRange(permitMenus);
                }
            } else {
                if(permitMenus != null) {
                    var dicForbidenMenus = forbidenMenus.ToDictionary(item => item.Id);
                    foreach(var item in permitMenus) {
                        if(dicForbidenMenus.ContainsKey(item.Id)) {
                            var subMenus = Subtract(dicForbidenMenus[item.Id].Children, item.Children);
                            if(subMenus.Count > 0) {
                                result.Add(new PermissionView.MenuView() {
                                    Id = item.Id ,
                                    Name = item.Name,
                                    Children = subMenus
                                });
                            }
                        } else {
                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }
        private static List<PermissionView.SubMenuView> Subtract(IEnumerable<PermissionView.SubMenuView> permitSubMenus, IEnumerable<PermissionView.SubMenuView> forbidenSubMenus) {
            var result = new List<PermissionView.SubMenuView>();
            if(forbidenSubMenus == null) {
                if(permitSubMenus != null) {
                    result.AddRange(permitSubMenus);
                }
            } else {
                if(permitSubMenus != null) {
                    result = new List<PermissionView.SubMenuView>();
                    var dicForbidenSubMenus = forbidenSubMenus.ToDictionary(item => item.Id);
                    foreach(var item in permitSubMenus) {
                        if(!dicForbidenSubMenus.ContainsKey(item.Id)) {
                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }
    }
}