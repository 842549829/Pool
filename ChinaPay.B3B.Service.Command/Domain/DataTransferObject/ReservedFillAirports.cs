using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    public class ReservedFillAirports
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 代理人编号
        /// </summary>
        public AirportPair AirportPair { get; set; }
    }
}
