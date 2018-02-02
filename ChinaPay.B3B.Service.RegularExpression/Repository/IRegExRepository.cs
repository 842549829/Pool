using System.Collections.Generic;

namespace ChinaPay.B3B.Service.RegularExpression.Repository
{
    interface IRegExRepository
    {
        IEnumerable<Domain.RegularExpression> Query();
    }
}
