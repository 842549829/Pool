
namespace ChinaPay.B3B.Service.CommandExecutor.Repository
{
    /// <summary>
    /// 命令执行器接口
    /// </summary>
    interface ICommandExecutorRepository
    {
        string Execute(string commandString, string userName, string userPassword);
    }
}
