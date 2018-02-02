using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Organization {
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    internal static class Regexes {
        /// <summary>
        /// 空字符串或空白字符串
        /// </summary>
        public static readonly Regex EmptyOrWhite = new Regex(@"^\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 用户名
        /// </summary>
        public static readonly Regex UserName = new Regex(@"(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 密码
        /// </summary>
        public static readonly Regex Password = new Regex(@"^[\x00-\xff]{6,20}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 公司名称
        /// </summary>
        public static readonly Regex CompanyName = new Regex(@"^[\u4e00-\u9fa5\w][\u4e00-\u9fa5\w\s\.,]*$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        /// <summary>
        /// 姓名
        /// </summary>
        public static readonly Regex Name = new Regex(@"^([\u4e00-\u9fa5]{2,10}|[a-z\s]+)$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        /// <summary>
        /// 固定电话
        /// </summary>
        public static readonly Regex Phone = new Regex(@"((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        /// <summary>
        /// 固定电话（一个或多个，以 / 分隔）
        /// </summary>
        public static readonly Regex Phones = new Regex(@"(?<phone>((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8})(/\k<phone>)*", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
        /// <summary>
        /// Office 号
        /// </summary>
        public static readonly Regex OfficeNumber = new Regex(@"^[a-z]{3}\d{3}$");
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public static readonly Regex OrginationCode = new Regex(@"^\d{8}-[\dxX]{1}$");
        /// <summary>
        /// 手机号码
        /// </summary>
        public static readonly Regex MobilePhone = new Regex(@"^1[3458][0-9]{9}$");
        public static readonly Regex Email = new Regex("^\\w+@\\w+(\\.\\w{2,4}){1,2}$");
        public static readonly Regex QQ = new Regex(@"^[0-9]{5,20}$");
        public static readonly Regex ZipCode = new Regex(@"^[1-9][0-9]{5}$");
    }
}
