using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// 卖出机票明细
    /// </summary>
    public class ProvideTicketView : PurchaseTicketView
    {
        /// <summary>
        /// OFFICE号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 销售关系
        /// </summary>
        public ChinaPay.B3B.Common.Enums.RelationType? RelationType { get; set; }
        /// <summary>
        /// 采购方
        /// </summary>
        public Guid? Purchase { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string ProcessorAccount { get; set; }
        /// <summary>
        /// 特殊票类型
        /// </summary>
        public SpecialProductType? SpecialProductType { get; set; }
    }
}
