using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    /// <summary>
    /// 旅客订座记录状态
    /// </summary>
    public enum PnrStatus
    {
        /// <summary>
        /// 不存在
        /// </summary>
        NotExists,
        /// <summary>
        /// 取消
        /// </summary>
        Cancelled,
        /// <summary>
        /// 预订
        /// </summary>
        Resrved,
        /// <summary>
        /// 出票
        /// </summary>
        Issued
    }
}
