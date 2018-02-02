using System;
using System.ComponentModel;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Announce {
    /// <summary>
    /// 所有公告列表
    /// </summary>
    public class AnnounceListView {
        public Guid Id {
            get;
            set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title {
            get;
            set;
        }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime {
            get;
            set;
        }
        /// <summary>
        /// 发布人
        /// </summary>
        public string PublishAccount {
            get;
            set;
        }
        /// <summary>
        /// 公告级别
        /// </summary>
        public AnnounceLevel AnnounceLevel {
            get;
            set;
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AduiteStatus AduiteStatus {
            get;
            set;
        }
        /// <summary>
        /// 公告范围
        /// </summary>
        public AnnounceScope AnnounceScope
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 公告级别
    /// </summary>
    public enum AnnounceLevel
    {
        [Description("普通公告")]
        Common,
        [Description("重要公告")]
        Important,
        [Description("紧急公告")]
        Emergency
    }
    /// <summary>
    /// 公告审核状态
    /// </summary>
    public enum AduiteStatus
    {
        [Description("已审")]
        Audited,
        [Description("未审")]
        UnAudit
    }
}
