using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// 提成明细
    /// </summary>
    public class SupplyTicketView
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 完成开始日期
        /// </summary>
        public DateTime? FinishBeginTime { get; set; }
        /// <summary>
        /// 完成结束日期
        /// </summary>
        public DateTime? FinishEndTime { get; set; }
        /// <summary>
        /// PNR 
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 机票状态
        /// </summary>
        public ChinaPay.B3B.Common.Enums.TicketState? TicketState { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Ariline { get; set; }
        /// <summary>
        /// 特殊票类型
        /// </summary>
        public SpecialProductType? SpecialProductType { get; set; }
    }
}
