using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LookUpInfo
{
    public partial class SpreadCompanyDetailInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    var info = CompanyService.GetCompanyDetail(Guid.Parse(companyId));
                    bindCompanyInfo(info);
                }
            }
        }

        private void bindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblAccountType.Text = info.AccountType.GetDescription();
            this.lblIsOpenExternalInterface.Text = info.IsOpenExternalInterface ? "已启用" : "未启用";
            if (info.AccountType == AccountBaseType.Individual)
            {
                this.companyContactInfo.Visible = false;
                this.lblCompany.Visible = false;
                this.lblTrueName.Text = info.CompanyName;
                this.lblCertNo.Text = info.CertNo;
                this.lblFixedPhone.Text = info.OfficePhones;
            }
            else
            {
                this.fixedPhone.Visible = false;
                this.lblIndividual.Visible = false;
                this.lblCompanyName.Text = info.CompanyName;
                this.lblOrginationCode.Text = info.OrginationCode;
                this.lblCompanyShortName.Text = info.AbbreviateName;
                this.lblCompanyPhone.Text = info.OfficePhones;
                this.lblPrincipal.Text = info.ManagerName;
                this.lblPrincipalPhone.Text = info.ManagerCellphone;
                this.lblUrgencyLinkMan.Text = info.EmergencyContact;
                this.lblUrgencyLinkManPhone.Text = info.EmergencyCall;
            }
            if (info.CompanyType == CompanyType.Platform || info.CompanyType == CompanyType.Purchaser)
            {
                this.timeTitle.Visible = false;
                this.timeValue.Visible = false;
            }
            this.lblLocation.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblFaxes.Text = info.Faxes;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkManPhone.Text = info.ContactPhone;
            this.lblEmail.Text = info.ContactEmail;
            this.lblQQ.Text = info.ContactQQ;
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.HasValue ? info.PeriodStartOfUse.Value.ToShortDateString() : string.Empty;
            this.lblEndDeadline.Text = info.PeriodEndOfUse.HasValue ? info.PeriodEndOfUse.Value.ToShortDateString() : string.Empty;
        }

    }
}