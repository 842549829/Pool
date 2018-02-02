using ChinaPay.B3B.Service.CommandBuilder;
using ChinaPay.B3B.Service.CommandBuilder.Domain;
using ChinaPay.B3B.Service.CommandExecutor.Repository;
using ChinaPay.B3B.Service.PidManagement.Domain;

namespace ChinaPay.B3B.Service.CommandExecutor
{
    public class CommandExecutorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="user">用户</param>
        /// <returns>结果字串</returns>
        public static string Execute(Command command, User user)
        {
            var repository = Factory.CreateCommandRepository();
            var commandString = command.CommandString;
            return repository.Execute(commandString, user.Name, user.Password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandset">命令集合</param>
        /// <param name="user">用户</param>
        /// <returns>结果字串</returns>
        public static string Execute(CommandSet commandset, User user)
        {
            var repository = Factory.CreateCommandRepository();
            var commandString = commandset.CommandString;
            return repository.Execute(commandString, user.Name, user.Password);
        }
    }
}
