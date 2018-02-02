using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Server {
    class CustomGCCollection : IDisposable {
        static CustomGCCollection m_instance = null;
        static object m_locker = new object();
        public static CustomGCCollection Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new CustomGCCollection();
                        }
                    }
                }
                return m_instance;
            }
        }

        volatile bool m_recycleMainQueue = false;
        Queue<ChinaPay.Net.TcpProcessor> m_mainQueue, m_assitantQueue;
        System.Timers.Timer m_recycleTimer = null;
        private CustomGCCollection() {
            m_mainQueue = new Queue<Net.TcpProcessor>();
            m_assitantQueue = new Queue<Net.TcpProcessor>();
        }
        private void Start() {
            if(m_recycleTimer == null) {
                m_recycleTimer = new System.Timers.Timer(5000);
                m_recycleTimer.Elapsed += delegate {
                    m_recycleMainQueue = !m_recycleMainQueue;
                    recycleQueue(m_recycleMainQueue ? m_mainQueue : m_assitantQueue);
                };
                m_recycleTimer.Start();
            }
        }
        public void Register(ChinaPay.Net.TcpProcessor item) {
            Start();
            if(item != null) {
                var queue = m_recycleMainQueue ? m_assitantQueue : m_mainQueue;
                queue.Enqueue(item);
            }
        }
        public void Dispose() {
            if(m_recycleTimer != null) {
                m_recycleTimer.Stop();
                m_recycleTimer.Dispose();
                m_recycleTimer = null;
            }
            recycleQueue(m_mainQueue);
            recycleQueue(m_assitantQueue);
            GC.SuppressFinalize(this);
        }
        void recycleQueue(Queue<ChinaPay.Net.TcpProcessor> queue) {
            while(queue.Count > 0) {
                var item = queue.Dequeue();
                item.Dispose();
            }
        }
    }
}