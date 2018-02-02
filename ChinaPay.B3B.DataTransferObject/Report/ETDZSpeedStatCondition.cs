using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// ��Ʊ�ٶ�ͳ�Ʋ�ѯ����
    /// </summary>
    public class ETDZSpeedStatCondition
    {
        /// <summary>
        /// ͳ����ʼʱ��
        /// </summary>
        public DateTime? StartStatTime { get; set; }

        /// <summary>
        /// ͳ�ƽ�ֹʱ��
        /// </summary>
        public DateTime? EndStatTime { get; set; }
        
        /// <summary>
        /// ������
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public TicketType? TicketType { get; set; }

        /// <summary>
        /// ��ͳ�ƵĹ�Ӧ��
        /// </summary>
        public Guid? Provider { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public SpeedStatGroup StatGroup { get; set; }
    }

    public class SpeedStatGroup
    {
        public bool GroupByCarrier { get; set; }
        public bool GroupByTicketType { get; set; }

        public bool GroupByProvider { get; set; }
    }
}