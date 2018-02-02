using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    /// <summary>
    /// 邮箱转移指令
    /// </summary>
    class QCCommand : Command
    {
        public QCCommand(QueueType queueType)
        {
            this.queueType = queueType;
            Initialize();
        }

        public QCCommand(QueueType queueType, string officeNo, string priority, bool isEndOperation)
        {
            this.queueType = queueType;
            this.officeNo = officeNo;
            this.priority = priority;
            this.isEndOperation = isEndOperation;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            StringBuilder cmdStr = new StringBuilder();
            cmdStr.AppendFormat("QC:{0}", queueType.ToString());
            if (officeNo != null)
            {
                cmdStr.AppendFormat("/{0}", officeNo);
            }
            if (priority != null)
            {
                cmdStr.AppendFormat("/{0}", priority);
            }
            if (isEndOperation)
            {
                cmdStr.AppendFormat("/E");
            }

            this.commandString = cmdStr.ToString();
        }
        // 信箱类型；
        private QueueType queueType;
        // 部门编号；
        private string officeNo;
        // 优先级；
        private string priority;
        // 退出选择；
        private bool isEndOperation;
    }
}
