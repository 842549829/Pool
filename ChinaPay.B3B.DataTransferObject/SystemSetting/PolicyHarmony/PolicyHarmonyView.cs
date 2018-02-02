namespace ChinaPay.B3B.DataTransferObject.SystemSetting.PolicyHarmony
{
    using System;
    using System.Collections.Generic;
    using B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Policy;
    using ChinaPay.Core;
    public class PolicyHarmonyView
    {
        public PolicyHarmonyView(Guid id)
        {
            this.Id = id;
        }
        public PolicyHarmonyView()
        {
        }
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public IEnumerable<string> Airlines 
        { 
            get; 
            set;
        }
        /// <summary>
        /// 出发城市
        /// </summary>
        public IEnumerable<string> Departure
        {
            get;
            set;
        }
        /// <summary>
        /// 到达城市
        /// </summary>
        public IEnumerable<string> Arrival
        {
            get;
            set;
        }
        /// <summary>
        /// 受限城市
        /// </summary>
        public IEnumerable<string> CityLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 政策协调值
        /// </summary>
        public decimal HarmonyValue
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
        /// 帐号类型（是否VIP）
        /// </summary>
        public bool IsVIP
        {
            get;
            set;
        }
        /// <summary>
        /// 返佣类型
        /// </summary>
        public DeductionType DeductionType{
            get;
            set;
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        public Range<DateTime> EffectiveDate
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 添加帐号
        /// </summary>
        public string Account
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}

