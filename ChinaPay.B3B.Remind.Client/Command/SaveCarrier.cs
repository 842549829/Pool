using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Client.Command {
    class SaveCarrier : RequestProcessor<string> {
        public SaveCarrier(string host, int port, System.Guid logonId, IList<string> carriers)
            : base(host, port, logonId) {
            this.Carriers = carriers;
        }

        public IList<string> Carriers {
            get;
            private set;
        }

        protected override string Command {
            get { return "SAVECARRIER"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString() + "/" + Utility.Join(this.Carriers, "|");
        }

        protected override string ParseResponseCore(string content) {
            return null;
        }
    }
}