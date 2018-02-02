using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.Queue
{
    /// <summary>
    /// 查看信箱的航信指令
    /// </summary>
    class QTCommand : Command
    {
        /// <summary>
        /// 查看信箱的航信指令
        /// </summary>
        public QTCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            this.commandString = "QT:";
        }
    }
}
