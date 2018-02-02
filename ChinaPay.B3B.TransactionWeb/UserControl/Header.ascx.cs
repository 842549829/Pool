using System;

namespace ChinaPay.B3B.TransactionWeb.UserControl
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                setLogonStatusInfo();
            }
        }

        private void setLogonStatusInfo()
        {
            string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
            if (LogonUtility.Logoned)
            {
                // 登录后
                this.divLogoff.Visible = false;
                this.divLogon.Visible = true;
                this.logonUserName.InnerText = BasePage.LogonUser.Name + "(" + BasePage.LogonUser.UserName + ")";
                this.homePageLink.Visible = true;
                this.lnkCustomerService.Visible = true;
                if (BasePage.LogonCompany.CompanyType == Common.Enums.CompanyType.Platform)
                {
                    this.mypoolpay.Visible = false;
                    IwillBuy.Visible = false;
                    ImportPNR.Visible = false;
                }
                else
                {
                    this.lnkJifen.Visible = true;
                }
                if (IwillBuy.InnerText == "我的机票")
                {
                    FlightHref = "/Index.aspx";
                }
            }
            else
            {
                // 未登录时
                this.divLogoff.Visible = true;
                this.divLogon.Visible = false;
                IwillBuy.Visible = false;
                this.lnkCustomerService.Visible = false;
                this.mypoolpay.Visible = false;
                registerPage.Visible = !BasePage.IsOEM || BasePage.IsOEM && BasePage.OEM.AllowSelfRegex;
                if (Request.Url.AbsolutePath == System.Web.Security.FormsAuthentication.LoginUrl)
                {
                    this.logonPage.Visible = false;
                }
                else
                {
                    if (Request.Url.AbsolutePath == "/OrganizationModule/Register/Register.aspx")
                    {
                        this.registerPage.Visible = false;
                    }
                }
            }
            var oem = BasePage.OEM;
            //加载头部连接
            if (oem != null && oem.Setting != null && oem.Setting.HeaderLinks != null)
            {
                string str = "";
                foreach (var item in oem.Setting.HeaderLinks)
                {
                    str += "<a href='" + item.URL + "' title='" + item.Remark + "'  target='_blank' >" + item.LinkName + "</a>";
                }
                links.InnerHtml += str;
                if (oem.LogoPath.Length > 5)
                    imgLogo.Src = FileWeb + "/" + oem.LogoPath + "?" + oem.LogoPath.Substring(oem.LogoPath.Length - 5, 4);
            }
        }

        protected void exit_ServerClick(object sender, EventArgs e)
        {
            LogonUtility.Logoff();
            Response.Redirect("/Agency.htm?target=" + System.Web.HttpUtility.UrlEncode("/Logon.aspx"), true);
        }

        public string FlightText { set { IwillBuy.InnerText = value; } }

        public string FlightHref { set { IwillBuy.HRef = value; } }
    }
}