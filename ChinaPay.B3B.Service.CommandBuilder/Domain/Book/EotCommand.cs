namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 订座封口指令
    /// </summary>
    public class EotCommand: Command
    {
        /// <summary>
        /// 封口指令
        /// </summary>
        public EotCommand()
        {
            Initialize();
	    }
        private void Initialize()
        {
            CommandString = string.Format("@");
        }
    }
}