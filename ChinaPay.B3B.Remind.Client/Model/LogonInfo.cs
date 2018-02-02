using System;

namespace ChinaPay.B3B.Remind.Client.Model {
    class LogonInfo {
        public LogonInfo(string userName, Guid batchNo, System.Net.Sockets.TcpClient connection) {
            this.UserName = userName;
            this.BatchNo = batchNo;
            this.Connection = connection;
        }
        public string UserName { get; private set; }
        public Guid BatchNo { get; private set; }
        public System.Net.Sockets.TcpClient Connection { get; private set; }
    }
}