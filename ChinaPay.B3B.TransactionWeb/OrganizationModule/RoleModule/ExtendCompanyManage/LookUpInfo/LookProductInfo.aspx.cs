using System;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LookUpInfo
{
    public partial class LookProductInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
	        {
                setBackButton();
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId)) this.BingInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId)));
	        }
        }
        private void BingInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblUserName.Text = info.CompanyName;
            this.lblPetName.Text = info.AbbreviateName;
            this.lblLocation.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.HasValue ? info.PeriodStartOfUse.Value.ToShortDateString() : string.Empty;
            this.lblEndDeadline.Text = info.PeriodEndOfUse.HasValue ? info.PeriodEndOfUse.Value.ToShortDateString() : string.Empty;
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblFaxes.Text = info.Faxes;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.lblEmail.Text = info.ContactEmail;
            this.lblMSN.Text = info.ContactMSN;
            this.lblQQ.Text = info.ContactQQ;
        }
        private void setBackButton() {
            this.btnGoBack.Attributes.Add("onclick", "window.location.href='"+ (Request.UrlReferrer ?? Request.Url).PathAndQuery+"';return false;");
        }
    }
}