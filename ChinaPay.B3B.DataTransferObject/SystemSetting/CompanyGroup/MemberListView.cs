using System;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup {
    using B3B.Common.Enums;

    /// <summary>
    /// 公司组成员列表
    /// </summary>
    public class MemberListView {
        /// <summary>
        /// 单位Id
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 公司账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 公司状态
        /// </summary>
        public CompanyStatus Status { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 所在组名
        /// </summary>
        public string GroupName { get; set; }
    }
}