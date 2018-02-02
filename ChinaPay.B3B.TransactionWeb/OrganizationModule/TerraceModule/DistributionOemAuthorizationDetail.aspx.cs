using System;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOemAuthorizationDetail : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                initData();
            }
        }

        private void initData()
        {
            string companyId = Request.QueryString["OemId"];
            if (!string.IsNullOrWhiteSpace(companyId))
            {
                var userNo = Request.QueryString["UserNo"];
                var companyDetailInfo = OEMService.QueryOEMById(Guid.Parse(companyId));
                this.lblAuthorizationTime.Text = companyDetailInfo.RegisterTime.ToString();
                this.lblUserNo.Text = string.IsNullOrWhiteSpace(userNo) ? string.Empty : userNo;
                this.lblCompanyShortName.Text = companyDetailInfo.Company.AbbreviateName;
                this.lblOemName.Text = companyDetailInfo.SiteName;
                this.lblAuthorizationDomain.Text = companyDetailInfo.DomainName;
                this.lblAuthorizationStatus.Text = companyDetailInfo.EffectTime >= DateTime.Now ? "正常" : "失效";
                this.lblAuthorizationOperator.Text = companyDetailInfo.OperatorAccount;
                this.lblAuthorizationDeadline.Text = companyDetailInfo.EffectTime.Value.ToString("yyyy-MM-dd");
                this.lblAuthorizationDeposit.Text = companyDetailInfo.AuthCashDeposit.TrimInvaidZero();
                var companyInfo = CompanyService.GetCompanyDetail(companyDetailInfo.OperatorAccount);
                hfdOperatorId.Value = companyInfo.CompanyId.ToString();
                hfdCompayId.Value = companyDetailInfo.CompanyId.ToString();
            }
        }

        protected string OEMID
        {
            get
            {
                return Request.QueryString["OemId"];
            }
        }
    }
}