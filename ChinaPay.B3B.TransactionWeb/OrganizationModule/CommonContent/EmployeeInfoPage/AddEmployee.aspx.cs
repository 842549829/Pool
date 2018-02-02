using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage
{
    public partial class AddEmployee : BasePage
    {
        private void SucceedInfo()
        {
            Response.Redirect("./StaffInfoMgr.aspx", false);
        }
        protected void Page_Load(object sender,EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");

        }

        private EmployeeInfo GetEmployeeCreatureInfo()
        {
           return new EmployeeInfo
            {
                Name = this.txtName.Text.Trim(),
                Gender = this.rdoMan.Checked ? Gender.Male : Gender.Female,
                UserName = this.txtAccountNo.Text.Trim(),
                UserPassword = this.txtPassword.Text.Trim(),
                ConfirmPassword = this.txtConfirmPassword.Text.Trim(),
                Cellphone = this.txtCellPhone.Text.Trim(),
                OfficePhone = this.txtPhone.Text.Trim(),
                Email = this.txtEmail.Text.Trim(),
                Enabled = this.rdoEnabled.Checked ? true : false,
                Remark = this.remark.InnerText.Trim()
            };
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.AddEmployee(this.CurrentCompany.CompanyId, this.GetEmployeeCreatureInfo(),this.CurrentUser.UserName);
                ShowMessage("添加成功");
                this.SucceedInfo();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"添加");
            }
        }
        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.AddEmployee(this.CurrentCompany.CompanyId, this.GetEmployeeCreatureInfo(), this.CurrentUser.UserName);
                ShowMessage("添加成功并继续");
                this.ClearTextBox();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "添加");
            }
        }
        private void ClearTextBox() {
            foreach (Control control in Controls)
                for (int i = 0; i < control.Controls.Count; i++)
                    if (control.Controls[i] is TextBox)
                        ((TextBox)control.Controls[i]).Text = string.Empty;
            this.remark.InnerText = string.Empty;
        }
    }
}