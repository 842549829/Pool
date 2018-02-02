using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.PNR
{

    /// <summary>
    /// 提取PNR的航信指令，这个东西现在只处理小编码，同时它由C系统执行；
    /// </summary>
    public class RTCommand : Command
    {
        /// <summary>
        /// 提取PNR的航信指令
        /// </summary>
        public RTCommand()
        {
            this.commandString = "RT:";
            Initialize();
        }

        /// <summary>
        /// 提取PNR的航信指令
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        public RTCommand(string pnrCode, CommandType commandType = CommandType.PNRModification, ReturnResultType returnType = ReturnResultType.Single)
        {
            this.commandType = commandType;
            this.returnType = returnType;
            this.pnrCode = pnrCode;
            this.commandString = string.Format("RT:{0}", pnrCode);
            Initialize();
        }

        /// <summary>
        /// 提取PNR的航信指令
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        /// <param name="transactionId">事务编号</param>
        public RTCommand(string pnrCode, string transactionId)
        {
            this.transactionId = transactionId;
            this.pnrCode = pnrCode;
            Initialize();
        }
        
        private void Initialize()
        {
            this.commandString = string.Format("RT:{0}", pnrCode);
        }

        // 旅客订座记录编号
        private string pnrCode;
    }
}
