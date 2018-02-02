using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Remind.Server.Command {
    class SaveStatus : CommandProcessor {
        protected override string ExecuteCore() {
            var array = this.Request.Split('/');
            if(array.Length == 2) {
                var batchNoString = array[0];
                var statusString = array[1];
                Guid batchNo;
                if(Guid.TryParse(batchNoString, out batchNo)) {
                    var user = LogonCenter.Instance.GetUser(batchNo);
                    if(user == null) {
                        return "2";
                    } else {
                        var status = getStatuses(statusString);
                        if(user.UpdateRemindStatus(status)) {
                            Console.WriteLine(string.Format("{0} 用户[{1}]修改提醒状态成功[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, statusString));
                            return "0";
                        } else {
                            Console.WriteLine(string.Format("{0} 用户[{1}]修改提醒状态失败[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, statusString));
                            return "99";
                        }
                    }
                }
            }
            return "3";
        }
        private static IEnumerable<Service.Remind.Model.RemindStatus> getStatuses(string statusString) {
            var statusValues = getStatusValues(statusString);
            return statusValues.Select(item => (Service.Remind.Model.RemindStatus) item).ToList();
        }
        private static IEnumerable<int> getStatusValues(string statuses) {
            var result = new List<int>();
            if(!string.IsNullOrWhiteSpace(statuses)) {
                var array = statuses.Split('|');
                foreach(var item in array) {
                    int num;
                    if(int.TryParse(item, out num)) {
                        result.Add(num);
                    }
                }
            }
            return result;
        }
    }
}