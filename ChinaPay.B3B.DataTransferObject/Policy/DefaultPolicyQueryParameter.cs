using System;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    public class DefaultPolicyQueryParameter
    {
        public string Airline { get; set; }
        public string AdultProviderName { get; set; }
        public string ChildProviderName { get; set; }
        public decimal? AdultCommission { get; set; }
        public decimal? ChildCommission { get; set; }
        public Guid? AdultProviderId { get; set; }
        public Guid? ChildProviderId { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}