namespace ChinaPay.B3B.DataTransferObject.Organization {
    public abstract class CompanyView : UnitView {
        /// <summary>
        /// 负责人
        /// </summary>
        public string Principal {
            get;
            set;
        }
        /// <summary>
        /// 负责人电话
        /// </summary>
        public string PrincipalPhone {
            get;
            set;
        }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string UrgentContact {
            get;
            set;
        }
        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string UrgentContactPhone {
            get;
            set;
        }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string Phone {
            get;
            set;
        }
    }
}
