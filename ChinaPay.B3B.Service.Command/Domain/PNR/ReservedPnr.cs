using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    /// <summary>
    /// 预订后旅客订座记录
    /// </summary>
    public class ReservedPnr
    {
        /// <summary>
        /// 是否为C系统编码提取的信息（2013-05-12 deng.zhao），因没有A系统，使用的是布尔值；
        /// </summary>
        public bool UsedCrsCode { get; set; }

        ///// <summary>
        ///// 是否成功执行
        ///// </summary>
        //public bool HasSucceeded { get; set; }
        
        /// <summary>
        /// PNR原始数据
        /// </summary>
        public string PnrRawData { get; set; }

        /// <summary>
        /// PAT原始数据
        /// </summary>
        public string PatRawData { get; set; }

        /// <summary>
        /// 编码是否取消
        /// </summary>
        public bool HasCanceled { get; set; }

        /// <summary>
        ///  编码对
        /// </summary>
        public PNRPair PnrPair { get; set; }

        /// <summary>
        /// 旅客信息
        /// </summary>
        public List<Passenger> Passengers { get; set; }
        
        /// <summary>
        /// 航段信息
        /// </summary>
        public Voyage Voyage { get; set; }

        /// <summary>
        /// 实际人数
        /// </summary>
        public int ActualNumber  { get { return Passengers.Count; }}

        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalNumber { get; set; }

        /// <summary>
        /// 是否需要搭桥
        /// </summary>
        public bool NeedFill { get; set; }

        /// <summary>
        /// 旅客组成类型
        /// </summary>
        public PassengerConsistsType PassengerConsistsType { get; set; }
        
        /// <summary>
        /// 是否团队
        /// </summary>
        public bool IsTeam
        {
            get { return PassengerConsistsType == PassengerConsistsType.Group; }
        }
        
        /// <summary>
        /// 生成编码的OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }

        /// <summary>
        /// 被授权的代理人编号列表
        /// </summary>
        public IEnumerable<string> Authorizes { get; set; }

    }
}
