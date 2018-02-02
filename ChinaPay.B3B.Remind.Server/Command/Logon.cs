using System;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Remind.Server.Command {
    class Logon : CommandProcessor {
        protected override string ExecuteCore() {
            var array = this.Request.Split('/');
            if(array.Length == 2) {
                var userName = array[0];
                var password = array[1];
                LogonModel.User user;
                string errorCode;
                if(LogonCenter.Instance.Logon(userName, password, this.Connection, out user, out errorCode)) {
                    Console.WriteLine(string.Format("{0} {1}用户[{2}]登录 {3} 地址:{4} 批次号:{5}",
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            user.Owner.Type.GetDescription(),
                                            user.Name,
                                            Environment.NewLine,
                                            (this.Connection.Client.RemoteEndPoint as System.Net.IPEndPoint).Address,
                                            user.BatchNo));
                    return "0/" + user.BatchNo.ToString();
                } else {
                    Console.WriteLine(string.Format("{0} 用户[{1}]登录失败 {2} 地址:{3} 错误代码:{4}",
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            userName,
                                            Environment.NewLine,
                                            (this.Connection.Client.RemoteEndPoint as System.Net.IPEndPoint).Address,
                                            errorCode));
                    return errorCode;
                }
            }
            return "3";
        }
        public override bool DisposeConnection {
            get {
                return false;
            }
        }
    }
}