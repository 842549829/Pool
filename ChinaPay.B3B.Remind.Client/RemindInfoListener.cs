using System.Collections.Generic;
using System.Text;

namespace ChinaPay.B3B.Remind.Client {
    class RemindInfoListener {
        private static RemindInfoListener m_instance = null;
        private static object m_locker = new object();
        public static RemindInfoListener Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new RemindInfoListener();
                        }
                    }
                }
                return m_instance;
            }
        }

        ChinaPay.Net.TcpProcessor m_processor = null;
        private RemindInfoListener() { }

        public void Start(System.Net.Sockets.TcpClient connection) {
            if(m_processor == null) {
                m_processor = new Net.TcpProcessor(connection);
                m_processor.DataReceived += m_processor_DataReceived;
                m_processor.ConnectionDisconnected += m_processor_ConnectionDisconnected;
                m_processor.StartReceive();
                m_processor.StartHeartBeat();
            }
        }
        public void Stop() {
            if(m_processor != null) {
                m_processor.Dispose();
                m_processor = null;
            }
        }

        void m_processor_DataReceived(object sender, Net.EventArgs.DataReceivedEventArgs e) {
            var dataContent = Encoding.GetEncoding("gb2312").GetString(e.Data);
            var remindRecords = parseRemindRecords(dataContent);
            if(remindRecords.Count > 0) {
                RemindForm.Instance.Show(remindRecords);
            }
        }
        void m_processor_ConnectionDisconnected(object sender, Net.EventArgs.ConnectionDisconnectedEventArgs e) {
            System.Windows.Forms.MessageBox.Show("与服务器断开连接");
            Program.Exit(false);
        }

        IList<Model.RemindRecord> parseRemindRecords(string content) {
            var result = new List<Model.RemindRecord>();
            var contentArray = content.Split('/');
            if(contentArray.Length == 2 && contentArray[0] == "REMIND") {
                if(!string.IsNullOrEmpty(contentArray[1])) {
                    var records = contentArray[1].Split('|');
                    foreach(var item in records) {
                        var array = item.Split('-');
                        if(array.Length == 2) {
                            int count;
                            string status; 
                            if(StatusForm.Instance.Statuses.TryGetValue(array[0], out status) && int.TryParse(array[1], out count)) {
                                result.Add(new Model.RemindRecord(status, count));
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}