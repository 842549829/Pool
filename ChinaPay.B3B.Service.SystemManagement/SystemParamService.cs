using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemManagement {
    /// <summary>
    /// 系统参数服务类
    /// </summary>
    public static class SystemParamService {
        /// <summary>
        /// 默认平台名称
        /// </summary>
        public static string DefaultPlatformName {
            get { return QueryString(SystemParamType.DefaultPlatformName); }
        }
        /// <summary>
        /// 默认平台登录地址
        /// </summary>
        public static string B3BDefalutLogonUrl
        {
            get { return QueryString(SystemParamType.B3BDefalutLogonUrl); }
        }
        ///// <summary>
        ///// 支付域名
        ///// </summary>
        //public static string PayHost
        //{
        //    get { return QueryString(SystemParamType.PayHost); }
        //}
        /// <summary>
        /// 出票方废票费
        /// </summary>
        public static decimal ProviderRate {
            get { 
                return QueryDecimal(SystemParamType.ProviderRate); 
            }
        }
        /// <summary>
        /// 产品方废票费
        /// </summary>
        public static decimal ResourcerRate { 
            get {
                return QueryDecimal(SystemParamType.ResourcerRate);
            }
        }
        /// <summary>
        /// 开户积分
        /// </summary>
        public static int OpenAccountIntegral {
            get {
                return QueryInt(SystemParamType.OpenAccountIntegral);
            }
        }
        /// <summary>
        /// 默认密码
        /// </summary>
        public static string DefaultPassword {
            get {
                return QueryString(SystemParamType.DefaultPassword);
            }
        }
        /// <summary>
        /// 客服电话
        /// </summary>
        public static string CustomerServiceTelphone {
            get {
                return QueryString(SystemParamType.CustomerServiceTel);
            }
        }
        /// <summary>
        /// 生成的编码中的联系电话
        /// </summary>
        public static string ContactInPNR {
            get {
                return QueryString(SystemParamType.ContactInPNR);
            }
        }
        /// <summary>
        /// 普通票支付时限
        /// </summary>
        public static int GeneralPayableLimit {
            get {
                return QueryInt(SystemParamType.GeneralPayableLimit);
            }
        }
        /// <summary>
        /// 特殊票支付时限
        /// </summary>
        public static int SpecialPayableLimit {
            get {
                return QueryInt(SystemParamType.SpecialPayableLimit);
            }
        }
        /// <summary>
        /// 改期支付时限
        /// </summary>
        public static int PostponePayableLimit {
            get {
                return QueryInt(SystemParamType.PostponePayableLimit);
            }
        }
        /// <summary>
        /// 取消编码时限
        /// 对于平台上生成的编码，超过多久未支付，直接取消编码
        /// </summary>
        public static int CancelPnrLimit {
            get {
                return QueryInt(SystemParamType.CancelPnrLimit);
            }
        }
        /// <summary>
        /// 不可预订时间段
        /// </summary>
        public static IEnumerable<Range<DateTime>> UnOrderableTimeZones {
            get {
                return QueryDateTimeRanges(SystemParamType.UnOrderableTimeZones);
            }
        }
        /// <summary>
        /// 不可申请退改签时间段
        /// </summary>
        public static IEnumerable<Range<DateTime>> NotApplyTimeZones {
            get {
                return QueryDateTimeRanges(SystemParamType.NotApplyTimeZones);
            }
        }
        /// <summary>
        /// 默认特殊票交易费率
        /// </summary>
        public static decimal DefaultTradeRateForSpecial {
            get {
                return QueryDecimal(SystemParamType.DefaultTradeRateForSpecial);
            }
        }
        /// <summary>
        /// 默认下级交易费率
        /// </summary>
        public static decimal DefaultTradeRateForJunior {
            get {
                return QueryDecimal(SystemParamType.DefaultTradeRateForJunior);
            }
        }
        /// <summary>
        /// 默认同行交易费率
        /// </summary>
        public static decimal DefaultTradeRateForBrother {
            get {
                return QueryDecimal(SystemParamType.DefaultTradeRateForBrother);
            }
        }
        /// <summary>
        /// 分润方手续费率
        /// </summary>
        public static decimal TradeRateForRoyalty {
            get { return QueryDecimal(SystemParamType.TradeRateForRoyalty); }
        }
        /// <summary>
        /// 航班时间与当前时间的最低间隔
        /// 用于航班缓存
        /// 以分钟计算
        /// </summary>
        public static int FlightDisableTime {
            get {
                return QueryInt(SystemParamType.FlightDisableTime);
            }
        }
        /// <summary>
        /// 航班结果有效时间
        /// 以分钟计算
        /// </summary>
        public static int FlightValidityMinutes {
            get { return QueryInt(SystemParamType.FlightValidityMinutes); }
        }
        /// <summary>
        /// 平台结算账号
        /// </summary>
        public static string PlatformSettleAccount {
            get { return QueryString(SystemParamType.PlatformSettleAccount); }
        }
        /// <summary>
        /// 平台收款账号
        /// </summary>
        public static string PlatformIncomeAccount {
            get { return QueryString(SystemParamType.PlatformIncomeAccount); }
        }
        /// <summary>
        /// 平台付款账号
        /// </summary>
        public static string PlatformPayoutAccount {
            get { return QueryString(SystemParamType.PlatformPayoutAccount); }
        }
        /// <summary>
        /// 平台改期费收款账号
        /// </summary>
        public static string PlatformIncodeAccountForPostpone {
            get { return QueryString(SystemParamType.PlatformIncodeAccountForPostpone); }
        }
        /// <summary>
        /// 默认使用期限
        /// 以年计算
        /// </summary>
        public static int DefaultUseLimit {
            get { return QueryInt(SystemParamType.DefaultUseLimit); }
        }
        /// <summary>
        /// 默认锁定政策累积退废票数
        /// </summary>
        public static int DefaultLockPolicyLimit {
            get { return QueryInt(SystemParamType.DefaultLockPolicyLimit); }
        }
        /// <summary>
        /// 默认自愿退票时限
        /// 以小时计算
        /// </summary>
        public static int DefaultVoluntaryRefundLimit {
            get { return QueryInt(SystemParamType.DefaultVoluntaryRefundLimit); }
        }
        /// <summary>
        /// 默认全退时限
        /// 以小时计算
        /// </summary>
        public static int DefaultFullRefundLimit {
            get { return QueryInt(SystemParamType.DefaultFullRefundLimit); }
        }
        /// <summary>
        /// 产品处理统计天数
        /// 非特殊产品的出票
        /// 特殊产品的处理
        /// </summary>
        public static int ProductStatisticDays {
            get { return QueryInt(SystemParamType.ProductStatisticDays); }
        }
        /// <summary>
        /// 特殊票出票方留点
        /// </summary>
        public static decimal ProviderDeductForSpecial {
            get { return QueryDecimal(SystemParamType.ProviderDeductForSpecial); }
        }
        /// <summary>
        /// 特殊票平台扣点
        /// </summary>
        public static decimal PlatformDeductForSpecial {
            get { return QueryDecimal(SystemParamType.PlatformDeductForSpecial); }
        }

        /// <summary>
        /// 政策备注追加说明
        /// </summary>
        public static string PolicyRemark {
            get {
                return QueryParam(SystemParamType.PolicyRemark).Value;
            }
        }
        /// <summary>
        /// 预订PAT时间段
        /// </summary>
        public static IEnumerable<Range<DateTime>> PATTimeZones {
            get {
                return QueryDateTimeRanges(SystemParamType.PATTimeZones);
            }
        }
        /// <summary>
        /// 预订PAT时间段
        /// </summary>
        public static IEnumerable<Range<DateTime>> FDTimeZones
        {
            get
            {
                return QueryDateTimeRanges(SystemParamType.FDTimeZones);
            }
        }


        /// <summary>
        /// 是否需要对外部政策进行全局贴点；
        /// </summary>
        public static bool NeedExternalGlobalSubsidy
        {
            get
            {
                return QueryInt(SystemParamType.NeedExternalGlobalSubsidy) == 1;
            }
        }

        /// <summary>
        /// 是否需要对外部政策进行全局贴点；
        /// </summary>
        public static bool ValidateRefundCondition
        {
            get
            {
                return QueryInt(SystemParamType.ValidateRefundCondition) == 1;
            }
        }


        /// <summary>
        /// 采购催单时间限制
        /// </summary>
        public static int PurchaseReminderTime
        {
            get
            {
                return QueryInt(SystemParamType.PurchaseReminderTime);
            }
        }

        public static int ReminderCycle
        {
            get
            {
                return QueryInt(SystemParamType.ReminderCycle);
            }
        }
        /// <summary>
        /// 支付公司交易手续费率
        /// </summary>
        public static decimal TradeRate {
            get { return QueryDecimal(SystemParamType.TradeRate); }
        }
        /// <summary>
        /// 改期交易手续费
        /// </summary>
        public static decimal PostponeTradeFee
        {
            get { return QueryDecimal(SystemParamType.PostponeTradeFee); }
        }
        
        public static SystemParam QueryParam(SystemParamType paramType) {
            var data = SystemParams.Instance[paramType];
            if(data == null) {
                throw new CustomException("无该参数");
            }
            return data;
        }
        public static string QueryString(SystemParamType paramType) {
            return QueryParam(paramType).Value;
        }
        public static int QueryInt(SystemParamType paramType) {
            var data = QueryParam(paramType);
            int result = 0;
            if(data != null && int.TryParse(data.Value, out result)) {
                return result;
            }
            throw new CustomException("参数格式错误");
        }
        public static decimal QueryDecimal(SystemParamType paramType) {
            var data = QueryParam(paramType);
            decimal result = 0;
            if(data != null && decimal.TryParse(data.Value, out result)) {
                return result;
            }
            throw new CustomException("参数格式错误");
        }
        public static float QueryFloat(SystemParamType paramType) {
            var data = QueryParam(paramType);
            float result = 0;
            if(data != null && float.TryParse(data.Value, out result)) {
                return result;
            }
            throw new CustomException("参数格式错误");
        }
        public static IEnumerable<Range<DateTime>> QueryDateTimeRanges(SystemParamType paramType) {
            var data = QueryParam(paramType);
            var result = new List<Range<DateTime>>();
            if(data != null) {
                if(!string.IsNullOrWhiteSpace(data.Value)) {
                    var rangeArray = data.Value.Trim().Split(';');
                    foreach(var range in rangeArray) {
                        if(!string.IsNullOrWhiteSpace(range)) {
                            var timeArray = range.Trim().Split('|');
                            if(timeArray.Length == 2) {
                                DateTime beginTime, endTime;
                                if(DateTime.TryParse(timeArray[0], out beginTime) && DateTime.TryParse(timeArray[1], out endTime)) {
                                    result.Add(new Range<DateTime>(beginTime, endTime));
                                }
                            }
                        }
                    }
                }
                return result;
            }
            throw new CustomException("参数格式错误");
        }
        public static IEnumerable<SystemParam> Query() {
            return SystemParams.Instance.Query();
        }
        public static void Update(SystemParamType paramType, string value, string account) {
            string originalValue = QueryString(paramType);
            // 修改数据
            SystemParams.Instance.Update(paramType, value);
            // 记录日志
            var content = "将参数 [" + paramType.GetDescription() + "] 的值由 " + (originalValue ?? string.Empty) + " 修改为 " + (value ?? string.Empty);
            var log = new Service.Log.Domain.OperationLog(OperationModule.系统参数, OperationType.Update, account, OperatorRole.Platform, ((int)paramType).ToString(), content, DateTime.Now);
            Service.LogService.SaveOperationLog(log);
        }
    }
}