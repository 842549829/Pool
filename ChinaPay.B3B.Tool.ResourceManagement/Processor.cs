using System;
using System.Timers;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Tool.ResourceManagement {
    class Processor {
        private static Processor _instance = null;
        private static object _locker = new object();
        public static Processor Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new Processor();
                        }
                    }
                }
                return _instance;
            }
        }

        private Timer _timer = null;
        private volatile bool _run = false;

        private Processor() {
        }

        public void Start() {
            if(_timer == null) {
                _run = true;
                _timer = new Timer() {
                    Interval = 5 * 60 * 1000
                };
                _timer.Elapsed += (sender, e) => process();
                _timer.Start();
                showMessage("开始处理");
                process();
            }
        }

        private void process() {
            showMessage("开始下一批处理");
            var condition = new OrderQueryCondition {
                Status = OrderStatus.Ordered,
                ProducedDateRange = new Core.Range<DateTime>(DateTime.Today.AddYears(-1), DateTime.Now.AddMinutes(-5))
            };
            var pagination = new Pagination {
                GetRowCount = false,
                PageIndex = 1,
                PageSize = int.MaxValue
            };
            var orders = Service.OrderQueryService.QueryOrders(condition, pagination);
            foreach(var order in orders) {
                if(!_run) break;
                showMessage("开始处理[" + order.ProductType.GetDescription() + "]订单[" + order.OrderId + "]...");
                try {
                    Service.OrderProcessService.ProcessWaitForPayOrder(order.OrderId);
                    showMessage("处理成功");
                } catch(Exception ex) {
                    showMessage("处理失败" + Environment.NewLine + "原因:" + ex.Message);
                }
                System.Threading.Thread.Sleep(50);
            }
            showMessage("当前批次处理结束");
        }

        public void Stop() {
            _run = false;
            if(_timer != null) {
                _timer.Stop();
                _timer.Close();
                _timer = null;
                showMessage("停止处理");
            }
        }
        private void showMessage(string message) {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 位置处理\n\t" + message);
        }
    }
}