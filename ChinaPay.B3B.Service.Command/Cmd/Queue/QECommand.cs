using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Queue
{
    /// <summary>
    /// 邮箱发送的航行指令
    /// </summary>
    class QECommand : Command
    {
        public QECommand(QueueType queueType,  string officeNo, string priority, string content)
        {
            this.queueType = queueType;
            this.officeNo = officeNo;
            this.priority = priority;
            this.content = content;
            Initialize();
        }

        public QECommand(QueueType queueType, string officeNo, string pnrCode, DateTime datetime, string priority)
        {
            this.queueType = queueType;
            this.officeNo = officeNo;
            this.pnrCode = pnrCode;
            this.datetime = datetime;
            this.priority = priority;
            Initialize();
        }


        private void Initialize()
        {
            this.commandType = CommandType.Queue;
            StringBuilder cmdStr = new StringBuilder();
            cmdStr.AppendFormat("QE:{0}", queueType.ToString());
            if (officeNo != null)
            {
                cmdStr.AppendFormat("/{0}", officeNo);
            }
            if (priority != null)
            {
                cmdStr.AppendFormat("/{0}", priority);
            }

            this.commandString = cmdStr.ToString();
        }

        private string content;
        // 信箱类型；
        private QueueType queueType;
        // 部门编号；
        private string officeNo;
        // 优先级；
        private string priority;
        // 
        private string pnrCode;
        private DateTime datetime;
    }
}
