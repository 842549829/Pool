namespace ChinaPay.B3B.Data.DataMapping
{
    using System;
    using System.Collections.Generic;
    using Common.Enums;
    using Izual;
    using Izual.Data.Mapping;

    /// <summary>
    /// T_Regulation
    /// </summary>
    [Mapping("T_Regulation")]
    public class Regulation : IPersistable<Regulation>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// Airline
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public byte Type { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 公司信息
    /// </summary>
    [Mapping("T_Company")]
    public class Company : IPersistable<Company>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public CompanyType Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 缩写名称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 所在地址
        /// </summary>
        public Guid? Address { get; set; }
        /// <summary>
        /// 已启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 已审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// OfficePhones
        /// </summary>
        public string OfficePhones { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrginationCode { get; set; }
        /// <summary>
        /// Faxes
        /// </summary>
        public string Faxes { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectTime { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? Manager { get; set; }
        /// <summary>
        /// Contact
        /// </summary>
        public Guid? Contact { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public Guid? EmergencyContact { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType AccountType { get; set; }
        /// <summary>
        /// 是否 VIP
        /// </summary>
        public bool IsVip { get; set; }
        /// <summary>
        /// 是否 OEM
        /// </summary>
        public bool IsOem { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 是否开通外接口
        /// </summary>
        public bool IsOpenExternalInterface { get; set; }
        /// <summary>
        /// 是否全局设置采买限制
        /// </summary>
        public PurchaseLimitationType PurchaseLimitationType { get; set; }
        /// <summary>
        /// 是否是全局收益设置
        /// </summary>
        public IncomeGroupLimitType IncomeGroupLimitType { get; set; }
    }

    /// <summary>
    /// 公司组信息
    /// </summary>
    [Mapping("T_CompanyGroup")]
    public class CompanyGroup : IPersistable<CompanyGroup>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        ///// <summary>
        ///// 允许外部采购(采购其他出票方产品)
        ///// </summary>
        //public bool AllowExternalPurchase { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }
    }

    /// <summary>
    /// 员工信息
    /// </summary>
    [Mapping("T_Employee")]
    public class Employee : IPersistable<Employee>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string OfficePhone { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdministrator { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 最后登录 IP
        /// </summary>
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 最后登录地点
        /// </summary>
        public string LastLoginLocation { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// IP限制
        /// </summary>
        public string IpLimitation { get; set; }
    }

    /// <summary>
    /// 配置信息
    /// </summary>
    [Mapping("T_Configuration")]
    public class Configuration : IPersistable<Configuration>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// SI:工作号/密码
        /// </summary>
        public string SI { get; set; }
        /// <summary>
        /// 打票机序号
        /// </summary>
        public int? PrinterSN { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeNumber { get; set; }
    }

    /// <summary>
    /// 工作时间设置
    /// </summary>
    [Mapping("T_WorkingHours")]
    public class WorkingHours : IPersistable<WorkingHours>
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// 工作日上班时间
        /// </summary>
        public Time WorkdayWorkStart { get; set; }
        /// <summary>
        /// 工作日下班时间
        /// </summary>
        public Time WorkdayWorkEnd { get; set; }
        /// <summary>
        /// 工作日退票开始时间
        /// </summary>
        public Time WorkdayRefundStart { get; set; }
        /// <summary>
        /// 工作日退票结束时间
        /// </summary>
        public Time WorkdayRefundEnd { get; set; }
        /// <summary>
        /// 休息日上班时间
        /// </summary>
        public Time RestdayWorkStart { get; set; }
        /// <summary>
        /// 休息日下班时间
        /// </summary>
        public Time RestdayWorkEnd { get; set; }
        /// <summary>
        /// 休息日退票开始时间
        /// </summary>
        public Time RestdayRefundStart { get; set; }
        /// <summary>
        /// 休息日退票结束时间
        /// </summary>
        public Time RestdayRefundEnd { get; set; }
    }

    /// <summary>
    /// 工作信息设置
    /// </summary>
    [Mapping("T_WorkingSetting")]
    public class WorkingSetting : IPersistable<WorkingSetting>
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// 默认出发城市
        /// </summary>
        public string DefaultDeparture { get; set; }
        /// <summary>
        /// 默认到达城市
        /// </summary>
        public string DefaultArrival { get; set; }
        /// <summary>
        /// 可出成人票航空公司
        /// </summary>
        public string AirlineForAdult { get; set; }
        /// <summary>
        /// 可出儿童票航空公司
        /// </summary>
        public string AirlineForChild { get; set; }
        /// <summary>
        /// 儿童返点
        /// </summary>
        public decimal? RebateForChild { get; set; }
        /// <summary>
        /// 默认政策的航空公司
        /// </summary>
        public string AirlineForDefault { get; set; }
        /// <summary>
        /// 默认返点
        /// </summary>
        public decimal? RebateForDefault { get; set; }
        /// <summary>
        /// 默认 Office 号
        /// </summary>
        public string DefaultOfficeNumber { get; set; }

        /// <summary>
        /// 退票需财务审核
        /// </summary>
        public bool RefundNeedAudit { get; set; }
        /// <summary>
        /// 是否需要授权
        /// </summary>
        public bool IsImpower { get; set; }
        ///// <summary>
        ///// 工作日上班时间
        ///// </summary>
        //public Time WorkdayWorkStart { get; set; }
        ///// <summary>
        ///// 工作日下班时间
        ///// </summary>
        //public Time WorkdayWorkEnd { get; set; }
        ///// <summary>
        ///// 工作日退票开始时间
        ///// </summary>
        //public Time WorkdayRefundStart { get; set; }
        ///// <summary>
        ///// 工作日退票结束时间
        ///// </summary>
        //public Time WorkdayRefundEnd { get; set; }
        ///// <summary>
        ///// 休息日上班时间
        ///// </summary>
        //public Time RestdayWorkStart { get; set; }
        ///// <summary>
        ///// 休息日下班时间
        ///// </summary>
        //public Time RestdayWorkEnd { get; set; }
        ///// <summary>
        ///// 休息日退票开始时间
        ///// </summary>
        //public Time RestdayRefundStart { get; set; }
        ///// <summary>
        ///// 休息日退票结束时间
        ///// </summary>
        //public Time RestdayRefundEnd { get; set; }
    }

    /// <summary>
    /// T_SettingPolicy
    /// </summary>
    [Mapping("T_SettingPolicy")]
    public class SettingPolicy : IPersistable<SettingPolicy>
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// 允许发布的特价政策数量
        /// </summary>
        public int BargainCount { get; set; }
        ///// <summary>
        ///// 允许发布的特殊政策数量
        ///// </summary>
        //public int SpecialCount { get; set; }
        /// <summary>
        /// 允许发布的单程控位产品政策数量
        /// </summary>
        public int SinglenessCount { get; set; }
        /// <summary>
        /// 允许发布的散冲团产品政策数量
        /// </summary>
        public int DisperseCount { get; set; }
        /// <summary>
        /// 允许发布的免票产品政策数量
        /// </summary>
        public int CostFreeCount { get; set; }
        /// <summary>
        /// 允许发布的集体票产品政策数量
        /// </summary>
        public int BlocCount { get; set; }
        /// <summary>
        /// 允许发布的商旅卡产品政策数量
        /// </summary>
        public int BusinessCount { get; set; }
        /// <summary>
        /// 允许发布的其他特殊政策数量
        /// </summary>
        public int OtherSpecialCount { get; set; }
        /// <summary>
        /// 允许发布的低打高返数量
        /// </summary>
        public int LowToHighCount { get; set; }
        /// <summary>
        /// 允许发布的出港城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 允许发布的航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否允许发布儿童政策
        /// </summary>
        public bool AllowChildPolicy { get; set; }
        /// <summary>
        /// 儿童政策返点
        /// </summary>
        public decimal ChildRebate { get; set; }
        /// <summary>
        /// 允许发布儿童政策的航空公司
        /// </summary>
        public string ChildAirlines { get; set; }
        /// <summary>
        /// 是否允许发布婴儿政策
        /// </summary>
        public bool AllowInfantPolicy { get; set; }
        /// <summary>
        /// 婴儿政策返点
        /// </summary>
        public decimal InfantRebate { get; set; }
        /// <summary>
        /// 允许发布婴儿政策的航空公司
        /// </summary>
        public string InfantAirlines { get; set; }
    }

    /// <summary>
    /// 业务负责人
    /// </summary>
    [Mapping("T_BusinessManager")]
    public class BusinessManager : IPersistable<BusinessManager>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 业务名称
        /// </summary>
        public string BusinessName { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Mamanger { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// QQ 号
        /// </summary>
        public string QQ { get; set; }
    }

    /// <summary>
    /// T_VIPManagement
    /// </summary>
    [Mapping("T_VIPManagement")]
    public class VIPManagement : IPersistable<VIPManagement>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// IsVip
        /// </summary>
        public bool IsVip { get; set; }
    }

    /// <summary>
    /// 公司参数
    /// </summary>
    [Mapping("T_CompanyParameter")]
    public class CompanyParameter : IPersistable<CompanyParameter>
    {
        /// <summary>
        /// 所属的公司
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// BSP 自动出票
        /// </summary>
        public bool AutoPrintBSP { get; set; }
        /// <summary>
        /// B2B 自动出票
        /// </summary>
        public bool AutoPrintB2B { get; set; }
        /// <summary>
        /// 自己取消 PNR
        /// </summary>
        public bool CancelPnrBySelf { get; set; }
        /// <summary>
        /// 可发布 VIP 政策
        /// </summary>
        public bool CanReleaseVip { get; set; }
        /// <summary>
        /// 可开设内部机构
        /// </summary>
        public bool CanHaveSubordinate { get; set; }
        /// <summary>
        /// 允许同行采购
        /// </summary>
        public bool AllowBrotherPurchase { get; set; }
        /// <summary>
        /// 锁定政策积累退废票数
        /// </summary>
        public int RefundCountLimit { get; set; }
        /// <summary>
        /// 自愿退票时限
        /// </summary>
        public int RefundTimeLimit { get; set; }
        /// <summary>
        /// 全退时限
        /// </summary>
        public int FullRefundTimeLimit { get; set; }
        /// <summary>
        /// 同行交易费率
        /// </summary>
        public decimal ProfessionRate { get; set; }
        /// <summary>
        /// 下级交易费率
        /// </summary>
        public decimal SubordinateRate { get; set; }
        /// <summary>
        /// 有效期开始日期
        /// </summary>
        public DateTime? ValidityStart { get; set; }
        /// <summary>
        /// 有效期结束日期
        /// </summary>
        public DateTime? ValidityEnd { get; set; }
        /// <summary>
        /// 平台自动审核特殊政策
        /// </summary>
        public bool AutoPlatformAudit { get; set; }
        /// <summary>
        /// 单程控位产品
        /// </summary>
        public bool Singleness { get; set; }
        /// <summary>
        /// 散冲团产品
        /// </summary>
        public bool Disperse { get; set; }
        /// <summary>
        /// 商旅卡特惠产品
        /// </summary>
        public bool CostFree { get; set; }
        /// <summary>
        /// 集团票产品
        /// </summary>
        public bool Bloc { get; set; }
        /// <summary>
        /// 其他特殊产品
        /// </summary>
        public bool OtherSpecial { get; set; }
        /// <summary>
        /// 低打高返特殊产品
        /// </summary>
        public bool LowToHigh { get; set; }
        /// <summary>
        /// 商旅卡产品
        /// </summary>
        public bool Business { get; set; }
        /// <summary>
        /// 单程控位产品费率
        /// </summary>
        public decimal SinglenessRate { get; set; }
        /// <summary>
        /// 散冲团产品费率
        /// </summary>
        public decimal DisperseRate { get; set; }
        /// <summary>
        /// 商旅卡特惠产品费率
        /// </summary>
        public decimal CostFreeRate { get; set; }
        /// <summary>
        /// 集团票产品费率
        /// </summary>
        public decimal BlocRate { get; set; }
        /// <summary>
        /// 商旅卡产品费率
        /// </summary>
        public decimal BusinessRate { get; set; }
        /// <summary>
        /// 其他特殊产品费率
        /// </summary>
        public decimal OtherSpecialRate { get; set; }
        /// <summary>
        /// 低打高返特殊产品费率
        /// </summary>
        public decimal LowToHighRate { get; set; }
        /// <summary>
        /// 信誉评级
        /// </summary>
        public decimal? Creditworthiness { get; set; }
        ///// <summary>
        ///// 是否是全局采买设置
        ///// </summary>
        //public bool IsGlobalPurchase { get; set; }
        ///// <summary>
        ///// 是否是全局收益组设置
        ///// </summary>
        //public bool IsGlobalProfit { get; set; }
    }

    /// <summary>
    /// 公司组限制
    /// </summary>
    [Mapping("T_CompanyGroupLimitation")]
    public class CompanyGroupLimitation : IPersistable<CompanyGroupLimitation>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司组
        /// </summary>
        public Guid Group { get; set; }
        /// <summary>
        /// 限制航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 限制出港城市
        /// </summary>
        public string Departures { get; set; }
        ///// <summary>
        ///// 默认返点
        ///// </summary>
        //public decimal DefaultRebate { get; set; }
        ///// <summary>
        ///// 允许采购其他代理的政策
        ///// </summary>
        //public bool AllowExtendPurchase { get; set; }
    }

    /// <summary>
    /// OEM 信息
    /// </summary>
    //[Mapping("T_OemInfo")]
    //public class OemInfo : IPersistable<OemInfo>
    //{
    //    /// <summary>
    //    /// OEM 信息关联的公司
    //    /// </summary>
    //    [Identifier]
    //    public Guid Company { get; set; }
    //    /// <summary>
    //    /// 公司的 OEM 名称
    //    /// </summary>
    //    public string CompanyName { get; set; }
    //    /// <summary>
    //    /// OEM 平台名称
    //    /// </summary>
    //    public string PlatformName { get; set; }
    //    /// <summary>
    //    /// 域名
    //    /// </summary>
    //    public string Domain { get; set; }
    //    /// <summary>
    //    /// ICP 备案号
    //    /// </summary>
    //    public string IcpRecord { get; set; }
    //    /// <summary>
    //    /// LoginUrl
    //    /// </summary>
    //    public string LoginUrl { get; set; }
    //    /// <summary>
    //    /// Logo 文件路径
    //    /// </summary>
    //    public string LogoPath { get; set; }
    //    /// <summary>
    //    /// 出票联系电话
    //    /// </summary>
    //    public string PrintTicketPhone { get; set; }
    //    /// <summary>
    //    /// 退票联系电话
    //    /// </summary>
    //    public string RefundTicketPhone { get; set; }
    //    /// <summary>
    //    /// 客服电话
    //    /// </summary>
    //    public string ServicePhone { get; set; }
    //    /// <summary>
    //    /// 投诉电话
    //    /// </summary>
    //    public string ComplaintPhone { get; set; }
    //    /// <summary>
    //    /// 是否显示平台公告
    //    /// </summary>
    //    public bool ShowPlatformNotice { get; set; }
    //    /// <summary>
    //    /// 是否允许下级登录
    //    /// </summary>
    //    public bool AllowSubordinateLogin { get; set; }
    //    /// <summary>
    //    /// 是否启用
    //    /// </summary>
    //    public bool Enabled { get; set; }
    //}

    /// <summary>
    /// 关系信息
    /// </summary>
    [Mapping("T_Relationship")]
    public class Relationship : IPersistable<Relationship>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 发起方
        /// </summary>
        public Guid Initiator { get; set; }
        /// <summary>
        /// 接受方
        /// </summary>
        public Guid Responser { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public RelationshipType Type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 账户信息
    /// </summary>
    [Mapping("T_Account")]
    public class Account : IPersistable<Account>
    {
        /// <summary>
        /// 单位id
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Mapping("Account")]
        public string No { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid { get; set; }
        /// <summary>
        /// 账号类型(0-收款;1-付款;255-大区)
        /// </summary>
        [Identifier]
        public AccountType Type { get; set; }
        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime Time { get; set; }
    }

    /// <summary>
    /// 区县
    /// </summary>
    [Mapping("T_District")]
    public class District : IPersistable<District>
    {
        /// <summary>
        /// 代码
        /// </summary>
        [Identifier]
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属城市
        /// </summary>
        public string City { get; set; }
    }

    /// <summary>
    /// T_SpecialPolicyType
    /// </summary>
    [Mapping("T_SpecialPolicyType")]
    public class SpecialPolicyType : IPersistable<SpecialPolicyType>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 公司组与公司对应关系信息
    /// </summary>
    [Mapping("T_CompanyGroupRelation")]
    public class CompanyGroupRelation : IPersistable<CompanyGroupRelation>
    {
        /// <summary>
        /// 公司组
        /// </summary>
        [Identifier]
        public Guid Group { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
    }

    /// <summary>
    /// 注册统计信息
    /// </summary>
    [Mapping("T_RegisterStatiscation")]
    public class RegisterStatiscation : IPersistable<RegisterStatiscation>
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        [Identifier]
        public Guid Owner { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }
        /// <summary>
        /// 拥有客户类型
        /// </summary>
        public HasClientType HasClientType { get; set; }
        /// <summary>
        /// 获知方式
        /// </summary>
        public HowToKnow HowToKnow { get; set; }
        /// <summary>
        /// 推荐者工号
        /// </summary>
        public string Recommender { get; set; }
    }

    /// <summary>
    /// 代理资质信息
    /// </summary>
    [Mapping("T_AgentQualification")]
    public class AgentQualification : IPersistable<AgentQualification>
    {
        /// <summary>
        /// 公司编号
        /// </summary>
        [Identifier]
        public Guid Company { get; set; }
        /// <summary>
        /// 航协经营批准号
        /// </summary>
        public string Licence { get; set; }
        /// <summary>
        /// 国际航协号
        /// </summary>
        public string IATA { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNumbers { get; set; }
        /// <summary>
        /// 中航协担保金
        /// </summary>
        public decimal? Deposit { get; set; }
        /// <summary>
        /// 航空代理资质类型
        /// </summary>
        public QualificationType QualificationType { get; set; }
    }

    /// <summary>
    /// 联系人信息
    /// </summary>
    [Mapping("T_Contact")]
    public class Contact : IPersistable<Contact>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否主要联系人
        /// </summary>
        public bool IsPrincipal { get; set; }
        /// <summary>
        /// 是否紧急联系人
        /// </summary>
        public bool IsEmergency { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CertNo { get; set; }
        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficePhone { get; set; }
        /// <summary>
        /// 紧急联系电话
        /// </summary>
        public string EmergencyCall { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// MSN帐号
        /// </summary>
        public string MSN { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public Guid? Address { get; set; }
    }

    /// <summary>
    /// 地址信息
    /// </summary>
    [Mapping("T_Address")]
    public class Address : IPersistable<Address>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 国家代码
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份代码
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市代码
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县代码
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 街道名称
        /// </summary>
        public string Avenue { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// 产品提供方
    /// </summary>
    [Mapping("T_Company")]
    public class Provider : IPersistable<Provider>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Mapping("AbbreviateName")]
        public string NickName { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public Guid Address { get; set; }
        ///// <summary>
        ///// 邮政编码
        ///// </summary>
        //public string ZipCode { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public Guid Contact { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public CompanyType Type
        {
            get
            {
                return CompanyType.Provider;
            }
        }
    }

    /// <summary>
    /// Office 号
    /// </summary>
    [Mapping("T_OfficeNumber")]
    public class OfficeNumber : IPersistable<OfficeNumber>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否授权
        /// </summary>
        public bool Impower { get; set; }
    }

    /// <summary>
    /// T_SuspendedPolicy
    /// </summary>
    [Mapping("T_SuspendedPolicy")]
    public class SuspendedPolicy : IPersistable<SuspendedPolicy>
    {
        [Identifier]
        public Guid Company { get; set; }

        [Identifier]
        public string Airline { get; set; }

        public bool SuspendedByPlatform { get; set; }
    }
    /// <summary>
    /// T_SuspendOperation
    /// </summary>
    [Mapping("T_SuspendOperation")]
    public class SuspendOperation : IPersistable<SuspendOperation>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 被挂起的政策所属的航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 挂起或解挂原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public PolicySuspendOperationType OperateType { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 操作者IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 操作者类型
        /// </summary>
        public PublishRole OperatorRoleType { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public Guid CompanyId { get; set; }
    }

    /// <summary>
    /// 政策锁定操作记录
    /// </summary>
    [Mapping("T_FreezeOperation")]
    public class FreezeOperation : IPersistable<FreezeOperation>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 操作的政策的 Id 列表
        /// </summary>
        public string PolicyIds { get; set; }
        /// <summary>
        /// 操作原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public PolicyFreezeOperationType OperateType { get; set; }
    }

    /// <summary>
    /// T_PermissionRole
    /// </summary>
    [Mapping("T_PermissionRole")]
    public class PermissionRole
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// Company
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// Valid
        /// </summary>
        public bool Valid { get; set; }
    }

    /// <summary>
    /// T_UserPermission
    /// </summary>
    [Mapping("T_UserPermission")]
    public class UserPermission
    {
        /// <summary>
        /// Role
        /// </summary>
        public Guid Role { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public Guid User { get; set; }
    }

    /// <summary>
    /// 政策协调信息
    /// </summary>
    [Mapping("T_PolicyHarmony")]
    public class PolicyHarmony : IPersistable<PolicyHarmony>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
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

    /// <summary>
    /// T_RoundTripPolicy，应该要作废了吧？
    /// </summary>
    [Mapping("T_RoundTripPolicy")]
    public class RoundTripPolicy : IPersistable<RoundTripPolicy>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策所属的航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 自动出票使用的 Office 号
        /// </summary>
        public string OfficeCode { get; set; }

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
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }
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
        /// 回程班期起始日期
        /// </summary>
        public DateTime? ReturnDateStart { get; set; }
        /// <summary>
        /// 回程班期结束日期
        /// </summary>
        public DateTime? ReturnDateEnd { get; set; }
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
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 使用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部返佣
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 票证类型
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
        /// 是否锁定
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
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
    }

    /// <summary>
    /// T_NormalPolicy
    /// </summary>
    [Mapping("T_NormalPolicy")]
    public class NormalPolicy : IPersistable<NormalPolicy>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策所属的航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 自动出票使用的 Office 号
        /// </summary>
        public string OfficeCode { get; set; }
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
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }
        ///// <summary>
        ///// 去程班期过滤(作废)
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }
        ///// <summary>
        ///// 去程班期过滤类型(作废)
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
        ///// <summary>
        ///// 回程班期起始日期(作废)
        ///// </summary>
        //public DateTime? ReturnDateStart { get; set; }
        ///// <summary>
        ///// 回程班期结束日期(作废)
        ///// </summary>
        //public DateTime? ReturnDateEnd { get; set; }
        ///// <summary>
        ///// 回程班期过滤(作废)
        ///// </summary>
        //public string ReturnDatesFilter { get; set; }
        ///// <summary>
        ///// 回程班期过滤类型(作废)
        ///// </summary>
        //public DateMode ReturnDatesFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }
        ///// <summary>
        ///// 旅游天数（这个在普通政策里也应该没有，应该作废）
        ///// </summary>
        //public short TravelDays { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 排除日期(新增)
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制(新增)
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部返佣
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        ///// <summary>
        ///// VIP 返佣（这期没有）
        ///// </summary>
        //public decimal Vip { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 是否自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        /// <summary>
        /// 是否适用于往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 票证类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否被锁定
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
        /// 修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否需要office授权
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 起飞前2小时可出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
    }

    /// <summary>
    /// T_TeamPolicy
    /// </summary>
    [Mapping("T_TeamPolicy")]
    public class TeamPolicy : IPersistable<TeamPolicy>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策所属的航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 自动出票使用的 Office 号
        /// </summary>
        public string OfficeCode { get; set; }
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
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 回程航班过滤
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 排除日期(新增)
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制(新增)
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 是否指定团队舱位
        /// </summary>
        public bool AppointBerths { get; set; }
        /// <summary>
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部返佣
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 是否自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        /// <summary>
        /// 是否适用于往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 票证类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否被锁定
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
        /// 修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否需要office授权
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 起飞前2小时可出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
    }
    /// <summary>
    /// 出发到达
    /// </summary>
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
        /// 是否是包含
        /// </summary>
        public bool IsAllowable { get; set; }
    }

    /// <summary>
    /// T_NotchPolicy
    /// </summary>
    public class NotchPolicy
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策所属的航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 是否需要可以发布内部返点
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// 是否需可以发同行返点
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 自动出票使用的 Office 号
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发到达城市
        /// </summary>
        public List<NotchPolicyDepartureArrival> DepartureArrival { get; set; }
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string DrawerCondition { get; set; }
        /// <summary>
        /// 排除日期(新增)
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制(新增)
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 内部返佣
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 票证类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否被锁定
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
        /// 修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否需要office授权
        /// </summary>
        public bool ImpowerOffice { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 起飞前2小时可出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }

        public override string ToString()
        {
            return string.Format(@"编号:{0} 政策所有者:{1} 航空公司:{2} OFFICE号:{3} 自定义编号:{4} 是否需要OFFICE号授权:{5} 是否可以发布内部返点:{6} 是否可以发布同行返点:{7} 行程类型:{8} 航班开始日期:{9} 航班结束日期:{10} 排除日期:{11} 出票条件:{12} 备注:{13} 舱位:{14} 内部返点:{15} 下级返点:{16} 同行返点:{17} 是否自动审核:{18} 是否需要换编码出票:{19} 客票类型:{20} 出票时间:{21} 是否被挂起:{22} 是否锁定:{23} 是否已审核:{24} 创建时间:{25} 审核时间:{26} 创建人:{27} 最后修改时间:{28} 公司简称:{29} 起飞前两小时是否可以出票:{30} 去程航班过滤:{31} 去程航班过滤类型:{32} 班期周期限制：{33}", Id, Owner, Airline, OfficeCode, CustomCode, ImpowerOffice, IsInternal, IsPeer, "缺口程", DepartureDateStart, DepartureDateEnd, DepartureDateFilter, DrawerCondition, Remark, Berths, InternalCommission, SubordinateCommission, ProfessionCommission, AutoAudit, ChangePNR, TicketType, StartPrintDate, Suspended, Freezed, Audited, CreateTime, AuditTime, Creator, LastModifyTime, AbbreviateName, PrintBeforeTwoHours, DepartureFlightsFilter, DepartureFlightsFilterType == LimitType.None ? "" : DepartureFlightsFilterType == LimitType.Include ? "限制" : "仅包含", DepartureWeekFilter);
        }
    }



    /// <summary>
    /// T_BargainPolicy
    /// </summary>
    [Mapping("T_BargainPolicy")]
    public class BargainPolicy : IPersistable<BargainPolicy>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策所属的航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 自动出票使用的 Office 号
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
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 去程航班限制(中转联程第一程)
        /// </summary>
        public string DepartureFlightsFilter { get; set; }

        ///// <summary>
        ///// 去程班期限制类型
        ///// </summary>
        //public DateMode DepartureDatesFilterType { get; set; }
        ///// <summary>
        ///// 去程班期限制
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }

        /// <summary>
        /// 去程航班限制类型
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
        /// 最少提前天数
        /// </summary>
        public short BeforehandDays { get; set; }
        /// <summary>
        /// 最多提前天数
        /// </summary>
        public short MostBeforehandDays { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 出行天数
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
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 适用多段联程
        /// </summary>
        public bool MultiSuitReduce { get; set; }
        /// <summary>
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 内部返佣
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 发布价格类型
        /// </summary>
        public PriceType PriceType { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否需要换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 票证类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否被锁定
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
        /// 修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 起飞前2小时可出票
        /// </summary>
        public bool PrintBeforeTwoHours { get; set; }
    }

    /// <summary>
    /// T_SpecialPolicy
    /// </summary>
    [Mapping("T_SpecialPolicy")]
    public class SpecialPolicy : IPersistable<SpecialPolicy>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 政策的所有者
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
        /// 是否是有位出票（false是无位，代表true有位）
        /// </summary>
        public bool IsSeat { get; set; }
        /// <summary>
        /// 特殊政策类型
        /// </summary>
        public SpecialProductType Type { get; set; }
        /// <summary>
        /// 政策所属的航空公司
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
        /// 去程航班日期起始
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程航班日期结束
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }

        //// 2012-10-18
        ///// <summary>
        ///// 去程班期限制类型
        ///// </summary>
        //public DateMode DepartureDatesFilterType { get; set; }
        ///// <summary>
        ///// 去程班期过滤
        ///// </summary>
        //public string DepartureDatesFilter { get; set; }

        /// <summary>
        /// 去程航班限制
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班限制类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public short BeforehandDays { get; set; }
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
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 资源提供日期
        /// </summary>
        public DateTime ProvideDate { get; set; }
        /// <summary>
        /// 提供资源数量
        /// </summary>
        public int ResourceAmount { get; set; }
        /// <summary>
        /// 是否自动审核
        /// </summary>
        public bool AutoAudit { get; set; }
        /// <summary>
        /// 是否已通过平台审核
        /// </summary>
        public bool PlatformAudited { get; set; }
        /// <summary>
        /// 是否黑屏同步(Synchronization)
        /// </summary>
        public bool SynBlackScreen { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
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
        /// 是否需要确认座位
        /// </summary>
        public bool ConfirmResource { get; set; }
        /// <summary>
        /// 是否挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否锁定
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
        /// 修改时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否是特价舱位(2012 10 20 wangshiling 新增)
        /// </summary>
        public bool IsBargainBerths { get; set; }
        /// <summary>
        /// 公司缩写名
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 起飞前2小时可用B2B出票
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


    /// <summary>
    /// 普通默认政策
    /// </summary>
    [Mapping("T_DefaultPolicy")]
    public class DefaultPolicy : IPersistable<DefaultPolicy>
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        [Identifier]
        public string Airline { get; set; }
        /// <summary>
        /// 成人默认出票方
        /// </summary>
        public Guid AdultProvider { get; set; }
        /// <summary>
        /// 成人默认佣金
        /// </summary>
        public decimal AdultCommission { get; set; }
        /// <summary>
        /// 儿童默认出票方
        /// </summary>
        public Guid ChildProvider { get; set; }
        /// <summary>
        /// 儿童默认佣金
        /// </summary>
        public decimal ChildCommission { get; set; }
    }
    /// <summary>
    /// 特价默认政策
    /// </summary>
    [Mapping("T_BargainDefaultPolicy")]
    public class BargainDefaultPolicy : IPersistable<BargainDefaultPolicy>
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        [Identifier]
        public string Airline { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Identifier]
        public string Province { get; set; }
        /// <summary>
        /// 成人出票方
        /// </summary>
        public Guid AdultProvider { get; set; }
        /// <summary>
        /// 返点
        /// </summary>
        public decimal AdultCommission { get; set; }
    }
    /// <summary>
    /// 政策设置信息
    /// </summary>
    [Mapping("T_PolicySetting")]
    public class PolicySetting : IPersistable<PolicySetting>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string Arrivals { get; set; }
        ///// <summary>
        ///// 适用的行程类型
        ///// </summary>
        //public VoyageType VoyageType { get; set; }
        /// <summary>
        /// true 表示扣点，false表示贴点
        /// </summary>
        public bool RebateType { get; set; }
        /// <summary>
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 生效起始时间
        /// </summary>
        public DateTime EffectiveTimeStart { get; set; }
        /// <summary>
        /// 生效结束时间
        /// </summary>
        public DateTime EffectiveTimeEnd { get; set; }
        /// <summary>
        /// 启用禁用
        /// </summary>
        public bool Enable { get; set; }

        ///// <summary>
        ///// 贴点时段开始
        ///// </summary>
        //public DateTime? MountStart { get; set; }
        ///// <summary>
        ///// 贴点时段结束
        ///// </summary>
        //public DateTime? MountEnd { get; set; }
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
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }
    }
    /// <summary>
    /// 扣点区域信息
    /// </summary>
    [Mapping("T_PolicySettingPeriod")]
    public class PolicySettingPeriod : IPersistable<PolicySettingPeriod>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 区间所属的政策设置 Id
        /// </summary>
        public Guid PolicySetting { get; set; }
        /// <summary>
        /// 扣点区域起始
        /// </summary>
        public decimal PeriodStart { get; set; }
        /// <summary>
        /// 扣点区域结束
        /// </summary>
        public decimal PeriodEnd { get; set; }
        /// <summary>
        /// 扣点/贴点(设置的值： 大于 0 扣点；小于 0 贴点)
        /// </summary>
        public decimal Rebate { get; set; }
        /// <summary>
        /// 最高贴点值
        /// </summary>
        public decimal MaxRebate { get; set; }
    }
    /// <summary>
    /// 员工分配授权自定义编号
    /// </summary>
    [Mapping("T_EmpowermentCustom")]
    public class EmpowermentCustom : IPersistable<EmpowermentCustom>
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public Guid Employee { get; set; }
        /// <summary>
        /// 自定义编号Id
        /// </summary>
        public Guid CustomNumber { get; set; }
    }
    /// <summary>
    /// 自定义编号
    /// </summary>
    [Mapping("T_CustomNumber")]
    public class CustomNumber : IPersistable<CustomNumber>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 自定义编号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool Enabled { get; set; }
    }


    /// <summary>
    /// B3B用户建议
    /// </summary>
    [Mapping("T_Suggest")]
    public class Suggest
    {
        /// <summary>
        /// 建议Id
        /// </summary>
        [Identifier]
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 建议类型
        /// </summary>
        public SuggestCategory SuggestCategory
        {
            get;
            set;
        }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContractInformation
        {
            get;
            set;
        }
        /// <summary>
        /// 建议内容
        /// </summary>
        public string SuggestContent
        {
            get;
            set;
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已阅
        /// </summary>
        public bool Readed
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool Handled
        {
            get;
            set;
        }
        /// <summary>
        /// 提交人帐号
        /// </summary>
        public string Creator
        {
            get;
            set;
        }
        /// <summary>
        /// 提交人姓名
        /// </summary>
        public string CreatorName
        {
            get;
            set;
        }
    }
}
