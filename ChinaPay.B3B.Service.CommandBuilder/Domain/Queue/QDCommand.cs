namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Queue
{
    public class QdCommand : Command
    {
        public QdCommand(string option)
        {
            _option = option;
            Initialize();
        }

        private void Initialize()
        {
            CommandString = string.Format("QD{0}", _option);
        }

        // 是否结束操作；
        private readonly string _option;
    }
}
