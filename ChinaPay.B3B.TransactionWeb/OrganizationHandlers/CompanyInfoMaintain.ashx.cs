using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using Izual;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using System.Text;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// CompanyInfoMaintain 的摘要说明
    /// </summary>
    public class CompanyInfoMaintain : BaseHandler
    {
        private const string m_BeginTime = "00:00";
        private const string m_EndTime = "23:59";
        private const string m_StartTime = ":00";
        private const string m_StopTime = ":59";
        private Guid RequestGuid(string companyId)
        {
            return string.IsNullOrEmpty(companyId) ? CurrentCompany.CompanyId : Guid.Parse(companyId);
        }
        public bool DeleteOfficeNo(string officeNo, string companyId)
        {
            try
            {
                CompanyService.DeleteOfficeNumber(RequestGuid(companyId), officeNo, this.CurrentUser.UserName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool UpdateOffice(string officeNumber, bool impower, string companyId)
        {
            try
            {
                CompanyService.UpdateOfficeNumber(RequestGuid(companyId), officeNumber, impower, this.CurrentUser.UserName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public object QueryOfficeNos(string companyId)
        {
            return CompanyService.QueryOfficeNumbers(RequestGuid(companyId));
        }
        public bool AddOffice(string officeNo, bool impower, string companyId)
        {
            try
            {
                CompanyService.AddOfficeNumber(RequestGuid(companyId), officeNo.ToUpper(), impower, this.CurrentUser.UserName);
                return true;
            }
            catch (DbException)
            {
                throw new Exception("保存失败！");
            }
        }
        public object BindPerson()
        {
            return CompanyService.GetBusinessManagers(this.CurrentCompany.CompanyId);
        }
        public bool UpdatePerson(List<BusinessManager> businesslist)
        {
            try
            {
                CompanyService.UpdateAllBusinessManager(this.CurrentCompany.CompanyId, businesslist, this.CurrentUser.UserName);
                return true;
            }
            catch (DbException)
            {
                throw new Exception("保存失败！");
            }
        }
        /// <summary>
        /// 获取工作时间
        /// </summary>
        private object BindWorkingHours()
        {
            WorkingHours workingHours = CompanyService.GetWorkinghours(this.CurrentCompany.CompanyId) ??
                                        new WorkingHours();
            return new
                       {
                           workstarttime =
                               workingHours.WorkdayWorkStart != Time.MinValue
                                   ? workingHours.WorkdayWorkStart.ToString("HH:mm")
                                   : m_BeginTime,
                           workendtime =
                               workingHours.WorkdayWorkEnd != Time.MinValue
                                   ? workingHours.WorkdayWorkEnd.ToString("HH:mm")
                                   : m_EndTime,
                           workstartweektime =
                               workingHours.RestdayWorkStart != Time.MinValue
                                   ? workingHours.RestdayWorkStart.ToString("HH:mm")
                                   : m_BeginTime,
                           workendweektime =
                               workingHours.RestdayWorkEnd != Time.MinValue
                                   ? workingHours.RestdayWorkEnd.ToString("HH:mm")
                                   : m_EndTime,
                           //作废时间
                           refundstarttime =
                               workingHours.WorkdayRefundStart != Time.MinValue
                                   ? workingHours.WorkdayRefundStart.ToString("HH:mm")
                                   : m_BeginTime,
                           refundendtime =
                               workingHours.WorkdayRefundEnd != Time.MinValue
                                   ? workingHours.WorkdayRefundEnd.ToString("HH:mm")
                                   : m_EndTime,
                           refundstartweektime =
                               workingHours.RestdayRefundStart != Time.MinValue
                                   ? workingHours.RestdayRefundStart.ToString("HH:mm")
                                   : m_BeginTime,
                           refundendweektime =
                               workingHours.RestdayRefundEnd != Time.MinValue
                                   ? workingHours.RestdayRefundEnd.ToString("HH:mm")
                                   : m_EndTime
                       };
        }
        public bool UpdateRefundTime(string workdayWorkStart, string workdayWorkEnd, string restdayWorkStart, string restdayWorkEnd, string workdayRefundStart, string workdayRefundEnd, string restdayRefundStart, string restdayRefundEnd)
        {
            try
            {
                WorkingHours workingHours = null;
                if (this.CurrentCompany.CompanyType == CompanyType.Provider)
                {
                    workingHours = new WorkingHours
                    {
                        WorkdayWorkStart = Time.Parse(workdayWorkStart),
                        WorkdayWorkEnd = Time.Parse(workdayWorkEnd),
                        RestdayWorkStart = Time.Parse(restdayWorkStart),
                        RestdayWorkEnd = Time.Parse(restdayWorkEnd),

                        WorkdayRefundStart = Time.Parse(workdayRefundStart),
                        WorkdayRefundEnd = Time.Parse(workdayRefundEnd),
                        RestdayRefundStart = Time.Parse(restdayRefundStart),
                        RestdayRefundEnd = Time.Parse(restdayRefundEnd),
                        Company = this.CurrentCompany.CompanyId
                    };
                }
                else
                {
                    workingHours = new WorkingHours
                    {
                        WorkdayWorkStart = Time.Parse(workdayWorkStart),
                        WorkdayWorkEnd = Time.Parse(workdayWorkEnd),
                        RestdayWorkStart = Time.Parse(restdayWorkStart),
                        RestdayWorkEnd = Time.Parse(restdayWorkEnd),
                        Company = this.CurrentCompany.CompanyId
                    };
                }
                CompanyService.SetWorkinghours(workingHours, this.CurrentUser.UserName);
                return true;
            }
            catch (DbException)
            {
                throw new Exception("保存失败！");
            }
        }
        public bool UpdateChilder(WorkingSetting workingSetting)
        {
            try
            {
                workingSetting.Company = this.CurrentCompany.CompanyId;
                CompanyService.SetWorkingSetting(workingSetting, this.CurrentUser.UserName);
                return true;
            }
            catch (DbException)
            {
                throw new Exception("保存失败！");
            }
        }
        public object BindOffice()
        {
            WorkingSetting city = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            return from item in CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId)
                   orderby item.Number
                   select new
                   {
                       office = item.Number,
                       defaultOffice = city != null && city.DefaultOfficeNumber != null ? city.DefaultOfficeNumber : ""
                   };
        }
        /// <summary>
        /// 查询当前所有员工
        /// </summary>
        /// <returns>员工列表</returns>
        public object BindEmoloyee(Guid officeId)
        {
            var officeList = CompanyService.GetEmpowermentCustoms(officeId).Select(item => item.Employee);
            var employeeList = EmployeeService.QueryEmployees(CurrentCompany.CompanyId).OrderByDescending(o => o.Enabled);
            var result = (from item in employeeList
                          select new
                          {
                              item.Name,
                              item.Id,
                              Enabled = !item.Enabled,
                              Impower = officeList.Any() && officeList.Contains(item.Id)
                          });
            return result;
        }
        /// <summary>
        /// 员工Office号授权
        /// </summary>
        public bool SetEmoloyee(Guid officeId, IEnumerable<Guid> employeeId)
        {
            return CompanyService.SetEmpowermentCustoms(officeId, employeeId, this.CurrentUser.UserName);
        }
        /// <summary>
        /// 查询自定义编码
        /// </summary>
        /// <returns></returns>
        public object QueryCustomNumber()
        {
            return CompanyService.QueryCustomNumber(CurrentCompany.CompanyId);
        }
        /// <summary>
        /// 删除自定义编号
        /// </summary>
        /// <param name="customNumberId">自定义编号Id</param>
        /// <returns></returns>
        public bool DeleteCustomNumber(Guid customNumberId)
        {
            try
            {
                CompanyService.DeleteCustomNumber(CurrentCompany.CompanyId, customNumberId, this.CurrentUser.UserName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 添加自定义编号
        /// </summary>
        /// <param name="customNumber">自定义编号</param>
        /// <param name="describe">描述</param>
        /// <returns></returns>
        public bool AddCustomNumber(string customNumber, string describe)
        {
            try
            {
                CompanyService.AddCustomNumber(customNumber.ToUpper(), describe, CurrentCompany.CompanyId, this.CurrentUser.UserName);
                return true;
            }
            catch (DbException)
            {
                throw new Exception("保存失败！");
            }
        }
        /// <summary>
        /// 认证中心拒绝审核
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="auditTypeValue">审核类型值</param>
        /// <param name="reason">拒绝审核原因</param>
        public void Audit(Guid companyId, string companyAccount, int auditTypeValue, string reason)
        {
            try
            {
                AuditType auditType = (AuditType)auditTypeValue;
                if (auditType == AuditType.NormalAudit)
                {
                    CompanyService.Reject(companyId, companyAccount, reason, this.CurrentUser.UserName);
                }
                else
                {
                    CompanyUpgradeService.Disable(companyId, companyAccount, reason, this.CurrentUser.UserName);
                }
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        /// <summary>
        /// 启用账号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public void Enable(Guid companyId)
        {
            try
            {
                CompanyService.Enable(companyId, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        /// <summary>
        /// 禁用账号
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="companyAccount">公司账号</param>
        /// <param name="reason">原因</param>
        public void Disable(Guid companyId, string companyAccount, string reason)
        {
            try
            {

                PolicyType policyType = Service.Policy.PolicyManageService.CheckIfHasDefaultPolicy(companyId);
                if (PolicyType.Unknown != policyType) throw new ChinaPay.Core.CustomException("该供应商在平台存在默认政策指向，请调整后再行操作");
                CompanyService.Disable(companyId, companyAccount, reason, CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }

        public bool UpdateDrawContext(Guid id, byte type, string title, string context)
        {
            try
            {
                CompanyService.UpdateCompanyDrawdition(new ChinaPay.B3B.Service.Organization.Domain.CompanyDrawdition() { Id = id, OwnerId = CurrentCompany.CompanyId, Title = title, Context = context, Type = type }, CurrentUser.UserName);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        public bool InsertDrawContext(string title, byte type, string context)
        {
            try
            {
                CompanyService.InsertCompanyDrawdition(new ChinaPay.B3B.Service.Organization.Domain.CompanyDrawdition() { OwnerId = CurrentCompany.CompanyId, Type = type, Title = title, Context = context }, CurrentUser.UserName);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        public bool DelDrawbyId(Guid id)
        {
            try
            {
                CompanyService.DelDrawditionById(id, CurrentUser.UserName);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        public object QueryDrawditions()
        {
            return CompanyService.QueryDrawditionByCompanyId(CurrentCompany.CompanyId);
            //StringBuilder str = new StringBuilder();
            //str.Append("<table>");
            //str.Append("<tr><th>序号</th><th>标题</th><th>内容</th><th>操作</th></tr>");
            //for (int i = 0; i < list.Count; i++)
            //{
            //    var dition = list[i];
            //    str.AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td> <span class='context'>{3}</span></td></tr>", i + 1, dition.Title, dition.Context, "<a href='javascript:UdpateDition(\'" + dition.Id + "\',\'" + dition.Title + "\',\'" + dition.Context + "\');'>修改</a>&nbsp;&nbsp;&nbsp;<a href='javascript:DelDition(\'" + dition.Id + "\')'>删除</a>");
            //}
            //str.Append("</table>");
            //return str.ToString();
        }
        /// <summary>
        /// 设置该公司下所有员工的Ip
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="ipLimitation">Ip限制</param>
        public bool SetAllIpLimitation(Guid companyId,string ipLimitation)
        {
            try
            {
                EmployeeService.SetAllIpLimitation(companyId, ipLimitation);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }
        /// <summary>
        /// 设置单个员工的Ip
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="ipLimitation">Ip限制</param>
        public bool SetSingleIpLimitation(Guid employeeId, string ipLimitation)
        {
            try
            {
                EmployeeService.SetSingleIpLimitation(employeeId, ipLimitation);
                return true;
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("发生未知错误，请稍后再试");
            }
        }

    }
}