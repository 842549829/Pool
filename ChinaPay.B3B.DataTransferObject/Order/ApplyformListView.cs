using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 申请单列表信息
    /// </summary>
    public class ApplyformListView {
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal ApplyformId { get; set; }
        /// <summary>
        /// 原编码
        /// </summary>
        public PNRPair OriginalPNR { get; set; }
        /// <summary>
        /// 新编码
        /// </summary>
        public PNRPair NewPNR { get; set; }
        /// <summary>
        /// 申请类型
        /// </summary>
        public ApplyformType ApplyformType { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType ProductType { get; set; }
        /// <summary>
        /// 乘机人姓名集合
        /// </summary>
        public List<string> Passengers { get; set; }
        /// <summary>
        /// 航段信息集合
        /// </summary>
        public List<FlightListView> Flights { get; set; }
        /// <summary>
        /// 采购方单位Id
        /// </summary>
        public Guid Purchaser { get; set; }
        /// <summary>
        /// 采购方单位简称
        /// </summary>
        public string PurchaserName { get; set; }
        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid Provider { get; set; }
        /// <summary>
        /// 出票方单位简称
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 资源方单位Id
        /// </summary>
        public Guid? Supplier { get; set; }
        /// <summary>
        /// 资源方单位简称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 是否需要修改价格信息
        /// </summary>
        public bool RequireRevisePrice { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public ApplyformProcessStatus ProcessStatus { get; set; }
        /// <summary>
        /// 申请单详细状态
        /// </summary>
        public byte ApplyDetailStatus { get; set; }
        /// <summary>
        /// 申请人账号
        /// </summary>
        public string ApplierAccount { get; set; }
        /// <summary>
        /// 申请操作员名称
        /// </summary>
        public string ApplierAccountName { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime AppliedTime { get; set; }
        /// <summary>
        /// 退票类型
        /// </summary>
        public RefundType? RefundType { get; set; }
        /// <summary>
        /// 申请单采购所属的OEMId
        /// </summary>
        public Guid OEMID { get; set; }
    }
}