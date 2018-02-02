namespace ChinaPay.B3B.Service.SystemManagement.Domain {
    using System.ComponentModel;

    public enum SystemParamType
    {
        [Description("默认密码")]
        DefaultPassword = 0,
        [Description("客服电话")]
        CustomerServiceTel = 1,
        [Description("编码中的联系电话")]
        ContactInPNR = 2,
        [Description("普通票支付时限")]
        GeneralPayableLimit = 3,
        [Description("取消编码时限")]
        CancelPnrLimit = 4,
        [Description("不可预订时间段")]
        UnOrderableTimeZones = 5,
        [Description("不可申请退改签时间段")]
        NotApplyTimeZones = 6,
        [Description("默认特殊票交易费率")]
        DefaultTradeRateForSpecial = 7,
        [Description("默认下级交易费率")]
        DefaultTradeRateForJunior = 8,
        [Description("默认同行交易费率")]
        DefaultTradeRateForBrother = 9,
        [Description("航班时间与当前时间的最低间隔")]
        FlightDisableTime = 10,
        [Description("航班信息有效时间")]
        FlightValidityMinutes = 11,
        [Description("平台结算账号")]
        PlatformSettleAccount = 12,
        [Description("平台收款账号")]
        PlatformIncomeAccount = 13,
        [Description("平台付款账号")]
        PlatformPayoutAccount = 14,
        [Description("平台改期费收款账号")]
        PlatformIncodeAccountForPostpone = 15,
        [Description("默认使用期限")]
        DefaultUseLimit = 16,
        [Description("默认锁定政策累积退废票数")]
        DefaultLockPolicyLimit = 17,
        [Description("默认自愿退票时限")]
        DefaultVoluntaryRefundLimit = 18,
        [Description("默认全退时限")]
        DefaultFullRefundLimit = 19,
        [Description("产品处理统计天数")]
        ProductStatisticDays = 20,
        [Description("出票方留点")]
        ProviderDeductForSpecial = 21,
        [Description("特殊票平台扣点")]
        PlatformDeductForSpecial = 22,
        [Description("后返比例")]
        SpreadingRate = 23,
        [Description("政策备注的追加说明")]
        PolicyRemark = 24,
        [Description("预订PAT时段")]
        PATTimeZones = 25,
        [Description("开户积分设置")]
        OpenAccountIntegral = 26,
        [Description("特殊票支付时限")]
        SpecialPayableLimit = 27,
        [Description("改期支付时限")]
        PostponePayableLimit = 28,
        [Description("出票方废票费")]
        ProviderRate = 29,
        [Description("产品方废票费")]
        ResourcerRate = 30,
        [Description("短信收款账号")]
        SMSIncomeAccount = 31,
        [Description("是否对外部政策做全局贴点")]
        NeedExternalGlobalSubsidy = 32,
        [Description("系统服务邮件地址")]
        SystemServiceMailAddress = 33,
        [Description("系统服务邮件密码")]
        SystemServiceMailPassword = 34,
        [Description("系统服务接收邮件地址")]
        SystemReceptionMailAddress = 35,
        [Description("采购催单时间限制")]
        PurchaseReminderTime = 36,
        [Description("循环催单周期")]
        ReminderCycle = 37,
        [Description("平台名称")]
        DefaultPlatformName = 38,
        [Description("分润方手续费率")]
        TradeRateForRoyalty = 39,
        [Description("平台默认登录地址")]  
        B3BDefalutLogonUrl = 40,
        [Description("退票验证开关")]
        ValidateRefundCondition = 41,
        //[Description("支付域名")]
        //PayHost = 42 ,
        [Description("FD开启时段")]
        FDTimeZones = 43,
        [Description("交易手续费率")]
        TradeRate = 44,
        [Description("改期交易手续费")]
        PostponeTradeFee = 45,
    }
}