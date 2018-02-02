using System;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using Izual;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class WorkTime :BasePage
    {
        private const string m_BeginTime = "00:00";
        private const string m_EndTime = "23:59";
        private const string m_StartTime=":00";
        private const string m_StopTime = ":59";
        protected void Page_Load(object sender,EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                if (this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier)
                    this.tbRefund.Visible = false;
                this.BindWorkingHours();
            }
        }
        /// <summary>
        /// 获取工作时间
        /// </summary>
        private void BindWorkingHours() 
        {
            WorkingHours workingHours = CompanyService.GetWorkinghours(this.CurrentCompany.CompanyId) ?? new WorkingHours();
            this.BindWorkingDays(workingHours);
            this.BindRestDays(workingHours);
            this.BindRefundWorkdays(workingHours);
            this.BindRefundRestDays(workingHours);

        }
        /// <summary>
        /// 获取废票周末时间
        /// </summary>
        private void BindRefundRestDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtRestdayRefundStart.Text = workingHours.RestdayRefundStart != Time.MinValue ? workingHours.RestdayRefundStart.ToString("HH:mm") : m_BeginTime;
            this.txtRestdayRefundEnd.Text = workingHours.RestdayRefundEnd !=Time.MinValue?workingHours.RestdayRefundEnd.ToString("HH:mm"):m_EndTime;
        }
        /// <summary>
        /// 获取废票工作日时间
        /// </summary>
        private void BindRefundWorkdays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtWorkdayRefundStart.Text = workingHours.WorkdayRefundStart!=Time.MinValue? workingHours.WorkdayRefundStart.ToString("HH:mm"):m_BeginTime;
            this.txtWorkdayRefundEnd.Text = workingHours.WorkdayRefundEnd !=Time.MinValue? workingHours.WorkdayRefundEnd.ToString("HH:mm"):m_EndTime;
        }
        /// <summary>
        /// 获取周末工作时间
        /// </summary>
        private void BindRestDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtRestdayWorkStart.Text = workingHours.RestdayWorkStart!=Time.MinValue? workingHours.RestdayWorkStart.ToString("HH:mm"):m_BeginTime;
            this.txtRestdayWorkEnd.Text = workingHours.RestdayWorkEnd!=Time.MinValue? workingHours.RestdayWorkEnd.ToString("HH:mm"):m_EndTime;
        }
        /// <summary>
        /// 获取工作日工作时间
        /// </summary>
        private void BindWorkingDays(Data.DataMapping.WorkingHours workingHours)
        {
            this.txtWorkdayWorkStart.Text = workingHours.WorkdayWorkStart != Time.MinValue ?workingHours.WorkdayWorkStart.ToString("HH:mm"):m_BeginTime;
            this.txtWorkdayWorkEnd.Text = workingHours.WorkdayWorkEnd != Time.MinValue ? workingHours.WorkdayWorkEnd.ToString("HH:mm") : m_EndTime;
        }
        private WorkingHours GetWorkingHoursProvider() 
        {
            return new WorkingHours
            {
                Company = this.CurrentCompany.CompanyId,
                WorkdayWorkStart = Time.Parse(this.txtWorkdayWorkStart.Text + m_StartTime),
                WorkdayWorkEnd = Time.Parse(this.txtWorkdayWorkEnd.Text + m_StopTime),
                RestdayWorkStart = Time.Parse(this.txtRestdayWorkStart.Text + m_StartTime),
                RestdayWorkEnd = Time.Parse(this.txtRestdayWorkEnd.Text + m_StopTime),
                WorkdayRefundStart = Time.Parse(this.txtWorkdayRefundStart.Text + m_StartTime),
                WorkdayRefundEnd = Time.Parse(this.txtWorkdayRefundEnd.Text + m_StopTime),
                RestdayRefundStart = Time.Parse(this.txtRestdayRefundStart.Text + m_StartTime),
                RestdayRefundEnd = Time.Parse(this.txtRestdayRefundEnd.Text + m_StopTime)
            };
        }
        private WorkingHours GetWorkingHoursSupplier() {
            return new WorkingHours {
                Company = this.CurrentCompany.CompanyId,
                WorkdayWorkStart = Time.Parse(this.txtWorkdayWorkStart.Text + m_StartTime),
                WorkdayWorkEnd = Time.Parse(this.txtWorkdayWorkEnd.Text + m_StopTime),
                RestdayWorkStart = Time.Parse(this.txtRestdayWorkStart.Text + m_StartTime),
                RestdayWorkEnd = Time.Parse(this.txtRestdayWorkEnd.Text + m_StopTime),
            };
        }
        protected void btnConfirmUpdate_Click(object sender, EventArgs e)
        {
            string str = this.txtWorkdayWorkStart.Text;
            try
            {
                if (this.CurrentCompany.CompanyType == Common.Enums.CompanyType.Supplier)
                    CompanyService.SetWorkinghours(this.GetWorkingHoursSupplier(),this.CurrentUser.UserName);
                else
                    CompanyService.SetWorkinghours(this.GetWorkingHoursProvider(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
                Response.Redirect("/TicketDefault.aspx",false);
            }
            catch (Exception)
            {
                BasePage.ShowMessage(this,"设置工作时间失败");
            }
        }
    }
}