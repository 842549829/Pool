namespace ChinaPay.B3B.DataTransferObject.SystemSetting.PolicyHarmony
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Policy;
    using ChinaPay.Core;
    public class PolicyHarmonyQueryCondition
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline
        {
            get;
            set;
        }
        /// <summary>
        /// 政策类型
        /// </summary>
        public PolicyType PolicyType
        {
            get;
            set;
        }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure
        {
            get;
            set;
        }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival
        {
            get;
            set;
        }
        /// <summary>
        /// 生效时间
        /// </summary>
        public Range<DateTime?> EffectiveDate
        {
            get;
            set;
        }
    }
}
