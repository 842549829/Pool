using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.SystemOperation
{
    class PBCommand : Command
    {
        /// <summary>
        /// 向上翻页
        /// </summary>
        /// <param name="commandType">指令类型</param>
        /// <param name="transactionId">事务编号</param>
        public PBCommand(CommandType commandType, string transactionId)
        {
            this.commandType = commandType;
            this.transactionId = transactionId;
            Initialize();
        }

        public PBCommand()
        {
            Initialize();
        }


        private void Initialize()
        {
            this.commandString = "PB";
        }
    }
}
