using System;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 采购信息
    /// </summary>
    public class PurchaserInfo : OrderRoleInfo {
        internal PurchaserInfo(Guid companyId, string name)
            : base(companyId, name) {
        }

        /// <summary>
        /// 操作员账号
        /// </summary>
        public string OperatorAccount { get; internal set; }
        /// <summary>
        /// 操作员名称
        /// </summary>
        public string OperatorName { get; internal set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime ProducedTime { get; internal set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; internal set; }
    }
}