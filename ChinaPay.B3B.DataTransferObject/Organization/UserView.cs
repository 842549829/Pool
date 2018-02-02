namespace ChinaPay.B3B.DataTransferObject.Organization {
    using ChinaPay.Core;

    /// <summary>
    /// 员工信息
    /// </summary>
    public class UserView {
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
        /// 密码
        /// </summary>
        public string Password {
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
        /// 座机
        /// </summary>
        public string Phone {
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
        /// 状态
        /// 是否启用
        /// </summary>
        public bool Enabled {
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
    }
}