using System;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.VIP {
    using B3B.Common.Enums;

    public class VIPManageView {
        public VIPManageView(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 公司帐号
        /// </summary>
        public string UserName {
            get;
            set;
        }
        /// <summary>
        /// 公司类型
        /// </summary>
        public CompanyType Type {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact {
            get;
            set;
        }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName {
            get;
            set;
        }
        /// <summary>
        /// 是否VIP
        /// </summary>
        public bool IsVip {
            get;
            set;
        }
    }
}
