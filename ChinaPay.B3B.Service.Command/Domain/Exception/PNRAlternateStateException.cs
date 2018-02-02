using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Exception
{
    class PNRAlternateStateException : CustomException
    {
        public PNRAlternateStateException(string message)
            : base(message)
        {

        }
    }
}
