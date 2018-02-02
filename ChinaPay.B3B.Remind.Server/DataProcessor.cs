using System.Threading;

namespace ChinaPay.B3B.Remind.Server {
    class DataProcessor {
        static DataProcessor m_instance = null;
        static object m_locker = new object();
        public static DataProcessor Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new DataProcessor();
                        }
                    }
                }
                return m_instance;
            }
        }

        volatile bool m_stop = true;
        Thread m_thread = null;

        private DataProcessor() { }

        public void Start() {
            if(m_stop) {
                m_stop = false;
                m_thread = new Thread(run) {
                    IsBackground = true
                };
                m_thread.Start();
                DataSource.DataCenter.Instance.Start();
                System.Console.WriteLine("开始数据提醒处理...");
            }
        }
        public void Stop() {
            m_stop = true;
            m_thread = null;
            DataSource.DataCenter.Instance.Stop();
        }

        void run() {
            while(!m_stop) {
                var remindDatas = DataSource.DataCenter.Instance.RemindDatas;
                if(remindDatas != null) {
                    foreach(var item in remindDatas) {
                        var company = LogonCenter.Instance.GetCompany(item.Key);
                        if(company != null) {
                            try {
                                company.Remind(item);
                            } catch { }
                            Thread.Sleep(10);
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}