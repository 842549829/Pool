using ChinaPay.B3B.Service.Command.Domain.Ticket;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    /// <summary>
    /// 电子客票信息。
    /// </summary>
    public class ETicketInfo
    {
        /// <summary>
        /// 电子客票信息
        /// </summary>
        public ElectronicTicket ElectronicTicket { get; set; }

        /// <summary>
        /// 行程单
        /// </summary>
        public JourneySheet JourneySheet { get; set; }

        /// <summary>
        /// 错误状态
        /// </summary>
        public DetrErrorStatus Status { get; set; }
    }
}
