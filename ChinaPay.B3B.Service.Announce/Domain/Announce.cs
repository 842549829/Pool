using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;

namespace ChinaPay.B3B.Service.Announce.Domain {
    public class Announce {
        public Announce()
            : this(Guid.NewGuid()) {
        }
        public Announce(Guid id) {
            this.Id = id;
        }

        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content {
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
        /// 公司Id
        /// </summary>
        public Guid Company {
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
        /// 发布角色
        /// </summary>
        public PublishRole PublishRole {
            get;
            set;
        }
        /// <summary>
        /// 公告类型
        /// </summary>
        public AnnounceLevel AnnounceType {
            get;
            set;
        }
        /// <summary>
        /// 公告范围
        /// </summary>
        public AnnounceScope AnnunceScope
        {
            get;
            set;
        }
    }
}
