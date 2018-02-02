using System;
using System.Linq;

namespace ChinaPay.B3B.Remind.Server.Command {
    class SaveCarrier : CommandProcessor {
        protected override string ExecuteCore() {
            var array = this.Request.Split('/');
            if(array.Length == 2) {
                var batchNoString = array[0];
                var carriersString = array[1];
                Guid batchNo;
                if(Guid.TryParse(batchNoString, out batchNo)) {
                    var user = LogonCenter.Instance.GetUser(batchNo);
                    if(user == null) {
                        return "2";
                    } else {
                        var carriers = carriersString.Split('|').ToList();
                        if(user.UpdateCarriers(carriers)) {
                            Console.WriteLine(string.Format("{0} 用户[{1}]修改乘运人成功[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, carriersString));
                            return "0";
                        } else {
                            Console.WriteLine(string.Format("{0} 用户[{1}]修改乘运人失败[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, carriersString));
                            return "99";
                        }
                    }
                }
            }
            return "3";
        }
    }
}