using System;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using System.Net;

namespace ChinaPay.B3B.MaintenanceWeb {
    internal class LogonUtility {
        const int ValidateCodeLength = 4;
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
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="message">错误信息</param>
        public static bool Logon(string userName, string password, string validateCode, out string message) {
            message = string.Empty;
            if(validateValidateCode(validateCode)) {
                try {
                    var ipAddress = AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
                    var employee = Service.Organization.EmployeeService.QueryEmployee(userName);
                    if (validateEmployee(employee, password, ipAddress, out message))
                    {
                        var company = Service.Organization.CompanyService.GetCompanyDetail(employee.Owner);
                        if(validateCompany(company, out message)) {
                            var permissionCollection = getPermission(employee, company);
                            if(permissionCollection == null) {
                                message = "加载权限信息失败";
                            } else if(!permissionCollection.GetMenus().Any()) {
                                message = "无访问权限";
                            } else {
                                HttpContext.Current.Session[BasePage.EmployeeSessionKey] = employee;
                                HttpContext.Current.Session[BasePage.CompanySessionKey] = company;
                                HttpContext.Current.Session[BasePage.PermissionSessionKey] = permissionCollection;
                                System.Web.Security.FormsAuthentication.SetAuthCookie(userName, true);
                                saveEmployeeLogonInfo(employee, company,ipAddress);
                                return true;
                            }
                        }
                    }
                } catch(System.Data.Common.DbException) {
                    message = "登录失败";
                } catch(Exception ex) {
                    message = string.Format("登录失败.{0}失败原因:{1}", Environment.NewLine, ex.Message);
                }
            } else {
                message = "验证码错误";
            }
            return false;
        }
        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout() {
            HttpContext.Current.Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();
        }
        public static bool Logoned {
            get { return BasePage.LogonUser != null; }
        }

        private static bool validateEmployee(DataTransferObject.Organization.EmployeeDetailInfo employee, string password, IPAddress ipaddress, out string message)
        {
            if(employee == null) {
                message = "用户名不存在";
                return false;
            }
            if (!string.IsNullOrWhiteSpace(employee.IpLimitation) && employee.IpLimitation != ipaddress.ToString())
            {
                message = "不允许在此IP上登录";
                return false;
            }
            if (employee.UserPassword != ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(password,"utf-8"))
            {
                message = "密码错误";
                return false;
            }
            if(!employee.Enabled) {
                message = "账号未启用";
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
            if(company.Audited && !isExpired(company)) {
                userRole = getUserRole(company);
            }
            return Service.Permission.PermissionService.QueryPermissionOfUser(employee.Owner,
                userRole, employee.Id, employee.IsAdministrator, DataTransferObject.Permission.Website.Maintenance);
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
            if(company.IsOem) {
                userRole |= DataTransferObject.Permission.UserRole.DistributionOEM;
            }
            return userRole;
        }
        private static void saveEmployeeLogonInfo(DataTransferObject.Organization.EmployeeDetailInfo employee, DataTransferObject.Organization.CompanyDetailInfo company, IPAddress ipAddress)
        {
            try {
                var ipLocation = ChinaPay.AddressLocator.CityLocator.GetIPLocation(ipAddress);
                Service.Organization.EmployeeService.Login(new DataTransferObject.Organization.LoginInfo() {
                    UserName = employee.UserName,
                    IP = ipAddress.ToString(),
                    Location = ipLocation.ToString()
                });
                Service.LogService.SaveLogonLog(new Service.Log.Domain.LogonLog() {
                    Account = employee.UserName,
                    Company = company.CompanyId,
                    CompanyName = company.AbbreviateName,
                    Mode = Service.Log.Domain.LogonMode.B3B,
                    IPAddress = ipAddress.ToString(),
                    Address = ipLocation.ToString(),
                    Time = DateTime.Now
                });
            } catch { }
        }
    }
}