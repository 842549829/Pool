using System;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 员工详细信息
    /// </summary>
    public class EmployeeDetailInfo : EmployeeInfo {
        /// <summary>
        /// 所属公司
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 是否管理员账号
        /// </summary>
        public bool IsAdministrator { get; set; }
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
    }
}
