namespace ChinaPay.B3B.Common.Enums {
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 日期方式
    /// </summary>
    public enum DateMode {
        /// <summary>
        /// 日期
        /// </summary>
        Date,
        /// <summary>
        /// 工作日
        /// </summary>
        Week
    }
    /// <summary>
    /// 限制类型
    /// </summary>
    public enum LimitType {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 包含
        /// </summary>
        Include,
        /// <summary>
        /// 排除
        /// </summary>
        Exclude
    }
    /// <summary>
    /// 客票类型
    /// </summary>
    public enum TicketType {
        B2B,
        BSP
    }
    /// <summary>
    /// 舱位类型
    /// </summary>
    public enum BunkType {
        [Description("经济舱")]
        Economic,
        [Description("头等公务舱")]
        FirstOrBusiness,
        [Description("特价舱")]
        Promotion,
        [Description("往返产品舱")]
        Production,
        [Description("中转联程舱")]
        Transfer
    }
    /// <summary>
    /// 乘客类型
    /// </summary>
    public enum PassengerType {
        [Description("成人")]
        Adult,
        [Description("儿童")]
        Child
    }
    /// <summary>
    /// 发布角色
    /// </summary>
    public enum PublishRole {
        [Description("平台")]
        平台,
        [Description("用户")]
        用户
    }
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradementType {
        Pay,
        Refund
    }
    public enum EnableEnum {
        [Description("无效")]
        DisEnable,
        [Description("有效")]
        Enable
    }

