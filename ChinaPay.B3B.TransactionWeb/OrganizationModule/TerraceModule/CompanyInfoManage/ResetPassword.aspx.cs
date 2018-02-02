using System;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class ResetPassword : BasePage
    {
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeService.ResetPassword(Guid.Parse(Request.QueryString["CompanyId"]), this.remark.InnerText.Trim(), this.CurrentUser.UserName);
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception)
            {
                BasePage.ShowMessage(this,"重置密码错误");   
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
        }
    }
}