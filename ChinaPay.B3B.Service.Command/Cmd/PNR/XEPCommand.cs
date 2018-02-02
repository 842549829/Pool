using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 取消PNR旅客记录的航信指令
    /// </summary>
    public class XEPCommad : Command
    {
        /// <summary>
        /// 取消PNR旅客记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">旅客对应的行号</param>
        public XEPCommad(List<int> lineNumbers)
        {
            this.lineNumbers = lineNumbers;
            Initialize();
        }

        /// <summary>
        /// 取消PNR旅客记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">待取消旅客对应的行号</param>
        /// <param name="transactionId">事务编号</param>
        public XEPCommad(List<int> lineNumbers, string transactionId)
        {
            this.transactionId = transactionId;
            this.lineNumbers = lineNumbers;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRModification;
            StringBuilder cmdStr = new StringBuilder("XEP");
            foreach (int item in lineNumbers)
            {
                cmdStr.Append(item.ToString() + "/");
            }
            this.commandString = cmdStr.ToString().TrimEnd("/".ToCharArray());
        }

        // 待取消旅客对应的行号
        private List<int> lineNumbers;
    }
}
