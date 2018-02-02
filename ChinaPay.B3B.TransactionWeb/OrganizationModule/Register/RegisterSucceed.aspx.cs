using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Register
{
    public partial class RegisterSucceed : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = BasePage.PlatformName;
                string type = Request.QueryString["Type"];
                InitData(type);
            }
        }
        private void InitData(string type) {
            if (BasePage.IsOEM)
                this.lblOem.Visible = false;
            CompanyType companyType =(CompanyType)Enum.Parse(typeof(CompanyType), type);
            divNotProvider.Visible = companyType != CompanyType.Provider;
            divProvider.Visible = companyType == CompanyType.Provider;
            string b3bAccount = Request.QueryString["Account"];
            lblProviderAccount.InnerText = lblNotProviderAccount.InnerText = b3bAccount;
            string accounType = Request.QueryString["AccounType"],name = Request.QueryString["Name"];
            string poolpayAcccount = Request.QueryString["pooypayAccount"];
            if (!string.IsNullOrEmpty(poolpayAcccount)&&b3bAccount != poolpayAcccount)
            {
                lbpoolpayAccount1.InnerText = lbpoolpayAccount2.InnerText = "您的国付通账号为：" + poolpayAcccount;
                lbpoolpayAccount1.Visible = lbpoolpayAccount2.Visible = true;
                accountTip1.InnerText = accountTip2.InnerText = "欢迎使用" + BasePage.PlatformName + "购买机票！";
            }
            if (bool.Parse(accounType))
                lblNotProviderCompanyName.InnerText = lblProviderCompanyName.InnerText = "您的个人名称为：" + name;
            else
                lblNotProviderCompanyName.InnerText = lblProviderCompanyName.InnerText = "您的企业名称为：" + name;
        }
    }
}