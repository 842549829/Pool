
namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    /// <summary>
    /// 显示特定类型信箱内容
    /// </summary>
    public class QsCommand: Command
    {
        public QsCommand(QueueType queueType)
        {
            _queueType = queueType;
            Initialize();
        }

        private void Initialize()
        {
            CommandString = string.Format("QS:{0}", _queueType.ToString());
        }

        private readonly QueueType _queueType;
    }
}