using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Client.Command {
    class QueryAllCarriers : RequestProcessor<IList<Model.Carrier>> {
        public QueryAllCarriers(string host, int port, System.Guid logonId)
            : base(host, port, logonId) {
        }

        protected override string Command {
            get { return "QUERYALLCARRIERS"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString();
        }

        protected override IList<Model.Carrier> ParseResponseCore(string content) {
            var result = new List<Model.Carrier>();
            var array = content.Split('|');
            foreach(var item in array) {
                var itemArray = item.Split('-');
                if(itemArray.Length == 2) {
                    result.Add(new Model.Carrier(itemArray[0], itemArray[1]));
                }
            }
            return result;
        }
    }
}
