using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    public enum QueueType
    {
        /// <summary>
        /// 综合信息（General Message）
        /// </summary>
        GQ,
        /// <summary>
        /// 自由格式（Supper Report）
        /// </summary>
        RP,
        /// <summary>
        /// 座位证实回复（Replay Record Queue）
        /// </summary>
        KK,
        /// <summary>
        /// 特殊服务（SSR Request Queue）
        /// </summary>
        SR,
        /// <summary>
        /// 机票更改(Ticket Change Queue)
        /// </summary>
        TC,
        /// <summary>
        /// 出票时限(Time Limit Queue)
        /// </summary>
        TL,
        /// <summary>
        /// 航班更改通知(Schedule Change Queue)
        /// </summary>
        SC,
        /// <summary>
        /// 旅客重复订座(Passenger Rebook)
        /// </summary>
        RE
    }
}
