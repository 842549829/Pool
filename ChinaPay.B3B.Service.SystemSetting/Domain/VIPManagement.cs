using System;

namespace ChinaPay.B3B.Service.SystemSetting.Domain {
    public class VIPManagement {
        public VIPManagement(Guid company) {
            this.Id = Guid.NewGuid();
            this.Company = company;
        }
        public VIPManagement(Guid id, Guid company) {
            this.Id = id;
            this.Company = company;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid Company {
            get;
            private set;
        }
        /// <summary>
        /// 是否VIP
        /// </summary>
        public bool IsVip {
            set;
            get;
        }
    }
}
