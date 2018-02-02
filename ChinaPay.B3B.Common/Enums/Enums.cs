namespace ChinaPay.B3B.Common.Enums
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// OEM利润类型
    /// </summary>
    public enum OemProfitType
    {
        /// <summary>
        /// 加价
        /// </summary>
        PriceMarkup,
        /// <summary>
        /// 折扣
        /// </summary>
        Discount
    }

    /// <summary>
    /// 日期方式
    /// </summary>
    public enum DateMode
    {
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
    public enum LimitType
    {
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
    public enum TicketType
    {
        [Description("B2B")]
        B2B,
        [Description("BSP")]
        BSP
    }

    /// <summary>
    /// 舱位类型
    /// </summary>
    public enum BunkType
    {
        [Description("经济舱")]
        Economic,
        [Description("头等公务舱")]
        FirstOrBusiness,
        [Description("特价舱")]
        Promotion,
        [Description("往返产品舱")]
        Production,
        [Description("中转联程舱")]
        Transfer,
        [Description("免票")]
        Free,
        [Description("团队")]
        Team
    }

    /// <summary>
    /// 乘客类型
    /// </summary>
    public enum PassengerType
    {
        [Description("成人")]
        Adult,
        [Description("儿童")]
        Child
    }

    /// <summary>
    /// 发布角色
    /// </summary>
    public enum PublishRole
    {
        [Description("平台")]
        平台,
        [Description("用户")]
        用户
    }

    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradementType
    {
        Pay,
        Refund
    }

    public enum EnableEnum
    {
        [Description("无效")]
        DisEnable,
        [Description("有效")]
        Enable
    }

    /// <summary>
    /// 公司类型
    /// </summary>
    [Flags]
    public enum CompanyType : byte
    {
        /// <summary>
        /// 出票方
        /// </summary>
        [Description("出票方")]
        Provider = 1,

        /// <summary>
        /// 采购商
        /// </summary>
        [Description("采购商")]
        Purchaser = 2,

        /// <summary>
        /// 产品方
        /// </summary>
        [Description("产品方")]
        Supplier = 4,

        /// <summary>
        /// 平台
        /// </summary>
        [Description("平台")]
        Platform = 8
    }

    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountBaseType : byte
    {
        [Description("个人")]
        Individual = 0x0,
        [Description("企业")]
        Enterprise = 0x1
    }

    public enum VerfiCodeType : byte
    {
        [Description("注册")]
        Register,
        [Description("忘记密码")]
        ForgetPwd,
        [Description("其他")]
        Other
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender : byte
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male,

        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female
    }

    /// <summary>
    /// 业务类型
    /// </summary>
    [Flags]
    public enum BusinessType : byte
    {
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
    public enum HasClientType : byte
    {
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
    public enum QualificationType : byte
    {
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

    public enum HowToKnow : byte
    {
        [Description("广告")]
        Advertising,
        [Description("网络宣传")]
        Web,
        [Description("同行介绍")]
        Profession,
        [Description("销售推荐")]
        Recommend
    }

    public enum WorkHoursType : byte
    {
        WorkingDays,
        RestDays
    }

    /// <summary>
    ///     关系类型
    /// </summary>
    public enum RelationshipType : byte
    {
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
    public enum CompanyStatus : byte
    {
        /// <summary>
        /// 未审
        /// </summary>
        [Description("未审")]
        UnAduited,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enabled,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disabled,
    }

    /// <summary>
    /// 政策类型
    /// </summary>
    [Flags]
    public enum PolicyType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0x0,

        /// <summary>
        /// 普通默认
        /// </summary>
        [Description("普通默认")]
        NormalDefault = 0x01,

        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Normal = 0x02,

        /// <summary>
        /// 特价
        /// </summary>
        [Description("特价")]
        Bargain = 0x04,

        /// <summary>
        /// 特殊
        /// </summary>
        [Description("特殊")]
        Special = 0x08,

        /// <summary>
        /// 团队
        /// </summary>
        [Description("团队")]
        Team = 0x10,

        /// <summary>
        /// 特价默认政策
        /// </summary>
        [Description("特价默认")]
        BargainDefault = 0x20,

        /// <summary>
        /// 所有者默认政策
        /// </summary>
        [Description("公司默认")]
        OwnerDefault = 0x40,
        /// <summary>
        /// 缺口
        /// </summary>
        [Description("缺口")]
        Notch = 0x80
    }

    public enum PriceType : byte
    {
        /// <summary>
        /// 直接价格
        /// </summary>
        [Description("价格")]
        Price,

        /// <summary>
        /// 按折扣
        /// </summary>
        [Description("折扣")]
        Discount,

        /// <summary>
        /// 按价格直减
        /// </summary>
        [Description("直减")]
        Subtracting,
        /// <summary>
        /// 按返佣
        /// </summary>
        [Description("返佣")]
        Commission
    }

    //采购时在面对需要授权的政策作出的选择
    public enum AuthenticationChoise : byte
    {
        [Description("")]
        NoNeedAUTH = 0x0,
        [Description("我愿意对编码进行授权")]
        AgreeAUTH = 0x1,
        [Description("我未对编码授权，但同意换编码出票")]
        NoAUTHandArgee = 0x2
    }

    /// <summary>
    /// 政策操作方
    /// </summary>
    public enum PolicyOperatorRole
    {
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
    [Flags]
    public enum VoyageType : byte
    {
        /// <summary>
        /// 单程OneWay
        /// </summary>
        [Description("单程")]
        OneWay = 1,

        /// <summary>
        /// 往返RoundTrip
        /// </summary>
        [Description("往返")]
        RoundTrip = 2,

        /// <summary>
        /// 单程/往返 OneWayOrRound
        /// </summary>
        [Description("单程/往返")]
        OneWayOrRound = 4,

        /// <summary>
        /// 中转联程 TransitWay
        /// </summary>
        [Description("中转联程")]
        TransitWay = 8,

        /// <summary>
        /// 缺口程
        /// </summary>
        [Description("缺口")]
        Notch = 16
    }

    /// <summary>
    /// 出票方式
    /// </summary>
    public enum ETDZMode
    {
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
    /// 审核状态
    /// </summary>
    public enum AuditStatus
    {
        /// <summary>
        /// 未审
        /// </summary>
        UnAudit,

        /// <summary>
        /// 已审
        /// </summary>
        Audited
    }

    public enum CompanyAuditStatus
    {
        [Description("未审")]
        UnAudit,
        [Description("审核通过")]
        Audited,
        [Description("审核拒绝")]
        Refused
    }

    public enum RegulationType : byte
    {
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

    public enum SpecialProductType
    {
        /// <summary>
        /// 单程控位产品
        /// </summary>
        [Description("单程控位产品")]
        Singleness,

        /// <summary>
        /// 散冲团产品
        /// </summary>
        [Description("散冲团产品")]
        Disperse,

        /// <summary>
        /// 免票产品
        /// </summary>
        [Description("免票产品")]
        CostFree,

        /// <summary>
        ///集团票产品
        /// </summary>
        [Description("集团票产品")]
        Bloc,

        /// <summary>
        /// 商旅卡产品
        /// </summary>
        [Description("商旅卡产品")]
        Business,

        /// <summary>
        /// 其他特殊产品
        /// </summary>
        [Description("其他特殊产品")]
        OtherSpecial,

        /// <summary>
        /// 低打高返产品
        /// </summary>
        [Description("低打高返产品")]
        LowToHigh

    }

    /// <summary>
    /// 政策锁定状态
    /// </summary>
    public enum PolicyLockStatus
    {
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
    public enum CredentialsType
    {
        身份证,
        护照,
        军官证,
        学生证,
        出生日期,
        台胞证,
        其他 = 9
    }

    /// <summary>
    /// 账号类型
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// 大区账号
        /// </summary>
        [Description("大区账号")]
        Area,

        /// <summary>
        /// 付款账号
        /// </summary>
        [Description("付款账号")]
        Payment,

        /// <summary>
        /// 收款账号
        /// </summary>
        [Description("收款账号")]
        Receiving
    }
    [Flags]
    public enum RelationType
    {
        /// <summary>
        /// 同行
        /// </summary>
        [Description("同行")]
        Brother = 1,

        /// <summary>
        /// 下级
        /// </summary>
        [Description("下级")]
        Junion = 2,

        /// <summary>
        /// 内部机构
        /// </summary>
        [Description("内部机构")]
        Interior = 4
    }

    /// <summary>
    /// 政策挂起操作类型
    /// </summary>
    public enum PolicySuspendOperationType
    {
        /// <summary>
        /// 挂起
        /// </summary>
        [Description("挂起")]
        Suspend,

        /// <summary>
        /// 解挂
        /// </summary>
        [Description("解挂")]
        Unsuspend
    }

    public enum PolicyFreezeOperationType
    {
        /// <summary>
        /// 冻结（锁定）
        /// </summary>
        [Description("锁定")]
        Freeze,

        /// <summary>
        /// 解冻（解锁）
        /// </summary>
        [Description("解锁")]
        Unfreeze
    }

    /// <summary>
    /// 返佣类型
    /// </summary>
    public enum DeductionType
    {
        /// <summary>
        /// 内部
        /// </summary>
        [Description("内部")]
        Internal,

        /// <summary>
        /// 下级
        /// </summary>
        [Description("下级")]
        Subordinate,

        /// <summary>
        /// 同行
        /// </summary>
        [Description("同行")]
        Profession
    }

    /// <summary>
    /// 扣点贴点类型
    /// </summary>
    public enum MountDiscountType
    {
        /// <summary>
        /// 扣点
        /// </summary>
        [Description("扣点")]
        Discount,

        /// <summary>
        /// 贴点
        /// </summary>
        [Description("贴点")]
        MountPoint
    }

    public enum ConfigurationType
    {
        [Description("C配置")]
        CConfig,
        [Description("信天游")]
        XTYConfig
    }

    public enum TicketState
    {
        /// <summary>
        /// 出票
        /// </summary>
        [Description("出票")]
        Provider,

        /// <summary>
        /// 退票
        /// </summary>
        [Description("退票")]
        Refund,

        /// <summary>
        /// 废票
        /// </summary>
        [Description("废票")]
        Invalid,

        /// <summary>
        /// 改期
        /// </summary>
        [Description("改期")]
        Change
    }

    /// <summary>
    /// 消费积分有效期
    /// </summary>
    public enum IntegralRangeTime
    {
        /// <summary>
        /// 永不
        /// </summary>
        [Description("永不")]
        Never,

        /// <summary>
        /// 五年
        /// </summary>
        [Description("五年")]
        FiveYears,

        /// <summary>
        /// 两年
        /// </summary>
        [Description("两年")]
        TwoYears,

        /// <summary>
        /// 一年
        /// </summary>
        [Description("一年")]
        OneYears,

        /// <summary>
        /// 半年
        /// </summary>
        [Description("半年")]
        HalfYears,

        /// <summary>
        /// 一季度
        /// </summary>
        [Description("一季度")]
        OneQuarter,

        /// <summary>
        /// 一个月
        /// </summary>
        [Description("一个月")]
        OnemMonth,

        /// <summary>
        /// 指定日期
        /// </summary>
        [Description("指定日期")]
        SpecifiedDate
    }

    /// <summary>
    /// 兑换状态
    /// </summary>
    public enum ExchangeState
    {
        [Description("处理中")]
        Processing,
        [Description("已处理（成功兑换）")]
        Success,
        [Description("已处理（拒绝兑换）")]
        Refuse,
        All = 99
    }

    /// <summary>
    /// 获取积分途径
    /// </summary>
    public enum IntegralWay
    {
        [Description("登录奖励")]
        SignIn,
        [Description("购买机票")]
        Buy,
        [Description("未登录减少")]
        NotSignIn,
        [Description("兑换实物商品")]
        Exchange,
        [Description("退票减少")]
        TuiPiao,
        [Description("定期清空")]
        ClearDate,
        [Description("开户奖励")]
        OpenAccount,
        [Description("兑换短信数量")]
        ExchangeSms,
        [Description("拒绝兑换商品返回积分")]
        RefuseExchange,
        [Description("所有")]
        All = 99

    }

    /// <summary>
    /// 审核类型
    /// </summary>
    public enum AuditType
    {
        [Description("普通审核")]
        NormalAudit,
        [Description("变更审核")]
        ApplyAudit
    }

    /// <summary>
    /// 旅行类型
    /// </summary>
    [Flags]
    public enum TravelTypeValue
    {
        /// <summary>
        /// 散客
        /// </summary>
        [Description("散客")]
        Individual = 1,

        /// <summary>
        /// 团队
        /// </summary>
        [Description("团队")]
        Team = 2
    }

    /// <summary>
    /// 适用行程
    /// </summary>
    [Flags]
    public enum VoyageTypeValue
    {
        /// <summary>
        /// 单程
        /// </summary>
        [Description("单程")]
        OneWay = 1,

        /// <summary>
        /// 往返
        /// </summary>
        [Description("往返")]
        RoundTrip = 2,

        /// <summary>
        /// 中转联程
        /// </summary>
        [Description("中转联程")]
        TransitWay = 4,

        /// <summary>
        /// 多段联程
        /// </summary>
        [Description("多段联程")]
        OneWayOrRound = 8,

        /// <summary>
        /// 缺口程
        /// </summary>
        [Description("缺口程")]
        Notch = 16
    }

    /// <summary>
    /// 旅客类型
    /// </summary>
    [Flags]
    public enum PassengerTypeValue
    {
        [Description("成人")]
        Adult = 1,
        [Description("儿童")]
        Child = 2
    }

    /// <summary>
    /// 用户建议分类
    /// </summary>
    public enum SuggestCategory
    {
        [Description("使用不方便")]
        UseBad = 0x1,
        [Description("政策不满意")]
        NoGodoPolicy = 0x2,
        [Description("页面有错误")]
        PageError = 0x4,
        [Description("数据不准确")]
        ErrorData = 0x8,
        [Description("界面不美观")]
        BadUI = 0x10,
        [Description("其他建议")]
        Other = 0x20
    }
    /// <summary>
    /// 操作角色
    /// </summary>
    public enum OperatorRole
    {
        [Description("平台")]
        Platform,
        [Description("出票方")]
        Provider,
        [Description("产品方")]
        Resourcer,
        [Description("采购")]
        Purchaser,
        /// <summary>
        /// 用户
        /// 包括出票方、产品方、采购
        /// </summary>
        [Description("用户")]
        User,
        [Description("系统")]
        System
    }

    /// <summary>
    /// 操作模块
    /// </summary>
    public enum OperationModule
    {
        基础数据,
        系统参数,
        系统字典表,
        权限,
        系统设置,
        公告管理,
        政策,
        单位,
        员工,
        公司组,
        积分,
        商品,
        特殊产品管理,
        短信套餐,
        网站更新日志,
        OEM信息设置,
        其他 = 99,
    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        [Description("新增")]
        Insert,
        [Description("修改")]
        Update,
        [Description("删除")]
        Delete,
        [Description("其它")]
        Else = 99
    }
    public enum PublishRoles
    {
        [Description("平台")]
        平台,
        [Description("OEM")]
        OEM
    }
    /// <summary>
    /// 清Q类型
    /// </summary>
    public enum TransferType
    {
        [Description("时间变动")]
        Delay = 1,
        [Description("航班调整")]
        Change = 2,
        [Description("航班取消")]
        Canceled = 3
    }


    public enum InformResult
    {
        [Description("未通知")]
        UnDo = 1,
        [Description("已通知成功")]
        Informed = 2,
        [Description("未通知成功")]
        InformFail = 4
    }

    /// <summary>
    /// 通知
    /// </summary>
    public enum InformType
    {

        [Description("QQ通知")]
        QQ = 1,
        [Description("电话通知")]
        PHONE = 2,
        [Description("邮件通知")]
        MAIL = 4,
        [Description("短信通知")]
        Message = 8,
    }

    public enum AllowTicketType
    {
        BSP = 1,
        B2B = 2,
        Both = 4,
        None = 8,
        B2BOnPolicy = 16
    }


    public enum PlatformType
    {
        [Description("B3B")]
        B3B = 1,
        [Description("易行")]
        Yeexing = 2,
        [Description("51Book")]
        WYbook = 4,
        [Description("517")]
        CD517 = 8,
    }

    public enum PayStatus
    {
        [Description("已支付")]
        Paied,
        [Description("支付失败")]
        PayFail,
        [Description("未支付")]
        NoPay
    }
    /// <summary>
    /// 特殊集团政策中低价的枚举
    /// </summary>
    public enum LowNoType
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,
        /// <summary>
        /// 票面价区间
        /// </summary>
        [Description("票面价区间")]
        LowInterval = 1
    }
    /// <summary>
    /// 更新日志的枚举
    /// </summary>
    public enum ReleaseNoteType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        None = 0,
        /// <summary>
        /// B3B
        /// </summary>
        [Description("B3B")]
        B3BVisible = 1,
        /// <summary>
        /// 国付通
        /// </summary>
        [Description("国付通")]
        PoolpayVisible = 2
    }

    /// <summary>
    /// 商品类型
    /// </summary>
    public enum CommodityType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        None = 0,
        /// <summary>
        /// 实物商品
        /// </summary>
        [Description("实物商品")]
        Entity = 1,
        /// <summary>
        /// 短信商品
        /// </summary>
        [Description("短信商品")]
        SMS = 2
    }

    /// <summary>
    /// 更新国付通日志的枚举
    /// </summary>
    public enum PoolpayNoteType
    {
        /// <summary>
        /// 前台可见
        /// </summary>
        [Description("前台可见")]
        ForeStageVisible = 1,
        /// <summary>
        /// 后台可见
        /// </summary>
        [Description("后台可见")]
        BackStageVisible = 2
    }

    /// <summary>
    /// 指令使用类型
    /// </summary>
    public enum ConfigUseType
    {
        [Description("订座")]
        Reserve = 0,
        [Description("航班查询")]
        Query = 1,
        [Description("清Q")]
        QS = 2
    }
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum OEMLinkType 
    {
        [Description("页头")]
        Header = 1,
        [Description("页脚")]
        Footer = 2
    }
    /// <summary>
    /// oem下用户设置收益组的扣点类型
    /// </summary>
    public enum PeriodType
    {
        [Description("区间扣点")]
        Interval = 1,
        [Description("统一扣点")]
        Unite = 2
    }

    /// <summary>
    /// 公告范围
    /// </summary>
    [Flags]
    public enum AnnounceScope
    {
        B3B=1,
        OEM=2
    }
    /// <summary>
    /// oem提交的商品状态
    /// </summary>
    public enum OEMCommodityState
    {
        [Description("处理中")]
        Processing = 1,
        [Description("已提交")]
        Success = 2
    }
    /// <summary>
    /// 代扣账号类型
    /// </summary>
    public enum WithholdingAccountType { 
        [Description("国付通")]
        Poolpay,
        [Description("支付宝")]
        Alipay,
    }
    /// <summary>
    /// 代扣签约状态
    /// </summary>
    public enum WithholdingProtocolStatus {
        [Description("已提交")]
        Submitted,
        [Description("成功")]
        Success,

    }
    public enum OrderType {
        [Description("订单")]
        Order = 0,
        [Description("申请单")]
        Postpone = 1,
    }
    /// <summary>
    /// 类型
    /// </summary>
    public enum RoyaltyReportType
    {
        [Description("收款")]
        Income = 0,
        [Description("退款")]
        Refund=1
    }

    /// <summary>
    /// 退款状态
    /// </summary>
    public enum RefundStatus
    {
        [Description("等待卖家退款")]
        WaitRefund = 0,
        [Description("退款失败")]
        RefundFailed = 1,
        [Description("退款成功")]
        RefundSuccess = 2,
    }
    /// <summary>
    /// 采买限制返点类型
    /// </summary>
    public enum PurchaseLimitationRateType
    {
        [Description("普通")]
        Normal,
        [Description("特价")]
        Bargain
    }
    /// <summary>
    /// 采买限制类型
    /// </summary>
    public enum PurchaseLimitationType
    {
        [Description("不限制")]
        None,
        [Description("分组限制")]
        Each,
        [Description("全局限制")]
        Global
    }
    /// <summary>
    /// 收益设置类型
    /// </summary>
    public enum IncomeGroupLimitType
    {
        [Description("不限制")]
        None,
        [Description("分组限制")]
        Each,
        [Description("全局限制")]
        Global
    }

}