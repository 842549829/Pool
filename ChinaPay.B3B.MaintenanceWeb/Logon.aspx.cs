using System;

namespace ChinaPay.B3B.MaintenanceWeb {
    public partial class Logon : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                if(LogonUtility.Logoned && this.Context.User.Identity.IsAuthenticated) {
                    Response.Redirect(System.Web.Security.FormsAuthentication.DefaultUrl, true);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e) {
            string message;
            if(LogonUtility.Logon(this.txtUserName.Text.Trim(), this.txtPassWord.Text, this.txtCode.Text.Trim(), out message)) {
                Response.Redirect(System.Web.Security.FormsAuthentication.DefaultUrl, true);
            } else {
                BasePage.ShowMessage(this, message);
            }
        }
    }
}