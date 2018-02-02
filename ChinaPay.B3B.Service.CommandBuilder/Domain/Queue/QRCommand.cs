namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    /// <summary>
    /// 重新显示当前邮箱的航信指令
    /// </summary>
    class QrCommand : Command
    {
        public QrCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            CommandString = string.Format("QR:");
        }
    }
}
