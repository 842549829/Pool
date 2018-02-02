using System;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using System.Net;

namespace ChinaPay.B3B.TransactionWeb {
    internal class LogonUtility {
        const int ValidateCodeLength = 5;
        const string ValidateCodeKey = "LogonValidateCode";

        private static string ValidateCode {
            get {
                if(HttpContext.Current.Session == null || HttpContext.Current.Session[ValidateCodeKey] == null)
                    return null;
                return HttpContext.Current.Session[ValidateCodeKey] as string;
            }
            set {
                HttpContext.Current.Session[ValidateCodeKey] = value;
            }
        }
        /// <summary>
        /// 产生登录验证码
        /// </summary>
        public static string GenerateValidateCode() {
            var result = Utility.VerifyCodeUtility.CreateVerifyCode(ValidateCodeLength);
            ValidateCode = result;
            return result;
        }
        private static bool validateValidateCode(string code) {
            return string.Equals(ValidateCode, code, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 登录
        /// </summary>
        public static bool Logon(string userName, string password, string validateCode, bool saveAccount, out string message) {
            if(validateValidateCode(validateCode)) {
                return logon(userName, password, true, saveAccount, out message);
            } else {
                message = "验证码错误，请重新输入";
                return false;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        public static bool Logon(string userName, string password, bool saveAccount, out string message) {
            return logon(userName, password, true, saveAccount, out message);
        }
        /// <summary>
        /// 无密码登录
        /// </summary>
        public static bool Logon(string userName, out string message) {
            return logon(userName, string.Empty, false, false, out message);
        }
        private static bool logon(string userName, string password, bool validatePassword, bool saveAccount, out string message) {
            try {
                var ipAddress = AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
                var employee = EmployeeService.QueryEmployee(userName);
                if (validateEmployee(employee, password, validatePassword, ipAddress, out message))
                {
                    var company = CompanyService.GetCompanyDetail(employee.Owner);
                    var superiorOEMInfo = GetSuperiorOEMInfo(company.CompanyId);
                    if(validateCompany(company, out message)) {
                        var permissionCollection = getPermission(employee, company);
                        if(permissionCollection == null) {
                            message = "加载权限信息失败";
                        }
                        else if (BasePage.OEM != null && (superiorOEMInfo == null || superiorOEMInfo.Id != BasePage.OEM.Id)
                                && BasePage.OEM.CompanyId != company.CompanyId)
                        {
                            message = "用户名不存在";
                            return false;
                        }
                        else if (permissionCollection.GetMenus().Any())
                        {
                            HttpContext.Current.Session[BasePage.EmployeeSessionKey] = employee;
                            HttpContext.Current.Session[BasePage.CompanySessionKey] = company;
                            HttpContext.Current.Session[BasePage.PermissionSessionKey] = permissionCollection;
                            HttpContext.Current.Session[BasePage.SuperiorOEMSessiongKey] = superiorOEMInfo;
                            System.Web.Security.FormsAuthentication.SetAuthCookie(userName, true);
                            var logonTime = saveEmployeeLogonInfo(employee, company,ipAddress);
                            processScore(company, employee, logonTime);
                            // 保存登录账号信息
                            if (saveAccount)
                            {
                                var cookie = new HttpCookie("userKey");//初使化并设置Cookie的名称
                                var ts = new TimeSpan(31, 0, 0, 0, 0);//过期时间为31天
                                cookie.Expires = DateTime.Now.Add(ts);//设置过期时间
                                var @byte = HttpUtility.UrlEncode(Encode(userName)) + "|" + HttpUtility.UrlEncode(Encode(password));
                                //var @byte = userName + "|" + password;
                                cookie.Values.Add("user", @byte);
                                HttpContext.Current.Response.AppendCookie(cookie);
                            }

                            return true;
                        } else {
                            message = "无访问权限";
                        }
                    }
                }
            } catch(System.Data.Common.DbException ex) {
                message = "登录失败";
                Service.LogService.SaveExceptionLog(ex);
            } catch(Exception ex) {
                message = string.Format("登录失败.{0}失败原因:{1}", Environment.NewLine, ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 查询上级的OEM信息
        /// </summary>
        private static OEMInfo GetSuperiorOEMInfo(Guid companyId) {
            var superiorInfo = CompanyService.QuerySuperiorInfo(companyId);
            if (superiorInfo == null) return null;
            return OEMService.QueryOEM(superiorInfo.Id);
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logoff() {
            // 清除保存的登录账号信息
            var cookie = new HttpCookie("userKey");//初使化并设置Cookie的名称
            cookie.Expires = DateTime.Now.AddDays(-1);//设置过期时间
            cookie.Values.Add("user", "");
            HttpContext.Current.Response.AppendCookie(cookie);

            HttpContext.Current.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
        }
        public static bool Logoned {
            get { return BasePage.LogonUser != null && HttpContext.Current.User.Identity.IsAuthenticated; }
        }
        private static bool validateEmployee(DataTransferObject.Organization.EmployeeDetailInfo employee, string password, bool validatePassword,IPAddress ipaddress, out string message) {
            if(employee == null) {
                message = "用户名不存在，请核对后输入";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(employee.IpLimitation) && employee.IpLimitation != ipaddress.ToString())
            {
                message = "不允许在此IP上登录";
                return false;
            }
            if(validatePassword && employee.UserPassword != Utility.MD5EncryptorService.MD5FilterZero(password, "utf-8")) {
                message = "登录密码错误，请重新输入";
                return false;
            }
            if(!employee.Enabled) {
                message = "账号未启用,请联系管理员";
                return false;
            }
            message = string.Empty;
            return true;
        }
        private static bool validateCompany(DataTransferObject.Organization.CompanyDetailInfo company, out string message) {
            if(company == null) {
                message = "加载单位信息失败";
                return false;
            }
            if(!company.Enabled) {
                message = "单位已被禁用";
                return false;
            }
            message = string.Empty;
            return true;
        }
        private static Service.Permission.Domain.PermissionCollection getPermission(DataTransferObject.Organization.EmployeeDetailInfo employee, DataTransferObject.Organization.CompanyDetailInfo company) {
            var userRole = DataTransferObject.Permission.UserRole.Purchaser;
            //company.Audited = company.Audited || company.CompanyType == CompanyType.Purchaser;
            if(company.Audited && !isExpired(company)) {
                userRole = getUserRole(company);
            }
            if (company.IsOem && !isExpired(company)) {
                userRole |= DataTransferObject.Permission.UserRole.DistributionOEM;
            }
            return Service.Permission.PermissionService.QueryPermissionOfUser(employee.Owner,
                userRole, employee.Id, employee.IsAdministrator, DataTransferObject.Permission.Website.Transaction);
        }
        private static bool isExpired(DataTransferObject.Organization.CompanyDetailInfo company) {
            if(company.PeriodStartOfUse.HasValue && company.PeriodStartOfUse.Value.Date > DateTime.Today) {
                return true;
            }
            if(company.PeriodEndOfUse.HasValue && company.PeriodEndOfUse.Value.Date < DateTime.Today) {
                return true;
            }
            return false;
        }
        private static DataTransferObject.Permission.UserRole getUserRole(DataTransferObject.Organization.CompanyDetailInfo company) {
            DataTransferObject.Permission.UserRole userRole;
            switch(company.CompanyType) {
                case CompanyType.Provider:
                    userRole = DataTransferObject.Permission.UserRole.Provider;
                    break;
                case CompanyType.Purchaser:
                    userRole = DataTransferObject.Permission.UserRole.Purchaser;
                    break;
                case CompanyType.Supplier:
                    userRole = DataTransferObject.Permission.UserRole.Supplier;
                    break;
                case CompanyType.Platform:
                    userRole = DataTransferObject.Permission.UserRole.Platform;
                    break;
                default:
                    throw new NotSupportedException();
            }
            return userRole;
        }
        private static DateTime saveEmployeeLogonInfo(DataTransferObject.Organization.EmployeeDetailInfo employee, DataTransferObject.Organization.CompanyDetailInfo company, IPAddress ipAddress)
        {
            try {
                var ipLocation = AddressLocator.CityLocator.GetIPLocation(ipAddress);
                var logonTime = EmployeeService.Login(new DataTransferObject.Organization.LoginInfo() {
                    CompanyId = company.CompanyId,
                    UserName = employee.UserName,
                    IP = ipAddress.ToString(),
                    Location = ipLocation.ToString()
                });
                Service.LogService.SaveLogonLog(new Service.Log.Domain.LogonLog {
                    Account = employee.UserName,
                    Company = company.CompanyId,
                    CompanyName = company.AbbreviateName,
                    Mode = Service.Log.Domain.LogonMode.B3B,
                    IPAddress = ipAddress.ToString(),
                    Address = ipLocation.ToString(),
                    Time = logonTime
                });
                return logonTime;
            } catch(Exception ex) {
                Service.LogService.SaveExceptionLog(ex);
                return DateTime.Now;
            }
        }
        private static void processScore(DataTransferObject.Organization.CompanyDetailInfo company, DataTransferObject.Organization.EmployeeDetailInfo employee, DateTime logonTime) {
            // 目前只有采购登录才有积分
            if(company.CompanyType != CompanyType.Purchaser) return;
            // 如果当天已经登录过，则不再处理
            var prevLogonTime = company.LastLoginTime;
            if(prevLogonTime.HasValue && prevLogonTime.Value.Date == logonTime.Date) return;
            try {
                Service.Integral.IntegralServer.InsertIntegralInfo(company.CompanyId, employee.UserName, prevLogonTime, logonTime);
            } catch(Exception ex) {
                Service.LogService.SaveExceptionLog(ex);
            }
        }
        /// <summary>
        /// 加密
        /// </summary>
        public static string Encode(string content) {
            var result = string.Empty;
            foreach(var item in content) {
                int ascii = item;
                var ch = (char)((ascii + result.Length + 1) << 8);
                result += ch;
            }
            return result;
        }
        /// <summary>
        /// 解密
        /// </summary>
        public static string Decode(string content) {
            var result = string.Empty;
            foreach(var item in content) {
                int ascii = item;
                var ch = (char)((ascii >> 8) - result.Length - 1);
                result += ch;
            }
            return result;
        }
    }
}