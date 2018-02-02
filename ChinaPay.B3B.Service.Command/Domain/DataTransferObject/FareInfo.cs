using System;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.DataTransferObject
{
    public class FareInfo
    {
        /// <summary>
        /// 承运人
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// 港口对
        /// </summary>
        public AirportPair AirportPair { get; set; }
        
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
        /// 往返票价（暂时未用）
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
        /// 服务类型
        /// </summary>
        public string ServiceType { get; set; }
    }
}
