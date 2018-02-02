using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Permission.Domain {
    public class SubMenu {
        List<Resource> m_resources;

        internal SubMenu(string name, string address)
            : this(Guid.NewGuid(), name, address) {
        }
        internal SubMenu(Guid id, string name, string address) {
            this.Id = id;
            this.Name = name;
            this.Address = address;
            m_resources = new List<Resource>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int SortLevel { get; internal set; }
        public string Address { get; private set; }
        public string Remark { get; internal set; }
        public bool Valid { get; internal set; }
        public bool Display { get; internal set; }
        public IEnumerable<Resource> Resources {
            get {
                return m_resources.AsReadOnly();
            }
        }

        public SubMenu Clone() {
            var result = new SubMenu(this.Id, this.Name, this.Address) {
                SortLevel = this.SortLevel,
                Remark = this.Remark,
                Valid = this.Valid,
                Display = this.Display
            };
            foreach(var item in m_resources) {
                result.AppendResource(item.Clone());
            }
            return result;
        }
        public override string ToString() {
            return string.Format("名称:{0} 地址:{1} 排序顺序:{2} 备注:{3} 状态:{4} 显示:{5}", this.Name, this.Address, this.SortLevel, this.Remark, this.Valid, this.Display);
        }

        internal void AppendResource(Resource item) {
            if(item == null) throw new ArgumentNullException("item");
            //2013年3月27日11:13:39 wangsl 如果有同一资源菜单就不进行添加了
            if (m_resources.Exists(menu => menu.Id == item.Id)) return;
            //if(m_resources.Exists(menu => menu.Id == item.Id)) throw new Core.Exception.RepeatedItemException(item.Address, "不能重复添加的同一资源");
            m_resources.Add(item);
        }
        internal bool ContainValidResource(string address) {
            if(this.Valid) {
                if(System.String.Compare(this.Address, address, System.StringComparison.OrdinalIgnoreCase) == 0) return true;
                return m_resources.Any(item => item.Valid && item.IsSameAddress(address));
            }
            return false;
        }

        internal static SubMenu GetSubMenu(DataTransferObject.Permission.SubMenuView view) {
            if(view == null) throw new ArgumentNullException("view");
            view.Validate();
            return new SubMenu(view.Name, view.Address) {
                SortLevel = view.SortLevel,
                Remark = view.Remark,
                Valid = view.Valid,
                Display = view.Display
            };
        }
        internal static SubMenu GetSubMenu(Guid id, DataTransferObject.Permission.SubMenuView view) {
            var result = GetSubMenu(view);
            result.Id = id;
            return result;
        }
    }
}