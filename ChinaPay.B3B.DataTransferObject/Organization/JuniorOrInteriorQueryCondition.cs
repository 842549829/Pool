namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    /// <summary>
    /// 查询下级/内部机构列表条件
    /// </summary>
    public class JuniorOrInteriorQueryCondition {
        public System.Guid Id { get; set; }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName {
            get;
            set;
        }
        /// <summary>
        /// 公司账号
        /// </summary>
        public string UserName {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public CompanyStatus?Status {
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
    }
}
