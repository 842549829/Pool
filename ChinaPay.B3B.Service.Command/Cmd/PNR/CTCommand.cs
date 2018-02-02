using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 建立联系组的航信指令。
    /// </summary>
    public class CTCommand: Command
    {
        /// <summary>
        /// 建立联系组的航信指令
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        public CTCommand(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
            Initialize();
        }

        /// <summary>
        /// 建立联系组的航信指令
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="transactionId">事务编号</param>
        public CTCommand(string phoneNumber, string transactionId)
        {
            this.commandType = CommandType.PNRCreation;
            this.phoneNumber = phoneNumber;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandString = string.Format("CT:{0}", phoneNumber);
        }

        // 联系电话；
        private string phoneNumber;
    }
}
