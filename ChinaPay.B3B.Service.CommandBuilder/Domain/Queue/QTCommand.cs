namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    /// <summary>
    /// 查看信箱的航信指令
    /// </summary>
    public class QtCommand : Command
    {
        /// <summary>
        /// 查看信箱的航信指令
        /// </summary>
        public QtCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            CommandString = "QT";
        }
    }
}
