using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Command.PNR;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 预订后的信息
    /// </summary>
    public class ReservedPNR
    {
        /// <summary>
        /// 旅客订座记录编号
        /// </summary>
        public PNRPair Code { get; internal set; }

        /// <summary>
        /// 旅客信息
        /// </summary>
        public Dictionary<int, Passenger> Passenges { get; internal set; }

        /// <summary>
        /// 航段信息
        /// </summary>
        public Dictionary<int, Segment> Segments { get; internal set; }

        /// <summary>
        /// 联系信息
        /// </summary>
        public Dictionary<int, string> ContractInformations { get; internal set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public Dictionary<int, string> CertificateNumbers { get; internal set; }

        /// <summary>
        /// 票价信息
        /// </summary>
        public IEnumerable<PriceView> Prices { get; internal set; }

        /// <summary>
        /// 是否取消预订
        /// </summary>
        public bool IsCanceled { get; internal set; }
    }
}
