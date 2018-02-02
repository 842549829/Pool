using System;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class SpreadingQueryParameter {
        public Guid Initiator { get; set; }

        /// <summary>
        /// 单位简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Contact { get; set; }
       /// <summary>
       /// 单位类型
       /// </summary>
        public CompanyType? Type { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType? AccountType { get; set; }
        /// <summary>
        /// 推广员工
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 单位是否启用
        /// </summary>
        public bool? Enabled { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTimeStart { get; set; }
        public DateTime? RegisterTimeEnd { get; set; }
    }
}
