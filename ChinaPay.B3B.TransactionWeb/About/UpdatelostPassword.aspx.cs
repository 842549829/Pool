using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class UpdatelostPassword : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (txtPwd.Value == "")
            {
                BasePage.RegisterJavaScript(this, "alert('新密码不能为空。');");
                return;
            }
            if (txtRePwd.Value == "")
            {
                BasePage.RegisterJavaScript(this, "alert('确认密码不能为空。');");
                return;
            }
            if (txtPwd.Value != txtRePwd.Value)
            {
                BasePage.RegisterJavaScript(this, "alert('新密码和确认密码不一致。');");
                return;
            }
            EmployeeService.ChangePassword(Session["accountno"].ToString(), txtPwd.Value);
            BasePage.RegisterScript(this, "window.location.href='Success.aspx'"); 
        }
    }
}