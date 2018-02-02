namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using B3B.Common.Enums;

    /// <summary>
    /// 推广信息视图类
    /// </summary>
    public class SpreadingView {
        /// <summary>
        /// 推广方 Id
        /// </summary>
        public Guid Initiator { get; set; }
        /// <summary>
        /// 被推广方 Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 被推广方的公司类型
        /// </summary>
        public CompanyType Type { get; set; }
        /// <summary>
        /// 被推广方的简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 被推广方所在城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 推广员工
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 被推广方联系人姓名
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 被推广方联系电话
        /// </summary>
        public string ContactCellphone { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string OfficePhone { get; set; }
        /// <summary>
        /// 管理员账号
        /// </summary>
        public string Admin { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType AccountType { get; set; }
        /// <summary>
        /// 推广时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// 被推广方当前是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
