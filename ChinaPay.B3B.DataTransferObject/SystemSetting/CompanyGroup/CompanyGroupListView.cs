using System;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup {
    /// <summary>
    /// 公司组列表
    /// </summary>
    public class CompanyGroupListView {
        public Guid Id { get; set; }
        /// <summary>
        /// 组名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 成员数
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// 是否只能采购自己的政策
        /// </summary>
        public bool PurchaseMyPolicyOnly { get; set; }
        /// <summary>
        /// 创建账号
        /// </summary>
        public string RegisterAccount { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime RegisterTime{ get;set; }
        /// <summary>
        /// 修改帐号
        /// </summary>
        public string UpdateAccount { get; set; }
        /// <summary>
        /// 上次修改时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}