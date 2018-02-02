using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 预订后的被授权代理人编号信息。
    /// </summary>
    public class ReservedAuthorizeInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 代理人编号
        /// </summary>
        public string OfficeNo { get; set; }
    }
}
