using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.Remind.Model;
using ChinaPay.Net;

namespace ChinaPay.B3B.Remind.Server.LogonModel {
    class User {
        TcpProcessor m_processor = null;
        System.Net.IPAddress m_address = null;
        object m_carrierLocker = new object();
        object m_statusLocker = new object();
        List<RemindInfo> m_previousRemindInfos = null;
        DateTime? m_previousRemindTime = null;
        // 以秒为单位
        readonly int m_remindInterval = 60;
        // 是否需要提醒，用于处理刚登录的用户，还没有得到登录的返回信息，就收到提醒数据了
        bool _requireRemind = false;

        public User(Guid id, System.Net.Sockets.TcpClient connection) {
            Id = id;
            Time = DateTime.Now;
            BatchNo = Guid.NewGuid();
            m_previousRemindInfos = new List<RemindInfo>();
            initTcpProcessor(connection);
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Company Owner { get; set; }
        public DateTime Time { get; private set; }
        public Guid BatchNo { get; private set; }
        public bool WorkOnCustomNO { get; set; }
        public System.Net.IPAddress Address {
            get {
                return m_address;
            }
        }
        public RemindSetting Setting {
            get {
                return Settings.Instance[this.Id];
            }
        }

        public string CustomNOs { get; set; }

        public bool UpdateCarriers(IEnumerable<string> carriers) {
            if(carriers != null && carriers.Any()) {
                lock(m_carrierLocker) {
                    try {
                        Service.Remind.OrderRemindSettingService.SaveCarriers(this.Id, carriers);
                        Setting.UpdateCarriers(carriers);
                        return true;
                    } catch(Exception ex) {
                        Service.LogService.SaveExceptionLog(ex, "订单提醒 设置乘运人");
                    }
                }
            }
            return false;
        }
        public bool UpdateRemindStatus(IEnumerable<RemindStatus> status) {
            if(status != null) {
                lock(m_statusLocker) {
                    try {
                        Service.Remind.OrderRemindSettingService.SaveStatus(this.Id, status);
                        this.Setting.UpdateRemindStatus(status);
                        return true;
                    } catch(Exception ex) {
                        Service.LogService.SaveExceptionLog(ex, "订单提醒 设置提醒状态");
                    }
                }
            }
            return false;
        }
        public void Remind(IEnumerable<RemindInfo> remindInfos) {
            if(!_requireRemind) {
                _requireRemind = (DateTime.Now - Time).TotalSeconds > 10;
                if(!_requireRemind) return;
            }
            var validRemindInfos = filter(remindInfos);
            if(requireRemind(validRemindInfos)) {
                var remindDatas = getRemindDatas(validRemindInfos);
                if(remindDatas.Any()) {
                    m_previousRemindInfos.Clear();
                    m_previousRemindInfos.AddRange(validRemindInfos);
                    m_previousRemindTime = DateTime.Now;
                    try {
                        var processor = new Command.Remind(m_processor, remindDatas);
                        var dataContent = processor.Execute();
                        Console.WriteLine(string.Format("{0} 提醒用户[{1}] {2} 地址:{3} 批次号:{4} 数据{5}",
                                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                        Name,
                                                        Environment.NewLine,
                                                        Address,
                                                        BatchNo,
                                                        dataContent));
                    } catch(Exception ex) {
                        Console.WriteLine(string.Format("{0} 提醒用户[{1}] {2} 地址:{3} 批次号:{4} 发送提醒数据失败:{5}",
                                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                        Name,
                                                        Environment.NewLine,
                                                        Address,
                                                        BatchNo,
                                                        ex.Message));
                    }
                }
            }
        }

        public void Release() {
            CustomGCCollection.Instance.Register(m_processor);
        }

        IEnumerable<RemindInfo> filter(IEnumerable<RemindInfo> remindInfos) {
            return (from item in remindInfos
                    where Setting.ContainsStatus(item.Status) && Setting.ContainsCarrier(item.Carrier)
                        && (!WorkOnCustomNO || string.IsNullOrWhiteSpace(item.CustomNO) || (!string.IsNullOrWhiteSpace(CustomNOs) && CustomNOs.Contains(item.CustomNO)))
                    select item).ToList();
        }

        bool requireRemind(IEnumerable<RemindInfo> remindInfos) {
            return isTimeout() || hasNewRemindInfo(remindInfos);
        }

        bool hasNewRemindInfo(IEnumerable<RemindInfo> remindInfos) {
            return remindInfos.Any(ri => !m_previousRemindInfos.Exists(item => item.Id == ri.Id));
        }

        bool isTimeout() {
            return !m_previousRemindTime.HasValue || (DateTime.Now - m_previousRemindTime.Value).TotalSeconds >= m_remindInterval;
        }

        IEnumerable<DataSource.RemindData> getRemindDatas(IEnumerable<RemindInfo> remindInfos) {
            var datas = new Dictionary<RemindStatus, int>();
            foreach(var item in remindInfos) {
                if(datas.ContainsKey(item.Status)) {
                    datas[item.Status] += 1;
                } else {
                    datas.Add(item.Status, 1);
                }
            }
            return datas.Select(item => new DataSource.RemindData(item.Key, item.Value)).ToList();
        }
        void initTcpProcessor(System.Net.Sockets.TcpClient connection) {
            m_address = (connection.Client.RemoteEndPoint as System.Net.IPEndPoint).Address;
            m_processor = new TcpProcessor(connection);
            m_processor.ConnectionDisconnected += m_processor_ConnectionDisconnected;
        }
        void m_processor_ConnectionDisconnected(object sender, Net.EventArgs.ConnectionDisconnectedEventArgs e) {
            Console.WriteLine(string.Format("{0} 用户[{1}]已断开连接.{2} 地址:{3} 批次号:{4}",
                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Name,
                                        Environment.NewLine,
                                        Address,
                                        BatchNo));
            LogonCenter.Instance.Logoff(BatchNo);
        }
    }
}