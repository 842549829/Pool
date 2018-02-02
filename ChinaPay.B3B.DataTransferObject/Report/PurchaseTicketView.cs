using System;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// 买入机票明细
    /// </summary>
    public class PurchaseTicketView
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid? CompanyId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? FinishBeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? FinishEndDate { get; set; }
        /// <summary>
        /// 支付开始时间
        /// </summary>
        public DateTime? PayBeginDate { get; set; }
        /// <summary>
        /// 支付结束时间
        /// </summary>
        public DateTime? PayEndDate { get; set; }
        /// <summary>
        /// 乘机开始时间
        /// </summary>
        public DateTime? TakeoffBeginDate { get; set; }
        /// <summary>
        /// 乘机结束时间
        /// </summary>
        public DateTime? TakeoffEndDate { get; set; }
        /// <summary>
        /// PNR
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNo { get; set; }
        /// <summary>
        /// 乘机人
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        ///  机票状态
        /// </summary>
        public ChinaPay.B3B.Common.Enums.TicketState? TicketState { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public byte? PolicyType { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public ChinaPay.B3B.Common.Enums.TicketType? TicketType { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public bool? PayType { get; set; }
    }
}
