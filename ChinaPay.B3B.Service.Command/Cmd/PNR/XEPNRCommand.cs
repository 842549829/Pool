using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 取消整个PNR的航信指令
    /// </summary>
    public class XEPNRCommad : Command
    {
        /// <summary>
        /// 取消整个PNR的航信指令
        /// </summary>
        public XEPNRCommad()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRModification;
            this.commandString = "XEPNR@";
        }
    }
}
