using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 下级单位查询参数
    /// </summary>
    public class SubordinateQueryParameter   {
        /// <summary>
        /// 父级公司 Id
        /// </summary>
        public Guid Superior { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 下级单位简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 下级单位账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType? AccountType { get; set; }
        /// <summary>
        /// 下级单位联系人姓名
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 下级单位状态
        /// </summary>
        public bool? Enabled { get; set; }
        /// <summary>
        /// 关系类型
        /// </summary>
        public RelationshipType? RelationshipType { get; set; }
        /// <summary>
        /// 注册开始时间
        /// </summary>
        public DateTime? RegisterTimeStart { get; set; }
        /// <summary>
        /// 注册结束时间
        /// </summary>
        public DateTime? RegisterTimeEnd { get; set; }
    }
}
