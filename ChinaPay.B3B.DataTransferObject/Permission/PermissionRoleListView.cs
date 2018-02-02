using System;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    /// <summary>
    /// 角色列表
    /// </summary>
    public class PermissionRoleListView {
        public Guid Id {
            get;
            set;
        }
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
    }
}
