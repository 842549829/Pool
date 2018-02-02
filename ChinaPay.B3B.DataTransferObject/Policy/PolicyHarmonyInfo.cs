using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy {
    using B3B.Common.Enums;

    /// <summary>
    /// 政策协调信息
    /// </summary>
    public class PolicyHarmonyInfo{
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public PolicyType PolicyType { get; set; }
        ///// <summary>
        ///// 城市限制
        ///// </summary>
        //public string CityLimit { get; set; }
        /// <summary>
        /// 生效日期（起始）
        /// </summary>
        public DateTime EffectiveLowerDate { get; set; }
        /// <summary>
        /// 生效日期（结束）
        /// </summary>
        public DateTime EffectiveUpperDate { get; set; }
        ///// <summary>
        ///// 是否 VIP
        ///// </summary>
        //public bool IsVIP { get; set; }
        /// <summary>
        /// 扣点类型
        /// </summary>
        public DeductionType DeductionType { get; set; }
        /// <summary>
        /// 协调值
        /// </summary>
        public decimal HarmonyValue { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } 
        /// <summary>
        /// 最后修改时间 
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public string LastModifyName { get; set; }
    }
}
