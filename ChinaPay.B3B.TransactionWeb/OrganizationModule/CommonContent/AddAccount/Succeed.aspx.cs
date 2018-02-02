using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CommonContent.AddAccount
{
    public partial class Succeed : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = lblPlatformName1.Text = PlatformName;
                setBackButton();
                string type = Request.QueryString["Type"];
                InitData(type);
            }
        }
        private void InitData(string type)
        {
            CompanyType companyType = (CompanyType)Enum.Parse(typeof(CompanyType), type);
            divNotProvider.Visible = companyType != CompanyType.Provider;
            divProvider.Visible = companyType == CompanyType.Provider;
            lblProviderAccount.InnerText = lblNotProviderAccount.InnerText = Request.QueryString["Account"];
            string accounType = Request.QueryString["AccounType"], name = Request.QueryString["Name"];
            if (bool.Parse(accounType))
                lblNotProviderCompanyName.InnerText = lblProviderCompanyName.InnerText = "您的个人名称为：" + name;
            else
                lblNotProviderCompanyName.InnerText = lblProviderCompanyName.InnerText = "您的企业名称为：" + name;
        }
        private void setBackButton()
        {
            btnGoBack.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
            btnGoBacks.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
        }
    }
}