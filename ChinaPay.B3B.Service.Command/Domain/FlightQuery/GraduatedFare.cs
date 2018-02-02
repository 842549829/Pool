using System;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    /// <summary>
    /// 分级票价（按舱位等级）
    /// </summary>
    public class GraduatedFare
    {
        /// <summary>
        /// 舱位
        /// </summary>
        public string ClassOfService { get; set; }

        /// <summary>
        /// 子舱位
        /// </summary>
        public string SubClass { get; set; }

        /// <summary>
        /// 单程票价
        /// </summary>
        public decimal OneWayFare { get; set; }

        /// <summary>
        /// 往返票价
        /// </summary>
        public decimal RoundTripFare { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 服务类型(YCF，也就是经济舱、头等舱、商务舱)
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 适用行程类型。
        /// </summary>
        public ApplyType ApplyType { get; set; }
    }
}
