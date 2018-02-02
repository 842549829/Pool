using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    /// <summary>
    /// 申请单查询条件
    /// </summary>
    public class ApplyformQueryCondition
    {
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal? ApplyformId { get; set; }

        /// <summary>
        /// 采购方单位Id
        /// </summary>
        public Guid? Purchaser { get; set; }

        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid? Provider { get; set; }

        /// <summary>
        /// 资源方单位Id
        /// </summary>
        public Guid? Supplier { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public ApplyformType? ApplyformType { get; set; }

        /// <summary>
        /// 申请单处理状态
        /// </summary>
        public ApplyformProcessStatus? ProcessStatus { get; set; }

        /// <summary>
        /// 申请单详细状态
        /// </summary>
        public byte? ApplyDetailStatus { get; set; }

        /// <summary>
        /// 退/废票状态内容
        /// </summary>
        public string RefundStatusText { get; set; }

        /// <summary>
        /// 改期状态内容
        /// </summary>
        public string PostponeStatusText { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// 申请的操作员账号
        /// </summary>
        public string Applier { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        public Range<DateTime> AppliedDateRange { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string PNR { get; set; }

        /// <summary>
        /// 乘机人姓名
        /// </summary>
        public string Passenger { get; set; }

        /// <summary>
        /// 退废票状态
        /// </summary>
        public RefundApplyformStatus? RefundStatuses { get; set; }

        /// <summary>
        /// 改期状态
        /// </summary>
        public PostponeApplyformStatus? PostponeStatuses { get; set; }

        /// <summary>
        /// 是否需要修改价格信息
        /// </summary>
        public bool? RequireRevisePrice { get; set; }

        /// <summary>
        /// 状态与需要修改价格信息条件 是否是并且关系
        /// </summary>
        public bool IsStatusAndRequireRevisePrice { get; set; }

        /// <summary>
        /// 退票类型
        /// </summary>
        public RefundType? RefundType { get; set; }

        /// <summary>
        /// 采购上级的OEMID
        /// </summary>
        public Guid? OEMID { get; set; }

        /// <summary>
        /// 是否包含差错退款申请
        /// </summary>
        public bool IncludeBlanceApplyform { get; set; }

        /// <summary>
        /// 差额退款处理状态
        /// </summary>
        public BalanceRefundProcessStatus? BalanceRefundProcessStatus { get; set; }
    }
}