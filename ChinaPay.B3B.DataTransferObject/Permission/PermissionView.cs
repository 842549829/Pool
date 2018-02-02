using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    public class PermissionView {
        /// <summary>
        /// 运维平台上的菜单集合
        /// </summary>
        public List<PermissionView.MenuView> MaintenanceMenus { get; set; }
        /// <summary>
        /// 交易平台上的菜单集合
        /// </summary>
        public List<PermissionView.MenuView> TransactionMenus { get; set; }

        public class MenuView {
            public Guid Id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 子菜单集合
            /// </summary>
            public List<PermissionView.SubMenuView> Children { get; set; }

            public bool IsEmpty {
                get { return this.Children == null || this.Children.Count == 0; }
            }
            public MenuView Clone() {
                var result = new MenuView() {
                    Id = this.Id,
                    Name = this.Name,
                    Children = new List<SubMenuView>()
                };
                foreach(var item in Children) {
                    result.Children.Add(item.Clone());
                }
                return result;
            }

            public static MenuView Union(MenuView first, MenuView second) {
                MenuView result = null;
                if(first == null) {
                    if(second != null) {
                        result = second.Clone();
                    }
                } else {
                    result = first.Clone();
                    if(second != null) {
                        foreach(var item in second.Children) {
                            if(!result.Children.Exists(c => c.Id == item.Id)) {
                                result.Children.Add(item);
                            }
                        }
                    }
                }
                return result;
            }
            public static MenuView Subtract(MenuView first, MenuView second) {
                if(first == null) {
                    return null;
                } else {
                    if(second == null) {
                        return first.Clone();
                    } else {
                        var result = new MenuView() {
                            Id = first.Id,
                            Name = first.Name,
                            Children = new List<SubMenuView>()
                        };
                        foreach(var item in first.Children) {
                            if(!second.Children.Exists(c => c.Id == item.Id)) {
                                result.Children.Add(item);
                            }
                        }
                        return result;
                    }
                }
            }
            public static MenuView Intersact(MenuView first, MenuView second) {
                if(first == null || second == null || first.IsEmpty || second.IsEmpty)
                    return null;
                var result = new MenuView() {
                    Id = first.Id,
                    Name = first.Name,
                    Children = new List<SubMenuView>()
                };
                foreach(var item in first.Children) {
                    if(second.Children.Exists(c => c.Id == item.Id)) {
                        result.Children.Add(item);
                    }
                }
                return result;
            }
        }
        public class SubMenuView {
            public Guid Id { get; set; }
            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }

            public SubMenuView Clone() {
                return new SubMenuView() {
                    Id = this.Id,
                    Name = this.Name
                };
            }
        }
    }
}
