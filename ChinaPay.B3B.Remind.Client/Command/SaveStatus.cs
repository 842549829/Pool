using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Client.Command {
    class SaveStatus : RequestProcessor<string> {
        public SaveStatus(string host, int port, Guid logonId, IList<string> statuses)
            : base(host, port, logonId) {
            this.Statuses = statuses;
        }

        public IList<string> Statuses {
            get;
            private set;
        }

        protected override string Command {
            get { return "SAVESTATUS"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString() + "/" + Utility.Join(this.Statuses, "|");
        }

        protected override string ParseResponseCore(string content) {
            return null;
        }
    }
}