    /// <summary>
    /// 公司类型
    /// </summary>
    public enum CompanyType : byte {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 出票方
        /// </summary>
        [Description("出票方")]
        Agent,
        /// <summary>
        /// 分销商
        /// </summary>
        [Description("分销商")]
        Distributor,
        /// <summary>
        /// 采购商
        /// </summary>
        [Description("采购商")]
        Purchaser,
        /// <summary>
        /// 产品方
        /// </summary>
        [Description("产品方")]
        Provider,
        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台")]
        Platform = byte.MaxValue
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender : byte {
        /// <summary>
        /// 男
        /// </summary>
        Male,
        /// <summary>
        /// 女
        /// </summary>
        Female
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    [Flags]
    public enum BusinessType : byte {
        /// <summary>
        /// 同行
        /// </summary>
        [Description("同行客户")]
        Profession = 0x1,
        /// <summary>
        /// 散客
        /// </summary>
        [Description("散客")]
        Individual = 0x2,
        /// <summary>
        /// 企业（公司）
        /// </summary>
        [Description("公司客户")]
        Enterprise = 0x4
    }

    /// <summary>
    /// 拥有客户的类型
    /// </summary>
    public enum HasClientType : byte {
        /// <summary>
        /// 企业客户为主
        /// </summary>
        [Description("企业客户为主")]
        EnterpriseMainly,
        /// <summary>
        /// 散客为主
        /// </summary>
        [Description("散客为主")]
        IndividualMainly,
        /// <summary>
        /// 固定客户
        /// </summary>
        [Description("固定客户")]
        Regular,
        /// <summary>
        /// 想要发展固定客户
        /// </summary>
        [Description("想要发展固定客户")]
        RegularWanted,
        /// <summary>
        /// 想要做散客
        /// </summary>
        [Description("想要做散客")]
        IndividualWanted,
        /// <summary>
        /// 销售推荐
        /// </summary>
        [Description("销售推荐")]
        SalesRecommend
    }

    /// <summary>
    /// 航空代理资质类型
    /// </summary>
    public enum QualificationType : byte {
        /// <summary>
        /// 一类
        /// </summary>
        Level1,
        /// <summary>
        /// 二类
        /// </summary>
        Level2,
        /// <summary>
        /// 三类
        /// </summary>
        Level3
    }

    public enum HowToKnow : byte {
        [Description("广告")]
        Advertising,
        [Description("网络宣传")]
        Web,
        [Description("同行介绍")]
        Profession,
        [Description("销售推荐")]
        Recommend
    }

    public enum WorkHoursType : byte {
        WorkingDays,
        RestDays
    }

    /// <summary>
    ///     关系类型
    /// </summary>
    public enum RelationshipType : byte {
        /// <summary>
        ///     组织机构关系
        /// </summary>
        Organization,

        /// <summary>
        ///     分销体系关系
        /// </summary>
        Distribution,

        /// <summary>
        ///     推广关系
        /// </summary>
        Spread,

        /// <summary>
        ///     服务提供关系
        /// </summary>
        ServiceProvide
    }
    /// <summary>
    /// 单位状态
    /// </summary>
    //public enum CompanyStatus : byte {
    //    /// <summary>
    //    /// 未审
    //    /// </summary>
    //    [Description("未审")]
    //    UnAduited,
    //    /// <summary>
    //    /// 启用
    //    /// </summary>
    //    [Description("启用")]
    //    Enabled,
    //    /// <summary>
    //    /// 禁用
    //    /// </summary>
    //    [Description("禁用")]
    //    Disabled,
    //}

    /// <summary>
    /// 政策类型
    /// </summary>
    public enum PolicyType {
        /// <summary>
        /// 普通
        /// </summary>
        General,
        /// <summary>
        /// 特价
        /// </summary>
        Promotion,
        /// <summary>
        /// 往返产品
        /// </summary>
        Prodution,
        /// <summary>
        /// 特殊
        /// </summary>
        Special,
    }
    public enum PriceType : byte {
        Price,
        Discount
    }
    /// <summary>
    /// 政策操作方
    /// </summary>
    public enum PolicyOperatorRole {
        /// <summary>
        /// 出票方
        /// </summary>
        Provider,
        /// <summary>
        /// 产品方
        /// </summary>
        Resourcer,
        /// <summary>
        /// 平台
        /// </summary>
        Platform
    }
    public enum VoyageType : byte {
        /// <summary>
        /// 单程OneWay
        /// </summary>
        OneWay = 1,
        /// <summary>
        /// 往返RoundTrip
        /// </summary>
        RoundTrip = 2,
        /// <summary>
        /// 单程/往返 OneWayOrRound
        /// </summary>
        OneWayOrRound = 3
    }
    /// <summary>
    /// 出票方式
    /// </summary>
    public enum ETDZMode {
        /// <summary>
        /// 手工出票
        /// </summary>
        Manual,
        /// <summary>
        /// 自动出票
        /// </summary>
        Auto,
    }
    /// <summary>
    /// 产品方审核
    /// </summary>
    public enum ResourcerAuditStatus {
        /// <summary>
        /// 未审
        /// </summary>
        UnAudit,
        /// <summary>
        /// 已审
        /// </summary>
        Audited
    }
    /// <summary>
    /// 平台审核
    /// </summary>
    public enum PlatformAuditStatus {
        /// <summary>
        /// 未审
        /// </summary>
        UnAudit,
        /// <summary>
        /// 已审
        /// </summary>
        Audited
    }
    public enum RegulationType : byte {
        /// <summary>
        /// 退票
        /// </summary>
        Refund,
        /// <summary>
        /// 废票
        /// </summary>
        Invalid,
        /// <summary>
        /// 改签
        /// </summary>
        Change,
        /// <summary>
        /// 签转
        /// </summary>
        Endorse,
        /// <summary>
        /// 备注
        /// </summary>
        Remark
    }
    public enum SpecialProductType {
        /// <summary>
        /// 免费
        /// </summary>
        Free,
        /// <summary>
        /// 散冲团特惠产品
        /// </summary>
        SanChonggroup,
        /// <summary>
        /// 商旅卡特惠产品
        /// </summary>
        Business
    }
    /// <summary>
    /// 政策审核状态
    /// </summary>
    public enum PolicyAuditStatus {
        /// <summary>
        /// 未审
        /// </summary>
        UnAudit,
        /// <summary>
        /// 已审
        /// </summary>
        Audited,
    }
    /// <summary>
    /// 政策锁定状态
    /// </summary>
    public enum PolicyLockStatus {
        /// <summary>
        /// 未锁定
        /// </summary>
        Free,
        /// <summary>
        /// 已锁定
        /// </summary>
        Locked,
    }
    /// <summary>
    /// 证件类型
    /// </summary>
    public enum CredentialsType {
        身份证,
        护照,
        军官证,
        学生证,
        出生日期,
        台胞证,
        其他 = 9
    }
}