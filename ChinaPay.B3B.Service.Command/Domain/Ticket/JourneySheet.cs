namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    /// <summary>
    /// 行程单
    /// </summary>
    public class JourneySheet
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 旅客姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 电子客票票号
        /// </summary>
        public string TicketNumber { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public JourneySheetStatus Staus { get; set; }


        public bool IsNotUsed()
        {
            return Staus != JourneySheetStatus.Used;
        }
    }
}
