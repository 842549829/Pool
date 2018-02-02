using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    using B3B.Common.Enums;

    /// <summary>
    /// 订单列表信息
    /// </summary>
    public class OrderListView {
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId { get; set; }
        /// <summary>
        /// 订单产品类型
        /// </summary>
        public ProductType ProductType { get; set; }
        /// <summary>
        /// 出票方产品类型
        /// </summary>
        public ProductType? ProviderProductType { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource Source { get; set; }
        /// <summary>
        /// 订座编码
        /// </summary>
        public PNRPair ReservationPNR { get; set; }
        /// <summary>
        /// 出票编码
        /// </summary>
        public PNRPair ETDZPNR { get; set; }
        /// <summary>
        /// 乘机人姓名集合
        /// </summary>
        public List<string> Passengers { get; set; }
        /// <summary>
        /// 航段信息集合
        /// </summary>
        public List<FlightListView> Flights { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 采购方单位Id
        /// </summary>
        public Guid Purchaser { get; set; }
        /// <summary>
        /// 采购方单位简称
        /// </summary>
        public string PurchaserName { get; set; }
        /// <summary>
        /// 采购方结算信息
        /// </summary>
        public Settlement SettlementForPurchaser { get; set; }
        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid? Provider { get; set; }
        /// <summary>
        /// 出票方单位简称
        /// </summary>
        public string ProviderName { get; set; }
        /// <summary>
        /// 出票方结算信息
        /// </summary>
        public Settlement SettlementForProvider { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 出票方式
        /// </summary>
        public ETDZMode? ETDZMode { get; set; }
        /// <summary>
        /// 资源方单位Id
        /// </summary>
        public Guid? Supplier { get; set; }
        /// <summary>
        /// 资源方单位简称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 供应方结算信息
        /// </summary>
        public Settlement SettlementForSupplier { get; set; }
        /// <summary>
        /// 预订人
        /// </summary>
        public string ProducedAccount { get; set; }
        /// <summary>
        /// 预订人名称
        /// </summary>
        public string ProducedAccountName { get; set; }
        /// <summary>
        /// 预订时间
        /// </summary>
        public DateTime ProducedTime { get; set; }
        /// <summary>
        /// 采购和供应商的关系
        /// </summary>
        public RelationType? PurcharseProviderRelation{get;set;}
        /// <summary>
        /// 采购时在面对需要授权的政策作出的选择
        /// </summary>
        public AuthenticationChoise Choise { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime{get;set;}

        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime? ETDZTime { get; set; }
        /// <summary>
        /// 乘客类型
        /// </summary>
        public PassengerType PassengerType { get; set; }

        /// <summary>
        /// 拒绝出票时间
        /// </summary>
        public DateTime? RefuseETDZTime
        {
            get;
            set;
        }

        /// <summary>
        /// 原始出票方Id
        /// </summary>
        public Guid? OriginalProvider { get; set; }
        /// <summary>
        /// 是否已发送短信
        /// </summary>
        public bool PassengerMsgSended { get; set; }

        /// <summary>
        /// 是否是当天的航班，数据绑定时候用
        /// </summary>
        public bool IsTodaysFlight { get; set; }
        /// <summary>
        /// 是否是订单标识
        /// </summary>
        public bool IsEmergentOrder { get; set; }

        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }

        /// <summary>
        /// 是否是特殊票
        /// </summary>
        public bool IsSpecial
        {
            get {
                return ProductType == ProductType.Special;
            }
        }

        /// <summary>
        /// 提供资源时间
        /// </summary>
        public DateTime? SupplyTime { get; set; }

        /// <summary>
        /// 采购催单时间
        /// </summary>
        public DateTime? RemindTime { get; set; }

        /// <summary>
        /// 采购催单内容
        /// </summary>
        public string RemindContent { get; set; }

        /// <summary>
        /// 是否需要平台催单
        /// </summary>
        public bool IsNeedReminded { get; set; }

        public Guid? OEMID { get; set; }
        /// <summary>
        /// 是否允许平台联系采购
        /// </summary>
        public bool AllowPlatformContractPurchaser { get; set; }
        /// <summary>
        /// 是否需要授权
        /// </summary>
        public bool NeedAUTH { get; set; }
    }
}
