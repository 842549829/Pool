using System;
using ChinaPay.Core;
using ChinaPay.B3B.Common.Enums;
namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 特殊政策 用于修改
    /// </summary>
    public class SpecialPolicyView
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public SpecialProductType Type { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 出发
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 去程班期
        /// </summary>
        public Schedule OutwardScheduleDay { get; set; }
        /// <summary>
        /// 航班限制
        /// </summary>
        public LimitType FlightLimit { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 提前时间
        /// </summary>
        public int Ahead { get; set; }
        /// <summary>
        /// 退改签规定
        /// </summary>
        public RefundAndReschedulingProvision Provision { get; set; }
        /// <summary>
        /// 政策备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 航班日期
        /// </summary>
        public Range<DateTime> FlightDateRange { get; set; }
        /// <summary>
        /// 提供资源开始日期
        /// </summary>
        public DateTime SupplyBeginDate { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 可提供资源总数
        /// </summary>
        public int ResourceCount { get; set; }
        /// <summary>
        /// 是否需要审核
        /// </summary>
        public bool RequireAudit { get; set; }
        /// <summary>
        /// 采购时，是否需要确认
        /// </summary>
        public bool RequireConfirmResource { get; set; }

    }
}
