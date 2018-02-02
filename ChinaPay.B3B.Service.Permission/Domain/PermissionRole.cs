using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Permission.Domain {
    /// <summary>
    /// 权限角色
    /// </summary>
    public class PermissionRole {
        List<Menu> m_menus;

        internal PermissionRole(Guid id) {
            this.Id = id;
            m_menus = new List<Menu>();
        }
        
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
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
        /// 可访问菜单集合
        /// </summary>
        public IEnumerable<Menu> Menus {
            get {
                return m_menus.AsReadOnly();
            }
        }

        internal void AppendMenu(Menu item) {
            if(item == null) throw new ArgumentNullException("item");
            if(m_menus.Exists(menu => menu.Id == item.Id))
                throw new ChinaPay.Core.Exception.RepeatedItemException(item.Name, "不能重复添加的同一菜单");
            m_menus.Add(item);
        }
    }
}