namespace ChinaPay.B3B.Service.Queue.Domain
{
    /// <summary>
    /// 信箱概要信息
    /// </summary>
    public class QueueSummary
    {
        public QueueSummary(string name, int unprocessedNumber, int totalNumber)
        {
            TotalNumber = totalNumber;
            UnprocessedNumber = unprocessedNumber;
            Name = name;
        }

        /// <summary>
        /// 信箱
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 未处理信件数量
        /// </summary>
        public int UnprocessedNumber { get; private set; }

        /// <summary>
        /// 总信件数量
        /// </summary>
        public int TotalNumber { get; private set; }

    }
}
