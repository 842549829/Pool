using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Linq;

namespace ChinaPay.B3B.Service.PolicyMatch {
    using System;
    using Common.Enums;
    using Izual;
    using Statistic;

    /// <summary>
    /// 利润
    /// </summary>
    public class OemProfit
    {
        /// <summary>
        /// 上级编号
        /// </summary>
        public Guid CompanyId { get; set; }
        
        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
    }
    
    /// <summary>
    /// 有可能被设定为一种匹配到的政策类型；
    /// </summary>
    public class OemPolicyInfo
    {
       /// <summary>
       /// 利润类型
       /// </summary>
        public OemProfitType ProfitType { get; set; }

        /// <summary>
        /// 利润列表
        /// </summary>
        public List<OemProfit> Profits { get; set; }
        
        /// <summary>
        /// 总利润值
        /// </summary>
        /// <returns></returns>
        public decimal TotalProfit
        {
            get
            {
                return Profits.Sum(p => p.Value);
            }
        }
    }
    
    public class MatchedPolicy {
        /// <summary>
        /// 政策ID，不一定会有，如取到了系统默认政策；也有可能存放的是公司组限制的组编号
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// 是否为外部政策
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// 外部原始政策
        /// </summary>
        public ExternalPolicyView OriginalExternalPolicy { get; set; }
        
        /// <summary>
        /// 出票方
        /// </summary>
        public Guid Provider { get; internal set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public PolicyType PolicyType { get; internal set; }
        /// <summary>
        /// 原始政策
        /// </summary>
        public PolicyInfoBase OriginalPolicy { get; set; }
        /// <summary>
        /// 出票方发布的原始返点
        /// </summary>
        public decimal Rebate { get; internal set; }
        /// <summary>
        /// 采购方得到的返点
        /// </summary>
        public decimal Commission { get; internal set; }
        /// <summary>
        /// 买家、卖家关系
        /// </summary>
        public RelationType RelationType { get; internal set; }
        /// <summary>
        /// 平台的扣、贴点
        /// </summary>
        public decimal Deduction { get; internal set; }
        
        /// <summary>
        /// 结算价
        /// </summary>
        public decimal SettleAmount { get; internal set; }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal ParValue { get; internal set; }
        /// <summary>
        /// 工作开始时间
        /// </summary>
        public Time WorkStart { get; set; }
        /// <summary>
        /// 工作结束时间
        /// </summary>
        public Time WorkEnd { get; set; }
        /// <summary>
        /// 退票开始时间（注释写错了，应该是废票时间）
        /// </summary>
        public Time RefundStart { get; set; }
        /// <summary>
        /// 退票结束时间（注释写错了，应该是废票时间）
        /// </summary>
        public Time RefundEnd { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeNumber { get; set; }

        /// <summary>
        /// 是否需要 编码授权
        /// </summary>
        public bool NeedAUTH {
            get;
            set;
        }
        /// <summary>
        /// 信誉评级
        /// </summary>
        public decimal CompannyGrade {
            get;
            set;
        }
        public GeneralProductSpeedInfo.Item Speed { get; set; }
        public SpecialProductStatisticOnVoyage Statistics { get; set; }

        public bool IsSeat { get; set; }

        public bool ConfirmResource { get; set; }
        /// <summary>
        /// 是否补贴过
        /// </summary>
        public bool HasSubsidized { get; set; }

        /// <summary>
        /// 协调值
        /// </summary>
        public decimal HarmonyValue { get; set; }

        /// <summary>
        /// 是否协调
        /// </summary>
        public bool HasHarmony { get; set; }

        ///// <summary>
        ///// 是否有OEM利润；
        ///// </summary>
        //public bool HasOemInfo { get; set; }

        private OemPolicyInfo _oemInfo;

        /// <summary>
        /// OEM相关信息，在2013-05-23的改动中，此处为收益组设置，而非oem的限制信息，但名称不变
        /// </summary>
        public OemPolicyInfo OemInfo
        {
            get {
                if (_oemInfo == null)
                {
                    _oemInfo = new OemPolicyInfo();
                    _oemInfo.Profits = new List<OemProfit>();
                    _oemInfo.ProfitType = OemProfitType.Discount;
                    if (OriginalPolicy != null && OriginalPolicy is SpecialPolicyInfo && ((SpecialPolicyInfo)OriginalPolicy).Type != SpecialProductType.LowToHigh)
                    {
                        _oemInfo.ProfitType = OemProfitType.PriceMarkup; 
                    }
                }
                return _oemInfo; }
            set { _oemInfo = value; }
        }

        public void SetSpeed(GeneralProductSpeedInfo speedInfo) {
            if(OriginalPolicy != null) {
                Speed = OriginalPolicy.TicketType == TicketType.B2B ? speedInfo.B2B : speedInfo.BSP;
            } else {
                Speed = speedInfo.B2B;
            }
        }
    }
}