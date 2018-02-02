namespace ChinaPay.B3B.DataTransferObject.Organization {
    public class LoginInfo {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// IP 地址
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 登录地点
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 公司Id
        /// </summary>
        public System.Guid CompanyId { get; set; }
    }
}
