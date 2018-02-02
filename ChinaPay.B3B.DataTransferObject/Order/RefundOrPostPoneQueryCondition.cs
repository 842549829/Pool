using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    /// <summary>
    /// ���뵥��ѯ����
    /// </summary>
    public class RefundOrPostPoneQueryCondition
    {
        /// <summary>
        /// ���뵥��
        /// </summary>
        public decimal? ApplyformId { get; set; }
        /// <summary>
        /// ��Ʊ����λId
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public ProductType? ProductType { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public Range<DateTime> AppliedDateRange { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// �˻�������
        /// </summary>
        public string Passenger { get; set; }
    }
}