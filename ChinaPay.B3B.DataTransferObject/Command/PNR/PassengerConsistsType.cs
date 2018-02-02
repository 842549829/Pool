using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    /// <summary>
    /// 旅客组成类型
    /// </summary>
    public enum PassengerConsistsType
    {
        /// <summary>
        /// 散客
        /// </summary>
        [Description("散客")]
        Individual,
        /// <summary>
        /// 团体
        /// </summary>
        [Description("团体")]
        Group
    }
}
