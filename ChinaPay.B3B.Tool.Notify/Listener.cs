using System;

namespace ChinaPay.B3B.Tool.Notify {
    public class NotifyRecord {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }

    class NotifyRecordEventArgs : EventArgs {
        public NotifyRecord Record { get; set; }
    }

    abstract class Listener : IDisposable {
        /// <summary>
        /// 收到消息的通知
        /// </summary>
        public event EventHandler<NotifyRecordEventArgs> NotifyRecordReceived;

        /// <summary>
        /// 开始侦听
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 结束侦听
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record">消息记录</param>
        protected void OnNotifyRecordReceived(NotifyRecord record) {
            if(NotifyRecordReceived != null) {
                var args = new NotifyRecordEventArgs {
                    Record = record
                };
                NotifyRecordReceived(this, args);
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if(disposing) {
                Stop();
            }
        }
    }
}
