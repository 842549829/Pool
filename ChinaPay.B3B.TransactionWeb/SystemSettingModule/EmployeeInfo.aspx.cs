using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class EmployeeInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {

                BindEmployeeInfo();
            }
        }
        void BindEmployeeInfo()
        {
            Guid employeeId;
            if (Guid.TryParse(Request.QueryString["employeeId"].ToString(), out employeeId))
            {
                var emp = EmployeeService.QueryEmployee(employeeId);
                lblAccountNo.Text = emp.UserName;
                lblCellphone.Text = emp.Cellphone;
                lblEmail.Text = emp.Email;
                lblName.Text = emp.Name;
                lblPhone.Text = emp.OfficePhone;
                lblRemark.Text = emp.Remark;
                lblSex.Text = emp.Gender.GetDescription();
                lblLook.Text = Guid.Equals(Guid.Parse("00000000-0000-0000-0000-000000000001"), emp.Owner) ? "" : "<a href='/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + emp.Owner + "'>( 查看该员工所在公司的信息 )</a>";
            }
            else
            {
                Response.Redirect("./SuggestList.aspx", true);
            }
        }
    }
}