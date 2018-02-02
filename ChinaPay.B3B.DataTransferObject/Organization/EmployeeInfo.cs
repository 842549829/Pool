namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using B3B.Common.Enums;

    /// <summary>
    /// 员工信息
    /// </summary>
    public class EmployeeInfo {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// 办公电话(座机)
        /// </summary>
        public string OfficePhone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Ip限制
        /// </summary>
        public string IpLimitation { get; set; }
    }
}
