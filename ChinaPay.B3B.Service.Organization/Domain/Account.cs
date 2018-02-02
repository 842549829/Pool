namespace ChinaPay.B3B.Service.Organization.Domain {
    using Common.Enums;
    using System;

    /// <summary>
    /// 账号信息
    /// </summary>
    public class Account {
        public Account(string no, AccountType type) : this(no, type, false, DateTime.Now) {
        }
        public Account(string no, AccountType type, bool valid) : this(no, type, valid, DateTime.Now) { 
        }
        internal Account(string no, AccountType type, bool valid, DateTime time) {
            this.No = no;
            this.Type = type;
            this.Valid = valid;
            this.Time = time;
        }
        internal Account(Guid company,string account,AccountType type,bool valid,DateTime time) {
            No = account;
            Company = company;
            Type = type;
            Valid = valid;
            Time = time;
        }

        public Guid Company { get; private set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string No {
            get;
            private set;
        }
        /// <summary>
        /// 账号类型
        /// </summary>
        public AccountType Type {
            get;
            private set;
        }
        /// <summary>
        /// 状态
        /// 是否有效
        /// </summary>
        public bool Valid {
            get;
            private set;
        }
        /// <summary>
        /// 账号绑定时间
        /// </summary>
        public System.DateTime Time {
            get;
            private set;
        }
    }
}