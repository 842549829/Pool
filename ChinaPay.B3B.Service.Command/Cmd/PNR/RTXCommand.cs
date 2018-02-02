using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 提取PNR的航信指令
    /// </summary>
    public class RTXCommand : Command
    {
        /// <summary>
        /// 提取PNR的航信指令
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        public RTXCommand(string pnrCode)
        {
            this.commandType = CommandType.PNRExtraction;
            this.pnrCode = pnrCode;
            this.commandString = string.Format("RT:X/{0}", pnrCode);
            Initialize();
        }

        /// <summary>
        /// 提取PNR的航信指令
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        /// <param name="transactionId">事务编号</param>
        public RTXCommand(string pnrCode, string transactionId)
        {
            this.transactionId = transactionId;
            this.pnrCode = pnrCode;
            this.commandString = string.Format("RT:X/{0}", pnrCode);
            Initialize();
        }
        
        private void Initialize()
        {
            this.returnType = ReturnResultType.All;
        }

        // 旅客订座记录编号
        private string pnrCode;
    }
}
