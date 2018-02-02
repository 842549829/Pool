using ChinaPay.B3B.Service.CommandExecutor.Repository.Ve;

namespace ChinaPay.B3B.Service.CommandExecutor.Repository
{
    class Factory
    {
        public static ICommandExecutorRepository CreateCommandRepository()
        {
            return new CommandExecutorRepository();
        }
    }
}
