using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.Authentication
{
    public partial class Login_blue : UnAuthBasePage
    {
        private static string logonUrl = ChinaPay.B3B.Service.SystemManagement.SystemParamService.B3BDefalutLogonUrl;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (BasePage.OEM != null && !BasePage.OEM.Enabled)
            {
                Response.Redirect(logonUrl, true);
                return;
            }
            if (!IsPostBack)
            {
                string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
                lblPlatformNameFooter.Text = lblPlatformNameTitle.Text = lblPlatformName.Text = BasePage.PlatformName;
                var oem = BasePage.OEM;
                //加载头部连接
                if (oem != null)
                {
                    Copyright.InnerHtml = (oem.Setting == null ? "" : oem.Setting.CopyrightInfo) + oem.DomainName + oem.ICPRecord + (oem.EmbedCode == "" ? "" : "<script src='" + oem.EmbedCode + "'  type='text/javascript' language='JavaScript'></script>");
                    if (oem.LogoPath.Length > 5)
                        imgLogo.Src = FileWeb + "/" + oem.LogoPath + "?" + oem.LogoPath.Substring(oem.LogoPath.Length - 5, 4);
                }
                if (BasePage.IsOEM && !BasePage.OEM.AllowSelfRegex)
                {
                    this.lblRegister.Visible = this.btnRegister.Visible = false;
                }
                //BasePage.AddStyleSheet(this, !BasePage.IsOEM || BasePage.OEM.OEMStyle == null || string.IsNullOrWhiteSpace(BasePage.OEM.OEMStyle.StylePath) ? BasePage.DefaultStyleFile : BasePage.OEM.OEMStyle.StylePath);
                BasePage.LoadStyle(this);
                lblServicePhone.Text = BasePage.CurrenContract.ServicePhone;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["key"]))
                {
                    Session["Spreader"] = Request.QueryString["key"];
                }
                if (LogonUtility.Logoned)
                {
                    Session["Spreader"] = null;
                    redirectDefaultPage();
                }
                AutoLogin();
            }
        }
        //自动登录
        private void AutoLogin()
        {
            HttpCookie cok = Request.Cookies["userKey"];
            //获取上次登录保存的cookice
            if (cok != null && cok.Values["user"] != null)
            {
                string user = cok.Values["user"];
                string message;
                if (user.Split('|').Length < 2) return;
                if (LogonUtility.Logon(LogonUtility.Decode(GetDecode(user.Split('|')[0])), LogonUtility.Decode(GetDecode(user.Split('|')[1])), false, out message))
                {
                    userLogonSuccess();
                }
            }
        }
        protected void btnLogon_Click(object sender, EventArgs e)
        {
            string message;
            if (LogonUtility.Logon(this.txtUserName.Text.Trim(), this.txtPassword.Text, this.txtCode.Text.Trim(), chkJizhu.Checked, out message))
            {
                userLogonSuccess();
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "tip", "msg = '" + message.Replace("\n", string.Empty).Replace("\r", string.Empty) + "'", true);
            }
        }
        private string GetDecode(string paramer)
        {
            return HttpUtility.UrlDecode(paramer);
        }
        private void userLogonSuccess()
        {
            Session["Spreader"] = null;
            if (BasePage.LogonCompany.CompanyType != CompanyType.Platform)
            {
                Session["ShowNotice"] = 1;
            }
            redirectDefaultPage();
        }
        private void redirectDefaultPage()
        {
            var defaultPage = System.Web.Security.FormsAuthentication.DefaultUrl;
            if (string.IsNullOrWhiteSpace(defaultPage))
            {
                defaultPage = "Default.aspx";
            }
            Response.Redirect(defaultPage);
        }
    }
}