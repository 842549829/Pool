using System;
using System.Text;

namespace ChinaPay.B3B.Remind.Server.Command {
    class QueryAllCarriers : CommandProcessor {
        protected override string ExecuteCore() {
            Guid batchNo;
            if(Guid.TryParse(this.Request, out batchNo)) {
                var user = LogonCenter.Instance.GetUser(batchNo);
                if(user == null) {
                    return "2";
                } else {
                    var carriers = getAllCarriers();
                    Console.WriteLine(string.Format("{0} 用户[{1}]查询所有乘运人[{2}]", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), user.Name, carriers));
                    return "0/" + carriers;
                }
            }
            return "3";
        }
        private string getAllCarriers() {
            var result = new StringBuilder();
            foreach(var item in ChinaPay.B3B.Service.FoundationService.Airlines){
                if(item.Valid) {
                    result.Append(string.Format("{0}-{1}|", item.Code, item.ShortName.Replace("-", "")));
                }
            }
            if(result.Length > 0) {
                result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }
    }
}