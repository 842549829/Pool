using System;

namespace ChinaPay.B3B.Service.Tradement {
    internal class RefundResult {
        public System.Collections.Generic.IEnumerable<Distribution.Domain.Role.TradeRoleType> Roles { get; set; }
        public string Account { get; set; }
        public bool Success { get; set; }
        public DateTime? RefundTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
