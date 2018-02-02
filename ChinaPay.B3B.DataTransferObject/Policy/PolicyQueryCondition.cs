using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;

    /// <summary>
    /// 普通政策查询条件
    /// </summary>
    public class PolicyQueryCondition {
        /// <summary>
        /// 供应商单位Id
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType? TicketType { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 航班日期范围
        /// </summary>
        public Range<DateTime?> FlightDateRange { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatus? AuditStatus { get; set; }
        /// <summary>
        /// 锁定状态
        /// </summary>
        public PolicyLockStatus? LockStatus { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType? TripTypeValue { get; set; }
    }
}