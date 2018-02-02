using System;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Remind.Server.Command {
    class QueryAllStatus : CommandProcessor {
        protected override string ExecuteCore() {
            Guid batchNo;
            if(Guid.TryParse(this.Request, out batchNo)) {
                var user = LogonCenter.Instance.GetUser(batchNo);
                if(user == null) {
                    return "2";
                } else {
                    var statuses = getAllStatus(user);
                    Console.WriteLine(string.Format("{0} 用户[{1}]查询所有状态[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, statuses));
                    return "0/" + statuses;
                }
            }
            return "3";
        }
        private string getAllStatus(LogonModel.User user) {
            var result = new StringBuilder();
            foreach(var item in user.Owner.Statuses) {
                result.Append(string.Format("{0}-{1}|", (int)item, item.GetDescription()));
            }
            if(result.Length > 0) {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }
    }
}