using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Exception
{
    class ParseException : CustomException
    {
        public ParseException(string message)
            : base(message)
        {

        }
    }
}
