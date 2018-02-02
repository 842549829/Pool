namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SystemParam {
        internal SystemParam(SystemParamType type, string value, string remark) {
            this.Type = type;
            this.Value = value;
            this.Remark = remark;
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        public SystemParamType Type {
            get;
            private set;
        }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value {
            get;
            internal set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            private set;
        }
    }
}