using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    /// <summary>
    /// 适用类型
    /// </summary>
    public enum ApplyType
    {
        /// <summary>
        /// 单程
        /// </summary>
        OneWay,
        /// <summary>
        /// 往返程
        /// </summary>
        Roundtrip,
        /// <summary>
        /// 所用行程
        /// </summary>
        All
    }
}
