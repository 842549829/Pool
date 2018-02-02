using System;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 资源方信息
    /// </summary>
    public class SupplierInfo : OrderRoleInfo {
        internal SupplierInfo(Guid companyId, string name)
            : base(companyId, name) {
        }

        /// <summary>
        /// 产品
        /// </summary>
        public SpeicalProductInfo Product {
            get;
            internal set;
        }
    }
}
