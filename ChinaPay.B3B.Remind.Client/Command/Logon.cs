namespace ChinaPay.B3B.Remind.Client.Command {
    class Logon : RequestProcessor<Model.LogonInfo> {
        public Logon(string host, int port, string userName, string password)
            : base(host, port, System.Guid.Empty) {
            this.UserName = userName;
            this.Password = password;
        }

        public string UserName {
            get;
            private set;
        }
        public string Password {
            get;
            private set;
        }

        protected override bool KeepConnection {
            get {
                return true;
            }
        }

        protected override string Command {
            get { return "LOGON"; }
        }

        protected override string PrepareRequestContent() {
            return string.Format("{0}/{1}", this.UserName, this.Password);
        }

        protected override Model.LogonInfo ParseResponseCore(string content) {
            var batchNo = new System.Guid(content);
            return new Model.LogonInfo(this.UserName, batchNo, this.Connection);
        }
    }
}