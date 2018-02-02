namespace ChinaPay.B3B.Service.Order.Domain {
    internal class StatusInfo<TSystemStatus> {
        public string Purchaser { get; set; }
        public string Provider { get; set; }
        public string Supplier { get; set; }
        public string Platform { get; set; }
        public string DistributionOEM { get; set;}
        public TSystemStatus System { get; set; }
    }
}