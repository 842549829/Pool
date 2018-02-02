using System;
using System.Timers;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Tool.Tradement {
    class RefundProcessor {
        private static RefundProcessor _instance = null;
        private static object _locker = new object();
        public static RefundProcessor Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new RefundProcessor();
                        }
                    }
                }
                return _instance;
            }
        }

        private Timer _timer = null;
        private volatile bool _run = false;

        private RefundProcessor() {
        }

        public void Start() {
            if(_timer == null) {
                _run = true;
                _timer = new Timer() {
                    Interval = 60 * 60 * 1000
                };
                _timer.Elapsed += (sender, e) => process();
                _timer.Start();
                showMessage("开始处理");
                process();
            }
        }

        private void process() {
            showMessage("开始下一批处理");
            var refundInfo = Service.OrderQueryService.QueryRefundFailedRecords();
            foreach(var item in refundInfo) {
                if(!_run) break;
                showMessage("开始处理单号:" + item.ApplyformId.ToString() + " 业务类型:" + item.BusinessType.GetDescription() + "...");
                try {
                    Service.OrderProcessService.ProcessRefundFailedRecord(item.ApplyformId);
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
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 退款处理\n\t" + message);
        }
    }
}
