using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    class QDCommand : Command
    {
        public QDCommand(bool isEndOperation)
        {
            this.isEndOperation = isEndOperation;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            this.commandString = string.Format("QD:{0}", isEndOperation ? "E" : "");
        }

        // 是否结束操作；
        private bool isEndOperation;
    }
}
