using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class CompanyDetailInfo  :  CompanyInfo{
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime{ get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 最后登录 IP
        /// </summary>
        public string LastLoginIP { get; set; }
        /// <summary>
        /// 最后登录地点
        /// </summary>
        public string LastLoginLocation { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 所属公司组
        /// </summary>
        ///public string Group { get; set; }
        /// <summary>
        /// 联系人 Email
        /// </summary>
        public string ContactEmail { get; set; }
        /// <summary>
        /// 联系人 MSN
        /// </summary>
        public string ContactMSN { get; set; }
        /// <summary>
        /// 联系人 QQ
        /// </summary>
        public string ContactQQ { get; set; }
        /// <summary>
        /// 审核类型（认证中心使用）
        /// </summary>
        //public AuditType AuditType { get; set; }
        /// <summary>
        /// 是否开通外接口
        /// </summary>
        public bool IsOpenExternalInterface { get; set; }
        /// <summary>
        /// 是否是OEM
        /// </summary>
        public bool IsOem { get; set; }

        /// <summary>
        /// 是否开启自定义编号出票
        /// </summary>
        //public bool CustomNO_On{get;set;}
    }

    public class SubordinateCompayInfo : CompanyDetailInfo {
        /// <summary>
        /// 公司间的关系类型
        /// </summary>
        public RelationshipType RelationshipType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SuperiorInfo {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 关系类型
        /// </summary>
        public RelationshipType Type { get; set; }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool Expired { get; set; }
        ///// <summary>
        ///// 挂起的航空公司列表
        ///// </summary>
        public string SuspendedCarirer { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }

    }
}
