using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;


namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LookUpInfo
{
    public partial class LookUpBuyOrOutInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                this.setBackButton();
                string companyId = Request.QueryString["CompanyId"];
                if(!string.IsNullOrWhiteSpace(companyId))
                {
                this.BingInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId)));
                }
            }
        }
        private void BingInfo(CompanyDetailInfo info) {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblCompanyName.Text = info.CompanyName;
            this.lblCompanyShortName.Text = info.AbbreviateName;
            this.lblLocation.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblCompanyPhone.Text = info.OfficePhones;
            this.lblFaxes.Text = info.Faxes;
            this.lblPrincipal.Text = info.ManagerName;
            this.lblPrincipalPhone.Text = info.ManagerCellphone;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkManPhone.Text = info.ContactPhone;
            this.lblUrgencyLinkMan.Text = info.EmergencyContact;
            this.lblUrgencyLinkManPhone.Text = info.EmergencyCall;
            this.lblEmail.Text = info.ManagerEmail;
            this.lblMSN.Text = info.ManagerMsn;
            this.lblQQ.Text = info.ManagerQQ;
            this.trTime.Visible = false;
            this.BindTime(info);
        }
        private void BindTime(CompanyInfo info)
        {
            if (info.CompanyType == CompanyType.Provider)
            {
                this.trTime.Visible = true;
                this.lblBeginDeadline.Text = info.PeriodStartOfUse.HasValue ? info.PeriodStartOfUse.Value.ToShortDateString() : string.Empty;
                this.lblEndDeadline.Text = info.PeriodEndOfUse.HasValue ? info.PeriodEndOfUse.Value.ToShortDateString() : string.Empty; 
            }
        }
        private void setBackButton()
        {
            this.btnGoBack.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
        }
    }
}