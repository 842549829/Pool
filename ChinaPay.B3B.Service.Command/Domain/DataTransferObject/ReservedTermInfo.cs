using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 订座后的团队信息
    /// </summary>
    public class ReservedTermInfo
    {
        /// <summary>
        /// 总人数
        /// </summary>
        public int TotalNumber { get; set; }
        /// <summary>
        /// 实际人数
        /// </summary>
        public int ActualNumber { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 旅客订座记录编号
        /// </summary>
        public string PnrCode { get; set; }
    }
}
