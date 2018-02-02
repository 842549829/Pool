using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.DataTransferObject.Report {
    /// <summary>
    /// 当日出票量统计报表查询条件
    /// </summary>
    public class TodayProvideStatisticQueryCondition {
        /// <summary>
        /// 开始时间(几点)
        /// </summary>
        public int? StartHour { get; set; }
        /// <summary>
        /// 结束时间(几点)
        /// </summary>
        public int? EndHour { get; set; }
        /// <summary>
        /// 乘运人
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Bunk { get; set; }
        /// <summary>
        /// 出票方
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType? ProductType { get; set; }
        /// <summary>
        /// 特殊产品类型
        /// </summary>
        public SpecialProductType? SpecialProductType { get; set; }
        /// <summary>
        /// 销售关系
        /// </summary>
        public RelationType? Relation { get; set; }
    }
}