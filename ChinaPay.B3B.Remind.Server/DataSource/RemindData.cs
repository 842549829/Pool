using ChinaPay.B3B.Service.Remind.Model;

namespace ChinaPay.B3B.Remind.Server.DataSource {
    class RemindData {
        public RemindData(RemindStatus status, int count) {
            this.Status = status;
            this.Count = count;
        }
        public RemindStatus Status {
            get;
            private set;
        }
        public int Count {
            get;
            private set;
        }
    }
}