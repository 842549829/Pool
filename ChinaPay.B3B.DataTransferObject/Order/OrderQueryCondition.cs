using System;
using ChinaPay.Core;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 订单查询条件
    /// </summary>
    public class OrderQueryCondition {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal? OrderId { get; set; }
        /// <summary>
        /// 采购方单位Id
        /// </summary>
        public Guid? Purchaser { get; set; }
        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// 供应方单位Id
        /// </summary>
        public Guid? Supplier { get; set; }
        /// <summary>
        /// 订单产品类型
        /// </summary>
        public ProductType? ProductType { get; set; }
        /// <summary>
        /// 出票方产品类型
        /// </summary>
        public ProductType? ProviderProductType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus? Status { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public byte? Source { get; set; }
        /// <summary>
        /// 生成订单的操作员账号
        /// </summary>
        public string ProducedAccount { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 订单生成日期
        /// </summary>
        public Range<DateTime> ProducedDateRange { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string PNR { get; set; }
        /// <summary>
        /// 乘机人姓名
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// 乘运人
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// 操作帐号授权的OfficeNo
        /// </summary>
        public string CustomNo{get;set;}
        /// <summary>
        /// 订单状态文本
        /// </summary>
        public string OrderStatusText { get; set; }
        /// <summary>
        /// 查询角色
        /// </summary>
        public OrderRole? OrderRole { get; set; }
        /// <summary>
        /// 销售关系的值(同行)
        /// </summary>
        public bool RelationBrother { get; set; }
        /// <summary>
        /// 销售关系的值(下级)
        /// </summary>
        public bool RelationJunion { get; set; }
        /// <summary>
        /// 销售关系的值(内部机构)
        /// </summary>
        public bool RelationInterior { get; set; }
        /// <summary>
        /// 销售关系
        /// </summary>
        public RelationType? RelationType { get; set; }
        /// <summary>
        ///所属OEM的ID
        /// </summary>
        public Guid? OEMID { get; set; }
    }
}