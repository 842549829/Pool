using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class CredentialsUpdateInfoListView {
        public Guid Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public PNRPair PNR { get; set; }
        /// <summary>
        /// 航段信息集合
        /// </summary>
        public IEnumerable<FlightListView> Flights { get; set; }
        /// <summary>
        /// 乘机人
        /// </summary>
        public Guid Passenger { get; set; }
        /// <summary>
        /// 乘机人名字
        /// </summary>
        public string PassengerName { get; set; }
        /// <summary>
        /// 采购单位Id
        /// </summary>
        public Guid Purchaser { get; set; }
        /// <summary>
        /// 采购单位简称
        /// </summary>
        public string PurchaserName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CommitTime { get; set; }
        /// <summary>
        /// 原证件号
        /// </summary>
        public string OriginalCredentials { get; set; }
        /// <summary>
        /// 新证件号
        /// </summary>
        public string NewCredentials { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Success { get; set; }
    }
}