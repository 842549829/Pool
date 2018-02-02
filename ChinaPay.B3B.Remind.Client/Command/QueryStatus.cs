﻿using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Client.Command {
    class QueryStatus : RequestProcessor<IList<string>> {
        public QueryStatus(string host, int port, System.Guid logonId)
            : base(host, port, logonId) {
        }

        protected override string Command {
            get { return "QUERYSTATUS"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString();
        }

        protected override IList<string> ParseResponseCore(string content) {
            var result = new List<string>();
            var array = content.Split('|');
            foreach(var item in array) {
                if(item != null && item.Trim().Length > 0) {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}