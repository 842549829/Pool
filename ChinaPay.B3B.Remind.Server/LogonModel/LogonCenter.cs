using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.Remind.Server {
    class LogonCenter : System.IDisposable {
        private static LogonCenter m_instance = null;
        private static object m_locker = new object();
        public static LogonCenter Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new LogonCenter();
                        }
                    }
                }
                return m_instance;
            }
        }
        // 以登录批准号作为键
        private Dictionary<Guid, LogonModel.User> m_users = null;
        // 以单位Id作为键
        private Dictionary<Guid, LogonModel.Company> m_companies = null;
        private LogonCenter() {
            m_users = new Dictionary<Guid, LogonModel.User>();
            m_companies = new Dictionary<Guid, LogonModel.Company>();
        }

        public IEnumerable<LogonModel.Company> Companies {
            get {
                return m_companies.Values;
            }
        }
        public IEnumerable<LogonModel.User> Users {
            get {
                return m_users.Values;
            }
        }

        public bool Logon(string userName, string password, System.Net.Sockets.TcpClient connection, out LogonModel.User logonUser, out string errorCode) {
            logonUser = null;
            errorCode = string.Empty;
            var originalEmployee = EmployeeService.QueryEmployee(userName);
            if(originalEmployee == null) {
                errorCode = "1";
                return false;
            }
            if(originalEmployee.UserPassword != Utility.MD5EncryptorService.MD5FilterZero(password, "utf-8")) {
                errorCode = "4";
                return false;
            }
            if(!originalEmployee.Enabled) {
                errorCode = "5";
                return false;
            }
            var originalCompany = CompanyService.GetCompanyDetail(originalEmployee.Owner);
            if(originalCompany == null) {
                errorCode = "6";
                return false;
            }
            if(!originalCompany.Enabled) {
                errorCode = "7";
                return false;
            }
            if(originalCompany.CompanyType == CompanyType.Platform) {
                errorCode = "8";
                return false;
            }
            var isExpired = false;
            if(originalCompany.PeriodStartOfUse.HasValue) {
                isExpired |= originalCompany.PeriodStartOfUse.Value.Date > DateTime.Today;
            }
            if(originalCompany.PeriodEndOfUse.HasValue) {
                isExpired |= originalCompany.PeriodEndOfUse.Value.Date < DateTime.Today;
            }
            var companyType = CompanyType.Purchaser;
            if(originalCompany.Audited || !isExpired) {
                companyType = originalCompany.CompanyType;
            }
            LogonModel.Company company = null;
            if(!m_companies.TryGetValue(originalCompany.CompanyId, out company)) {
                company = new LogonModel.Company(originalCompany.CompanyId) {
                    Name = originalCompany.AbbreviateName,
                    Type = companyType
                };
                m_companies.Add(company.Id, company);
            }
            logonUser = new LogonModel.User(originalEmployee.Id, connection) {
                Name = userName,
                Owner = company,
                WorkOnCustomNO = originalCompany.CustomNO_On
            };
            if(logonUser.WorkOnCustomNO) {
                logonUser.CustomNOs = string.Join(",", CompanyService.GetCustomNumberByEmployee(originalEmployee.Id).Select(c => c.Number));
            }
            company.RegisterMember(logonUser);
            m_users.Add(logonUser.BatchNo, logonUser);
            DataProcessor.Instance.Start();
            return true;
        }
        public LogonModel.User Logoff(Guid batchNo) {
            LogonModel.User result = null;
            m_users.TryGetValue(batchNo, out result);
            if(result != null) {
                m_users.Remove(batchNo);
                result.Owner.Remove(batchNo);
                if(result.Owner.IsEmpty) {
                    m_companies.Remove(result.Owner.Id);
                }
                if(m_companies.Count == 0) {
                    DataProcessor.Instance.Stop();
                }
            }
            return result;
        }
        public LogonModel.Company GetCompany(Guid company) {
            LogonModel.Company result = null;
            m_companies.TryGetValue(company, out result);
            return result;
        }
        public LogonModel.User GetUser(Guid batchNo) {
            LogonModel.User result = null;
            m_users.TryGetValue(batchNo, out result);
            return result;
        }
        public void Dispose() {
            foreach(var item in m_users.Values) {
                item.Release();
            }
            m_users.Clear();
            m_companies.Clear();
            GC.SuppressFinalize(this);
        }
    }
}