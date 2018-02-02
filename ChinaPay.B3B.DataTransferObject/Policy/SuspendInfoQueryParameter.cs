
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using System;

    public class SuspendInfoQueryParameter {
        /// <summary>
        /// 被挂起政策的公司 Id
        /// </summary>
        public Guid? Company { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页记录数量
        /// </summary>
        public int PageSize { get; set; }
    }

    public class SuspendOperationQueryParameter {
        /// <summary>
        /// 被挂起政策的公司 Id
        /// </summary>
        public Guid? Company { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public Range<DateTime> OperateDate { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页记录数量
        /// </summary>
        public int PageSize { get; set; }
    }
}
