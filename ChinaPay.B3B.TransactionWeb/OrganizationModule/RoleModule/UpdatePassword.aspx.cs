using System;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class UpdatePassword : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");

        }

        protected void btnConfirmUpdate_Click(object sender, EventArgs e)
        {
            var employeeInfo = Session["CurrentEmployee"] as EmployeeDetailInfo;
            var companyInfo = Session["CurrentCompany"] as CompanyDetailInfo;
            if (employeeInfo != null)
            {
                try
                {
                    EmployeeService.ChangePassword(new ChangePasswordInfo
                    {
                        EmployeeId = employeeInfo.Id,
                        UserNo = employeeInfo.UserName,
                        OldPassword = this.txtOriginalPassword.Text.Trim(),
                        NewPassword = this.txtNewPassword.Text.Trim(),
                        ConfirmPassword = this.txtConfirmPassword.Text.Trim()
                    },employeeInfo.UserName);
                    if (companyInfo.CompanyType == Common.Enums.CompanyType.Purchaser)
                    {
                        BasePage.RegisterScript(this, "alert('修改密码成功');window.location.href='/Index.aspx';", false);
                    }
                    else
                    {
                        BasePage.RegisterScript(this, "alert('修改密码成功');window.location.href='/TicketDefault.aspx';", false);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    BasePage.ShowMessage(this, ex.Message);
                }
                catch (Exception ex)
                {
                    BasePage.ShowExceptionMessage(this, ex, "修改密码");
                }
            }
            else
            {
                Response.Redirect("~/Logon.aspx");
            }
        }
    }
}