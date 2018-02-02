using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;
    using Data.DataMapping;

    public class PolicySettingInfo : PolicySetting {
        ///// <summary>
        ///// Id
        ///// </summary>
        //public Guid Id { get; set; }
        ///// <summary>
        ///// 航空公司
        ///// </summary>
        //public string Airline { get; set; }
        ///// <summary>
        ///// 出发城市
        ///// </summary>
        //public string Departure { get; set; }
        ///// <summary>
        ///// 到达城市
        ///// </summary>
        //public string Arrivals { get; set; }
        ///// <summary>
        ///// 适用的行程类型
        ///// </summary>
        //public VoyageType VoyageType { get; set; }
        ///// <summary>
        ///// 适用舱位
        ///// </summary>
        //public string Berths { get; set; }
        ///// <summary>
        ///// 生效起始时间
        ///// </summary>
        //public DateTime EffectiveTimeStart { get; set; }
        ///// <summary>
        ///// 生效结束时间
        ///// </summary>
        //public DateTime EffectiveTimeEnd { get; set; }
        /// <summary>
        /// 扣点区间信息
        /// </summary>
        public IEnumerable<PolicySettingPeriod> Periods { get; set; }
        ///// <summary>
        ///// 扣点区域起始
        ///// </summary>
        //public decimal PeriodStart { get; set; }
        ///// <summary>
        ///// 扣点区域结束
        ///// </summary>
        //public decimal PeriodEnd { get; set; }
        ///// <summary>
        ///// 扣点/贴点(设置的值： 大于 0 扣点；小于 0 贴点)
        ///// </summary>
        //public decimal Rebate { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Remark { get; set; }
        ///// <summary>
        ///// 创建者
        ///// </summary>
        //public string Creator { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }
        ///// <summary>
        ///// 最后修改时间
        ///// </summary>
        //public DateTime LastModifyTime { get; set; }
    }
}
