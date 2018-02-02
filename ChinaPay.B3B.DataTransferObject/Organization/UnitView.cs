namespace ChinaPay.B3B.DataTransferObject.Organization {
    public abstract class UnitView {
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
        /// 名称
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 简称/昵称
        /// </summary>
        public string AbbreviateName {
            get;
            set;
        }
        /// <summary>
        /// 地址信息
        /// </summary>
        public RegisterAddress Address {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact {
            get;
            set;
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone {
            get;
            set;
        }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax {
            get;
            set;
        }
        public string EMail {
            get;
            set;
        }
        public string MSN {
            get;
            set;
        }
        public string QQ {
            get;
            set;
        }
    }
}
