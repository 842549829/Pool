using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 取消PNR记录的航信指令（没有对参数有效性进行判断呢）
    /// </summary>
    public class XECommad : Command
    {
        /// <summary>
        /// 取消PNR记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">待取消记录的行号</param>
        public XECommad(List<int> lineNumbers)
        {
            this.lineNumbers = lineNumbers;
            Initialize();
        }

        /// <summary>
        /// 取消PNR记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">待取消记录的行号</param>
        /// <param name="transactionId">事务编号</param>
        public XECommad(List<int> lineNumbers, string transactionId)
        {
            this.lineNumbers = lineNumbers;
            this.transactionId = transactionId;            
            Initialize();
        }
        
        private void Initialize()
        {
            this.commandType = CommandType.PNRModification;
            StringBuilder cmdStr = new StringBuilder("XE:");

            foreach (int item in lineNumbers)
            {
                cmdStr.Append(item.ToString() + "/");
            }

            this.commandString = cmdStr.ToString().TrimEnd("/".ToCharArray());
        }
        
        // 待取消记录的行号
        private List<int> lineNumbers;
    }
}
