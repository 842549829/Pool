using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    /// <summary>
    /// 显示特定类型信箱内容
    /// </summary>
    class QSCommand: Command
    {
        public QSCommand(QueueType queueType)
        {
            this.queueType = queueType;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            this.commandString = string.Format("QS:{0}", this.queueType.ToString());
        }

        private QueueType queueType;
    }
}
