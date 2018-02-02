using System;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.Tool.Notify {
    class Program {
        static void Main(string[] args) {
            Program.Print("通知程序启动中...");
            //定义监听者对象
            var listener = new MQListener(System.Configuration.ConfigurationManager.AppSettings["QueuePath"]);
            //当监听者得到接收到消息的通知，则触发Inject事件。
            listener.NotifyRecordReceived += (sender, eventArgs) => Processor.Instance.Inject(eventArgs.Record);
            try
            {
                //开始接收消息
                listener.Start();
                Program.Print("通知程序启动成功...");
            }
            catch(Exception ex)
            {
                Program.Print("通知程序启动失败..." + Environment.NewLine + "失败原因：" + ex.Message);
            }

            while(true) {
                var input = Console.ReadLine().ToLower();
                if(input == "start") {
                    listener.Start();
                } else if(input == "stop") {
                    listener.Stop();
                } else if(input == "cls") {
                    Console.Clear();
                } else if(input == "exit") {
                    break;
                } else {
                    Console.WriteLine("无效指令");
                }
            }
            //停止接收消息
            listener.Stop();
        }

        /// <summary>
        /// 打印信息
        /// </summary>
        /// <param name="message">需要打印的信息内容</param>
        public static void Print(string message) {
            Console.WriteLine(DateTime.Now.ToString("yy-MM-dd HH:mm:ss fff") + Environment.NewLine + message + Environment.NewLine);
            LogService.SaveTextLog(message+"\r\n");
        }
    }
}