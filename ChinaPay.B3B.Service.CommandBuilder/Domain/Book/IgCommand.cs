namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    class IgCommand : Command
    {
        /// <summary>
        /// 取消指令；
        /// </summary>
        public IgCommand()
        {
            Initialize();
        }

        private void Initialize()
        {
            CommandString = "IG";
        }
    }
}
