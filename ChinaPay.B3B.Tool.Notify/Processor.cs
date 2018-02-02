using System;
using System.Collections.Generic;
using System.Threading;

namespace ChinaPay.B3B.Tool.Notify
{
    internal class Processor
    {
        private static Processor _instance = null;
        private static readonly object _locker = new object();
        private static readonly int _timeout = Core.Extension.StringExtension.ToInt(System.Configuration.ConfigurationManager.AppSettings["Timeout"], 3) * 1000;

        public static Processor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new Processor();
                        }
                    }
                }
                return _instance;
            }
        }

        //失败记录的线程
        private readonly FailedRecordProcessor _failedProcessor = null;

        private Processor()
        {
            _failedProcessor = new FailedRecordProcessor();
        }

        /// <summary>
        /// 处理消息方法
        /// </summary>
        /// <param name="record">消息</param>
        public void Inject(NotifyRecord record)
        {
            if (record == null) return;
            _failedProcessor.Remove(record.Id);

            DateTime requestTime;
            if (!send(record, 1, out requestTime))
            {
                _failedProcessor.Add(record, requestTime);
            }
        }

        /// <summary>
        /// 判断消息是否发送成功
        /// </summary>
        /// <param name="record">消息记录</param>
        /// <returns>成功：true；失败：false</returns>
        private static bool send(NotifyRecord record, int times, out DateTime requestTime)
        {
            requestTime = DateTime.Now;
            var notifyResult = Utility.HttpRequestUtility.GetHttpResult(record.Content, _timeout);
            var responseTime = DateTime.Now;
            var success = notifyResult == "0";
            saveLog(record, requestTime, notifyResult, responseTime, success, times);
            return success;
        }

        /// <summary>
        /// 将发送消息的情况记录日志，并在控制台打印相应记录
        /// </summary>
        /// <param name="record">消息记录</param>
        /// <param name="requestTime">请求时间</param>
        /// <param name="notifyResult">消息内容</param>
        /// <param name="responseTime">响应时间</param>
        /// <param name="success">发送是否成功</param>
        private static void saveLog(NotifyRecord record, DateTime requestTime, string notifyResult, DateTime responseTime, bool success, int times)
        {
            var log = new Service.Log.Domain.NotifyLog
            {
                OrderId = record.Id,
                Type = record.Type,
                Request = record.Content,
                RequestTime = requestTime,
                Response = notifyResult,
                ResponseTime = responseTime,
                Success = success
            };
            Service.LogService.SaveNotifyLog(log);

            var message = string.Format("业务单号:{1} 类型:{2}{0}请求时间:{3} 请求次数:{4}{0}请求内容:{5}{0}响应时间:{6} 响应内容:{7}{0}处理状态:{8}",
                Environment.NewLine, log.OrderId, log.Type, log.RequestTime.ToString("HH:mm:ss"), times, log.Request, log.ResponseTime.ToString("HH:mm:ss"), log.Response, success);
            Program.Print(message);
        }

        private class FailedRecordProcessor
        {
            private readonly object _recordLocker = new object();
            //定义存放失败记录的一个字典
            private readonly Dictionary<decimal, FailedRecord> _records = new Dictionary<decimal, FailedRecord>();

            /// <summary>
            /// 添加发送失败的消息记录，并开启该线程
            /// </summary>
            /// <param name="record">消息记录</param>
            public void Add(NotifyRecord record, DateTime failedRequestTime)
            {
                if (record == null) return;
                lock (_recordLocker)
                {
                    if (_records.ContainsKey(record.Id)) return;
                    var failedRecord = new FailedRecord
                    {
                        Record = record,
                        Times = 1,
                        Container = this,
                        LastTime = failedRequestTime
                    };
                    _records.Add(record.Id, failedRecord);
                    failedRecord.Start();
                }
            }

            /// <summary>
            /// 移除指定Id的消息记录，并中断该线程
            /// </summary>
            /// <param name="id">消息记录的Id</param>
            public void Remove(decimal id)
            {
                lock (_recordLocker)
                {
                    if (_records.ContainsKey(id))
                    {
                        var record = _records[id];
                        record.Abort();
                        _records.Remove(id);
                    }
                }
            }

            /// <summary>
            /// 将记录从字典中移除
            /// </summary>
            /// <param name="record">失败的消息记录</param>
            private void Remove(FailedRecord record)
            {
                _records.Remove(record.Record.Id);
            }

            private class FailedRecord
            {
                private static readonly int _interval = Core.Extension.StringExtension.ToInt(System.Configuration.ConfigurationManager.AppSettings["Interval"], 30) * 1000;
                private static readonly int _maxTimes = Core.Extension.StringExtension.ToInt(System.Configuration.ConfigurationManager.AppSettings["MaxTimes"], 5);

                public NotifyRecord Record { get; set; }
                public int Times { get; set; }
                public FailedRecordProcessor Container { get; set; }
                public DateTime LastTime { get; set; }

                private volatile bool _running = false;
                private Thread _thread = null;

                /// <summary>
                /// 开启重发失败记录的线程，并将其设置为后台线程
                /// </summary>
                public void Start()
                {
                    _running = true;
                    if (_thread == null)
                    {
                        _thread = new Thread(run)
                        {
                            IsBackground = true
                        };
                        _thread.Start();
                    }
                }

                //中断该线程
                public void Abort()
                {
                    _running = false;
                    _thread = null;
                }

                /// <summary>
                /// 重新发送之前发送失败的消息
                /// </summary>
                private void run()
                {
                    while (_running)
                    {
                        var currentTimesInterval = _interval * (int)Math.Pow(2, Times - 1);
                        currentTimesInterval -= (int)(DateTime.Now - LastTime).TotalMilliseconds;
                        Thread.Sleep(Math.Abs(currentTimesInterval));

                        if (_running)
                        {
                            Times++;
                            DateTime requestTime;
                            // 发送成功 或 超出最大发送次数，直接将该条数据废弃
                            if (send(Record, Times, out requestTime) || Times >= _maxTimes)
                            {
                                _running = false;
                                Container.Remove(this);
                            }
                            LastTime = requestTime;
                        }
                    }
                }
            }
        }
    }
}