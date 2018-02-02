using System;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class CredentialsUpdateInfo {
        public decimal OrderId { get; set; }
        public string Passenger { get; set; }
        public string OriginalCredentials { get; set; }
        public string NewCredentials { get; set; }
        public bool Success { get; set; }
        public DateTime CommitTime { get; set; }
        public string CommitAccount { get; set; }
    }
}