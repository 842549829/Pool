using System.Text;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    /// <summary>
    /// 邮箱转移指令
    /// </summary>
    class QcCommand : Command
    {
        public QcCommand(QueueType queueType)
        {
            _queueType = queueType;
            Initialize();
        }

        public QcCommand(QueueType queueType, string officeNo, string priority, bool isEndOperation)
        {
            _queueType = queueType;
            _officeNo = officeNo;
            _priority = priority;
            _isEndOperation = isEndOperation;
            Initialize();
        }

        private void Initialize()
        {
            var cmdStr = new StringBuilder();
            cmdStr.AppendFormat("QC:{0}", _queueType.ToString());
            if (_officeNo != null)
            {
                cmdStr.AppendFormat("/{0}", _officeNo);
            }
            if (_priority != null)
            {
                cmdStr.AppendFormat("/{0}", _priority);
            }
            if (_isEndOperation)
            {
                cmdStr.AppendFormat("/E");
            }

            CommandString = cmdStr.ToString();
        }

        // 信箱类型；
        private readonly QueueType _queueType;
        // 部门编号；
        private readonly string _officeNo;
        // 优先级；
        private readonly string _priority;
        // 退出选择；
        private readonly bool _isEndOperation;
    }
}
