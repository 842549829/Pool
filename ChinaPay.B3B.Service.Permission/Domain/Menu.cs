using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Permission.Domain {
    public class Menu {
        List<SubMenu> m_children;

        internal Menu(string name)
            : this(Guid.NewGuid(), name) {
        }
        internal Menu(Guid id, string name) {
            Id = id;
            Name = name;
            m_children = new List<SubMenu>();
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name {
            get;
            private set;
        }
        /// <summary>
        /// 排序顺序
        /// </summary>
        public int SortLevel {
            get;
            internal set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            internal set;
        }
        /// <summary>
        /// 状态
        /// 是否有效
        /// </summary>
        public bool Valid {
            get;
            internal set;
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Display { get; internal set; }
        /// <summary>
        /// 是否空菜单
        /// </summary>
        public bool IsEmpty {
            get { return m_children.Count == 0; }
        }
        /// <summary>
        /// 子菜单集合
        /// </summary>
        public IEnumerable<SubMenu> Children {
            get {
                return m_children.AsReadOnly();
            }
        }

        public Menu Clone() {
            var result = CloneMain();
            m_children.ForEach(item => result.AppendChild(item.Clone()));
            return result;
        }
        private Menu CloneMain() {
            return new Menu(this.Id, this.Name) {
                SortLevel = this.SortLevel,
                Remark = this.Remark,
                Valid = this.Valid,
                Display = this.Display
            };
        }
        public override string ToString() {
            return string.Format("名称:{0} 排序顺序:{1} 备注:{2} 状态:{3} 显示:{4}", Name, SortLevel, Remark, Valid, Display);
        }

        internal void AppendChild(SubMenu item) {
            if(item == null) throw new ArgumentNullException("item");
            if(m_children.Exists(menu => menu.Id == item.Id)) throw new Core.Exception.RepeatedItemException(item.Name, "不能重复添加的同一子菜单");
            m_children.Add(item);
        }
        internal bool ContainsChild(Guid child) {
            return m_children.Exists(item => item.Id == child);
        }
        internal bool ContainValidResource(string address) {
            return Valid && m_children.Any(item => item.ContainValidResource(address));
        }
        internal void Sort() {
            if(m_children.Count > 1) {
                m_children = m_children.OrderBy(subMenu => subMenu.SortLevel).ToList();
            }
        }
        internal static Menu Union(Menu first, Menu second) {
            Menu result = null;
            if(first == null) {
                if(second != null) {
                    result = second.Clone();
                }
            } else {
                result = first.Clone();
                if(second != null) {
                    foreach(var item in second.Children) {
                        if(!result.ContainsChild(item.Id)) {
                            result.AppendChild(item);
                        }
                    }
                }
            }
            return result;
        }
        internal static Menu Subtract(Menu first, Menu second) {
            if(first == null) return null;
            if(second == null) return first.Clone();
            var result = first.CloneMain();
            foreach(var item in first.Children) {
                if(!second.ContainsChild(item.Id)) {
                    result.AppendChild(item.Clone());
                }
            }
            return result;
        }
        internal static Menu Intersact(Menu first, Menu second) {
            if(first == null || second == null || first.IsEmpty || second.IsEmpty) return null;
            var result = first.CloneMain();
            foreach(var item in first.Children) {
                if(second.ContainsChild(item.Id)) {
                    result.AppendChild(item.Clone());
                }
            }
            return result;
        }
        internal static Menu GetMenu(DataTransferObject.Permission.MenuView view) {
            if(view == null) throw new ArgumentNullException("view");
            view.Validate();
            return new Menu(view.Name) {
                SortLevel = view.SortLevel,
                Remark = view.Remark,
                Valid = view.Valid,
                Display = view.Display
            };
        }
        internal static Menu GetMenu(Guid id, DataTransferObject.Permission.MenuView view) {
            var result = GetMenu(view);
            result.Id = id;
            return result;
        }
    }
}