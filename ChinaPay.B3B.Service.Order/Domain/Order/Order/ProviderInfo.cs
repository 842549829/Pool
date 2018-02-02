using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 出票方信息
    /// </summary>
    public class ProviderInfo : OrderRoleInfo {
        internal ProviderInfo(Guid companyId, string name)
            : base(companyId, name) {
        }

        /// <summary>
        /// 与买家关系
        /// </summary>
        public RelationType PurchaserRelationType {
            get;
            internal set;
        }
        /// <summary>
        /// 产品信息
        /// </summary>
        public ProductInfo Product {
            get;
            internal set;
        }
        /// <summary>
        /// 出票操作员账号
        /// </summary>
        public string OperatorAccount { get; internal set; }
        /// <summary>
        /// 出票操作员名称
        /// </summary>
        public string OperatorName { get; internal set; }
    }
}