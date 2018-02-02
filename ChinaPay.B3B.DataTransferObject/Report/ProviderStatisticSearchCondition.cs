using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    public class ProviderStatisticSearchCondition
    {
        /// <summary>
        /// ����ʼʱ��
        /// </summary>
        public DateTime? ReportStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// �������ʱ��
        /// </summary>
        public DateTime? ReportEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// ��Ʊ��Id
        /// </summary>
        public Guid? Provider
        {
            get;
            set;
        }
        /// <summary>
        /// �Ƿ��н���
        /// </summary>
        public bool? IsHasTrade
        {
            get;
            set;
        }
        /// <summary>
        /// ������
        /// </summary>
        public string Carrier
        {
            get;
            set;
        }
        /// <summary>
        /// ���۵�
        /// </summary>
        public string Departure
        {
            get;
            set;
        }

        /// <summary>
        /// ����ش���
        /// </summary>
        public string Arrival { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// �����Ʒ����
        /// </summary>
        public SpecialProductType? SpecialProductType { get; set; }

        /// <summary>
        /// ���۹�ϵ
        /// </summary>
        public RelationType? SaleRelation { get; set; }
    }
}