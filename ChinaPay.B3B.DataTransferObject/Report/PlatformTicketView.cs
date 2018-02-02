using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// 平台机票销售
    /// </summary>
    public class PlatformTicketView
    {
        /// <summary>
        /// 完成开始时间
        /// </summary>
        public DateTime? FinishBeginTime { get; set; }
        /// <summary>
        /// 出票结束时间
        /// </summary>
        public DateTime? FinishEndTime { get; set; }
        /// <summary>
        /// 乘机开始时间
        /// </summary>
        public DateTime? TakeoffBeginDate { get; set; }
        /// <summary>
        /// 乘机结束时间
        /// </summary>
        public DateTime? TakeoffEndDate { get; set; }
        /// <summary>
        /// 采购
        /// </summary>
        public Guid? Purchaser { get; set; }
        /// <summary>
        /// 产品
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        ///出票
        /// </summary>
        public Guid? Supplier { get; set; }
        /// <summary>
        /// PNR
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string  Airline { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNo { get; set; }
        /// <summary>
        /// 乘机人
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public bool? PayType { get; set; }
        /// <summary>
        ///  机票状态
        /// </summary>
        public ChinaPay.B3B.Common.Enums.TicketState? TicketState { get; set; }
        /// <summary>
        /// 销售关系
        /// </summary>
        public ChinaPay.B3B.Common.Enums.RelationType? RelationType { get; set; }
    }
}
