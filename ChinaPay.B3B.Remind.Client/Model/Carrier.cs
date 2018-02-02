namespace ChinaPay.B3B.Remind.Client.Model {
    class Carrier {
        public Carrier(string code, string name) {
            this.Code = code;
            this.Name = name;
        }
        public string Code {
            get;
            private set;
        }
        public string Name {
            get;
            private set;
        }
    }
}