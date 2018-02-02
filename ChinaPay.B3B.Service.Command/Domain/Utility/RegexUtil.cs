using System.Collections.Generic;
using ChinaPay.B3B.Service.RegularExpression;

namespace ChinaPay.B3B.Service.Command.Domain.Utility
{
    using RegularExpression = RegularExpression.Domain.RegularExpression;

    public class RegexUtil
    {
        private Dictionary<string, RegularExpression> _regularExpresstions;
        private readonly object _thisLock = new object();

        object ThisLock
        {
            get { return _thisLock; }
        }

        public Dictionary<string, RegularExpression> RegularExpresstions
        {
            get
            {
                lock (ThisLock)
                {
                    return _regularExpresstions ?? (_regularExpresstions = RegularExpressionService.GetAllRegEx());
                }
            }
        }

        public RegularExpression GetRegularExpression(string key)
        {
            return RegularExpresstions[key];
        }

        public string GetRegexString(string key)
        {
            return RegularExpresstions[key].Value;
        }
    }
}
