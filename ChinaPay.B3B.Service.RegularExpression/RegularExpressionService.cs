using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.RegularExpression.Repository;

namespace ChinaPay.B3B.Service.RegularExpression
{
    using RegularExpression = Domain.RegularExpression;

    public class RegularExpressionService
    {
        public static Dictionary<string, RegularExpression> GetAllRegEx()
        {
            var repository = Factory.CreateRegExRepository();
            return repository.Query().ToDictionary(p => p.Id);
        }
    }
}
