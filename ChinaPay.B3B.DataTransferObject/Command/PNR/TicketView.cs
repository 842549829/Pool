using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    public class TicketView
    {
        /// <summary>
        /// 票号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 旅客订座记录编号
        /// </summary>
        public PNRPair PNRCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 航段
        /// </summary>
        //public Segment segment { get; set; }

        /// <summary>
        /// 旅客姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 出票航空公司
        /// </summary>
        public string  AirlineCompany { get; set; }

    }
}
