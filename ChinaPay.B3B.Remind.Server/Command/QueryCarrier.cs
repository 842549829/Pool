using System;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Remind.Server.Command {
    class QueryCarrier : CommandProcessor {
        protected override string ExecuteCore() {
            Guid batchNo;
            if(Guid.TryParse(this.Request, out batchNo)) {
                var user = LogonCenter.Instance.GetUser(batchNo);
                if(user == null) {
                    return "2";
                } else {
                    var carriers = user.Setting.Carriers.Join("|");
                    Console.WriteLine(string.Format("{0} 用户[{1}]查询乘运人[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, carriers));
                    return "0/" + carriers;
                }
            }
            return "3";
        }
    }
}