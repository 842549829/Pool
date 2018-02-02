using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Command.PNR;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    /// <summary>
    /// 现在存储的是预订后的信息，按名字来，应该是出票后的信息，要做更改，变动较大；
    /// </summary>
    public class IssuedPNR
    {
        /// <summary>
        /// 旅客订座记录编号
        /// </summary>
        public PNRPair Code { get; internal set; }

        /// <summary>
        /// 行程类型
        /// </summary>
        public ItineraryType ItineraryType { get; set; }

        /// <summary>
        /// 是否团队
        /// </summary>
        public bool IsTeam { get; set; }

        /// <summary>
        /// 缺口程是否搭桥
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// 团队人数
        /// </summary>
        public int NumberOfTerm { get; set; }
        
        /// <summary>
        /// 旅客信息，随着订票阶段不同，会有部分信息缺失；
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
        /// 票价信息，若未出票，则为null；
        /// </summary>
        public PriceView Price { get; internal set; }

        /// <summary>
        /// 票号信息，若未出票，则为null；
        /// </summary>
        public Dictionary<int, string> TicketNumbers { get; internal set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public Dictionary<int, string> CertificateNumbers { get; internal set; }
        
        /// <summary>
        /// 是否取消预订，若此项为true，则无其它信息；
        /// </summary>
        public bool IsCanceled { get; internal set; }
    }
}
