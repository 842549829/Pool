using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.CommandExecutor.VeWebReference;

namespace ChinaPay.B3B.Service.CommandExecutor.Repository.Ve
{
    internal class CommandExecutorRepository : ICommandExecutorRepository
    {
        private static string ReplaceUrl(string url)
        {
            return Regex.Replace(url, "http://.*?/VeShareWebService", "http://116.55.243.38:56065/VeShareWebService");
        }
        /// <summary>
        /// 使用共享系统的用户名密码，向航信系统发送指令字串，并得到执行结果。
        /// </summary>
        /// <param name="commandString">指令字串</param>
        /// <param name="userName">用户名</param>
        /// <param name="userPassword">密码</param>
        /// <returns>结果字串</returns>
        public string Execute(string commandString, string userName, string userPassword)
        {
            var service = new VeShareWebServiceService();
            service.Url = ReplaceUrl(service.Url);
            return service.ProcessRequest(commandString, userName, userPassword);

        }
    }
}
