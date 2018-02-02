using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 员工列表信息
    /// </summary>
    public class EmployeeListView {
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
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Cellphone { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 所属角色名字
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdministrator { get; set; }
        /// <summary>
        /// Ip限制
        /// </summary>
        public string IpLimitation { get; set; }
    }
}