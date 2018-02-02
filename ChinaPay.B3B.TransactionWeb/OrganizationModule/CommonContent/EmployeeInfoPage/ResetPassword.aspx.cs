using System;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage
{
    public partial class ResetPassword : BasePage
    {
        protected void btnReset_Click(object sender, EventArgs e)
        {
            string employeeId = Request.QueryString["EmployeeId"];
            try
            {
                EmployeeService.ResetPassword(Guid.Parse(employeeId), this.remark.InnerText.Trim(),this.CurrentUser.UserName);
                BasePage.ShowMessage(this, "重置密码成功\n\r默认密码" + ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultPassword);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "重置密码");
            }
        }
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");

        }
    }
}