using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    public enum JourneySheetStatus
    {
        /// <summary>
        /// 未使用的；
        /// </summary>
        NotUsed,
        /// <summary>
        /// 使用的
        /// </summary>
        Used,
        /// <summary>
        /// 作废
        /// </summary>
        Canceled
    }
}
