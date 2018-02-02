using System;
using System.Collections.Generic;
using System.Transactions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core;
using ChinaPay.PoolPay.Service;
using Izual.Linq;

namespace ChinaPay.B3B.Service.Organization {
    public static class EmployeeService {
        /// <summary>
        /// 为指定的公司添加一个员工。
        /// </summary>
        /// <param name="companyId">要添加员工的公司的 Id</param>
        /// <param name="info">要添加的员工的信息。</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回添加操作是否成功。</returns>
        public static bool AddEmployee(Guid companyId, EmployeeInfo info, string operatorAccount) {
            if(info == null) throw new ArgumentNullException("info");
            var company = CompanyService.GetCompanyInfo(companyId);
            if(company == null)
                throw new InvalidOperationException("指定的公司不存在，不能添加新员工。");

            var employee = CreateEmployee(info);
            if (EmployeeService.ExistsUserName(employee.Login))
            {
                throw new InvalidOperationException("系统中已存在指定的账号。");
            }
            employee.Owner = company.Id;
            employee.Insert();
            saveLog(OperationType.Insert, string.Format("添加员工账号{0}的信息", info.UserName), OperatorRole.User, info.UserName, operatorAccount);
            return true;
        }
        /// <summary>
        /// 更新指定的员工。
        /// </summary>
        /// <param name="info">要更新的员工的信息。</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回更新操作是否成功。</returns>
        public static bool UpdateEmployee(EmployeeInfo info, string operatorAccount) {
            bool isSuccess = false;
            if(info == null) throw new ArgumentNullException("info");
            if(info.Id == Guid.Empty) throw new InvalidOperationException("缺少员工 Id");
            if(!Regexes.Name.Match(info.Name).Success)
                throw new InvalidOperationException("姓名无效。");
            if(!Regexes.UserName.Match(info.UserName).Success)
                throw new InvalidOperationException("账号无效。");
            if(!Regexes.Phone.Match(info.OfficePhone).Success)
                throw new InvalidOperationException("座机号码无效。");

            isSuccess = DataContext.Employees.Update(e => new
            {
                info.Name,
                info.Gender,
                info.Cellphone,
                info.OfficePhone,
                info.Email,
                info.Remark,
            }, e => e.Id == info.Id) > 0;
            saveLog(OperationType.Update, string.Format("修改员工账号为{0}的信息", info.UserName), OperatorRole.User, info.UserName, operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 禁用指定的员工。
        /// </summary>
        /// <param name="id">将被禁用的员工的 Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回禁用操作是否成功。</returns>
        public static bool DisableEmployee(Guid id, string operatorAccount) {
            bool isSuccess = false;
            var employee = EmployeeService.QueryEmployee(id);
            if (employee == null) throw new InvalidOperationException("指定的员工不存在");
            if (employee.IsAdministrator) throw new InvalidOperationException("指定的员工不能被禁用");
            if (!employee.Enabled) throw new InvalidOperationException("指定的员工已经被禁用。");
            isSuccess = DataContext.Employees.Update(e => new
            {
                Enabled = false
            }, e => e.Id == id) > 0;
            saveLog(OperationType.Update, string.Format("将员工Id{0}的账号禁用", id), OperatorRole.User, id.ToString(), operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 启用指定的员工。
        /// </summary>
        /// <param name="id">将被启用的员工的 Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回启用操作是否成功。</returns>
        public static bool EnableEmployee(Guid id, string operatorAccount) {
            bool isSuccess = false;
            var employee = EmployeeService.QueryEmployee(id);
            if (employee == null) throw new InvalidOperationException("指定的员工不存在");
            if (employee.IsAdministrator) throw new InvalidOperationException("指定的员工不能被禁用");
            if (employee.Enabled) throw new InvalidOperationException("指定的员工已经被启用。");
            isSuccess = DataContext.Employees.Update(e => new
            {
                Enabled = true
            }, e => e.Id == id) > 0;
            saveLog(OperationType.Update, string.Format("将员工Id{0}的账号启用", id), OperatorRole.User, id.ToString(), operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 员工修改自身密码
        /// </summary>
        /// <param name="info">密码修改信息。</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回修改操作是否成功。</returns>
        public static bool ChangePassword(ChangePasswordInfo info, string operatorAccount) {
            bool isSuccess = false;
            if(info == null) throw new ArgumentNullException("info");
            if(info.NewPassword != info.ConfirmPassword) {
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");
            }
            var employee = EmployeeService.QueryEmployee(info.EmployeeId);
            
            //var employee = DataContext.Employees.Where(e => e.Id == info.EmployeeId).Select(e => new Employee
            //{
            //    Id = e.Id,
            //    Password = e.Password
            //}).FirstOrDefault();
            if(employee == null)
                throw new InvalidOperationException("指定的账号不存在。");
            if(employee.UserPassword != Utility.MD5EncryptorService.MD5FilterZero(info.OldPassword, "utf-8"))
                throw new InvalidOperationException("密码不正确。");
            if(AccountBaseService.GetMebershipUser(info.UserNo)) {
                AccountBaseService.B3BResetLoginPassword(info.UserNo, info.NewPassword);
            }
            isSuccess = DataContext.Employees.Update(e => new
            {
                Password = Utility.MD5EncryptorService.MD5FilterZero(info.NewPassword, "utf-8")
            }, e => e.Id == info.EmployeeId) > 0;
            saveLog(OperationType.Update, "修改密码。", OperatorRole.User, info.EmployeeId.ToString(), operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 根据账号修改密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newPassword">新密码</param>
        public static bool ChangePassword(string userName, string newPassword) {
            if(PoolPay.Service.AccountBaseService.GetMebershipUser(userName)) {
                PoolPay.Service.AccountBaseService.B3BResetLoginPassword(userName, newPassword);
            }
            return DataContext.Employees.Update(e => new
            {
                Password = Utility.MD5EncryptorService.MD5FilterZero(newPassword, "utf-8")
            }, e => e.Login == userName) > 0;
        }
        /// <summary>
        /// 重置指定员工的密码。
        /// </summary>
        /// <param name="id">将被重置密码的员工的 Id</param>
        /// <param name="reason"> 重置密码原因 </param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回重置操作是否成功。</returns>
        public static bool ResetPassword(Guid id, string reason, string operatorAccount) {
            bool isSuccess = false;
            var defaultPassword = SystemManagement.SystemParamService.DefaultPassword;
            var employee = EmployeeService.QueryEmployee(id);
            //var employee = DataContext.Employees.Where(e => e.Id == id).Select(e => new Employee {
            //    Id = e.Id,
            //    Password = e.Password
            //}).FirstOrDefault();
            if(employee == null)
                throw new InvalidOperationException("指定的账号不存在。");
            if(AccountBaseService.GetMebershipUser(employee.UserName)) {
                AccountBaseService.B3BResetLoginPassword(employee.UserName, defaultPassword);
            }
            isSuccess = DataContext.Employees.Update(e => new
            {
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(defaultPassword, "utf-8")
            }, e => e.Id == id) > 0;
            saveLog(OperationType.Update, string.Format("将账号{0}密码重置为{1}", employee.UserName, defaultPassword), OperatorRole.User, employee.UserName, operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 根据登录账号重置员工密码
        /// </summary>
        /// <param name="login">登录账号</param>
        /// <param name="reason"> 重置密码原因 </param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <returns>返回重置密码操作是否成功</returns>
        public static bool ResetPassword(string login, string reason, string operatorAccount) {
            bool isSuccess = false;
            var defaultPassword = SystemManagement.SystemParamService.DefaultPassword;
            if(AccountBaseService.GetMebershipUser(login)) {
                AccountBaseService.B3BResetLoginPassword(login, defaultPassword);
            }
            isSuccess = DataContext.Employees.Update(e => new
            {
                Password = ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(defaultPassword, "utf-8")
            }, e => e.Login == login) > 0;
            saveLog(OperationType.Update, string.Format("将账号{0}密码重置为{1}", login, defaultPassword), OperatorRole.User, login, operatorAccount);
            return isSuccess;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="info">登录信息</param>
        /// <returns>返回是否通过验证，并正确记录登录信息。</returns>
        public static DateTime Login(LoginInfo info) {
            var loginTime = DateTime.Now;
            using(var trans = new TransactionScope()) {
                DataContext.Companies.Update(e => new
                {
                    LastLoginTime = loginTime
                }, e => e.Id == info.CompanyId);
                DataContext.Employees.Update(o => new
                {
                    LastLoginIP = info.IP,
                    LastLoginLocation = info.Location,
                    LastLoginTime = loginTime
                }, e => e.Login == info.UserName);
                trans.Complete();
                return loginTime;
            }
        }
        /// <summary>
        /// 检查用户名是否存在。
        /// </summary>
        /// <param name="name">要检查的用户名。</param>
        /// <returns>存在返回 true，否则返回 false</returns>
        public static bool ExistsUserName(string name) {
            return Factory.CreateEmployeeRepository().ExistsUserName(name);
        }
        /// <summary>
        /// 员工列表查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页条件</param>
        public static IEnumerable<EmployeeListView> QueryEmployees(EmployeeQueryParameter condition, Pagination pagination) {
            var repository = Factory.CreateEmployeeRepository();
            return repository.QueryEmployeeInfo(condition, pagination);
        }
        /// <summary>
        /// 获取指定公司的所有员工
        /// </summary>
        /// <param name="ownerId">所属公司Id</param>
        public static IEnumerable<EmployeeListView> QueryEmployees(Guid ownerId) {
            var condition = new EmployeeQueryParameter {
                Owner = ownerId
            };
            var pagination = new Pagination {
                GetRowCount = false,
                PageIndex = 1,
                PageSize = int.MaxValue
            };
            return QueryEmployees(condition, pagination);
        }
        /// <summary>
        /// 根据员工 Id 查询员工信息
        /// </summary>
        /// <param name="id">员工编号</param>
        public static EmployeeDetailInfo QueryEmployee(Guid id) {
            var repository = Factory.CreateEmployeeRepository();
            return repository.QueryEmployee(id);
        }
        /// <summary>
        /// 根据用户名查询员工信息
        /// </summary>
        /// <param name="userName">用户名</param>
        public static EmployeeDetailInfo QueryEmployee(string userName) {
            var repository = Factory.CreateEmployeeRepository();
            return repository.QueryEmployee(userName);
        }

        private static Employee CreateEmployee(EmployeeInfo info) {
            if(info == null) throw new ArgumentNullException("info");
            if(!Regexes.Name.Match(info.Name).Success)
                throw new InvalidOperationException("姓名无效。");
            if(!Regexes.UserName.Match(info.UserName).Success)
                throw new InvalidOperationException("账号无效。");

            if(!Regexes.Password.Match(info.UserPassword).Success)
                throw new InvalidOperationException("密码无效。");
            if(info.UserPassword != info.ConfirmPassword)
                throw new InvalidOperationException("\"密码\" 与 \"确认密码\" 不相同。");
            if(!Regexes.Phone.Match(info.OfficePhone).Success)
                throw new InvalidOperationException("座机号码无效。");

            return new Employee {
                Id = info.Id == Guid.Empty ? Guid.NewGuid() : info.Id,
                Name = info.Name,
                Gender = info.Gender,
                Cellphone = info.Cellphone,
                OfficePhone = info.OfficePhone,
                Email = info.Email,
                Login = info.UserName,
                Password = Utility.MD5EncryptorService.MD5FilterZero(info.UserPassword, "utf-8"),
                Remark = info.Remark,
                Enabled = info.Enabled,
                CreateTime = DateTime.Now
            };
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account) {
            var log = new Log.Domain.OperationLog(OperationModule.员工, operationType, account, role, key, content);
            try {
                LogService.SaveOperationLog(log);
            } catch { }
        }

        public static Employee QueryCompanyAdmin(Guid companyId) {
            var repository = Factory.CreateEmployeeRepository();
            return repository.QueryCompanyAdmin(companyId);

        }
        /// <summary>
        /// 设置单个公司下所有员工的Ip
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static void SetAllIpLimitation(Guid companyId,string ipLimitation)
        {
            var repository = Factory.CreateEmployeeRepository();
            repository.SetAllIpLimitation(companyId,ipLimitation);
        }
        /// <summary>
        /// 设置单个员工的Ip
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        public static void SetSingleIpLimitation(Guid employeeId,string ipLimitation)
        {
            var repository = Factory.CreateEmployeeRepository();
            repository.SetSingleIpLimitation(employeeId,ipLimitation);
        }
    }
}