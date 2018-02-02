namespace ChinaPay.B3B.DataTransferObject.Policy {
    public class ExternalPolicyView {
        /// <summary>
        /// 平台代码
        /// </summary>
        public B3B.Common.Enums.PlatformType Platform { get; set; }
        /// <summary>
        /// 政策发布方
        /// </summary>
        public System.Guid Provider { get; set; }
        /// <summary>
        /// 政策编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public B3B.Common.Enums.TicketType TicketType { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public B3B.Common.Enums.PolicyType? PolicyType { get; set; }
        /// <summary>
        /// 是否需要换编码
        /// </summary>
        public bool RequireChangePNR { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 出票速度
        /// 以秒计算
        /// </summary>
        public int ETDZSpeed { get; set; }
        /// <summary>
        /// 工作开始时间
        /// </summary>
        public Izual.Time WorkStart { get; set; }
        /// <summary>
        /// 工作结束时间
        /// </summary>
        public Izual.Time WorkEnd { get; set; }
        /// <summary>
        /// 废票开始时间
        /// </summary>
        public Izual.Time ScrapStart { get; set; }
        /// <summary>
        /// 废票结束时间
        /// </summary>
        public Izual.Time ScrapEnd { get; set; }

        #region  代码段描述
        /// <summary>
        /// 工作日工作开始时间
        /// </summary>
        public Izual.Time WorkTimeStart { get; set; }
        /// <summary>
        /// 工作日工作结束时间
        /// </summary>
        public Izual.Time WorkTimeEnd { get; set; }
        /// <summary>
        /// 休息日工作开始时间
        /// </summary>
        public Izual.Time RestWorkTimeStart { get; set; }
        /// <summary>
        /// 休息日工作结束时间
        /// </summary>
        public Izual.Time RestWorkTimeEnd { get; set; }
        /// <summary>
        /// 工作日废票开始时间
        /// </summary>
        public Izual.Time WorkRefundTimeStart { get; set; }
        /// <summary>
        /// 工作日废票结束时间
        /// </summary>
        public Izual.Time WorkRefundTimeEnd { get; set; }
        /// <summary>
        /// 休息日废票开始时间
        /// </summary>
        public Izual.Time RestRefundTimeStart { get; set; }
        /// <summary>
        /// 休息日废票结束时间
        /// </summary>
        public Izual.Time RestRefundTimeEnd { get; set; }

        #endregion

        /// <summary>
        /// 原始返点
        /// </summary>
        public decimal OriginalRebate { get; set; }
        /// <summary>
        /// 卖出返点
        /// </summary>
        public decimal Rebate { get; set; }
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal ParValue { get; set; }
        /// <summary>
        /// 需要授权的Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 是否需要授权
        /// </summary>
        public bool RequireAuth { get; set; }
        /// <summary>
        /// 原始内容
        /// </summary>
        public string OriginalContent { get; set; }
    }
    /// <summary>
    /// 日志
    /// </summary>
    public class ExternalPolicyLog : ExternalPolicyView {
        /// <summary>
        /// 订单编号
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public System.DateTime RecordDate { get; set; }

    }
    public class YeexingPolicyView : ExternalPolicyView {
        public string IBEPrice { get; set; }
        public string Disc { get; set; }
        public string extReward { get; set; }
        /// <summary>
        /// 支付的支付方式
        /// </summary>
        public System.Collections.Generic.IEnumerable<Common.PayInterface> PayInterfaces { get; set; }
    }
    public class _517NaPolicyView : ExternalPolicyView {
        /// <summary>
        /// 子政策编号
        /// </summary>
        public string SubId { get; set; }
    }
    public class ExternalPolicyFilter {
        /// <summary>
        /// b3b最高返点
        /// </summary>
        public decimal B3BMaxRebate { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public B3B.Common.Enums.TicketType? TicketType { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public Command.PNR.ItineraryType VoyageType { get; set; }
        /// <summary>
        /// 内容中的编码是否是大编码
        /// </summary>
        public bool UseBPNR
        {
            get;
            set;
        }
    }
}