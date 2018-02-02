using System;
using System.Text;
using ChinaPay.Net;

namespace ChinaPay.B3B.Remind.Server {
    class RequestListner : System.IDisposable {
        private static RequestListner m_instance = null;
        private static object m_locker = new object();
        public static RequestListner Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new RequestListner();
                        }
                    }
                }
                return m_instance;
            }
        }

        TcpServer m_server = null;
        Encoding m_encoding = Encoding.GetEncoding("gb2312");
        readonly string m_listenPortSetting = "listenPort";

        private RequestListner() { }

        public bool Start() {
            if(m_server == null) {
                var portString = System.Configuration.ConfigurationManager.AppSettings[m_listenPortSetting];
                if(string.IsNullOrWhiteSpace(portString)) {
                    Console.WriteLine("缺少侦听端口:" + m_listenPortSetting);
                    return false;
                } else {
                    int port;
                    if(int.TryParse(portString, out port)) {
                        m_server = new TcpServer(port);
                        m_server.Connected += server_Connected;
                        m_server.Exception += server_Exception;
                        m_server.Start();
                        Console.WriteLine("成功开启侦听端口 " + portString);
                    } else {
                        Console.WriteLine("侦听端口格式 " + portString);
                        return false;
                    }
                }
            }
            return true;
        }
        public void Dispose() {
            if(m_server != null) {
                m_server.Dispose();
                m_server = null;
            }
            GC.SuppressFinalize(this);
        }

        void server_Connected(object sender, Net.EventArgs.ConnectedEventArgs e) {
            var processor = new Net.TcpProcessor(e.Client);
            processor.StartReceive();
            processor.DataReceived += processor_DataReceived;
        }
        void server_Exception(object sender, Net.EventArgs.ExceptionEventArgs e) {
            Program.ApplicationError(e.Exception == null ? "未知错误" : e.Exception.Message);
        }
        void processor_DataReceived(object sender, Net.EventArgs.DataReceivedEventArgs e) {
            var request = m_encoding.GetString(e.Data);
            var cmdProcessor = Command.CommandProcessor.GetCommandProcessor(request, e.Client);
            if(cmdProcessor != null) {
                var response = cmdProcessor.Execute();
                (sender as TcpProcessor).Send(m_encoding.GetBytes(response));
                if(!cmdProcessor.DisposeConnection) {
                    return;
                }
            }
            CustomGCCollection.Instance.Register(sender as TcpProcessor);
        }
    }
}