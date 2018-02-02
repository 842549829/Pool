using System;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 员工查询参数
    /// </summary>
    public class EmployeeQueryParameter {
        /// <summary>
        /// 员工所属公司 Id
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; }
        /// <summary>
        /// Ip限制
        /// </summary>
        public string IpLimitation { get; set; }
    }
}
