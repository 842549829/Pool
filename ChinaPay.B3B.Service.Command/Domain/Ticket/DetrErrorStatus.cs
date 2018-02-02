using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    /// <summary>
    /// DETR的错误状态
    /// </summary>
    public enum DetrErrorStatus
    {
        /// <summary>
        /// 无错误
        /// </summary>
        None,
        /// <summary>
        /// 权限
        /// </summary>
        Authority,
        /// <summary>
        /// 票号
        /// </summary>
        TickerNumber,
        /// <summary>
        /// 格式
        /// </summary>
        Format
    }
}
