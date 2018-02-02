using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Client.Command {
    class QueryAllStatus : RequestProcessor<Dictionary<string, string>> {
        public QueryAllStatus(string host, int port, System.Guid logonId)
            : base(host, port, logonId) {
        }

        protected override string Command {
            get { return "QUERYALLSTATUS"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString();
        }

        protected override Dictionary<string, string> ParseResponseCore(string content) {
            var result = new Dictionary<string, string>();
            var array = content.Split('|');
            foreach(var item in array) {
                var itemArray = item.Split('-');
                if(itemArray.Length == 2) {
                    result.Add(itemArray[0], itemArray[1]);
                }
            }
            return result;
        }
    }
}
