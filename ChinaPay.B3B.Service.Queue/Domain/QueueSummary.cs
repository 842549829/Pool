namespace ChinaPay.B3B.Service.Queue.Domain
{
    /// <summary>
    /// �����Ҫ��Ϣ
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
        /// ����
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// δ�����ż�����
        /// </summary>
        public int UnprocessedNumber { get; private set; }

        /// <summary>
        /// ���ż�����
        /// </summary>
        public int TotalNumber { get; private set; }

    }
}
