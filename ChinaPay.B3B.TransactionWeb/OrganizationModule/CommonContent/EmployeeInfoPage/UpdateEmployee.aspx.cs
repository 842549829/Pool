using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.EmployeeInfoPage
{
    public partial class UpdateEmployee : BasePage
    {
        private static string employeeId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                employeeId = Request.QueryString["EmployeeId"];
                if (!string.IsNullOrWhiteSpace(employeeId))
                {
                    this.BindEmployeeInfo(EmployeeService.QueryEmployee(Guid.Parse(employeeId)));
                }
            }
        }
        private void SucceedInfo()
        {
            Response.Redirect("./StaffInfoMgr.aspx", false);
        }
        private void BindEmployeeInfo(DataTransferObject.Organization.EmployeeDetailInfo info)
        {
            this.lblName.Text = info.Name;
            this.lblAccountNo.Text = info.UserName;
            bool bnlGender = info.Gender == Gender.Male ? this.rdoMan.Checked = true : this.rdoWoan.Checked =true;
            this.txtCellPhone.Text = info.Cellphone;
            this.txtPhone.Text = info.OfficePhone;
            this.txtEmail.Text = info.Email;
            bool bnlEnabled = info.Enabled ? this.rdoEnabled.Checked=true : this.rdoOnEnabled.Checked = true;
            this.rdoEnabled.Enabled = false;
            this.rdoOnEnabled.Enabled = false;
            this.remark.InnerText = info.Remark;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.UpdateEmployee(this.GetEmployeeCreatureInfo(),this.CurrentUser.UserName);
                ShowMessage("修改成功");
                this.SucceedInfo();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
        }
        private EmployeeInfo GetEmployeeCreatureInfo() 
        {
           return new EmployeeInfo
            {
                Id = Guid.Parse(employeeId),
                UserName = this.lblAccountNo.Text,
                Gender = this.rdoMan.Checked ? Gender.Male : Gender.Female,
                Cellphone = this.txtCellPhone.Text.Trim(),
                OfficePhone = this.txtPhone.Text.Trim(),
                Email = this.txtEmail.Text.Trim(),
                Enabled = this.rdoEnabled.Checked ? true : false,
                Remark = this.remark.InnerText.Trim(),
                Name = this.lblName.Text
            };
        }
    }
}