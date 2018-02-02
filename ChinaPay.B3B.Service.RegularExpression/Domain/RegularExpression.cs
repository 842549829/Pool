namespace ChinaPay.B3B.Service.RegularExpression.Domain
{
    public class RegularExpression
    {
        public RegularExpression(string id, string value, string exapmle, string descriotption)
        {
            Id = id;
            Value = value;
            Example = exapmle;
            Description = descriotption;
        }

        public string Id { get; set; }
        public string Value { get; set; }
        public string Example { get; set; }
        public string Description { get; set; }
    }
}
