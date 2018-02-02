using System;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    /// <summary>
    /// 权限角色
    /// </summary>
    public class PermissionRoleView {
        public PermissionRoleView()
            : this(Guid.NewGuid()) {
        }
        public PermissionRoleView(Guid id) {
            this.Id = id;
        }
        public Guid Id { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// 是否有效
        /// </summary>
        public bool Valid {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Name))
                throw new ArgumentNullException("name");
        }
    }
}