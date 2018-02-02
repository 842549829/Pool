using System;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Remind.Server.Command {
    class QueryStatus : CommandProcessor {
        protected override string ExecuteCore() {
            Guid batchNo;
            if(Guid.TryParse(this.Request, out batchNo)) {
                var user = LogonCenter.Instance.GetUser(batchNo);
                if(user == null) {
                    return "2";
                } else {
                    var status = user.Setting.RemindStatus.Join("|", item => ((int)item).ToString());
                    Console.WriteLine(string.Format("{0} 用户[{1}]查询提醒状态[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, status));
                    return "0/" + status;
                }
            }
            return "3";
        }
    }
}