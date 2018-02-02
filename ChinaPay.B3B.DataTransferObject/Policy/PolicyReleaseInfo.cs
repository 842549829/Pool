namespace ChinaPay.B3B.DataTransferObject.Policy
{
    using System;
    using System.Collections.Generic;
    using B3B.Common.Enums;

    /// <summary>
    /// 普通政策发布信息
    /// </summary>
    public class NormalPolicyReleaseInfo
    {
        private readonly List<NormalPolicyRebateInfo> rebates = new List<NormalPolicyRebateInfo>();
        public NormalPolicyBasicInfo BasicInfo { get; set; }
        public List<NormalPolicyRebateInfo> Rebates { get; set; }
    }
    /// <summary>
    /// 普通政策基本信息
    /// </summary>
    public class NormalPolicyBasicInfo
    {
        /// <summary>
        /// 政策 Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        ///// <summary>
        ///// 政策类型
        ///// </summary>
        //public PolicyType PolicyType { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市 
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 联程中转城市 
        /// </summary>
        public string Transit { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        ///// <summary>
        ///// 出行天数
        ///// </summary>
        //public short TravelDays { get; set; } 
        ///// <summary>
        ///// 去程班期过滤(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }
        ///// <summary>
        ///// 去程班期过滤类型(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public DateMode DepartureDatesFilterType { get; set; }
        ///// <summary>
        ///// 返程班期过滤(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public string ReturnDatesFilter { get; set; }
        ///// <summary>
        ///// 返程班期过滤类型 (2012-10-23 wangshiling 作废)
        ///// </summary>
        //public DateMode ReturnDatesFilterType { get; set; } 
        /// <summary>
        /// 去程航班过滤 (中转联程第一程)
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤(中转联程第二程)
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
    }
    /// <summary>
    /// 普通政策返点信息
    /// </summary>
    public class NormalPolicyRebateInfo
    {
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }

        ///// <summary>
        ///// 回程班期起始日期(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public DateTime? ReturnDateStart { get; set; }
        ///// <summary>
        ///// 回程班期结束日期(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public DateTime? ReturnDateEnd { get; set; }

        /// <summary>
        /// 排除日期限制
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }

        /// <summary>
        /// 内部返点
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返点
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返点
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        /// <summary>
        /// 换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 开始出票日期
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 适用往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 起飞前2小时内可用B2B出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
        ///// <summary>
        ///// 是否挂起
        ///// </summary>
        //public bool Suspended { get; set; }
        ///// <summary>
        ///// 是否已审核
        ///// </summary>
        //public bool Audited { get; set; }
        ///// <summary>
        ///// 是否冻结
        ///// </summary>
        //public bool Freezed { get; set; }
        ///// <summary>
        ///// VIP 返点(2012-10-23 wangshiling 作废)
        ///// </summary>
        //public decimal Vip { get; set; }
    }

    /// <summary>
    /// 特价政策发布信息
    /// </summary>
    public class BargainPolicyReleaseInfo
    {
        private readonly List<BargainPolicyRebateInfo> rebates = new List<BargainPolicyRebateInfo>();
        public BargainPolicyBasicInfo BasicInfo { get; set; }
        public List<BargainPolicyRebateInfo> Rebates { get; set; }
    }
    /// <summary>
    /// 特价政策基本信息
    /// </summary>
    public class BargainPolicyBasicInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 自定义编号
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 中转城市
        /// </summary>
        public string Transit { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        ///// <summary>
        ///// 去程班期过滤类型
        ///// </summary>
        //public DateMode DepartureDatesFilterType { get; set; }
        /// <summary>
        /// 去程航班过滤(中转联程第一程)
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤(中转联程第二程)
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 作废规定
        /// </summary>
        public string InvalidRegulation { get; set; }
        /// <summary>
        /// 改签规定
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 签转规定
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string RefundRegulation { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        ///// <summary>
        ///// 去程班期过滤
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
    }
    /// <summary>
    /// 特价政策返点信息
    /// </summary>
    public class BargainPolicyRebateInfo
    {
        /// <summary>
        /// 出行天数
        /// </summary>
        public short TravelDays { get; set; }
        /// <summary>
        /// 最少提前天数
        /// </summary>
        public short BeforehandDays { get; set; }
        /// <summary>
        /// 最多提前天数
        /// </summary>
        public short MostBeforehandDays { get; set; }
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部佣金
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级佣金
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行佣金
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 价格类型
        /// </summary>
        public PriceType PriceType { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 起飞前2小时可B2B出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
        ///// <summary>
        ///// 是否挂起
        ///// </summary>
        //public bool Suspended { get; set; }
        ///// <summary>
        ///// 是否冻结
        ///// </summary>
        //public bool Freezed { get; set; }
        ///// <summary>
        ///// 是否已审核
        ///// </summary>
        //public bool Audited { get; set; }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public DateTime CreateTime { get; set; }
        ///// <summary>
        ///// 审核时间
        ///// </summary>
        //public DateTime AuditTime { get; set; }
    }

    public class SpecialPolicyReleaseInfo
    {
        private readonly List<SpecialPolicyRebateInfo> rebates = new List<SpecialPolicyRebateInfo>();
        public SpecialPolicyBasicInfo BasicInfo { get; set; }
        public List<SpecialPolicyRebateInfo> Rebates { get; set; }
    }
    public class SpecialPolicyBasicInfo
    {
        /// <summary>
        /// T_SpecialPolicy
        /// </summary>
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 自定义编号
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 特殊产品类型
        /// </summary>
        public SpecialProductType Type { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        ///// <summary>
        ///// 去程班期过滤
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }
        ///// <summary>
        ///// 去程班期过滤类型
        ///// </summary>
        //public DateMode DepartureDatesFilterType { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 作废规定
        /// </summary>
        public string InvalidRegulation { get; set; }
        /// <summary>
        /// 改签规定
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 签转规定
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string RefundRegulation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
    }
    public class SpecialPolicyRebateInfo
    {
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public short BeforehandDays { get; set; }
        /// <summary>
        /// 是否黑屏同步(Synchronization)
        /// </summary>
        public bool SynBlackScreen { get; set; }
        /// <summary>
        /// 是否是有位出票（false是无位，代表true有位）
        /// </summary>
        public bool IsSeat { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 开始出票日期
        /// </summary>
        public DateTime ProvideDate { get; set; }
        /// <summary>
        /// 可提供资源数量
        /// </summary>
        public int ResourceAmount { get; set; }
        /// <summary>
        /// 自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 采购时需要确认资源
        /// </summary>
        public bool ConfirmResource { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 内部佣金
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级佣金
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行佣金
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 价格类型
        /// </summary>
        public PriceType PriceType { get; set; }
        /// <summary>
        /// 是否是特价舱位(2012 10 20 wangshiling 新增)
        /// </summary>
        public bool IsBargainBerths { get; set; }
        /// <summary>
        /// 起飞前2小时可B2B出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
        /// <summary>
        /// 低价出票类型
        /// </summary>
        public LowNoType LowNoType { get; set; }
        /// <summary>
        /// 价格限制的上限（不包含）
        /// </summary>
        public decimal LowNoMaxPrice { get; set; }
        /// <summary>
        /// 价格限制的下限（包含）
        /// </summary>
        public decimal LowNoMinPrice { get; set; }
    }

    public class RoundTripPolicyReleaseInfo
    {
        private readonly List<RoundTripPolicyRebateInfo> rebates = new List<RoundTripPolicyRebateInfo>();
        public RoundTripPolicyBasicInfo BasicInfo { get; set; }
        public List<RoundTripPolicyRebateInfo> Rebates { get; set; }
    }
    public class RoundTripPolicyBasicInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 去程班期过滤
        /// </summary>
        public string DepartureDatesFilter { get; set; }
        /// <summary>
        /// 去程班期过滤类型
        /// </summary>
        public DateMode DepartureDatesFilterType { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 回程班期过滤
        /// </summary>
        public string ReturnDatesFilter { get; set; }
        /// <summary>
        /// 回程班期过滤类型
        /// </summary>
        public DateMode ReturnDatesFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public short BeforehandDays { get; set; }
        /// <summary>
        /// 旅游天数
        /// </summary>
        public short TravelDays { get; set; }
        /// <summary>
        /// 作废规定
        /// </summary>
        public string InvalidRegulation { get; set; }
        /// <summary>
        /// 改签规定
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 签转规定
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string RefundRegulation { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
    }
    public class RoundTripPolicyRebateInfo
    {
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 回程班期起始日期
        /// </summary>
        public DateTime? ReturnDateStart { get; set; }
        /// <summary>
        /// 回程班期结束日期
        /// </summary>
        public DateTime? ReturnDateEnd { get; set; }
        /// <summary>
        /// 班期日期限制
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部佣金
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级佣金
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行佣金
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool Freezed { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
    }
    /// <summary>
    /// 缺口程
    /// </summary>
    public class NotchPolicyReleaseInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 自定义编号
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }

        /// <summary>
        /// 出发达到
        /// </summary>
        public List<NotchPolicyDepartureArrival> DepartureArrival { get; set; }
        /// <summary>
        /// 返点
        /// </summary>
        public List<NotchPolicyRebateInfo> RebateInfo { get; set; }

    }
    public class NotchPolicyRebateInfo
    {
        /// <summary>
        /// 航班起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 航班结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 班期日期限制
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部佣金
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级佣金
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行佣金
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 起飞前2小时可B2B出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }

    }



    public class NotchPolicyDepartureArrival
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }  
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 是否包含
        /// </summary>
        public bool IsAllowable { get; set; }
    }



    /// <summary>
    /// 团队政策发布信息
    /// </summary>
    public class TeamPolicyReleaseInfo
    {
        private readonly List<TeamPolicyRebateInfo> rebates = new List<TeamPolicyRebateInfo>();
        public TeamPolicyBasicInfo BasicInfo { get; set; }
        public List<TeamPolicyRebateInfo> Rebates { get; set; }
    }
    /// <summary>
    /// 团队政策基本信息
    /// </summary>
    public class TeamPolicyBasicInfo
    {
        /// <summary>
        /// 政策 Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 是否需要授权office
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市 
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 联程中转城市 
        /// </summary>
        public string Transit { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// 去程航班过滤 (中转联程第一程)
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤(中转联程第二程)
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
    }
    /// <summary>
    /// 团队政策返点信息
    /// </summary>
    public class TeamPolicyRebateInfo
    {
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }

        /// <summary>
        /// 班期日期限制
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }

        /// <summary>
        /// 内部返点
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返点
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返点
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 是否指定团队舱位
        /// </summary>
        public bool AppointBerths { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        /// <summary>
        /// 换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 开始出票日期
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 适用往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 起飞前2小时可B2B出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
    }
}
