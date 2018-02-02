using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Interface {
    internal class InvokeStatistic {
        private static InvokeStatistic _instance = null;
        private static object _locker = new object();

        public static InvokeStatistic Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new InvokeStatistic();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, DateTime> _statistics = null;
        private InvokeStatistic() {
            _statistics = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// 按接口、用户 和 IP地址进行控制
        /// 访问间隙不能小于0.1秒
        /// </summary>
        public void Save(string interfaceName, string userName, string ip) {
            var accessable = true;
            try {
                var key = string.Format("{0}|{1}|{2}", interfaceName, ip, userName);
                DateTime prevInvokeTime;
                if(_statistics.TryGetValue(key, out prevInvokeTime)) {
                    if((DateTime.Now - prevInvokeTime).TotalMilliseconds < 100) {
                        accessable = false;
                    } else {
                        _statistics[key] = DateTime.Now;
                    }
                } else {
                    _statistics.Add(key, DateTime.Now);
                }
            } catch(Exception ex) {
                Service.LogService.SaveExceptionLog(ex, "接口调用频率控制");
            }
            if(!accessable) {
                throw new InterfaceInvokeException("80");
            }
        }
        /// <summary>
        /// 按接口、用户 和 IP地址进行控制
        /// 访问间隙不能小于0.1秒
        /// </summary>
        public void Save(RequestContext context) {
            Save(context.Service, context.UserName, context.ClientIP);
        }
    }
}