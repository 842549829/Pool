using System;
using System.Collections.Generic;
namespace ChinaPay.B3B.DataTransferObject.Organization {
    using Data.DataMapping;

    /// <summary>
    /// 公司组详细信息
    /// </summary>
    public class CompanyGroupDetailInfo {
        /// <summary>
        /// 公司组 Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 公司组所属的公司
        /// </summary>
        //public CompanyDetailInfo Owner { get; set; }
        public Guid Company { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        ///// <summary>
        ///// 允许采购同行政策
        ///// </summary>
        //public bool AllowExternalPurchase { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 成员
        /// </summary>
        //public IEnumerable<CompanyDetailInfo> Members { get; set; }
        /// <summary>
        /// 采购限制
        /// </summary>
        public IEnumerable<CompanyGroupLimitation> Limitations { get; set; }
    }
    public class CompanyGroupInfo {
        /// <summary>
        /// 公司组 Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 公司组所属的公司
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// 允许采购同行政策
        /// </summary>
        public bool AllowExternalPurchase { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }
    }
    public class CompanyGroupQueryParameter  {
        /// <summary>
        /// 公司组所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 公司组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建起始时间
        /// </summary>
        public DateTime? CreateTimeStart { get; set; }
        /// <summary>
        /// 创建结束时间
        /// </summary>
        public DateTime? CreateTimeEnd { get; set; }

        /// <summary>
        /// 每页条目数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }
    }
}
