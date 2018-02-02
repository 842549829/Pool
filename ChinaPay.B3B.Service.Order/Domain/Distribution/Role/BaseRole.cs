using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Role {
    /// <summary>
    /// 角色
    /// </summary>
    public abstract class BaseRole {
        string _account = null;

        protected BaseRole(Guid id) {
            this.Id = id;
        }
        protected BaseRole(Guid id, string account)
            : this(id) {
            this._account = account;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 收/付款账号
        /// </summary>
        public string Account {
            get {
                if(null == _account) {
                    _account = GetAccount() ?? string.Empty;
                }
                return _account;
            }
            internal set {
                if(string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException("account");
                _account = value;
            }
        }

        protected abstract string GetAccount();
    }
}