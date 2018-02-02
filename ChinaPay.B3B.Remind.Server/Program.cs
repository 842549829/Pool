using System;
using System.Reflection;
using System.Text;

namespace ChinaPay.B3B.Remind.Server {
    class Program {
        static void Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => {
                var ex = e.ExceptionObject as Exception;
                ApplicationError(ex == null ? "未知错误" : ex.Message);
                if(ex != null) {
                    Service.LogService.SaveExceptionLog(ex, "订单提醒系统错误");
                }
            };
            Booting();
            string message;
            if(BootServer(out message)) {
                BootSuccessed();
                EnterCommandMode();
            } else {
                BootFailed(message);
            }
        }
        static void Booting() {
            Console.Title = "订单提醒服务器端";
            Console.WriteLine("启动中...");
        }
        static void BootSuccessed() {
            Console.WriteLine("启动成功");
            Console.WriteLine("    启动时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("    版本号:   " + GetVersion());
            WriteLine();
        }
        static void BootFailed(string reason) {
            Console.WriteLine("启动失败" + Environment.NewLine + "失败原因:" + reason);
        }
        public static void ApplicationError(string message) {
            Console.WriteLine("程序出错" + Environment.NewLine + "错误信息:" + message);
        }
        static bool BootServer(out string message) {
            try {
                RequestListner.Instance.Start();
            } catch(Exception ex) {
                message = ex.Message;
                return false;
            }
            message = string.Empty;
            return true;
        }

        static void EnterCommandMode() {
            Console.WriteLine("目前支持命令:");
            ShowCommands();
            WriteLine();
            while(true) {
                var inputString = Console.ReadLine().ToLower();
                switch(inputString) {
                    case "cls":
                        Console.Clear();
                        break;
                    case "exit":
                        RequestListner.Instance.Dispose();
                        DataProcessor.Instance.Stop();
                        LogonCenter.Instance.Dispose();
                        CustomGCCollection.Instance.Dispose();
                        return;
                    case "company":
                        ShowCompanies();
                        break;
                    case "user":
                        ShowUsers();
                        break;
                    case "help":
                        ShowCommands();
                        break;
                    default:
                        Console.WriteLine("不存在的命令");
                        break;
                }
            }
        }
        static void ShowCommands() {
            Console.WriteLine("    help       帮助");
            Console.WriteLine("    cls        清屏");
            Console.WriteLine("    exit       退出");
            Console.WriteLine("    company    查询当前登录单位");
            Console.WriteLine("    user       查看当前登录用户");
        }
        static void ShowCompanies() {
            Program.WriteLine();
            var companies = new StringBuilder();
            companies.Append("    名称                    登录数");
            foreach(var item in LogonCenter.Instance.Companies) {
                companies.Append(Environment.NewLine);
                companies.AppendFormat("    {0}                    {1}", item.Name, item.MemberCount);
            }
            Console.WriteLine(companies);
            Program.WriteLine();
        }
        static void ShowUsers() {
            Program.WriteLine();
            var users = new StringBuilder();
            users.Append("    账号               时间              地址                  单位");
            foreach(var item in LogonCenter.Instance.Users) {
                users.Append(Environment.NewLine);
                users.AppendFormat("    {0}       {1}      {2}         {3}", item.Name, item.Time.ToString("yyyy-MM-dd HH:mm:ss"), item.Address, item.Owner.Name);
            }
            Console.WriteLine(users);
            Program.WriteLine();
        }
        static void WriteLine() {
            Console.WriteLine("-------------------------------------------------------------------------------");
        }
        static string GetVersion() {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var versionAttribute = Attribute.GetCustomAttribute(currentAssembly, typeof(AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute;
            return versionAttribute.Version;
        }
    }
}