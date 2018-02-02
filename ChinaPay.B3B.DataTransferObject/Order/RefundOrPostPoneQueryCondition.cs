using System;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    /// <summary>
    /// 申请单查询条件
    /// </summary>
    public class RefundOrPostPoneQueryCondition
    {
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal? ApplyformId { get; set; }
        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType? ProductType { get; set; }
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
    }
}