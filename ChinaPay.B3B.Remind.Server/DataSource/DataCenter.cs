using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ChinaPay.B3B.Service.Remind.Model;

namespace ChinaPay.B3B.Remind.Server.DataSource {
    class DataCenter {
        static DataCenter m_instance = null;
        static object m_locker = new object();
        public static DataCenter Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new DataCenter();
                        }
                    }
                }
                return m_instance;
            }
        }

        private Thread m_thread = null;
        private volatile bool m_stop = true;
        private DataCenter() { }

        public IEnumerable<IGrouping<Guid, RemindInfo>> RemindDatas {
            get;
            private set;
        }
        public void Start() {
            if(m_stop) {
                m_thread = new Thread(run) {
                    IsBackground = true
                };
                m_thread.Start();
            }
        }
        public void Stop() {
            m_stop = true;
            m_thread = null;
        }
        void run() {
            while(m_stop) {
                queryData();
                Thread.Sleep(5000);
            }
        }
        void queryData() {
            try {
                var remindInfos = Service.Remind.OrderRemindService.Query();
                this.RemindDatas = remindInfos.GroupBy(item => item.Acceptor);
            } catch(Exception ex) {
                Service.LogService.SaveExceptionLog(ex);
                Console.WriteLine("获取提醒数据出错");
                Console.WriteLine("错误原因:" + ex.Message);
            }
        }
    }
}