using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Order {

    /// <summary>
    /// 票号信息
    /// </summary>
    public class TicketNoView {
        /// <summary>
        /// 出票编码信息
        /// </summary>
        public PNRPair ETDZPNR { get; set; }
        /// <summary>
        /// 出票方式
        /// </summary>
        public ETDZMode Mode { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 新结算码
        /// </summary>
        public string NewSettleCode { get; set; }
        /// <summary>
        /// 票号项
        /// </summary>
        public IEnumerable<Item> Items { get; set; }
        public class Item {
            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 票号
            /// </summary>
            public IEnumerable<string> TicketNos { get; set; }
        }
    }

}