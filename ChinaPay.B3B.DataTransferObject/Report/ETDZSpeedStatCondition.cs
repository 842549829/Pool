using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    /// <summary>
    /// 出票速度统计查询条件
    /// </summary>
    public class ETDZSpeedStatCondition
    {
        /// <summary>
        /// 统计起始时间
        /// </summary>
        public DateTime? StartStatTime { get; set; }

        /// <summary>
        /// 统计截止时间
        /// </summary>
        public DateTime? EndStatTime { get; set; }
        
        /// <summary>
        /// 承运人
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType? TicketType { get; set; }

        /// <summary>
        /// 被统计的供应商
        /// </summary>
        public Guid? Provider { get; set; }

        /// <summary>
        /// 分组类型
        /// </summary>
        public SpeedStatGroup StatGroup { get; set; }
    }

    public class SpeedStatGroup
    {
        public bool GroupByCarrier { get; set; }
        public bool GroupByTicketType { get; set; }

        public bool GroupByProvider { get; set; }
    }
}