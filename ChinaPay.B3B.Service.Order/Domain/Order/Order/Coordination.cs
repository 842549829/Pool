using System;
using System.ComponentModel;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 协调信息
    /// </summary>
    public class Coordination {

        public Coordination(string account, string content, string result, BusinessType type, ContactMode mode)
            : this(account, content, result, type, mode, DateTime.Now,OrderRole.Platform) {
        }
        public Coordination(string account, string content, string result, BusinessType type, ContactMode mode, DateTime time,OrderRole role) {
            this.Account = account;
            this.Content = content;
            this.Result = result;
            this.Type = type;
            this.Mode = mode;
            this.Time = time;
            this.OrderRole = role;
        }

        /// <summary>
        /// 协调角色,显示时候用不持久化
        /// </summary>
        public OrderRole OrderRole
        {
            get;
            private set;
        }

        /// <summary>
        /// 操作账号
        /// </summary>
        public string Account {
            get; set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content {
            get;
            private set;
        }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result {
            get;
            private set;
        }
        /// <summary>
        /// 协调类型
        /// </summary>
        public BusinessType Type {
            get;
            private set;
        }
        /// <summary>
        /// 联系方式
        /// </summary>
        public ContactMode Mode {
            get;
            private set;
        }
        /// <summary>
        /// 协调时间
        /// </summary>
        public DateTime Time {
            get;
            private set;
        }
    }
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType {
        出票,
        退票,
        废票,
        改期,
        差额退款
    }
    /// <summary>
    /// 联系方式
    /// </summary>
    public enum ContactMode {
        [Description("电话")]
        Telphone,
        QQ,
        [Description("邮件")]
        EMail
    }
}