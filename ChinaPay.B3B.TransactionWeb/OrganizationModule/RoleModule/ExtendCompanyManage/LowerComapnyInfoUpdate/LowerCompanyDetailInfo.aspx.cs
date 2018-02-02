using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.LowerComapnyInfoUpdate
{
    public partial class LowerCompanyDetailInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            setBackButton();
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    bindCompanyInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId)));
                }
            }
        }

        private void bindCompanyInfo(CompanyDetailInfo info)
        {
            string type = Request.QueryString["Type"];
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblRelation.Text = "-" + (!string.IsNullOrWhiteSpace(type) && type == "Organization" ? "内部机构" : "下级采购");
            this.lblAccountType.Text = info.AccountType.GetDescription();
            this.lblIsOpenExternalInterface.Text = info.IsOpenExternalInterface ? "已启用" : "未启用";
            if (info.AccountType == AccountBaseType.Individual)
            {
                this.lblCompany.Visible = false;
                this.lblTrueName.Text = info.CompanyName;
                this.lblCertNo.Text = info.CertNo;
                this.fixedPhoneTitle.Visible = true;
                this.fixedPhoneValue.Visible = true;
                this.lblFixedPhone.Text = info.OfficePhones;
            }
            else
            {
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
            this.lblLoaction.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area, info.Province, info.City, info.District);
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblFax.Text = info.Faxes;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.lblEmail.Text = info.ContactEmail;
            this.lblQQ.Text = info.ContactQQ;
        }

        private void setBackButton()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}