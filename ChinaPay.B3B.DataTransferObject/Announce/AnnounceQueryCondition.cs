using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Announce
{
    using System;
    using ChinaPay.Core;
    /// <summary>
    /// 公告查询条件
    /// </summary>
    public class AnnounceQueryCondition
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 发布人
        /// </summary>
        public string PublishAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public AduiteStatus? AduiteStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 发布日期范围
        /// </summary>
        public Range<DateTime?> PublishTime
        {
            get;
            set;
        }
    }
}
