using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.SystemOperation
{
    class PNCommand : Command
    {
        /// <summary>
        /// 向后翻页
        /// </summary>
        /// <param name="commandType">指令类型</param>
        /// <param name="transactionId">事务编号</param>
        public PNCommand(CommandType commandType, string transactionId)
        {
            this.commandType = commandType;
            this.transactionId = transactionId;
            Initialize();
        }



        private void Initialize()
        {
            this.commandString = "PN";
        }
    }
}
