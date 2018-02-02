namespace ChinaPay.B3B.DataTransferObject.Organization {
    using ChinaPay.Core;

    /// <summary>
    /// 员工查询列表
    /// </summary>
    public class UserListView {
        /// <summary>
        /// Id
        /// </summary>
        public System.Guid UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName {
            get;
            set;
        }
        /// <summary>
        /// 角色组
        /// </summary>
        public System.Collections.Generic.IEnumerable<string> PermissionRoles {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EMail {
            get;
            set;
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// 是否启用
        /// </summary>
        public bool Enabled {
            get;
            set;
        }
    }
}
