using System;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class TransferInformation
    {
        /// <summary>
        /// �ɹ�Id
        /// </summary>
        public Guid PurchaserId{get;set;}

        /// <summary>
        /// �ɹ�����
        /// </summary>
        public string PurchaserName { get; set; }

        /// <summary>
        /// �ɹ���ϵ�绰
        /// </summary>
        public string ContractPhone { get; set; }

        /// <summary>
        /// �ɹ��ʺ�
        /// </summary>
        public string PurchaserAccount { get; set; }

        /// <summary>
        /// ��Ӱ�충����
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// ��Ӱ�캽����
        /// </summary>
        public int FlightCount { get; set; }

    }
}