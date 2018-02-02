using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class RegisterSucceed : BasePage
    {

        protected static string AccountType = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ShowUserInfo();
                this.setBackButton();
            }
        }
        private void ShowUserInfo()
        {
            CompanyInfo info = Session["Info"] as CompanyInfo;
            if (info != null)
            {
                AccountType = info.CompanyType == CompanyType.Provider ? "Agent" : "Purchaser";
                this.lblCompanyName.InnerText = info.CompanyName;
                this.lblAccountNo.InnerText = info.UserName;
            }
            else
            {
                SupplierCreatureInfo providerInfo = Session["Info"] as SupplierCreatureInfo;
                if (providerInfo != null)
                {
                    AccountType = "Provider";
                    this.lblCompanyName.InnerText = providerInfo.Name;
                    this.lblAccountNo.InnerText = providerInfo.UserName;
                }
            }
        }
        private void setBackButton()
        {
            this.btnGoBack.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
        }
    }
}