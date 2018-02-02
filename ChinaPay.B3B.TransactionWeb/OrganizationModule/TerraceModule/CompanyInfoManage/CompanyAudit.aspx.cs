using System;
using System.Reflection;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class CompanyAudit : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId)) { this.BindCompanyInfo(CompanyService.GetCompanyDetail(Guid.Parse(companyId))); }
            }
        }
        private void BindCompanyInfo(CompanyDetailInfo info)
        {
            this.lblAccuountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblCompanyName.Text = info.CompanyName;
            this.lblCompanyShortName.Text = info.AbbreviateName;
            this.lblCompanyAddress.Text = info.Address;
            this.lblBeginDeadline.Text = info.PeriodStartOfUse.ToString();
            this.lblEndDeadline.Text = info.PeriodEndOfUse.ToString();
            this.lblAddress.Text = AddressShow.GetAddressText(info.Area,info.Province,info.City,info.District);
            this.lblPostCode.Text = info.ZipCode;
            this.lblCompanyPhone.Text = info.OfficePhones;
            this.lblFaxes.Text = info.Faxes;
            this.lblLinkMan.Text = info.Contact;
            this.lblLinkManPhone.Text = info.ContactPhone;
            this.lblEmail.Text = info.ContactEmail;
            this.lblMSN.Text = info.ContactMSN;
            this.lblQQ.Text = info.ContactQQ;
            this.tbAgen.Visible = false;
            this.tbAgens.Visible = false;
            this.BindAgentAgentQualification(info.CompanyId, info);
        }
        private void BindAgentAgentQualification(Guid id, CompanyInfo info)
        {
            if (info.CompanyType == CompanyType.Provider)
            {
                this.tbAgen.Visible = true;
                this.tbAgens.Visible = true;
                this.lblEmail.Text = info.ManagerEmail;
                this.lblMSN.Text = info.ManagerMsn;
                this.lblQQ.Text = info.ManagerQQ;
                this.lblPrincipal.Text = info.ManagerName;
                this.lblPrincipalPhone.Text = info.ManagerCellphone;
                this.lblUrgencyLinkman.Text = info.EmergencyContact;
                this.lblUrgencyLinkmanPhone.Text = info.EmergencyCall;
                AgentQualification agen = CompanyService.GetAgentQualification(id);
                if (agen != null)
                {
                    this.lblIATABusinessApprovalNumber.Text = agen.Licence;
                    this.lblTATANumber.Text = agen.IATA;
                    this.lblCaticAssociationSuch.Text = agen.Deposit.Value.ToString();
                    if (agen.QualificationType == QualificationType.Level1) this.rdoFirst.Checked = true;
                    if (agen.QualificationType == QualificationType.Level2) this.rdoTwo.Checked = true;
                    if (agen.QualificationType == QualificationType.Level3) this.rdoLast.Checked = true;
                }
            }
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.Accept(Guid.Parse(Request.QueryString["CompanyId"]));
                ShowMessage("审核通过");
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"审核");
            }
        }
        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
               CompanyService.Reject(Guid.Parse(Request.QueryString["CompanyId"]),this.CurrentUser.UserName);
               ShowMessage("拒绝审核通过");
               Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "拒绝审核");
            }
        }
    }
}