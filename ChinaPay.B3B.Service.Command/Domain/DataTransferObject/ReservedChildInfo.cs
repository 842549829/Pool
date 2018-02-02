using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 儿童信息
    /// </summary>
    public class ReservedChildInfo
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 乘客编号
        /// </summary>
        public int PassengerId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Birthday { get; set; }
    }
}
