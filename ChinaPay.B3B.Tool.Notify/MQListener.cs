using System;
using System.Messaging;
using System.Threading;

namespace ChinaPay.B3B.Tool.Notify {
    class MQListener : Listener {
        private volatile bool _running = false;
        private MessageQueue _queue = null;

        public MQListener(string path) {
            Path = path;
        }

        /// <summary>
        /// 获取 队列路径
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 开始接收消息
        /// </summary>
        public override void Start() {
            if(string.IsNullOrWhiteSpace(Path)) throw new InvalidOperationException("缺少队列路径");
            _running = true;
            if(_queue == null) {
                if (MessageQueue.Exists(Path))
                {
                    _queue = new MessageQueue(Path, QueueAccessMode.Receive);
                }
                else
                {
                    _queue = MessageQueue.Create(Path);
                }
                if(!_queue.CanRead) throw new InvalidOperationException("队列不能读取");

                _queue.DenySharedReceive = true;
                _queue.Formatter = new XmlMessageFormatter(new[] { typeof(NotifyRecord) });

                ThreadPool.QueueUserWorkItem(runReceiveMessage, null);
            }
        }

        /// <summary>
        /// 停止接收消息
        /// </summary>
        public override void Stop() {
            _running = false;
            if(_queue != null) {
                _queue.Close();
                _queue.Dispose();
                _queue = null;
            }
        }

        /// <summary>
        /// 循环接收消息队列中的消息
        /// </summary>
        /// <param name="state"></param>
        private void runReceiveMessage(object state) {
            while(_running) {
                var mq = _queue.Receive();

                OnNotifyRecordReceived(mq.Body as NotifyRecord);

                Thread.Sleep(500);
            }
        }

        public override string ToString() {
            return "消息队列侦听";
        }
    }
}