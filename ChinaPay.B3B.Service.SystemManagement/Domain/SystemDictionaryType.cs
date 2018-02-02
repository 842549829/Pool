namespace ChinaPay.B3B.Service.SystemManagement.Domain
{
    using System.ComponentModel;

    /// <summary>
    /// 系统字典类型
    /// </summary>
    public enum SystemDictionaryType
    {
        #region 描述 11**
        /// <summary>
        /// 头等/公务舱描述
        /// </summary>
        [Description("头等/公务舱描述")]
        FirstOrBusinessBunkDescription = 0x1101,
        /// <summary>
        /// 特价舱描述
        /// </summary>
        [Description("特价舱描述")]
        PromotionBunkDescription = 0x1102,
        /// <summary>
        /// 免票舱描述
        /// </summary>
        [Description("免票舱描述")]
        FreeDescription = 0x1103,
        #endregion

        #region 退改签规定 2***
        #region 特价单程 21**
        /// <summary>
        ///特价单程作废规定
        /// </summary>
        [Description("特价单程作废规定")]
        BargainOneWayInvalidRegulation = 0x2101,
        /// <summary>
        /// 特价单程改签规定
        /// </summary>
        [Description("特价单程更改规定")]
        BargainOneWayChangeRegulation = 0x2102,
        /// <summary>
        /// 特价单程签转规定
        /// </summary>
        [Description("特价单程签转规定")]
        BargainOneWayEndorseRegulation = 0x2103,
        /// <summary>
        /// 特价单程退票规定
        /// </summary>
        [Description("特价单程退票规定")]
        BargainOneWayRefundRegulation = 0x2104,
        #endregion
        #region 特价往返 22**
        /// <summary>
        /// 特价往返作废规定
        /// </summary>
        [Description("特价往返作废规定")]
        BargainRoundTripInvalidRegulation = 0x2201,
        /// <summary>
        /// 特价往返改签规定
        /// </summary>
        [Description("特价往返更改规定")]
        BargainRoundTripChangeRegulation = 0x2202,
        /// <summary>
        /// 特价往返签转规定
        /// </summary>
        [Description("特价往返签转规定")]
        BargainRoundTripEndorseRegulation = 0x2203,
        /// <summary>
        /// 特价往返退票规定
        /// </summary>
        [Description("特价往返退票规定")]
        BargainRoundTripRefundRegulation = 0x2204,
        #endregion
        #region 特价中转联程 23**
        /// <summary>
        /// 特价中转联程作废规定
        /// </summary>
        [Description("特价中转联程作废规定")]
        BargainTransitWayInvalidRegulation = 0x2301,
        /// <summary>
        /// 特价中转联程改签规定
        /// </summary>
        [Description("特价中转联程更改规定")]
        BargainTransitWayChangeRegulation = 0x2302,
        /// <summary>
        /// 特价中转联程签转规定
        /// </summary>
        [Description("特价中转联程签转规定")]
        BargainTransitWayEndorseRegulation = 0x2303,
        /// <summary>
        /// 特价中转联程退票规定
        /// </summary>
        [Description("特价中转联程退票规定")]
        BargainTransitWayRefundRegulation = 0x2304,
        #endregion
        #region 特殊单程控位产品 24** Singleness
        /// <summary>
        /// 特殊单程控位产品作废规定
        /// </summary>
        [Description("特殊单程控位产品作废规定")]
        SpecialSinglenessInvalidRegulation = 0x2401,
        /// <summary>
        /// 特殊单程控位产品改签规定
        /// </summary>
        [Description("特殊单程控位产品更改规定")]
        SpecialSinglenessChangeRegulation = 0x2402,
        /// <summary>
        /// 特殊单程控位产品签转规定
        /// </summary>
        [Description("特殊单程控位产品签转规定")]
        SpecialSinglenessEndorseRegulation = 0x2403,
        /// <summary>
        /// 特殊单程控位产品退票规定
        /// </summary>
        [Description("特殊单程控位产品退票规定")]
        SpecialSinglenessRefundRegulation = 0x2404,
        #endregion
        #region 散冲团产品 25** Disperse
        /// <summary>
        /// 特殊散冲团产品作废规定
        /// </summary>
        [Description("特殊散冲团产品作废规定")]
        SpecialDisperseInvalidRegulation = 0x2501,
        /// <summary>
        /// 特殊散冲团产品改签规定
        /// </summary>
        [Description("特殊散冲团产品更改规定")]
        SpecialDisperseChangeRegulation = 0x2502,
        /// <summary>
        /// 特殊散冲团产品签转规定
        /// </summary>
        [Description("特殊散冲团产品签转规定")]
        SpecialDisperseEndorseRegulation = 0x2503,
        /// <summary>
        /// 特殊散冲团产品退票规定
        /// </summary>
        [Description("特殊散冲团产品退票规定")]
        SpecialDisperseRefundRegulation = 0x2504,
        #endregion
        #region 免票产品 26** CostFree
        /// <summary>
        /// 特殊免票产品作废规定
        /// </summary>
        [Description("特殊免票产品作废规定")]
        SpecialCostFreeInvalidRegulation = 0x2601,
        /// <summary>
        /// 特殊免票产品更改规定
        /// </summary>
        [Description("特殊免票产品更改规定")]
        SpecialCostFreeChangeRegulation = 0x2602,
        /// <summary>
        /// 特殊免票产品签转规定
        /// </summary>
        [Description("特殊免票产品签转规定")]
        SpecialCostFreeEndorseRegulation = 0x2603,
        /// <summary>
        /// 特殊免票产品退票规定
        /// </summary>
        [Description("特殊免票产品退票规定")]
        SpecialCostFreeRefundRegulation = 0x2604,
        #endregion
        #region 集团票产品 27** Bloc
        /// <summary>
        /// 特殊集团票产品作废规定
        /// </summary>
        [Description("特殊集团票产品作废规定")]
        SpecialBlocInvalidRegulation = 0x2701,
        /// <summary>
        /// 特殊集团票产品更改规定
        /// </summary>
        [Description("特殊集团票产品更改规定")]
        SpecialBlocChangeRegulation = 0x2702,
        /// <summary>
        /// 特殊集团票产品签转规定
        /// </summary>
        [Description("特殊集团票产品签转规定")]
        SpecialBlocEndorseRegulation = 0x2703,
        /// <summary>
        /// 特殊集团票产品退票规定
        /// </summary>
        [Description("特殊集团票产品退票规定")]
        SpecialBlocRefundRegulation = 0x2704,
        #endregion
        #region 商旅卡产品 28** Business
        /// <summary>
        /// 特殊商旅卡产品作废规定
        /// </summary>
        [Description("特殊商旅卡产品作废规定")]
        SpecialBusinessInvalidRegulation = 0x2801,
        /// <summary>
        /// 特殊商旅卡产品更改规定
        /// </summary>
        [Description("特殊商旅卡产品更改规定")]
        SpecialBusinessChangeRegulation = 0x2802,
        /// <summary>
        /// 特殊商旅卡产品签转规定
        /// </summary>
        [Description("特殊商旅卡产品签转规定")]
        SpecialBusinessEndorseRegulation = 0x2803,
        /// <summary>
        /// 特殊商旅卡产品退票规定
        /// </summary>
        [Description("特殊商旅卡产品退票规定")]
        SpecialBusinessRefundRegulation = 0x2804,
        #endregion
        #region 其他特殊产品 29 **OtherSpecial
        /// <summary>
        /// 其他特殊产品作废规定
        /// </summary>
        [Description("其他特殊产品作废规定")]
        SpecialOtherSpecialInvalidRegulation = 0x2901,
        /// <summary>
        /// 其他特殊产品改签规定
        /// </summary>
        [Description("其他特殊产品改签规定")]
        SpecialOtherSpecialChangeRegulation = 0x2902,
        /// <summary>
        /// 其他特殊产品签转规定
        /// </summary>
        [Description("其他特殊产品签转规定")]
        SpecialOtherSpecialEndorseRegulation = 0x2903,
        /// <summary>
        /// 其他特殊产品退票规定
        /// </summary>
        [Description("其他特殊产品退票规定")]
        SpecialOtherSpecialRefundRegulation = 0x2904,
        #endregion
        #region 低打高返特殊产品 30 **LowToHigh
        /// <summary>
        /// 低打高返特殊产品作废规定
        /// </summary>
        [Description("低打高返特殊产品作废规定")]
        SpecialLowToHighInvalidRegulation = 0x3001,
        /// <summary>
        /// 低打高返特殊产品改签规定
        /// </summary>
        [Description("低打高返特殊产品改签规定")]
        SpecialLowToHighChangeRegulation = 0x3002,
        /// <summary>
        /// 低打高返特殊产品签转规定
        /// </summary>
        [Description("低打高返特殊产品签转规定")]
        SpecialLowToHighEndorseRegulation = 0x3003,
        /// <summary>
        /// 低打高返特殊产品退票规定
        /// </summary>
        [Description("低打高返特殊产品退票规定")]
        SpecialLowToHighRefundRegulation = 0x3004,
        #endregion
        //[Description("作废规定")]
        //InvalidRegulation,
        //[Description("改签规定")]
        //ChangeRegulation,
        //[Description("签转规定")]
        //EndorseRegulation,
        //[Description("退票规定")]
        //RefundRegulation,
        //特殊
        #endregion

        #region  退票原因 31**
        /// <summary>
        /// 当日作废
        /// </summary>
        [Description("当日作废")]
        IntradayScrap = 0x3101,
        /// <summary>
        /// 升舱全退
        /// </summary>
        [Description("升舱全退")]
        UpgradeAllRefund = 0x3102,
        /// <summary>
        /// 自愿按客规退票
        /// </summary>
        [Description("自愿按客规退票")]
        SelfImposedRefund = 0x3103,
        /// <summary>
        /// 非自愿退票
        /// </summary>
        [Description("非自愿退票")]
        InvoluntaryRefund = 0x3104,
        /// <summary>
        /// 特殊原因退票
        /// </summary>
        [Description("特殊原因退票")]
        SpeicalReasonRefund = 0x3105,

        #endregion

        #region  拒绝出票原因 32**
        /// <summary>
        /// 拒绝出票自身原因
        /// </summary>
        [Description("拒绝出票自身原因")]
        RefuseETDZSelfReason = 0x3201,
        /// <summary>
        /// 拒绝出票平台原因
        /// </summary>
        [Description("拒绝出票平台原因")]
        RefuseETDZPlatformReason = 0x3202,
        /// <summary>
        /// 拒绝出票采购原因
        /// </summary>
        [Description("拒绝出票采购原因")]
        RefuseETDZPurchaseReason = 0x3203,
        /// <summary>
        /// 拒绝出票其他原因
        /// </summary>
        [Description("拒绝出票其他原因")]
        RefuseETDZOtherReason = 0x3204,
        #endregion

        #region  拒绝退废票 33**
        /// <summary>
        /// 拒绝退废票自身原因
        /// </summary>
        [Description("拒绝退废票自身原因")]
        RefuseRefundSelfReason = 0x3301,
        /// <summary>
        /// 拒绝退废票平台原因
        /// </summary>
        [Description("拒绝退废票平台原因")]
        RefuseRefundPlatformReason = 0x3302,
        /// <summary>
        /// 拒绝退废票采购原因
        /// </summary>
        [Description("拒绝退废票采购原因")]
        RefuseRefundPurchaseReason = 0x3303,
        /// <summary>
        /// 拒绝退废票其他原因
        /// </summary>
        [Description("拒绝退废票其他原因")]
        RefuseRefundOtherReason = 0x3304,
        #endregion

        #region  拒绝提供资源 34**
        /// <summary>
        /// 拒绝提供资源自身原因
        /// </summary>
        [Description("拒绝提供座位自身原因")]
        RefuseSupplySelfReason = 0x3401,
        /// <summary>
        /// 拒绝提供资源平台原因
        /// </summary>
        [Description("拒绝提供座位平台原因")]
        RefuseSupplyPlatformReason = 0x3402,
        /// <summary>
        /// 拒绝提供资源采购原因
        /// </summary>
        [Description("拒绝提供座位采购原因")]
        RefuseSupplyPurchaseReason = 0x3403,
        /// <summary>
        /// 拒绝提供资源其他原因
        /// </summary>
        [Description("拒绝提供座位其他原因")]
        RefuseSupplyOtherReason = 0x3404,
        #endregion

        #region 出票条件 41**
        /// <summary>
        /// 单程控位产品出票条件
        /// </summary>
        [Description("单程控位产品出票条件")]
        SpecialSinglenessDrawerCondition = 0x4101,
        /// <summary>
        /// 散冲团出票条件
        /// </summary>
        [Description("散冲团出票条件")]
        SpecialDisperseDrawerCondition = 0x4102,
        /// <summary>
        /// 免票产品出票条件
        /// </summary>
        [Description("免票产品出票条件")]
        SpecialCostFreeDrawerCondition = 0x4103,
        /// <summary>
        /// 其他特殊产品出票条件
        /// </summary>
        [Description("其他特殊产品出票条件")]
        SpecialOtherSpecialDrawerCondition = 0x4104,
        #endregion

        #region 51**
        /// <summary>
        /// 候补机票描述和申请机票描述
        /// </summary>
        [Description("候补机票描述和申请机票描述")]
        StandbyAndApplyFor = 0x5101,
        #endregion

        #region  航空公司改期限制 61**
        [Description("航空公司改期限制")]
        FlightChangeLimit = 0x6101,
        #endregion

        #region  政策协调备选项 71**
        [Description("协调内容")]
        CoordinationContent = 0x7101,
        [Description("协调结果")]
        CoordinationResult = 0x7102,
        [Description("标识订单")]
        IdentificationOrder = 0x7103,

        #endregion
        
        #region 信箱清理收件人地址 81**
        [Description("信箱清理收件人地址")]
        QueueMailList = 0x8101, 
        #endregion

        #region 系统刷新缓存地址 91**
        [Description("系统刷新缓存地址")]
        SystemRefreshCacheAddress =0x9101, 
        #endregion

        #region 系统敏感词库 10**
        [Description("系统敏感词库")]
        SystemSensitiveWords = 0x10101,
        #endregion

        #region 业务服务联系方式 111*
        /// <summary>
        /// 业务服务联系方式
        /// </summary>
        [Description("业务服务联系方式")]
        OEMContract = 0x11101,
        #endregion
    }
}
