
namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;

    /// <summary>
    /// 修改密码
    /// </summary>
    public sealed class ChangePasswordInfo {
        /// <summary>
        /// 员工编号
        /// </summary>
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// 员工账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
