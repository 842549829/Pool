using ChinaPay.B3B.Service.Command.Repository.Ve;

namespace ChinaPay.B3B.Service.Command.Repository
{
    class Factory
    {
        public static ICommandRepository CreateCommandRepository()
        {
            return new CommandRepository();
        }

        /// <summary>
        /// 暂时用一下；
        /// </summary>
        /// <returns></returns>
        public static ICommandRepository CreateVeWebCommandRepository()
        {
            return new ChinaPay.B3B.Service.Command.Repository.VeWeb.CommandRepository();
        }
    }
}
