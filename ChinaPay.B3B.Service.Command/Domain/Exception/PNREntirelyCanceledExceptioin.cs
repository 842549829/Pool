using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Exception
{
    /// <summary>
    /// PNR完全取消
    /// </summary>
    class PNRCanceledExceptioin : CustomException
    {
        public PNRCanceledExceptioin(string message)
            : base(message)
        {

        }
    }
}
