namespace ChinaPay.B3B.DataTransferObject.Order.External {
    public class AutoPayResult {
        public decimal Id { get; set; }
        public string ExternalId { get; set; }
        public bool Success { get; set; }
        public Payment Payment { get; set; }
        public string ErrorMessage { get; set; }
    }
}