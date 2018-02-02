using System;

namespace ChinaPay.B3B.Service.FlightTransfer
{
    public class TransferInformation
    {
        /// <summary>
        /// 采购Id
        /// </summary>
        public Guid PurchaserId{get;set;}

        /// <summary>
        /// 采购名称
        /// </summary>
        public string PurchaserName { get; set; }

        /// <summary>
        /// 采购联系电话
        /// </summary>
        public string ContractPhone { get; set; }

        /// <summary>
        /// 采购帐号
        /// </summary>
        public string PurchaserAccount { get; set; }

        /// <summary>
        /// 受影响订单数
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 受影响航班数
        /// </summary>
        public int FlightCount { get; set; }

    }
}