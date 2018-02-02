using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的旅客订座记录
    /// </summary>
    public class ReservedPnrInfo
    {
        /// <summary>
        /// 编码对
        /// </summary>
        public PNRPair PnrPair { get; set; }

        /// <summary>
        /// 团队信息，若不是团队，则此项为空
        /// </summary>
        public ReservedTermInfo TermInformation { get; set; }

        /// <summary>
        /// 旅客信息
        /// </summary>
        public List<ReservedPassengerInfo> Passengers { get; internal set; }
        
        /// <summary>
        /// 航段信息
        /// </summary>
        public List<ReservedSegmentInfo> Segments { get; internal set; }

        /// <summary>
        /// 联系信息
        /// </summary>
        public List<ReservedContractInfo> ContractInformations { get; internal set; }
        
        /// <summary>
        /// 身份证号
        /// </summary>
        public List<ReservedCertificateInfo> CertificateNumbers { get; internal set; }

        /// <summary>
        /// 生成编码的OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }

        /// <summary>
        /// 被授权的代理人编号列表
        /// </summary>
        public List<ReservedAuthorizeInfo> Authorizes { get; set; }
        
        /// <summary>
        /// 存放机场对信息（包含缺口程搭桥信息），用于判断缺口程信息。
        /// </summary>
        public List<AirportPair> AirportPairs { get; set; }
        
        /// <summary>
        /// 南航儿童票信息；
        /// </summary>
        public List<ReservedChildInfo> Children { get; set; }
    }
}
