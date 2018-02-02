namespace ChinaPay.B3B.DataTransferObject.Organization {
    using ChinaPay.Core;

    public class UserUpdateView {
        public UserUpdateView(string userName){
            this.UserName = userName;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName {
            get;
            private set;
        }
        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex {
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
        public string TelPhone {
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
        /// </summary>
        public bool IsValid {
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