using System;
using System.Text;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    /// <summary>
    /// 邮箱发送的航行指令
    /// </summary>
    class QeCommand : Command
    {
        public QeCommand(QueueType queueType,  string officeNo, string priority, string content)
        {
            _queueType = queueType;
            _officeNo = officeNo;
            _priority = priority;
            _content = content;
            Initialize();
        }

        public QeCommand(QueueType queueType, string officeNo, string pnrCode, DateTime datetime, string priority)
        {
            _queueType = queueType;
            _officeNo = officeNo;
            _pnrCode = pnrCode;
            _datetime = datetime;
            _priority = priority;
            Initialize();
        }


        private void Initialize()
        {
            var cmdStr = new StringBuilder();
            cmdStr.AppendFormat("QE:{0}", _queueType.ToString());
            if (_officeNo != null)
            {
                cmdStr.AppendFormat("/{0}", _officeNo);
            }
            if (_priority != null)
            {
                cmdStr.AppendFormat("/{0}", _priority);
            }

            CommandString = cmdStr.ToString();
        }

        // 信箱类型；
        private readonly QueueType _queueType;
        // 部门编号；
        private readonly string _officeNo;
        // 优先级；
        private readonly string _priority;
        private readonly string _content;
        // 
        private string _pnrCode;
        private DateTime _datetime;
    }
}
