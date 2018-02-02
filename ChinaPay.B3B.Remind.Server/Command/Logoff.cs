using System;

namespace ChinaPay.B3B.Remind.Server.Command {
    class Logoff: CommandProcessor {
        protected override string ExecuteCore() {
            Guid batchNo;
            if(Guid.TryParse(this.Request, out batchNo)) {
                var user = LogonCenter.Instance.Logoff(batchNo);
                if(user == null) {
                    return "2";
                } else {
                    Console.WriteLine(string.Format("{0} 用户[{1}]注销 {2} 地址:{3} 批次号:{4}",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        user.Name, Environment.NewLine,
                        (this.Connection.Client.RemoteEndPoint as System.Net.IPEndPoint).Address,
                        user.BatchNo));
                    user.Release();
                    return "0";
                }
            }
            return "3";
        }
    }
}