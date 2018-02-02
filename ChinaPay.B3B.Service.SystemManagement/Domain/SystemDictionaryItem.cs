namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    using System;

    /// <summary>
    /// 系统字典项
    /// </summary>
    public class SystemDictionaryItem {
        public SystemDictionaryItem(string name, string value, string remark)
            : this(Guid.NewGuid(), name, value, remark) {
        }
        public SystemDictionaryItem(Guid id, string name, string value, string remark) {
            this.Id = id;
            this.Name = name;
            this.Value = value;
            this.Remark = remark;
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
        /// 值
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
            internal set;
        }

        public override string ToString() {
            return string.Format("Name:{0} Value:{1} Remark:{2}", Name, Value, Remark);
        }
    }
}