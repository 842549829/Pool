using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Exception
{
    class SellOutException : CustomException
    {
        public SellOutException(string message)
            : base(message)
        {

        }
    }
}
