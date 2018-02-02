using System;
using System.Timers;

namespace ChinaPay.B3B.Tool.Tradement {
    class RoyaltyProcessor {
        private static RoyaltyProcessor _instance = null;
        private static object _locker = new object();
        public static RoyaltyProcessor Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new RoyaltyProcessor();
                        }
                    }
                }
                return _instance;
            }
        }

        private Timer _timer = null;
        private volatile bool _run = false;

        private RoyaltyProcessor() {
        }

        public void Start() {
            if(_timer == null) {
                _run = true;
                _timer = new Timer() {
                    Interval = 30 * 60 * 1000
                };
                _timer.Elapsed += (sender, e) => process();
                _timer.Start();
                showMessage("开始处理");
                process();
            }
        }

        private void process() {
            showMessage("开始下一批处理");
            var royaltyInfo = Service.OrderQueryService.QueryRoyaltyFailedRecords();
            foreach(var item in royaltyInfo) {
                if(!_run) break;
                showMessage("开始处理订单:" + item.ToString() + "...");
                try {
                    Service.OrderProcessService.ProcessRoyaltyFailedOrder(item);
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
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 分润处理\n\t" + message);
        }
    }
}