using System.Messaging;

namespace ChinaPay.B3B.Service.Order.Notify
{
    internal class OrderSender
    {
        private static OrderSender _instance;
        private static object _locker = new object();
        public static OrderSender Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new OrderSender();
                        }
                    }
                }
                return _instance;
            }
        }

        private string _path;
        private MessageQueue _messageQueue;

        private OrderSender()
        {
            _path = System.Configuration.ConfigurationManager.AppSettings["NotifyQueuePath"];
        }

        public void Send(decimal orderId, string type, string notifyUrl)
        {
            var view = new NotifyRecord { Id = orderId, Type = type, Content = notifyUrl };

            if (_messageQueue == null)
            {
                _messageQueue = new MessageQueue(_path, QueueAccessMode.Send)
                {
                    Formatter = new XmlMessageFormatter(new[] { typeof(NotifyRecord) })
                };
            }
            _messageQueue.Send(new Message
            {
                Recoverable = true,
                Body = view,
            });
        }
    }
}
