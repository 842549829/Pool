using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Announce {
    public class AnnounceView {
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
        /// 公告类型
        /// </summary>
        public AnnounceLevel AnnounceType {
            get;
            set;
        }
        public DateTime PublishTime {
            get;
            set;
        }
        ///// <summary>
        ///// 公告范围
        ///// </summary>
        //public int AnnounceScopeValue
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 公告范围
        /// </summary>
        public AnnounceScope AnnounceScope
        {
            get;
            set;
        }
    }
}
