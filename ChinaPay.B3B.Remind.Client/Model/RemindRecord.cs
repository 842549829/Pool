namespace ChinaPay.B3B.Remind.Client.Model {
    class RemindRecord {
        public RemindRecord(string name, int count) {
            this.Name = name;
            this.Count = count;
        }
        public string Name { get; private set; }
        public int Count { get; private set; }
    }
}