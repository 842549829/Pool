using System;
using System.Windows.Forms;

namespace ChinaPay.B3B.Remind.Client {
    static class Program {
        public static Model.LogonInfo LogonInfo {
            get;
            set;
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(IsRuned()) {
                MessageBox.Show("软件已运行，不能重复启动");
                return;
            }
            RemindForm.Init();
            Application.Run(LogonForm.Instance);
        }
        public static void Logoff() {
            var request = new Command.Logoff(Settings.ServerHost, Settings.ServerPort, Program.LogonInfo.BatchNo);
            request.Execute();
            RemindInfoListener.Instance.Stop();
            LogonInfo = null;
        }
        public static void Exit(bool isNormal) {
            if(isNormal) {
                Logoff();
            }
            Application.ExitThread();
            Application.Exit();
        }
        static bool IsRuned() {
            var processName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            return System.Diagnostics.Process.GetProcessesByName(processName).Length > 1;
        }
    }
}