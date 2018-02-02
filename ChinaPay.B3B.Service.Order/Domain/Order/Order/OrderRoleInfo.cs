using System;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 订单角色信息
    /// </summary>
    public abstract class OrderRoleInfo {
        LazyLoader<DataTransferObject.Organization.CompanyInfo> _companyLoader;

        protected OrderRoleInfo(Guid companyId, string name) {
            this.CompanyId = companyId;
            this.Name = name;
            _companyLoader = new LazyLoader<DataTransferObject.Organization.CompanyInfo>(() => {
                return ChinaPay.B3B.Service.Organization.CompanyService.GetCompanyDetail(this.CompanyId);
            });
        }

        /// <summary>
        /// 单位Id
        /// </summary>
        public Guid CompanyId {
            get;
            private set;
        }
        /// <summary>
        /// 单位简称
        /// </summary>
        public string Name {
            get;
            private set;
        }
        /// <summary>
        /// 单位信息
        /// </summary>
        public DataTransferObject.Organization.CompanyInfo Company {
            get {
                return _companyLoader.QueryData();
            }
        }
        /// <summary>
        /// 返点信息
        /// </summary>
        public decimal Rebate {
            get;
            internal set;
        }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission {
            get;
            internal set;
        }
        /// <summary>
        /// 结算价格
        /// </summary>
        public decimal Amount {
            get;
            internal set;
        }
    }
}