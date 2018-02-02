using System;
using System.Web;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb {
    public partial class Logon : UnAuthBasePage {
        private static string logonUrl = Service.SystemManagement.SystemParamService.B3BDefalutLogonUrl;
        protected void Page_Load(object sender, EventArgs e) {
            if (OEMLogin()) return;
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if(OEM != null && !OEM.Enabled) {
                Response.Redirect(logonUrl, true);
                return;
            }
            if(!IsPostBack) {
                lblPlatformName.Text = PlatformName;
                if(IsOEM && !OEM.AllowSelfRegex) {
                    this.btnRegister.Visible = false;
                }
                //BasePage.AddStyleSheet(this, !BasePage.IsOEM || BasePage.OEM.OEMStyle == null || string.IsNullOrWhiteSpace(BasePage.OEM.OEMStyle.StylePath) ? BasePage.DefaultStyleFile : BasePage.OEM.OEMStyle.StylePath);
                BasePage.LoadStyle(this);
                lblServicePhone.Text = BasePage.CurrenContract.ServicePhone;
                if(!string.IsNullOrWhiteSpace(Request.QueryString["key"])) {
                    Session["Spreader"] = Request.QueryString["key"];
                }
                if(LogonUtility.Logoned) {
                    Session["Spreader"] = null;
                    redirectDefaultPage();
                }
                AutoLogin();
            }
        }

        private bool OEMLogin() { 
            const string AuthenticationUrl = "/Authentication/";
            if (IsOEM)
            {
                Server.Transfer(AuthenticationUrl + OEM.LoginUrl, true);
                return true;
            }
            return false;
        }

        //自动登录
        private void AutoLogin() {
            HttpCookie cok = Request.Cookies["userKey"];
            //获取上次登录保存的cookice
            if(cok != null && cok.Values["user"] != null) {
                string user = cok.Values["user"];
                string message;
                if(user.Split('|').Length < 2) return;
                if(LogonUtility.Logon(LogonUtility.Decode(GetDecode(user.Split('|')[0])), LogonUtility.Decode(GetDecode(user.Split('|')[1])), false, out message)) {
                    userLogonSuccess();
                }
            }
        }
        protected void btnLogon_Click(object sender, EventArgs e) {
            string message;
            if(LogonUtility.Logon(this.txtUserName.Text.Trim(), this.txtPassword.Text, this.txtCode.Text.Trim(), chkJizhu.Checked, out message)) {
                userLogonSuccess();
            } else {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "tip", "msg = '" + message.Replace("\n", string.Empty).Replace("\r", string.Empty) + "'", true);
            }
        }
        private string GetDecode(string paramer) {
            return HttpUtility.UrlDecode(paramer);
        }
        private void userLogonSuccess() {
            Session["Spreader"] = null;
            if(BasePage.LogonCompany.CompanyType != CompanyType.Platform) {
                Session["ShowNotice"] = 1;
            }
            redirectDefaultPage();
        }
        private void redirectDefaultPage() {
            var defaultPage = System.Web.Security.FormsAuthentication.DefaultUrl;
            if(string.IsNullOrWhiteSpace(defaultPage)) {
                defaultPage = "Default.aspx";
            }
            Response.Redirect(defaultPage);
        }
    }
}