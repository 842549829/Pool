namespace ChinaPay.B3B.Remind.Client.Command {
    class Logoff : RequestProcessor<string> {
        public Logoff(string host, int port, System.Guid logonId)
            : base(host, port, logonId) {
        }

        protected override string Command {
            get { return "LOGOFF"; }
        }

        protected override string PrepareRequestContent() {
            return this.LogonId.ToString();
        }

        protected override string ParseResponseCore(string content) {
            return content;
        }
    }
}